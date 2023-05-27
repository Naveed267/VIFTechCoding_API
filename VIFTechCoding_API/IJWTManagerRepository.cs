namespace VIFTechCoding_API
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(Users users);
    }
}
