using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.DTOs.Account;

public class AuthenticationResponse
{
    public string Email { get; set; }

    public string Id { get; set; }

    public bool IsVerified { get; set; }

    public string JWToken { get; set; }

    [JsonIgnore]
    public string RefreshToken { get; set; }

    public List<string> Roles { get; set; }

    public string UserName { get; set; }
}