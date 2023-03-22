using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyCouch;
using MyCouch.Net;

namespace DotCart.Drivers.CouchDB;

public static class ServerInfo
{
    public static readonly Func<ServerConnectionInfo>
        LocalFromConfig =
            () =>
            {
                var res = new ServerConnectionInfo(Config.LocalEndpoint)
                {
                    BasicAuth = new BasicAuthString(Config.LocalUser, Config.LocalPwd)
                };
                return res;
            };

    public static readonly Func<ServerConnectionInfo>
        RemoteFromConfig =
            () =>
            {
                var res = new ServerConnectionInfo(Config.ClusterEndpoint)
                {
                    BasicAuth = new BasicAuthString(Config.ClusterUser, Config.ClusterPwd)
                };
                return res;
            };
}

public static class Inject
{
    public static IServiceCollection AddDotCouch<TDbInfo, TDoc, TID>(this IServiceCollection services)
        where TDbInfo : ICouchDbInfoB
        where TDoc : IState
        where TID : IID
    {
        services.TryAddSingleton(ServerInfo.LocalFromConfig);
        services.TryAddSingleton<IMyCouchFactory, MyCouchClientFactory>();
        services.TryAddSingleton<ICouchAdminBuilderT<TDbInfo>, CouchStoreBuilderT<TDbInfo, TDoc, TID>>();
        //        services.TryAddSingleton<ICouchStoreBuilderT<TDbInfo, TDoc, TID>, CouchStoreBuilderT<TDbInfo, TDoc, TID>>();
        services.TryAddSingleton<IStoreBuilderT<TDbInfo, TDoc, TID>, CouchStoreBuilderT<TDbInfo, TDoc, TID>>();
        return services;
    }
}