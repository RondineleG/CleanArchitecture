﻿using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Account;

public class RegisterRequest
{
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [MinLength(6)]
    public string UserName { get; set; }
}