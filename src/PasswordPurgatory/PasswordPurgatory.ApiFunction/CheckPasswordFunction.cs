using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PasswordPurgatory.ApiFunction.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace PasswordPurgatory.ApiFunction
{
    public class CheckPasswordFunction
    {
        private readonly TelemetryClient telemetryClient;
        
        public CheckPasswordFunction(TelemetryConfiguration telemetryConfiguration)
        {
            this.telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        [FunctionName("CheckPassword")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing password strength...");

            string password = req.Query["password"];

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            password ??= data?.name;

            if (string.IsNullOrEmpty(password))
                return new BadRequestErrorMessageResult("You must send a parameter value for 'password' in the query.");

            // Initialize the string with a success message... but the user is never going to reach it :D
            var responseMessage = "Congratulations! Take a screenshot of this message and share it to https://www.twitter.com/@l_anceM for the recognition you deserve.";

            log.LogInformation($"Password: {password}");

            var checksPassed = 0;

            try
            {
                // Go through the checks and return the first failure (they get more ridiculous as they get farther into the checks)
                foreach (var check in Check.Checks)
                {
                    check.Password = password;

                    if (check.ValidatePassword())
                    {
                        checksPassed++;
                        continue;
                    }

                    responseMessage = check.Message;

                    var checkNumber = Check.Checks.IndexOf(check) + 1;

                    telemetryClient.TrackEvent("Check Rule Failed", new Dictionary<string, string>
                    {
                        { "Value", password },
                        { "Rule Number", $"{checkNumber}"}
                    });

                    log.LogInformation($"Failed Check #{checkNumber} - {check.Message}");

                    break;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Check Loop Error");

                telemetryClient.TrackException(ex);

                throw;
            }

            telemetryClient.TrackMetric("Check Rules Passed", checksPassed);

            return new OkObjectResult(responseMessage);
        }
    }
}
