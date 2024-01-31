using System;
using System.Collections.Generic;

namespace MachineTst.Model
{
    public partial class EmployeeTest
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? MangerId { get; set; }
        public int EmployeeSalary { get; set; }
    }
}
