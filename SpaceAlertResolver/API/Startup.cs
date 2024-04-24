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
					policy.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET", "POST");
				});
			});
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
			});
			app.UseSwagger();
			app.UseSwaggerUI();
		}
	}
}
