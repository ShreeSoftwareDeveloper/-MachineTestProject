using MachineTst.Model;
using MachineTst.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MachineTst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly TestMachineContext _context;
        private readonly List<EmployeeTest> EmployeeTests =new List<EmployeeTest>();

        public EmployeeController(TestMachineContext _context)
        {
            this._context = _context;
        }
        [HttpGet("GetEmployeeAndItsManagerName")]
        public async Task<ActionResult<IEnumerable<EmployeeTest>>>Get()
        {
            //return await _context.EmployeeTests.ToListAsync();
            var employeesWithManagers = _context.EmployeeTests
           .Select(employee => new EmployeeViewModel
           {
               Id = employee.Id,
               Name = employee.Name,
               MangerId=employee.MangerId,
               ManagerName = _context.EmployeeTests
              .Where(manager => manager.Id == employee.MangerId)
              .Select(manager => manager.Name)
              .FirstOrDefault(),               
           }).ToList();
           
           return Ok(employeesWithManagers);


        }

        [HttpGet("GetManagersWithEmployees")]
        public ActionResult<IEnumerable<EmployeeTest>> GetManagersWithEmployees()
        {
            var managersWithEmployees = from emp in _context.EmployeeTests
                                        join manager in _context.EmployeeTests on emp.MangerId equals manager.Id into employeesGroup
                                        from manager in employeesGroup.DefaultIfEmpty()
                                        orderby manager.Id, emp.Id
                                        select new
                                        {
                                            ManagerName = manager != null ? manager.Name : null,
                                            EmployeeName = emp.Name
                                            
                                        };

            var response = managersWithEmployees.GroupBy(x => x.ManagerName)
                                               .Select(group => new
                                               {
                                                   ManagerName = group.Key,
                                                   Employees = group.Select(x => x.EmployeeName).ToList()
                                               })
                                               .ToList();

          return Ok(response);
        }

        [HttpGet("GetManagersWithHisSalary")]
        public ActionResult<IEnumerable<EmployeeViewModel>> GetManagers()
        {
            var managers = _context.EmployeeTests
                .Where(e => e.Id<=3).AsEnumerable() 
        .GroupBy(e => e.MangerId)
        .Select(group => group.First())
                .Select(manager => new EmployeeViewModel
                {
                    //Id=manager.Id,
                    //Name=manager.Name,
                    MangerId = manager.MangerId,
                    ManagerName = manager.Name,                   
                    ManagerSalary = manager.EmployeeSalary
                   
                })
           .ToList();

            return Ok(managers);
        }
        [HttpGet("GetSecondLargestSalaryEmployee")]
        public ActionResult<IEnumerable<EmployeeViewModel>> GetSecondLargestSalaryEmployeeWithCount()
        {
            var employees = from emp in _context.EmployeeTests
                            join manager in _context.EmployeeTests on emp.MangerId equals manager.Id into mg
                            from manager in mg.DefaultIfEmpty()
                            orderby emp.EmployeeSalary descending
                            select new
                             {
                                 Id = emp.Id,
                                 ManagerName = manager.Name,
                                 EmployeeName = emp.Name,
                                 Salary = emp.EmployeeSalary,
                                 ManagerId = emp.MangerId

                             };

            var secondLargestSalaryEmployee = employees.Skip(1).Take(1).FirstOrDefault();


            if (secondLargestSalaryEmployee != null)
            {
               
                var response = new
                {
                    SecondLargestSalaryEmployee = new EmployeeViewModel
                    {
                        Id = secondLargestSalaryEmployee.Id,
                        Name = secondLargestSalaryEmployee.EmployeeName,
                        ManagerName = secondLargestSalaryEmployee.ManagerName,
                        EmployeeSalary = secondLargestSalaryEmployee.Salary,
                        MangerId= secondLargestSalaryEmployee.ManagerId,


                    },
                };

                return Ok(response);
            }

            return NotFound();
        }


    }
}
