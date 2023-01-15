using CouchDB.Driver;
using CouchDB.Driver.Options;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Defaults.CouchDb;
using DotCart.Drivers.CouchDB.Internal;
using DotCart.Drivers.CouchDB.Internal.Interfaces;
using Flurl.Http.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using static DotCart.Core.AppErrors;

namespace DotCart.Drivers.CouchDB;

public static class Inject
{
    public static readonly Action<CouchSettings>
        settingsFromConfig =
            _ =>
            {
                new CouchSettings(Config.LocalUser, Config.LocalPwd, Config.LocalHost, Config.LocalPort,
                    Config.DatabasePrefix)
                {
                    Host = Config.LocalHost,
                    DatabasePrefix = Config.DatabasePrefix,
                    Port = Config.LocalPort
                };
            };

    public static readonly Action<CouchOptionsBuilder>
        optionsFromConfig =
            builder => builder
                .UseBasicAuthentication(
                    Config.LocalUser,
                    Config.LocalPwd)
                .UseEndpoint(Config.CouchLocalEndpoint)
                .DisableDocumentPluralization();

    public static readonly Action<ClientFlurlHttpSettings>
        httpFromConfig =
            settings =>
            {
                settings.OnErrorAsync =
                    async call => { Log.Error(Error(call.Exception.InnerAndOuter())); };
            };


    public static IServiceCollection AddDotCouch<TICouchInfo, TDoc, TID>(this IServiceCollection services,
        Action<CouchSettings> settings = null,
        Action<CouchOptionsBuilder> options = null,
        Action<ClientFlurlHttpSettings> httpSettings = null)
        where TICouchInfo : ICouchDbInfoB
        where TDoc : IState
        where TID : IID
    {
        if (settings == null)
            services.AddTransient(_ => settingsFromConfig);
        if (options == null)
            services.AddTransient(_ => optionsFromConfig);
        if (httpSettings == null)
            services.AddTransient(_ => httpFromConfig);
        services
            .TryAddSingleton<ICouchStoreBuilder<TICouchInfo, TDoc, TID>, CouchStoreBuilder<TICouchInfo, TDoc, TID>>();
        services.TryAddSingleton<IStoreBuilderT<TICouchInfo, TDoc, TID>, CouchStoreBuilder<TICouchInfo, TDoc, TID>>();
        return services
            .AddCouchServer()
            .AddCouchClient<TICouchInfo, TDoc, TID>(settingsFromConfig);
    }

    private static IServiceCollection AddCouchServer(this IServiceCollection services)
    {
        return services
            .AddTransient<ICouchServer, CouchServer>();
    }


    private static IServiceCollection AddCouchClient<TICouchInfo, TDoc, TID>(this IServiceCollection services,
        Action<CouchSettings> couchSettingsFunc)
    {
        return services
            .AddSingleton<ICouchClient, CouchClient>();
    }
}