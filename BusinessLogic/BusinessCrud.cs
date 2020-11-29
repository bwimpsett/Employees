using AutoMapper;
using BusinessLogic.Models;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Repos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class BusinessCrud
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>());
        private readonly MapperConfiguration revConfig = new MapperConfiguration(cfg =>
            cfg.CreateMap<EmployeeDTO, Employee>().ForMember(x => x.ID, opt => opt.Ignore()));

        public BusinessCrud()
        {
            _unitOfWork = new UnitOfWork(new EmployeeEntities());
        }
        public BusinessCrud(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(EmployeeDTO emp)
        {
            var objEmployees = _unitOfWork.EmployeeRepo.Search(x => 
                x.Name == emp.Name &&
                x.Address == emp.Address &&
                x.Employment_Date == emp.Employment_Date &&
                x.Telephone == emp.Telephone &&
                x.Type == emp.Type);
            if (objEmployees.Count() > 0) throw new ArgumentException("Employee already exists!");

            var mapper = new Mapper(revConfig);
            var crVal = mapper.Map<Employee>(emp);

            _unitOfWork.EmployeeRepo.Insert(crVal);
            _unitOfWork.Save();
        }

        public IEnumerable<EmployeeDTO> Retrive()
        {
            var mapper = new Mapper(config);
            return mapper.Map<List<Employee>, List<EmployeeDTO>>(_unitOfWork.EmployeeRepo.GetAll().ToList());
        }

        public EmployeeDTO RetriveById(int id)
        {
            var emp = _unitOfWork.EmployeeRepo.GetById(id);
            var mapper = new Mapper(config);
            return mapper.Map<EmployeeDTO>(emp);
        }

        public IEnumerable<EmployeeDTO> Search(EmployeeDTO employeeDTO)
        {
            var results = _unitOfWork.EmployeeRepo.Search(x =>
                (!string.IsNullOrEmpty(employeeDTO.Name) && x.Name.Contains(employeeDTO.Name)) ||
                (!string.IsNullOrEmpty(employeeDTO.Address) && x.Address.Contains(employeeDTO.Address)) ||
                (x.Employment_Date == employeeDTO.Employment_Date) ||
                (!string.IsNullOrEmpty(employeeDTO.Telephone) && x.Telephone.Contains(employeeDTO.Telephone)) ||
                (x.Type != 0 && x.Type == employeeDTO.Type));
            var mapper = new Mapper(config);
            return mapper.Map<List<Employee>, List<EmployeeDTO>>(results.ToList());
        }

        public void Update(EmployeeDTO empDTO)
        {
            var objEmployees = _unitOfWork.EmployeeRepo.GetById(empDTO.ID);

            if (objEmployees == null) throw new ArgumentException("Employee not found!");
            objEmployees.Name = empDTO.Name;
            objEmployees.Address = empDTO.Address;
            objEmployees.Telephone = empDTO.Telephone;
            objEmployees.Type = empDTO.Type;
            objEmployees.Employment_Date = empDTO.Employment_Date;
            _unitOfWork.EmployeeRepo.Update(objEmployees);
            _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            var employeeItem = _unitOfWork.EmployeeRepo.GetById(id);
            if (employeeItem == null) throw new ArgumentException("Employee not found!");
            _unitOfWork.EmployeeRepo.Delete(id);
            _unitOfWork.Save();
        }
    }
}
