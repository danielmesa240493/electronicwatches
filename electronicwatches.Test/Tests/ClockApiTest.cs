using electronicwatches.Common.Models;
using electronicwatches.Functions.Entities;
using electronicwatches.Functions.Functions;
using electronicwatches.Test.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using Xunit;

namespace electronicwatches.Test.Tests
{
    public class ClockApiTest : TableEntity
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void CreateRegister_Should_Return_200()
        {
            //Arrenge
            MockCloudTableRegisters mockRegisters = new MockCloudTableRegisters(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Clock clockRequest = TestFactory.GetRegisterRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(clockRequest);

            //Act
            IActionResult response = await ClockApi.CreateRegister(request, mockRegisters, logger);

            //Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void UpdateRegister_Should_Return_200()
        {
            //Arrenge
            MockCloudTableRegisters mockRegisters = new MockCloudTableRegisters(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Clock clockRequest = TestFactory.GetRegisterRequest();
            Guid registerId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(registerId, clockRequest);

            //Act
            IActionResult response = await ClockApi.UpdateRegister(request, mockRegisters, registerId.ToString(), logger);

            //Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void GetRegisterById_Should_Return_200()
        {
            //Arrenge
            MockCloudTableRegisters mockRegisters = new MockCloudTableRegisters(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Clock clockRequest = TestFactory.GetRegisterRequest();
            ClockEntity clockEntity = new ClockEntity();
            Guid registerId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(registerId, clockRequest);

            //Act
            IActionResult response = ClockApi.GetRegisterById(request, clockEntity, registerId.ToString(), logger);

            //Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }
    }
}
