using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Program
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly string baseUrl = "http://your-api-url";

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Would you like to (1) Register or (2) Login?");
        string choice = Console.ReadLine();

        Console.WriteLine("Please enter your username:");
        string username = Console.ReadLine();

        Console.WriteLine("Please enter your password:");
        SecureString password = GetPasswordFromConsole();

        if (choice == "1")
        {
            await Register(username, password);
        }
        else if (choice == "2")
        {
            await Login(username, password); //2
        }
        else
        {
            Console.WriteLine("Invalid option selected");
        }

        Console.ReadKey();
    }

    public static SecureString GetPasswordFromConsole()
    {
        var password = new SecureString();
        while (true)
        {
            ConsoleKeyInfo i = Console.ReadKey(true);
            if (i.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (i.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password.RemoveAt(password.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else
            {
                password.AppendChar(i.KeyChar);
                Console.Write("*");
            }
        }
        return password;
    }

    static async Task Register(string username, SecureString password)
    {
        // Unsecured password conversion. This is insecure and used just for illustration.
        var passwordBstr = Marshal.SecureStringToBSTR(password);
        var insecurePassword = Marshal.PtrToStringBSTR(passwordBstr);
        Marshal.ZeroFreeBSTR(passwordBstr);

        var requestData = new { username = username, password = insecurePassword };
        var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

        try
        {
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
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    static async Task Login(string username, SecureString password)
    {
        // Unsecured password conversion. This is insecure and used just for illustration.
        var passwordBstr = Marshal.SecureStringToBSTR(password);
        var insecurePassword = Marshal.PtrToStringBSTR(passwordBstr);
        Marshal.ZeroFreeBSTR(passwordBstr);

        var requestData = new { username = username, password = insecurePassword };
        var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

        try
        {
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
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}