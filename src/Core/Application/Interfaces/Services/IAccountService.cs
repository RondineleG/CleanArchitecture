﻿using Application.DTOs.Account;
using Application.Wrappers;

using System.Threading.Tasks;

namespace Application.Interfaces.Services;

public interface IAccountService
{
    Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);

    Task<Response<string>> ConfirmEmailAsync(string userId, string code);

    Task ForgotPassword(ForgotPasswordRequest model, string origin);

    Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);

    Task<Response<string>> ResetPassword(ResetPasswordRequest model);
}