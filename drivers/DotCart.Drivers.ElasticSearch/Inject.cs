////////////////////////////////////////////////////////////////////////////////////////////////
// MIT License
////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c)2023 DisComCo Sp.z.o.o. (http://discomco.pl)
////////////////////////////////////////////////////////////////////////////////////////////////
// Permission is hereby granted, free of charge,
// to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS",
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////

using System.Diagnostics;
using DotCart.Logging;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace DotCart.Drivers.ElasticSearch;

public static class Inject
{
    public static IServiceCollection AddElasticSearch(this IServiceCollection services,
        IConnectionSettingsValues forceConnectionSettings = null)
    {
        if (forceConnectionSettings != null)
            services.AddSingleton<IConnectionSettingsValues>(_ => forceConnectionSettings);
        else
            services.AddSingleton<IConnectionSettingsValues>(new ConnectionSettings(new Uri(Config.Url)));
        return services
            .AddTransient<IElasticLowLevelClient, ElasticLowLevelClient>()
            .AddTransient<IElasticClient, ElasticClient>();
    }

    /// <summary>
    ///     Adds Serilog for Elasticsearch.
    ///     Please visit https://github.com/serilog/serilog-aspnetcore for more info
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <param name="enableSelfLog"></param>
    /// <returns></returns>
    private static ILogger CreateSeriLogElasticSearchLogger(ElasticsearchSinkOptions? options = null,
        bool enableSelfLog = false)
    {
        if (enableSelfLog)
        {
            SelfLog.Enable(msg => Debug.WriteLine(msg));
            SelfLog.Enable(Console.Error);
        }

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.ControlledBy(new EnvLogLevelSwitch(Logging.EnVars.LOG_LEVEL_MIN))
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .WriteTo.Elasticsearch(options)
            .CreateLogger();
        return Log.Logger;
    }

    public static IServiceCollection AddElasticSearchLogger(this ServiceCollection services,
        ElasticsearchSinkOptions? options = null,
        bool enableSelfLog = false)
    {
        return services
            .AddSingleton(x => CreateSeriLogElasticSearchLogger(options, enableSelfLog));
    }
}