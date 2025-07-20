using Application.DTOs.Account;
using Application.DTOs.Email;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;

using Domain.Settings;

using Infrastructure.Identity.Helpers;
using Infrastructure.Identity.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services;

public class AccountService : IAccountService
{
    public AccountService(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<JWTSettings> jwtSettings,
        IDateTimeService dateTimeService,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtSettings.Value;
        _dateTimeService = dateTimeService;
        _signInManager = signInManager;
        this._emailService = emailService;
    }

    private readonly IDateTimeService _dateTimeService;

    private readonly IEmailService _emailService;

    private readonly JWTSettings _jwtSettings;

    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly UserManager<ApplicationUser> _userManager;

    public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
    {
        ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new ApiException($"No Accounts Registered with {request.Email}.");
        }
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            throw new ApiException($"Invalid Credentials for '{request.Email}'.");
        }
        if (!user.EmailConfirmed)
        {
            throw new ApiException($"Account Not Confirmed for '{request.Email}'.");
        }
        JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
        AuthenticationResponse response = new()
        {
            Id = user.Id,
            JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            Email = user.Email,
            UserName = user.UserName
        };
        IList<string> rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        response.Roles = rolesList.ToList();
        response.IsVerified = user.EmailConfirmed;
        RefreshToken refreshToken = GenerateRefreshToken(ipAddress);
        response.RefreshToken = refreshToken.Token;
        return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
    }

    public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(userId);
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);
        return result.Succeeded
            ? new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.")
            : throw new ApiException($"An error occured while confirming {user.Email}.");
    }

    public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
    {
        ApplicationUser account = await _userManager.FindByEmailAsync(model.Email);

        // always return ok response to prevent email enumeration
        if (account == null)
        {
            return;
        }

        string code = await _userManager.GeneratePasswordResetTokenAsync(account);
        string route = "api/account/reset-password/";
        _ = new Uri(string.Concat($"{origin}/", route));
        EmailRequest emailRequest = new()
        {
            Body = $"You reset token is - {code}",
            To = model.Email,
            Subject = "Reset Password",
        };
        await _emailService.SendAsync(emailRequest);
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
    {
        ApplicationUser userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
        if (userWithSameUserName != null)
        {
            throw new ApiException($"Username '{request.UserName}' is already taken.");
        }
        ApplicationUser user = new()
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName
        };
        ApplicationUser userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail == null)
        {
            IdentityResult result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                _ = await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                string verificationUri = await SendVerificationEmail(user, origin);
                //TODO: Attach Email Service here and configure it via appsettings
                await _emailService.SendAsync(new Application.DTOs.Email.EmailRequest() { From = "mail@codewithmukesh.com", To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });
                return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
            }
            else
            {
                throw new ApiException($"{result.Errors}");
            }
        }
        else
        {
            throw new ApiException($"Email {request.Email} is already registered.");
        }
    }

    public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
    {
        ApplicationUser account = await _userManager.FindByEmailAsync(model.Email);
        if (account == null)
        {
            throw new ApiException($"No Accounts Registered with {model.Email}.");
        }

        IdentityResult result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
        return result.Succeeded
            ? new Response<string>(model.Email, message: $"Password Resetted.")
            : throw new ApiException($"Error occured while reseting the password.");
    }

    private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
    {
        IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);
        IList<string> roles = await _userManager.GetRolesAsync(user);

        List<Claim> roleClaims = [];

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }

        string ipAddress = IpHelper.GetIpAddress();

        IEnumerable<Claim> claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id),
            new Claim("ip", ipAddress)
        }
        .Union(userClaims)
        .Union(roleClaims);

        SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurityToken = new(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

    private RefreshToken GenerateRefreshToken(string ipAddress)
    {
        return new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }

    private string RandomTokenString()
    {
        using RNGCryptoServiceProvider rngCryptoServiceProvider = new();
        byte[] randomBytes = new byte[40];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        // convert random bytes to hex string
        return BitConverter.ToString(randomBytes).Replace("-", "");
    }

    private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
    {
        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        string route = "api/account/confirm-email/";
        Uri _enpointUri = new(string.Concat($"{origin}/", route));
        string verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
        //Email Service Call Here
        return verificationUri;
    }
}