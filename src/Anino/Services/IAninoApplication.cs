using Anino.Configuration;

namespace Anino.Services;

public interface IAninoApplication
{
    int Run(AninoOptions options);
}