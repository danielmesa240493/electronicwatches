using System;

namespace electronicwatches.Common.Models
{
    public class Clock
    {
        public DateTime Hour { get; set; }

        public int EmployeeId { get; set; }

        public int Type { get; set; }

        public bool Consolidated { get; set; }
    }
}
