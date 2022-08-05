using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PasswordPurgatory.ApiFunction.Services;
using System.IO;
using System.Threading.Tasks;

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

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            password ??= data?.name;

            var beelzebub = new BeelzebubService();

            var checkResult = beelzebub.Infuriate(password);
            
            var responseMessage = checkResult != null 
                ? checkResult.Message 
                : "Congratulations! Take a screenshot of this message and share it to https://www.twitter.com/@l_anceM for the recognition you deserve.";
            
            return new OkObjectResult(responseMessage);
        }
    }
}
