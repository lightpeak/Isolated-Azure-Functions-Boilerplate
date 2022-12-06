using System.Net;
using LightPeak.Domain.Models;
using LightPeak.Domain.Settings;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LightPeak.Functions
{
    public class HeroFunctions
    {
        private readonly ILogger _logger;
        private readonly MyOptions _myOptions;

        private List<Hero> heroes = new List<Hero>()
        {
            new Hero("Spiderman"),
            new Hero("Batman"),
            new Hero("Ironman"),
            new Hero("Doctor Strange"),
            new Hero("Thor"),
        };

        public HeroFunctions(ILoggerFactory loggerFactory, IOptions<MyOptions> myOptions)
        {
            _logger = loggerFactory.CreateLogger<HeroFunctions>();
            _myOptions = myOptions.Value;
        }

        [Function("GetHeroes")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var test = _myOptions.MyFirstSecret;

            _logger.LogInformation(test);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(heroes);

            return response;
        }
    }
}
