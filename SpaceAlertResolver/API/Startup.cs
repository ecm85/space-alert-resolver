namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDistributedMemoryCache();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("https://space-alert-resolver.stormtide.net");
                    });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseOptions();
            app.UseMiddleware<NoCacheMiddleware>();
            app.UseRouting();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=SpaceAlert}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
