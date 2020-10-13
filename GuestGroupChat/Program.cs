using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;


namespace GuestGroupChat
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls("http://*:" + "8080")
                .Build();

            host.Run();
        }
    }
}