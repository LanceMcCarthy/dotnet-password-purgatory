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

            log.LogInformation("Processing password strength...");

            // List of password checks the increase in ridiculousness and complexity
            var checks = new List<Check>
            {
                new()
                {
                    PasswordIsValid = !new Regex(@"/\d+/").IsMatch(password),
                    Message = "Password must contain at least 1 number",
                    InfuriationLevel = InfuriationLevel.Low
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/!$/").IsMatch(password),
                    Message = "Password must not end in '!'",
                    InfuriationLevel = InfuriationLevel.Low
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/[A-Z]/").IsMatch(password),
                    Message = "Password must contain at least 1 lowercase character",
                    InfuriationLevel = InfuriationLevel.Low
                },
                new()
                {
                    PasswordIsValid = password.Length < 8,
                    Message = "Password must be at least 8 characters long",
                    InfuriationLevel = InfuriationLevel.Low
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/1$/").IsMatch(password),
                    Message = "Password must not end in '1'",
                    InfuriationLevel = InfuriationLevel.Moderate
                },
                new()
                {
                    PasswordIsValid = password.Length > 20,
                    Message = $"Password must not be {password.Length} characters long'",
                    InfuriationLevel = InfuriationLevel.Moderate
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/dog$/").IsMatch(password),
                    Message = "Password must end with dog",
                    InfuriationLevel = InfuriationLevel.Moderate
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/^cat/").IsMatch(password),
                    Message = "Password must start with cat",
                    InfuriationLevel = InfuriationLevel.Moderate
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/Homer|Marge|Bart|Lisa|Maggie/").IsMatch(password),
                    Message = "Password must contain at least 1 primary Simpsons family character",
                    InfuriationLevel = InfuriationLevel.High
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/Peter|Lois|Chris|Meg|Brian|Stewie/").IsMatch(password),
                    Message = "Password must contain at least 1 primary Griffin family character",
                    InfuriationLevel = InfuriationLevel.High
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/:‑\)|:\)|:\-\]|:\]|:>|:\-\}|:\}|:o\)\)|:\^\)|=\]|=\)|:\]|:\->|:>|8\-\)|:\-\}|:\}|:o\)|:\^\)|=\]|=\)|:‑D|:D|B\^D|:‑\(|:\(|:‑<|:<|:‑\[|:\[|:\-\|\||>:\[|:\{|:\(|;\(|:\'‑\(|:\'\(|:=\(|:\'‑\)|:\'\)|:‑O|:O|:‑o|:o|:\-0|>:O|>:3|;‑\)|;\)|;‑\]|;\^\)|:‑P|:\-\/|:\/|:‑\.|>:|>:\/|:|:‑\||:\||>:‑\)|>:\)|\}:‑\)|>;‑\)|>;\)|>:3|\|;‑\)|:‑J|<:‑\||~:>/,").IsMatch(password),
                    Message = "Password must contain at least one emoticon",
                    InfuriationLevel = InfuriationLevel.High
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/bobcat|Lynx rufus|L. rufus/").IsMatch(password),
                    Message = "Password must contain a bobcat",
                    InfuriationLevel = InfuriationLevel.High
                },
                new()
                {
                    PasswordIsValid = !IsDivisibleByThree(password, log),
                    Message = "Password when stripped of non-numeric characters must be a number divisible by 3",
                    InfuriationLevel = InfuriationLevel.Ridiculous
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/[ÄÜÖ\u1e9e]/").IsMatch(password),
                    Message = "Password must contain at least one upper case German Umlaut",
                    InfuriationLevel = InfuriationLevel.Ridiculous
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/Luna|Deimos|Phobos|Amalthea|Callisto|Europa|Ganymede|Io|Dione|Enceladus|Hyperion|Iapetus|Mimas|Phoebe|Rhea|Tethys|Titan|Ariel|Miranda|Oberon|Titania|Umbriel|Nereid|Triton|Charon|Himalia|Carme|Ananke|Adrastea|Elara|Adrastea|Elara|Epimetheus|Callirrhoe|Kalyke|Thebe|Methone|Kiviuq|Ijiraq|Paaliaq|Albiorix|Erriapus|Pallene|Polydeuces|Bestla|Daphnis|Despina|Puck|Carpo|Pasiphae|Themisto|Cyllene|Isonoe|Harpalyke|Hermippe|Iocaste|Chaldene|Euporie/").IsMatch(password),
                    Message = "Password must contain at least one named solarian planetary satellite",
                    InfuriationLevel = InfuriationLevel.Ridiculous
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/(?:[^1234569]*[1234569]){3}[^1234569]*/").IsMatch(password),
                    Message = "Password must contain at least 3 digits from the first 10 decimal places of pi",
                    InfuriationLevel = InfuriationLevel.Ridiculous
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/(?=^[A-Za-z0-9]+$)(.)+.*\1.*/").IsMatch(password),
                    Message = "Password must contain only unique characters.",
                    InfuriationLevel = InfuriationLevel.Ridiculous
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/Password\smust\scontain/").IsMatch(password),
                    Message = "Password must contain 'Password must contain'",
                    InfuriationLevel = InfuriationLevel.Ridiculous
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/[ÅåÄäÖöÆæØø]/").IsMatch(password),
                    Message = "Password must contain at least 1 Nordic character",
                    InfuriationLevel = InfuriationLevel.Ridiculous
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/[\u0370-\u03ff\u1f00-\u1fff]/").IsMatch(password),
                    Message = "Password must contain at least 1 Greek character",
                    InfuriationLevel = InfuriationLevel.Ridiculous
                },
                new()
                {
                    PasswordIsValid = !new Regex(@"/(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])/").IsMatch(password),
                    Message = "Password must be a palindrome.",
                    InfuriationLevel = InfuriationLevel.Ridiculous
                }
            };

            // Initialize the string with a success message... but the user is never going to reach it :D
            var responseMessage = "Congratulations! Take a screenshot of this message and share it to https://www.twitter.com/@l_anceM for the recognition you deserve.";

            log.LogInformation($"Password: {password}");

            try
            {
                // Go through the checks and return the first failure (they get more ridiculous as they get farther into the checks
                foreach (var check in checks.Where(check => !check.PasswordIsValid))
                {
                    responseMessage = check.Message;

                    log.LogInformation($"Check #{checks.IndexOf(check)} - {check.Message}");

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


        private static bool IsDivisibleByThree(string pwd, ILogger log)
        {
            try
            {
                var numbersOnly = new string(pwd.Where(char.IsDigit).ToArray());
            
                return numbersOnly.Sum(Convert.ToInt32) % 3 == 0;
            }
            catch (Exception ex)
            {
                log.LogError(ex,"DivideByThreeError");

                // Just in case there's a problem with my logic/math, I don't want to ruin the fun... so lets keep moving on.
                return true;
            }
        }
    }
}
