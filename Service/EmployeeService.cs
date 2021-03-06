using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
	{
		private readonly IRepositoryManager _repository;
		private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger,IMapper mapper)
		{
			_repository = repository;
			_logger = logger;
            _mapper = mapper;
        }

        public EmployeeDTO GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
			var company = _repository.Company.GetCompany(companyId, trackChanges);
			if (company is null)
				throw new CompanyNotFoundException(companyId);

			var employeeDb = _repository.Employee.GetEmployee(companyId, id, trackChanges);
			if (employeeDb is null)
				throw new EmployeeNotFoundException(id,companyId);

			var employee = _mapper.Map<EmployeeDTO>(employeeDb);
			return employee;
		}

        public EmployeeDTO CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDTO employeeForCreation, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            _repository.Save();

            var employeeToReturn = _mapper.Map<EmployeeDTO>(employeeEntity);

            return employeeToReturn;
		}

        public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges); 
            if (company is null) throw new CompanyNotFoundException(companyId);
            var employeeForCompany = _repository.Employee.GetEmployee(companyId, id, trackChanges);
            if (employeeForCompany is null) throw new EmployeeNotFoundException(id,companyId);
            _repository.Employee.DeleteEmployee(employeeForCompany); _repository.Save();
        }

        public IEnumerable<EmployeeDTO> GetEmployees(Guid companyId, bool trackChanges)
        {
			var company = _repository.Company.GetCompany(companyId, trackChanges);
			if (company is null)
				throw new CompanyNotFoundException(companyId);

			var employeesFromDb = _repository.Employee.GetEmployees(companyId, trackChanges);
			var employeesDTO = _mapper.Map<IEnumerable<EmployeeDTO>>(employeesFromDb);

			return employeesDTO;
		}
    }
}
