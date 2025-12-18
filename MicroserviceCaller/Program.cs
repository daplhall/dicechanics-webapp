using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text.Json;
using System.Text.Json.Serialization;

//https://learn.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
class Program
{
    static HttpClient client = new();

    static public void Main(string[] args)
    {
        string program = "print('Hello from python')";
        string return_ = GetAsync($"http://localhost:5032/api/Python?program={program}")
             .GetAwaiter().GetResult();
        Console.WriteLine(return_);
    }

    static async Task<string> GetAsync(string uri)
    {
        using HttpResponseMessage response = await client.GetAsync(uri);
        var taskString = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
        return taskString.Result;
    }
}