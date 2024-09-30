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


        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(string Search)
        {
            var employees = await _unitOfWork.EmployeeRepository
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
                    Department = employee.Department.Name,
                    Image = employee.Image,


                }).ToListAsync();



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


        public async Task<EmployeeDetailsDto?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id);

            if (employee is { })
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


        public async Task<int> CreateEmployeeAsync(CreatedEmployeeDto employeeDto)
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


            if (employeeDto.Image is not null)
                employee.Image = await _attachmentService.UploadFileAsync(employeeDto.Image, "images");



            _unitOfWork.EmployeeRepository.Add(employee);

            return await _unitOfWork.CompleteAsync();

        }

        public async Task<int> UpdateEmployeeAsync(UpdatedEmployeeDto employeeDto)
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
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {


            var employeeRepo = _unitOfWork.EmployeeRepository;


            var employee = await employeeRepo.GetAsync(id);
            if (employee is { })
                employeeRepo.Delete(employee);

            return await _unitOfWork.CompleteAsync() > 0;
        }



    }
}
