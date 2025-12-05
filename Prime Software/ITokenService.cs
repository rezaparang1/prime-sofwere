namespace Prime_Software
{
    public interface ITokenService
    {
        TokenResponse GenerateToken(BusinessEntity.Settings.User user);
    }
}
