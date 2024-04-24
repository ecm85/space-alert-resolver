using Amazon.SimpleEmail;

namespace API
{
	public class Startup
	{
		public Startup(IWebHostEnvironment environment, IConfiguration configuration)
		{
			Configuration = configuration;
			Environment = environment;
		}

		public IConfiguration Configuration { get; }
		public IWebHostEnvironment Environment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddDistributedMemoryCache();
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.AddAWSService<IAmazonSimpleEmailService>();
			services.AddTransient<EmailService>();

			services.AddCors(options =>
			{
				options.AddDefaultPolicy(policy =>
				{
					policy
						//.WithOrigins(
						//	"http://localhost:6510",
						//	"http://localhost:5000",
						//	"https://localhost:5001",
						//	"https://space-alert-resolver.stormtide.net",
						//	"https://space-alert.stormtide.net"
						//)
						.AllowAnyOrigin()
						.AllowAnyHeader()
						.WithMethods("GET", "POST");
				});
			});
			services.AddSignalR();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseCors();
			app.UseOptions();
			app.UseMiddleware<NoCacheMiddleware>();
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=SpaceAlert}/{action=Index}/{id?}"
				);
				endpoints.MapHub<GameHub>("hub");
			});
			app.UseSwagger();
			app.UseSwaggerUI();
		}
	}
}
