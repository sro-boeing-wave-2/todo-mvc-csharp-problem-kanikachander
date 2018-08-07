using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Text;
using Xunit;
using NotesAPI.Models;

namespace NotesAPI.Tests
{
    public class NotesIntegrationTests
    {
        private HttpClient _client;

        public NotesIntegrationTests()
        {
            var host = new TestServer(new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>()
                );
            _client = host.CreateClient();
        }

        //[Fact]
        //public async void TestGetAll_IT()
        //{
        //    var response = await _client.GetAsync("/api/notes");
        //    var responseBody = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine("ahsgv");
        //    Console.WriteLine(responseBody);
        
    }
}
