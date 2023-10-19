using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration.Json;
using OpenWeatherMapAPI;
using Newtonsoft.Json.Linq;

var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

var services = new ServiceCollection()
   .AddOptions()
   .Configure<WeatherMapApi>(config.GetSection("WeatherMapApi"))
   .BuildServiceProvider();

var apiSettings = services.GetService<IOptions<WeatherMapApi>>();

var _apiKey = apiSettings.Value.ApiKey;

var client = new HttpClient();

while (true)
{
    try
    {
        Console.WriteLine();
        Console.WriteLine("Please enter your city name:");

        var cityName = Console.ReadLine();

        var weatherURL = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&units=imperial&appid={_apiKey}";

        var weatherResponse = client.GetStringAsync(weatherURL).Result;

        var formattedResponse = JObject.Parse(weatherResponse).GetValue("main").ToString();

        Console.WriteLine("----");

        Console.WriteLine($"{formattedResponse}");

        Console.WriteLine("Would you like to choose a different city? yes or no");
        var userAnswer = Console.ReadLine();

        if (userAnswer.ToLower() == "no")
        {
            Console.WriteLine("Goodbye then!");
            break;
        }
    }

    catch (Exception e)
    {
        Console.WriteLine($"Hmm, that doesn't look right. Try again.");
    }

}