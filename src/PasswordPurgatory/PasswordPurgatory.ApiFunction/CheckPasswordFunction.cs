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
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace PasswordPurgatory.ApiFunction
{
    public static class CheckPasswordFunction
    {
        [FunctionName("CheckPassword")]
        public static async Task<IActionResult> Run(
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

            try
            {
                // Go through the checks and return the first failure (they get more ridiculous as they get farther into the checks)
                foreach (var check in Check.Checks)
                {
                    check.Password = password;
                    
                    if (check.ValidatePassword())
                        continue;

                    responseMessage = check.Message;

                    log.LogInformation($"Failed Check #{Check.Checks.IndexOf(check)} - {check.Message}");

                    break;
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Check Loop Error");
                throw;
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
