using System.Net;
using BoDi;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using TechTalk.SpecFlow;

namespace NadinSoft.CRUD.AcceptanceTest.Hook;

[Binding]
public class DockerControllerHooks
{
    private static ICompositeService _compositeService;
    private static IObjectContainer _objectContainer;

    public DockerControllerHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeTestRun]
    public static void DockerComposeUp()
    {
        string dockerComposePath = GetDockerComposeLocation();

        _compositeService = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(dockerComposePath + "/docker-compose.yml")
            .RemoveOrphans()
            .WaitForHttp("NadinSoft_API", "http://localhost:8080/api/Product/get-all",
                continuation: (response, _) => response.Code != HttpStatusCode.OK ? 2000 : 0)
            .Build().Start();
    }

    [AfterTestRun]
    public static void DockerComposeDown()
    {
        _compositeService.Stop();
        _compositeService.Dispose();
    }

    [BeforeScenario]
    public void AddHttpClient()
    {
        HttpClient httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:8080/api/")
        };
        _objectContainer.RegisterInstanceAs(httpClient);
    }

    private static string GetDockerComposeLocation()
    {
        string currentDir = AppDomain.CurrentDomain.BaseDirectory;

        while (Directory.GetParent(currentDir) != null)
        {
            string dockerComposePath = Path.Combine(currentDir, "docker-compose.yml");
            if (File.Exists(dockerComposePath))
            {
                return currentDir;
            }

            currentDir = Directory.GetParent(currentDir).FullName;
        }

        throw new InvalidOperationException("Solution root not found.");
    }
}