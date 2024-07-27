using System.Net;
using HoneyRaesAPI.Models;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

List<Customer> customers = new List<Customer>()
{
    new Customer()
    {
        Id = 1,
        FirstName = "Rachel",
        Address = "123 Main St, NYC"
    },
    new Customer()
    {
        Id = 2,
        FirstName = "Monica",
        Address = "146 First St, NYC",
    },
    new Customer()
    {
        Id = 3,
        FirstName = "Chandler",
        Address = "231 Central Perk Dr, NYC"
    }
};

List<Employee> employees = new List<Employee>
{
    new Employee()
    {
        Id = 1,
        FirstName = "Joey",
        Specialty = "Acting"
    },
    new Employee()
    {
        Id = 2,
        FirstName = "Ross",
        Specialty = "History/Dinosaurs"
    }
};

List<ServiceTicket> serviceTickets = new List<ServiceTicket>
{
    new ServiceTicket()
    {
        Id = 123,
        CustomerId = 1,
        EmployeeId = 2,
        Description = "Customer is struggling with WW2 topics.",
        Emergency = false,
        DateCompleted = null
    },
    new ServiceTicket()
    {
        Id = 124,
        CustomerId = 2,
        EmployeeId = 1,
        Description = "Needs acting classes",
        Emergency = false,
        DateCompleted = new DateTime(2024, 07, 12)
    },
    new ServiceTicket()
    {
        Id = 125,
        CustomerId = 3,
        EmployeeId = null,
        Description = "Wants to learn more about dinos.",
        Emergency = false,
        DateCompleted = new DateTime(2023, 06, 20)
    },
    new ServiceTicket()
    {
        Id = 126,
        CustomerId = 1,
        EmployeeId = 1,
        Description = "Has a casting call and needs some pointers.",
        Emergency = false,
        DateCompleted = new DateTime(2024, 07, 21)
    },
    new ServiceTicket()
    {
        Id = 127,
        CustomerId = 2,
        EmployeeId = null,
        Description = "Got a call back and needs to run lines ASAP.",
        Emergency = true,
        DateCompleted = null
    }
};

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapGet("/servicetickets", () =>
{
    return serviceTickets;
});

app.MapGet("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }
    serviceTicket.Customer = customers.FirstOrDefault(c => c.Id == serviceTicket.CustomerId);
    serviceTicket.Employee = employees.FirstOrDefault(e => e.Id == serviceTicket.EmployeeId);
    return Results.Ok(serviceTicket);
});

app.MapPost("/servicetickets", (ServiceTicket serviceTicket) =>
{
    // creates a new id (When we get to it later, our SQL database will do this for us like JSON Server did!)
    serviceTicket.Id = serviceTickets.Max(st => st.Id) + 1;
    serviceTickets.Add(serviceTicket);
    return serviceTicket;
});

app.MapPost("/servicetickets/{id}/complete", (int id) =>
{
    ServiceTicket ticketToComplete = serviceTickets.FirstOrDefault(st => st.Id == id);
    ticketToComplete.DateCompleted = DateTime.Today;
    return ticketToComplete;
});

app.MapDelete("/servicetickets/{id}", async (int id) =>
{
    var serviceTicketToDelete = serviceTickets.FirstOrDefault(st => st.Id == id);

    if (serviceTicketToDelete == null)
    {
        return Results.NotFound();
    }

    serviceTickets.Remove(serviceTicketToDelete);
    return Results.NoContent();
});

app.MapPut("/servicetickets/{id}", (int id, ServiceTicket serviceTicket) =>
{
    ServiceTicket ticketToUpdate = serviceTickets.FirstOrDefault(st => st.Id == id);
    int ticketIndex = serviceTickets.IndexOf(ticketToUpdate);
    if (ticketToUpdate == null)
    {
        return Results.NotFound();
    }
    //the id in the request route doesn't match the id from the ticket in the request body. That's a bad request!
    if (id != serviceTicket.Id)
    {
        return Results.BadRequest();
    }
    serviceTickets[ticketIndex] = serviceTicket;
    return Results.Ok();
});

app.MapGet("/servicetickets/emergencynotcomplete", () =>
{
    var notCompleteEmergency = serviceTickets.Where(st => st.Emergency == true && st.DateCompleted == null).ToList();

    return Results.Ok(notCompleteEmergency);
});

app.MapGet("/servicetickets/unassigned", () =>
{
    var unassgined = serviceTickets.Where(st => st.EmployeeId == null).ToList();

    return Results.Ok(unassgined);
});

app.MapGet("/employee", () =>
{
    return employees;
});

app.MapGet("/employee/{id}", async (int id) =>
{
    Employee employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    employee.ServiceTickets = serviceTickets.Where(st => st.EmployeeId == id).ToList();
    return Results.Ok(employee);
});

app.MapGet("/employee/available", () =>
{
    List<Employee> availableEmployees = employees
               .Where(emp => !serviceTickets
               .Any(st => st.EmployeeId == emp.Id && st.DateCompleted == null))
               .ToList();

    if (availableEmployees.Count == 0)
    {
       return Results.NotFound();
    }

    return Results.Ok(availableEmployees);
});

app.MapGet("/employee/{id}/customers", (int id) =>
{
    var customerIds = serviceTickets
                        .Where(st => st.EmployeeId == id)
                        .Select(st => st.CustomerId)
                        .Distinct()
                        .ToList();

    List<Customer> employeeCustomers = customers.Where(c => customerIds.Contains(c.Id)).ToList();
}); 

app.MapGet("/customer", () =>
{
    return customers;
});

app.MapGet("/customer/{id}", (int id) =>
{
    Customer customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer == null)
    {
        return Results.NotFound();
    }
    customer.ServiceTickets = serviceTickets.Where(st => st.CustomerId == id).ToList();
    return Results.Ok(customer);
});

app.MapGet("/customer/inactive", () =>
{
    DateTime lastYear = DateTime.Now.AddYears(-1);

    List<Customer> inactiveCustomers = customers
                   .Where(c => !serviceTickets
                   .Any(st => st.CustomerId == c.Id && st.DateCompleted.HasValue && st.DateCompleted.Value > lastYear))
                   .ToList();

    return Results.Ok(inactiveCustomers);
});

// always make sure this is at the end of the file:
app.Run();
