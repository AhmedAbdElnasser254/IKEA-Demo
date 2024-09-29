using LinkDev.IKEA.BLL.Common.Attachments;
using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.DAL.Models.Employees;
using LinkDev.IKEA.DAL.Persistance.Repositories.Employees;
using LinkDev.IKEA.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        public EmployeeService(
            IUnitOfWork unitOfWork,
            IAttachmentService attachmentService
            )
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }


        public IEnumerable<EmployeeDto> GetAllEmployees(string Search)
        {
            var employees = _unitOfWork.EmployeeRepository
                .GetAllAsIQueryable()
                .Where(E => !E.IsDeleted && (string.IsNullOrEmpty(Search) || E.Name.ToLower().Contains(Search.ToLower())))
                .Include(E => E.Department)
                .Select(employee => new EmployeeDto()
                {

                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Address = employee.Address,
                    IsActive = employee.IsActive,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Gender = nameof(employee.Gender),
                    EmployeeType = nameof(employee.EmployeeType),
                    Department = employee.Department.Name


                }).ToList();



            return employees;

        }

        //public EmployeeDetailsDto? GetEmployeeById(int id)
        //{
        //    var employee = _unitOfWork.EmployeeRepository.Get(id);

        //    if (employee != null)
        //        return new EmployeeDetailsDto()
        //        {
        //            Id = employee.Id,
        //            Name = employee.Name,
        //            Age = employee.Age,
        //            Address = employee.Address,
        //            IsActive = employee.IsActive,
        //            Email = employee.Email,
        //            Salary = employee.Salary,
        //            Gender = employee.Gender,
        //            EmployeeType = employee.EmployeeType,
        //            Department = employee.Department.Name,
        //        };

        //    return null;
        //}


        public EmployeeDetailsDto? GetEmployeeById(int id)
        {
            var employee = _unitOfWork.EmployeeRepository.Get(id);

            if (employee != null)
            {
                return new EmployeeDetailsDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Address = employee.Address,
                    IsActive = employee.IsActive,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Gender = employee.Gender,
                    EmployeeType = employee.EmployeeType,
                    Department = employee.Department?.Name ?? "Unknown", // Safeguard if Department is null
                };
            }

            return null; // Employee not found
        }


        public int CreateEmployee(CreatedEmployeeDto employeeDto)
        {

            var employee = new Employee()
            {
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive = employeeDto.IsActive,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                Salary = employeeDto.Salary,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId = employeeDto.DepartmentId,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
            };


            if(employeeDto.Image is not null)
           employee.Image = _attachmentService.Upload(employeeDto.Image, "images");



            _unitOfWork.EmployeeRepository.Add(employee);

            return _unitOfWork.Complete();

        }

        public int UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {
            var employee = new Employee()
            {
                Id = employeeDto.Id,
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive = employeeDto.IsActive,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                Salary = employeeDto.Salary,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId = employeeDto.DepartmentId,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
            };
            _unitOfWork.EmployeeRepository.Update(employee);
            return _unitOfWork.Complete();
        }

        public bool DeleteEmployee(int id)
        {


            var employeeRepo = _unitOfWork.EmployeeRepository;


            var employee = employeeRepo.Get(id);
            if (employee is { })
                employeeRepo.Delete(employee);

            return _unitOfWork.Complete() > 0;
        }



    }
}
