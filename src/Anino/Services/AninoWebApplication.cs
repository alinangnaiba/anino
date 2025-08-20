using Microsoft.AspNetCore.Builder;

namespace Anino.Services;

public class AninoWebApplication : IAninoWebApplication
{
    private readonly WebApplication _webApplication;

    public AninoWebApplication(WebApplication webApplication)
    {
        _webApplication = webApplication;
    }

    public void Run(string url)
    {
        _webApplication.Run(url);
    }
}