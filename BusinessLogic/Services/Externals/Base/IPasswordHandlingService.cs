namespace BusinessLogic.Services.Externals.Base
{
    public interface IPasswordHandlingService
    {
        string GetHashPassword(string password);
    }
}
