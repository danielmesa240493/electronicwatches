using electronicwatches.Common.Models;
using electronicwatches.Functions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace electronicwatches.Test.Helpers
{
    public class TestFactory
    {
        public static ClockEntity GetClockEntity()
        {
            return new ClockEntity
            {
                ETag = "*",
                PartitionKey = "REGISTER",
                RowKey = Guid.NewGuid().ToString(),
                Hour = DateTime.UtcNow,
                Consolidated = false,
                EmployeeId = 1,
                Type = 0,
                Timestamp = DateTime.UtcNow
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid registerId, Clock clockRequest)
        {
            string request = JsonConvert.SerializeObject(clockRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request),
                Path = $"/{registerId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Guid registerId)
        {
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Path = $"/{registerId}"
            };
        }

        public static DefaultHttpRequest CreateHttpRequest(Clock clockRequest)
        {
            string request = JsonConvert.SerializeObject(clockRequest);
            return new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = GenerateStreamFromString(request)
            };
        }

        public static DefaultHttpRequest CreateHttpRequest()
        {
            return new DefaultHttpRequest(new DefaultHttpContext());
        }

        public static Clock GetRegisterRequest()
        {
            return new Clock
            {
                Hour = DateTime.UtcNow,
                Consolidated = false,
                EmployeeId = 1,
                Type = 0,                
            };
        }

        public static Stream GenerateStreamFromString(string stringToConvert)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;
            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}
