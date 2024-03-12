using System.Security.Cryptography.X509Certificates;
using System.Text;
using DotCart.TestKit;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

namespace DotCart.Drivers.EventStoreDB.Tests;

public class ResilientESDBClientTests(
    ITestOutputHelper output,
    IoCTestContainer testEnv)
    : IoCTests(output, testEnv)
{
    [Fact]
    public void ShouldResolveESDBSettings()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var options = TestEnv.ResolveRequired<IOptions<ESDBSettings>>();
        // THEN
        Assert.NotNull(options);
        Assert.NotNull(options.Value);
    }

    [Fact]
    public void ShouldResolveResilientESDBClient()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        // WHEN
        var client = TestEnv.ResolveRequired<IResilientESDBClient>();
        // THEN
        Assert.NotNull(client);
    }


    [Fact]
    public async Task ShouldAppendToStreamAsync()
    {
        // GIVEN
        Assert.NotNull(TestEnv);
        var client = TestEnv.ResolveRequired<EventStoreClient>();
        Assert.NotNull(client);
        var eventData = new EventData[]
        {
            new(
                Uuid.NewUuid(),
                "tested_00",
                "test0"u8.ToArray()
            ),
            new(
                Uuid.NewUuid(),
                "tested_01",
                "test1"u8.ToArray()
            ),
            new(
                Uuid.NewUuid(),
                "tested_02",
                "test2"u8.ToArray()
            ),
            new(
                Uuid.NewUuid(),
                "tested_02",
                "test3"u8.ToArray()
            )
        };
        // WHEN
        var res = await client
            .AppendToStreamAsync("my_stream", StreamState.Any, eventData)
            .ConfigureAwait(true);
        // THEN
        Assert.NotNull(res);
        Assert.Equal(3, res.NextExpectedStreamRevision.ToInt64());
    }


    protected override void Initialize()
    {
    }

    protected override void SetEnVars()
    {
    }

    protected override void InjectDependencies(IServiceCollection services)
    {
        services
            .AddESDSettingsFromAppDirectory()
            .AddEventStoreClient(settings =>
            {
                var esdbSettings = services
                    .BuildServiceProvider()
                    .GetRequiredService<IOptions<ESDBSettings>>()
                    .Value;
                settings.ConnectivitySettings.Address = new Uri(esdbSettings.Uri);
                settings.ConnectivitySettings.Insecure = esdbSettings.IsInsecure;
                settings.DefaultCredentials = new UserCredentials(
                    esdbSettings.Username,
                    esdbSettings.Password);
                // settings.ConnectivitySettings.TlsCaFile = X509Certificate2
                //     .CreateFromPemFile(esdbSettings.TlsCaFile);
                settings.ConnectivitySettings.TlsVerifyCert = false;
            });
    }
}