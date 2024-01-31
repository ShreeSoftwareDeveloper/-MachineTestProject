namespace MachineTst.ViewModel
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? MangerId { get; set; }
        public int EmployeeSalary { get; set; }
        public string ManagerName { get; set; }
        public int ManagerSalary { get; set; }
    }
}
