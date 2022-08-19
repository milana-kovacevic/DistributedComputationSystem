using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ClientApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string baseUrl = "https//localhost:/";
            var client = new DistributedCalculationClient(baseUrl, new HttpClient());

            var jobs = client.JobsAllAsync().GetAwaiter().GetResult();

            Console.WriteLine("Jobs:");
            foreach (var job in jobs)
            {
                Console.WriteLine(job.ToString());
            }

            // TODO AAD integration

        }
    }
}