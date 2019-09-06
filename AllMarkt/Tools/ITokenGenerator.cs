using AllMarkt.ViewModels;

namespace AllMarkt.Tools
{
    public interface ITokenGenerator
    {
        string Generate(AppSettings _appSettings, UserTokenDataModel user);
    }
}