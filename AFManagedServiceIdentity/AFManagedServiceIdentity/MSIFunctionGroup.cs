using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Azure.Services.AppAuthentication;

namespace AFManagedServiceIdentity
{
    public static class MSIFunctionGroup
    {
        [FunctionName("HelloFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("HttpTriggerWithMSI")]
        public static async Task<IActionResult> HttpTriggerWithMSIRun(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string firstname = string.Empty,
              lastname = string.Empty, email = string.Empty, devicelist = string.Empty;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            firstname = firstname ?? data?.firstname;
            lastname = lastname ?? data?.lastname;
            email = email ?? data?.email;
            devicelist = devicelist ?? data?.devicelist;

            log.LogInformation("Connecting to Azure SQL Database for inserting data.");
            SqlConnection con = null;
            try
            {
                string query = "INSERT INTO EmployeeInfo (firstname,lastname, email, devicelist) " + "VALUES (@firstname,@lastname, @email, @devicelist) ";

                //Connection string without credentials
                con = new SqlConnection("Server=tcp:YOURSERVER.database.windows.net,1433;Initial Catalog=YOURDATABASE;Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                SqlCommand cmd = new SqlCommand(query, con);

                //Retrieving the access token
                con.AccessToken = (new AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/").Result;

                cmd.Parameters.Add("@firstname", SqlDbType.VarChar, 50).Value = firstname;

                cmd.Parameters.Add("@lastname", SqlDbType.VarChar, 50).Value = lastname;
                cmd.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = email;
                cmd.Parameters.Add("@devicelist", SqlDbType.VarChar).Value = devicelist; 
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            return new OkObjectResult("Data successfully inserted.");

        }

    }
}
