using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

using NetUtils;

namespace TesterFrm {
	static class Program {
		[STAThread]
		static void Main() {
			Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(new ConfigurationBuilder()
		.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
		.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		.Build())
	.CreateLogger();
			Log.Logger.Error("this");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			using (var host = CreateHostBuilder().Build()) {
				host.Start();
				var form = host.Services.GetRequiredService<MainForm>();
				Application.Run(form);
				Log.CloseAndFlush();
				Log.Information("Helllo.........");
				Log.Logger.Debug("Hi");
			}
		}
		static IHostBuilder CreateHostBuilder() =>
				Host.CreateDefaultBuilder()
					.ConfigureAppConfiguration((context, config) => {
						config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
							  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
					})
				.UseSerilog((context, services, loggerConfig) => {
					loggerConfig.ReadFrom.Configuration(context.Configuration);
				})
					.ConfigureServices((context, services) => {
						services.Configure<SalesforceConfig>(context.Configuration.GetSection("Salesforce"));
						services.AddMemoryCache(); // For IMemoryCache
						services.AddScoped<ISalesforceService, SalesforceService>();
						services.AddScoped<PubSubService>(); // Register PubSubService	
						services.AddHttpClient();
						services.AddScoped<MainForm>(); // Register the form
					});

	}
}