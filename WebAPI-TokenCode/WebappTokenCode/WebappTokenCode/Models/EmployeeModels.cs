﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebappTokenCode.Models
{
    public class Employee
    {
        [Key]
        public int EmpNo { get; set; }
        public string EmpName { get; set; }
        public int Salary { get; set; }
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