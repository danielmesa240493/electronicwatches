using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace electronicwatches.Functions.Entities
{
    public class ClockEntity : TableEntity
    {
        public DateTime Hour { get; set; }

        public int EmployeeId { get; set; }

        public int Type { get; set; }

        public bool Consolidated { get; set; }
    }
}
