using Microsoft.AspNetCore.Mvc;

namespace PwC.Case.Backend.Controllers;

public class NinjaApiDTO
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

[ApiController]
[Route("[controller]")]
public class CityDistanceController : ControllerBase
{
    private readonly ILogger<CityDistanceController> _logger;
    private readonly IConfiguration _config;

    public CityDistanceController(ILogger<CityDistanceController> logger, IConfiguration config) : base()
    {
        _logger = logger;
        _config = config;
    }

    [HttpGet(Name = "GetCityDistance")]
    public async Task<CityDistanceDTO> Get([FromQuery] string city1, [FromQuery] string city2)
    {
        var httpClient = new HttpClient();

        var apiKey = _config.GetValue<string>("NinjaApi.ApiKey");

        httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

        var latlongCity1 = await httpClient.GetFromJsonAsync<List<NinjaApiDTO>>($"https://api.api-ninjas.com/v1/city?name={city1}");
        var latlongCity2 = await httpClient.GetFromJsonAsync<List<NinjaApiDTO>>($"https://api.api-ninjas.com/v1/city?name={city2}");

        return new CityDistanceDTO()
        {
            City1 = city1,
            City2 = city2,
            Distance = Distance(
                latlongCity1!.First().Latitude,
                latlongCity2!.First().Latitude,
                latlongCity1!.First().Longitude,
                latlongCity2!.First().Longitude
            )
        };
    }

    public static double ToRadians(
           double angleIn10thofaDegree)
    {
        // Angle in 10th
        // of a degree
        return (angleIn10thofaDegree * 
                       Math.PI) / 180;
    }
    public static double Distance(double lat1,
                           double lat2,
                           double lon1,
                           double lon2)
    {
 
        // The math module contains
        // a function named toRadians
        // which converts from degrees
        // to radians.
        lon1 = ToRadians(lon1);
        lon2 = ToRadians(lon2);
        lat1 = ToRadians(lat1);
        lat2 = ToRadians(lat2);
 
        // Haversine formula
        double dlon = lon2 - lon1;
        double dlat = lat2 - lat1;
        double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Pow(Math.Sin(dlon / 2),2);
             
        double c = 2 * Math.Asin(Math.Sqrt(a));
 
        // Radius of earth in
        // kilometers. Use 3956
        // for miles
        double r = 6371;
 
        // calculate the result
        return (c * r);
    }
}
