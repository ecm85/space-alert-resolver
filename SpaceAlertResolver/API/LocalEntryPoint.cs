using Microsoft.AspNetCore;

namespace API
{
	public class LocalEntryPoint
	{
		public static void Main() =>
			WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build().Run();
	}
}
