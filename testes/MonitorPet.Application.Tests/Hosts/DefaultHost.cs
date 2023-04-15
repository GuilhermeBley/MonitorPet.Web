using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MonitorPet.Application.Tests.Hosts;

public class DefaultHost
{
    private IServiceProvider _serviceProvider;
    public IServiceProvider ServiceProvider => _serviceProvider.CreateScope().ServiceProvider;

    public DefaultHost()
    {
        _serviceProvider = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(ConfigureConfiguration)
            .ConfigureServices(ConfigureServices)
            .ConfigureLogging(ConfigureLogging)
            .Build().Services;
        AfterCreated(_serviceProvider);
    }

    protected virtual void ConfigureServices(HostBuilderContext context, IServiceCollection serviceCollection)
    {

    }

    protected virtual void ConfigureLogging(HostBuilderContext context, ILoggingBuilder loggingBuilder)
    {

    }

    protected virtual void ConfigureConfiguration(HostBuilderContext context, IConfigurationBuilder configurationBuilder)
    {
        
    }

    protected virtual void AfterCreated(IServiceProvider provider)
    {

    }
}