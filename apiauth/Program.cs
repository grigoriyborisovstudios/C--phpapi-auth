using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Program
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly string baseUrl = "https://cdn.hdanime.org/api/authapi.php/login";

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Would you like to (1) Register or (2) Login?");
        string choice = Console.ReadLine();

        Console.WriteLine("Please enter your username:");
        string username = Console.ReadLine();

        Console.WriteLine("Please enter your password:");
        string password = Console.ReadLine();

        if (choice == "1")
        {
            await Register(username, password);
        }
        else if (choice == "2")
        {
            await Login(username, password);
        }
        else
        {
            Console.WriteLine("Invalid option selected");
        }

        Console.ReadKey();
    }

    static async Task Register(string username, string password)
    {
        var requestData = new { username = username, password = password };
        var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(baseUrl + "/register", content);

        string responseStr = await response.Content.ReadAsStringAsync();
        dynamic responseObj = JsonConvert.DeserializeObject(responseStr);

        if (responseObj != null && responseObj.success == true)
        {
            Console.WriteLine("Registration successful.");
        }
        else
        {
            Console.WriteLine("Registration failed. Response: " + responseStr);
        }
    }

    static async Task Login(string username, string password)
    {
        var requestData = new { username = username, password = password };
        var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(baseUrl + "/login", content);

        string responseStr = await response.Content.ReadAsStringAsync();
        dynamic responseObj = JsonConvert.DeserializeObject(responseStr);

        if (responseObj != null && responseObj.success == true)
        {
            Console.WriteLine("Login successful.");
        }
        else
        {
            Console.WriteLine("Login failed. Response: " + responseStr);
        }
    }
}