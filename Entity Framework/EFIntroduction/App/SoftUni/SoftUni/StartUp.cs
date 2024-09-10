using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SoftUni.Data;
using SoftUni.Models;
using System.Text;
using System.Xml.Schema;

public class StartUp
{
    public static void Main()
    {
        SoftUniContext context = new SoftUniContext();
        Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
    }

    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        var employees = context.Employees
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            })
            .ToList();

        StringBuilder sb = new();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        var employees = context.Employees
            .Where(e => e.Salary > 50000)
            .Select(e => new
            {
                e.FirstName,
                e.Salary,
            })
            .OrderBy(e => e.FirstName)
            .ToList();

        StringBuilder sb = new();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
        var employees = context.Employees
            .Where(e => e.Department.Name == "Research and Development")
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.Department,
                e.Salary
            })
            .OrderBy(e => e.Salary)
            .ThenByDescending(e => e.FirstName)
            .ToList();

        StringBuilder sb = new();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        Address newAddress = new Address()
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };

        var nakov = context.Employees
            .FirstOrDefault(e => e.LastName == "Nakov");

        if (nakov != null) 
        { 
            nakov.Address =  newAddress;
            context.SaveChanges();
        }

        return GetTenEmployeesWithNewAddres(context).ToString();
    }

    public static string GetTenEmployeesWithNewAddres(SoftUniContext context)
    {
        var employees = context.Employees
            .OrderByDescending(e => e.AddressId)
            .Take(10)
            .Select(e => e.Address.AddressText)
            .ToList();

        return string.Join(Environment.NewLine, employees);
    }

    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        var employees = context.Employees
            .Take(10)
            .Select(e => new
            {
                EmployeeFullName = $"{e.FirstName} {e.LastName}",
                ManagerFullName = $"{e.Manager.FirstName} {e.Manager.LastName}",
                Projects = e.EmployeeProjects
                    .Where(ep => ep.Project.StartDate.Year >= 2001 &&
                           ep.Project.StartDate.Year <= 2003)
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        ep.Project.StartDate,
                        EndDate = ep.Project.EndDate.HasValue ?
                        ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished"
                    })
            })
            .ToList();

        StringBuilder sb = new();

        foreach (var e in employees) 
        {
            sb.AppendLine($"{e.EmployeeFullName} - Manager: {e.ManagerFullName}");
            if (e.Projects.Any())
            {
                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"--{p.ProjectName} - {p.StartDate:M/d/yyyy h:mm:ss tt} - {p.EndDate}");
                }
            } 
        }
           
        return sb.ToString().TrimEnd();
    }

    public static string GetAddressesByTown(SoftUniContext context)
    {
        var addresses = context.Addresses
            .OrderByDescending(a => a.Employees.Count)
            .ThenBy(a => a.Town.Name)
            .ThenBy(a => a.AddressText)
            .Take(10)
            .Select(a => new
            {
                a.AddressText,
                TownName = a.Town.Name,
                EmployeesCount = a.Employees.Count()
            })
            .ToList();

        StringBuilder sb = new();

        foreach (var a in addresses)
        {
            sb.AppendLine($"{a.AddressText}, {a.TownName} - {a.EmployeesCount} employees");
        }

        return sb.ToString().TrimEnd();
    }

    public static string GetEmployee147(SoftUniContext context)
    {
        var employees = context.Employees
            .Where(e => e.EmployeeId == 147)
            .Select(e => new
            {
                EmployeeFullName = $"{e.FirstName} {e.LastName}",
                e.JobTitle,
                Projects = e.EmployeeProjects
                .Select(p => p.Project.Name)
                .OrderBy(p => p)
                .ToList()
            })
            .ToList()
            .FirstOrDefault();

        StringBuilder sb = new();

        sb.AppendLine($"{employees.EmployeeFullName} - {employees.JobTitle}");
        sb.AppendLine($"{string.Join(Environment.NewLine, employees.Projects)}");

        return sb.ToString().TrimEnd();
    }



}