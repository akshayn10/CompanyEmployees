namespace Entities.Exceptions
{
    public sealed class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(Guid employeeId,Guid companyId) 
            : base($"Employee with id: {employeeId} doesn't exist in the database for the given company Id {companyId}.") { }

    }
}
