using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace ResourcesServer.Models
{
    /// <summary>
    /// Class Representative of an Employee
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Emp Number - Identifier Attribute
        /// </summary>        
        [Key]
        public int EmpNo { get; set; }
        /// <summary>
        /// Emp Name - Full Name of Employee
        /// </summary>
        public string EmpName { get; set; }
        /// <summary>
        /// Salary - His Salary
        /// </summary>
        public int Salary { get; set; }
        /// <summary>
        /// Dept Name - Department Name
        /// </summary>
        public string DeptName { get; set; }
    }

    public class EmployeesContext : DbContext
    {
        public EmployeesContext()
                : base("name=DefaultConnection")
        {
        }
        public DbSet<Employee> Employees { get; set; }
    }
}