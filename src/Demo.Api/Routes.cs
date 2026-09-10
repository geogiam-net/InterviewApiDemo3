namespace Demo.Api;

public static class Routes
{
    public const string Employees = "/api/employees";
    public const string EmployeeById = "/api/employees/{id}";

    public const string Shifts = "/api/shifts";
    public const string ShiftById = "/api/shifts/{id}";
    public const string ShiftsFromEmployee = "/api/shifts/employee/{employeeId}";
    public const string ShiftsOnDate = "/api/shifts/date/{date}";
    public const string AssignEmployeeToShift = "/api/shifts/{shiftId}/employee/{employeeId}";
}