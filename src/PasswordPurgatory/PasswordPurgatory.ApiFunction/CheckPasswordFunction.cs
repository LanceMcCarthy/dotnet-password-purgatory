using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PasswordPurgatory.ApiFunction.Services;
using System.IO;
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
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            string password = req.Query["password"];
            
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            password ??= data?.name;

            if (string.IsNullOrEmpty(password))
                return new BadRequestErrorMessageResult("You must send a parameter value for 'password' in the query.");

            var beelzebub = new BeelzebubService(password);

            // Go through all the rules, from lowest to most infuriating.
            var checkResult = beelzebub.Infuriate();
            
            var responseMessage = checkResult != null 
                ? checkResult.Message 
                : "Congratulations! Take a screenshot of this message and share it to https://www.twitter.com/@l_anceM for the recognition you deserve.";
            
            return new OkObjectResult(responseMessage);
        }
    }
}
