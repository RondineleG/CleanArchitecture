using System;

namespace Application.DTOs.Account;

public class RefreshToken
{
    public DateTime Created { get; set; }

    public string CreatedByIp { get; set; }

    public DateTime Expires { get; set; }

    public int Id { get; set; }

    public bool IsActive => Revoked == null && !IsExpired;

    public bool IsExpired => DateTime.UtcNow >= Expires;

    public string ReplacedByToken { get; set; }

    public DateTime? Revoked { get; set; }

    public string RevokedByIp { get; set; }

    public string Token { get; set; }
}