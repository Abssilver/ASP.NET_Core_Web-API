using MetricsManagerClient.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Windows;
using AutoMapper;
using MetricsManagerClient.Client.Interfaces;
using MetricsManagerClient.DataLayer;
using MetricsManagerClient.DataLayer.Interfaces;
using MetricsManagerClient.DataLayer.Models;
using MetricsManagerClient.Jobs;
using MetricsManagerClient.Jobs.JobFactory;
using MetricsManagerClient.Jobs.Schedule;
using MetricsManagerClient.MappingSettings;
using MetricsManagerClient.Services;

namespace MetricsManagerClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
	    private const string CronExpression = "0/5 * * * * ?";

		private IHost _host;
		private NLog.Logger _logger;

		public App()
		{
			var serviceCollection = new ServiceCollection();
			_logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

			_host = new HostBuilder()
				.ConfigureServices((hostContext, services) =>
				{
					ConfigureServices(services);
				})
			.ConfigureLogging(logging =>
			{
				logging.AddDebug();
				logging.ClearProviders(); 
				logging.SetMinimumLevel(LogLevel.Trace); 
			})
			.UseNLog()
			.Build();
		}
		
		private void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<MainWindow>();

			services.AddSingleton<IAppModel, AppModel>();
			services.AddSingleton<ICpuMetricModel, CpuMetricModel>();
			services.AddSingleton<IDotNetMetricModel, DotNetMetricModel>();
			services.AddSingleton<IHddMetricModel, HddMetricModel>();
			services.AddSingleton<INetworkMetricModel, NetworkMetricModel>();
			services.AddSingleton<IRamMetricModel, RamMetricModel>();

			services.AddHttpClient<IAgentInfoClient, AgentInfoClient>();
			services.AddHttpClient<ICpuMetricsClient, CpuMetricsClient>();
			services.AddHttpClient<IDotNetMetricsClient, DotNetMetricsClient>();
			services.AddHttpClient<IHddMetricsClient, HddMetricsClient>();
			services.AddHttpClient<INetworkMetricsClient, NetworkMetricsClient>();
			services.AddHttpClient<IRamMetricsClient, RamMetricsClient>();

			ConfigureMapper(services);

			JobsSheduleSettings(services);
		}
		
		private void ConfigureMapper(IServiceCollection services)
		{
			var mapperConfiguration = new MapperConfiguration(
				mp => mp.AddProfile(new MapperProfile()));
			var mapper = mapperConfiguration.CreateMapper();
			services.AddSingleton(mapper);
		}

		private async void OnStartup(object sender, StartupEventArgs e)
		{

			using (var serviceScope = _host.Services.CreateScope())
			{
				var services = serviceScope.ServiceProvider;
				try
				{
					await _host.StartAsync();
					
					var mainWindow = services.GetRequiredService<MainWindow>();
					mainWindow.Show();
				}
				catch (Exception exception)
				{
					_logger.Error(exception, "Stopped program because of exception");
				}
			}

		}

		protected override async void OnExit(ExitEventArgs e)
		{
			using (_host)
			{
				NLog.LogManager.Shutdown();

				await _host.StopAsync(TimeSpan.FromSeconds(5));
			}

			base.OnExit(e);
		}
		
		private void JobsSheduleSettings(IServiceCollection services)
		{
			services.AddSingleton<IJobFactory, SingletonJobFactory>();
			services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

			services.AddSingleton<CpuMetricJob>();
			services.AddSingleton<DotNetMetricJob>();
			services.AddSingleton<HddMetricJob>();
			services.AddSingleton<NetworkMetricJob>();
			services.AddSingleton<RamMetricJob>();

			services.AddSingleton(new JobSchedule(
				jobType: typeof(CpuMetricJob),
				cronExpression: CronExpression));
			
			services.AddSingleton(new JobSchedule(
				jobType: typeof(DotNetMetricJob),
				cronExpression: CronExpression));
			
			services.AddSingleton(new JobSchedule(
				jobType: typeof(HddMetricJob),
				cronExpression: CronExpression));
			
			services.AddSingleton(new JobSchedule(
				jobType: typeof(NetworkMetricJob),
				cronExpression: CronExpression));
			
			services.AddSingleton(new JobSchedule(
				jobType: typeof(RamMetricJob),
				cronExpression: CronExpression));

			services.AddHostedService<QuartzHostedService>();

		}
    }
}
