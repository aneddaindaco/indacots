namespace IndacoProject.Corso.Core
{
    public interface ITokenService
    {
        string GenerateAccessToken(string username);
        string GenerateRefreshToken(string username);
        string VerifyRefreshToken(string token);
    }
}