using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Runtime.InteropServices;

//https://learn.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
class Program
{
    static readonly HttpClient client = new();

    static public void Main(string[] args)
    {
        string? host = Environment.GetEnvironmentVariable("PYTHONSERIVCE");
        string program = "print('Hello from python')";
        while (true)
        {
            string url = host + $"/api/Python?program={program}";
            string return_ = GetAsync(url)
                 .GetAwaiter().GetResult();
            Console.WriteLine(return_);
            Thread.Sleep(1000);
        }
    }

    static async Task<string> GetAsync(string uri)
    {
        using HttpResponseMessage response = await client.GetAsync(uri);
        var taskString = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        return taskString.Result;
    }
}