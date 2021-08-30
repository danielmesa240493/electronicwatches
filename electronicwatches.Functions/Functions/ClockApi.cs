using electronicwatches.Common.Models;
using electronicwatches.Common.Responses;
using electronicwatches.Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace electronicwatches.Functions.Functions
{
    public static class ClockApi
    {
        [FunctionName(nameof(CreateRegister))]
        public static async Task<IActionResult> CreateRegister(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "register")] HttpRequest req,
            [Table("register", Connection = "AzureWebJobsStorage")] CloudTable registerTable,
            ILogger log)
        {
            log.LogInformation("Recieved a new register.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Clock clock = JsonConvert.DeserializeObject<Clock>(requestBody);

            if ((clock?.EmployeeId == null) || (clock?.Hour == null) || (clock?.Type == null))
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "You must meet all the requirements."
                });
            }

            ClockEntity clockEntity = new ClockEntity
            {
                Hour = clock.Hour,
                ETag = "*",
                Consolidated = false,
                PartitionKey = "REGISTER",
                RowKey = Guid.NewGuid().ToString(),
                EmployeeId = clock.EmployeeId,
                Type = clock.Type,
                Timestamp = DateTime.UtcNow
            };

            TableOperation addOperation = TableOperation.Insert(clockEntity);
            await registerTable.ExecuteAsync(addOperation);

            string message = "New register stored in table.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = clockEntity
            });
        }
    }
}
