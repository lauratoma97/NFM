namespace NFM.WebApi.Authentication;

public class JwtOptionsSettings
{
    public string? Issuer { get; set; }

    public string? Audience { get; set; }

    public string? ValidForSeconds { get; set; }

    public string? SigningKey { get; set; }
}