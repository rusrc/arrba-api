namespace Arrba.Services
{
    public interface ICaptchaService
    {
        bool Check(string greCaptchaResponse);
    }
}
