using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace PL
{
    public class LocalEntryPoint
    {
        public static void Main() =>
            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build()
                .Run();
    }
}
