﻿using Application.DTOs.Account;
using Application.Interfaces.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using System.Threading.Tasks;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    private readonly IAccountService _accountService;

    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
    {
        return Ok(await _accountService.AuthenticateAsync(request, GenerateIPAddress()).ConfigureAwait(false));
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
    {
        var origin = Request.Headers["origin"].ToString();
        return Ok(await _accountService.ConfirmEmailAsync(userId, code).ConfigureAwait(false));
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
    {
        await _accountService.ForgotPassword(model, Request.Headers["origin"]).ConfigureAwait(false);
        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        StringValues origin = Request.Headers["origin"];
        return Ok(await _accountService.RegisterAsync(request, origin).ConfigureAwait(false));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
    {
        return Ok(await _accountService.ResetPassword(model).ConfigureAwait(false));
    }

    private string GenerateIPAddress()
    {
        return Request.Headers.ContainsKey("X-Forwarded-For")
            ? ((string)Request.Headers["X-Forwarded-For"])
            : HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }
}