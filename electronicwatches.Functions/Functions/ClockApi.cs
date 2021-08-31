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

        [FunctionName(nameof(UpdateRegister))]
        public static async Task<IActionResult> UpdateRegister(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "register/{id}")] HttpRequest req,
            [Table("register", Connection = "AzureWebJobsStorage")] CloudTable registerTable,
            string id,
            ILogger log)
        {
            log.LogInformation($"Update for register: {id}, received.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Clock clock = JsonConvert.DeserializeObject<Clock>(requestBody);

            //Validate register id
            TableOperation findOperation = TableOperation.Retrieve<ClockEntity>("REGISTER", id);
            TableResult findResult = await registerTable.ExecuteAsync(findOperation);
            if (findResult.Result == null)
            {
                return new BadRequestObjectResult(new Response
                {
                    IsSuccess = false,
                    Message = "Register not found."
                });
            }

            //Update register
            ClockEntity clockEntity = (ClockEntity)findResult.Result;
            clockEntity.Consolidated = clock.Consolidated;
            if (!string.IsNullOrEmpty(clock.EmployeeId.ToString()) || !string.IsNullOrEmpty(clock.Hour.ToString()) || !string.IsNullOrEmpty(clock.Type.ToString()))
            {
                clockEntity.EmployeeId = clock.EmployeeId;
                clockEntity.Hour = clock.Hour;
                clockEntity.Type = clock.Type;
            }

            TableOperation addOperation = TableOperation.Replace(clockEntity);
            await registerTable.ExecuteAsync(addOperation);

            string message = $"Register: {id}, updated in table.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = clockEntity
            });
        }

        [FunctionName(nameof(GetAllRegisters))]
        public static async Task<IActionResult> GetAllRegisters(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "register")] HttpRequest req,
            [Table("register", Connection = "AzureWebJobsStorage")] CloudTable registerTable,
            ILogger log)
        {
            log.LogInformation("Get all registers received.");

            TableQuery<ClockEntity> query = new TableQuery<ClockEntity>();
            TableQuerySegment<ClockEntity> registers = await registerTable.ExecuteQuerySegmentedAsync(query, null);

            string message = "Retrieved all registers.";
            log.LogInformation(message);

            return new OkObjectResult(new Response
            {
                IsSuccess = true,
                Message = message,
                Result = registers
            });
        }
    }
}
