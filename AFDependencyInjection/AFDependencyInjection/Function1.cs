using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AFDependencyInjection
{
    //Standard template
    //public static class Function1
    //{
    //    [FunctionName("Function1")]
    //    public static async Task<IActionResult> Run(
    //        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
    //        ILogger log)
    //    {
    //        log.LogInformation("C# HTTP trigger function processed a request.");

    //        string name = req.Query["name"];

    //        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    //        dynamic data = JsonConvert.DeserializeObject(requestBody);
    //        name = name ?? data?.name;

    //        string responseMessage = string.IsNullOrEmpty(name)
    //            ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
    //            : $"Hello, {name}. This HTTP triggered function executed successfully.";

    //        return new OkObjectResult(responseMessage);
    //    }
    //}

    //New template with dependency injection
    public class Function1
    {
        private readonly IEnumerable<IAlertService> _alertServices;

        public Function1(IEnumerable<IAlertService> alertServices)
        {
            _alertServices = alertServices;
        }

        [FunctionName("Function1")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string message = req.Query["message"];

            var customer = new Customer { Name = "Stefano", Email = "stefano@demiliani.com" };
            var title = "Alert";
            var body = "ALERT: " + message;

            foreach (var alertService in _alertServices)
            {
                alertService.SendAlert(customer, title, body);
            }

            return await Task.FromResult(new OkObjectResult("OK"));
        }
    }

}
