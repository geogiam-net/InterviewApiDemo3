using Demo.Business.Models;

namespace Demo.Business.Validators;

public static class EmployeeValidator
{
    public static string[] Validate(Employee employee)
    {
        var errors = new List<string>();
        errors.AddRange(ValidateName(employee));
        errors.AddRange(ValidateLastname(employee));
        return errors.ToArray();
    }

    public static string[] ValidateName(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Name))
        {
            return ["The 'Name' is required"];
        }

        if (employee.Name.Length < 5 || employee.Name.Length > 50)
        {
            return ["The 'Name' has to be between 5 and 50 characters long"];
        }

        return [];
    }

    public static string[] ValidateLastname(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.Lastname))
        {
            return ["The 'Lastname' is required"];
        }

        if (employee.Lastname.Length < 5 || employee.Lastname.Length > 50)
        {
            return ["The 'Lastname' has to be between 5 and 50 characters long"];
        }

        return [];
    }

}