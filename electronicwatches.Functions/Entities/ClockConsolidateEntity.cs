using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace electronicwatches.Functions.Entities
{
    internal class ClockConsolidateEntity : TableEntity
    {
        public int EmployeeId { get; set; }

        public DateTime Hour { get; set; }

        public int MinutesWorked { get; set; }
    }
}
