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

        public HeroFunctions(ILogger<HeroFunctions> logger, IOptions<MyOptions> myOptions)
        {
            _logger = logger;
            _myOptions = myOptions.Value;
            // More info about the different types of interfaces:
            // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-6.0#options-interfaces
        }

        [Function("GetHeroes")]
        public HttpResponseData GetHeroes([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            // the secret is now easily accessable
            var mySecret = _myOptions.MySecret;
            _logger.LogInformation($"The value of MySecret is: '{mySecret}'.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(heroes);

            return response;
        }

        [Function("ServiceBusFunction")]
        [ServiceBusOutput("lightpeak-queue")]
        public string SendHeroToQueue([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            var message = $"Output message created at {DateTime.Now}";
            return message;
        }
    }
}
