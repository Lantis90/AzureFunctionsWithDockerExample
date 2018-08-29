
using System;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Demo
{
    public static class HttpTriggerDemo
    {
        [FunctionName("HttpTriggerDemo")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            string sYear = req.Query["year"];
            string sMonth = req.Query["month"];
            string sDay = req.Query["day"];

            DateTime date;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            try
            {
                date = DateTime.Parse(sMonth + "/" + sDay + "/" + sYear, culture);
            }
            catch(Exception ex)
            {
                log.LogInformation(ex.Message);
                return new BadRequestObjectResult("Please enter a valid date");
            }

            return name != null
                ? (ActionResult)checkBirthDate(name, date)
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }

        private static OkObjectResult checkBirthDate(string name, DateTime date)
        {
            string message = String.Format("Hello {0}, \n You were born on a {1} and have been alive for {2} days. Good job!", name,
                                        date.DayOfWeek, (DateTime.Now - date).Days);
            return new OkObjectResult(message);
        }
    }
}
