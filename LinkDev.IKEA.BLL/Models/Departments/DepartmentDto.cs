﻿ using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Models.Departments
{
    public class DepartmentDto
    {
        public int Id { get; set; } 
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        

        [Display(Name = "Date Of Creation")]
        public DateOnly CreationDate { get; set; }
    }
}