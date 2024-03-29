namespace ProjectFinalEngineer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls("https://localhost:5001/")
                    .UseStartup<Startup>();
                });
    }
}