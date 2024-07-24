using System.Net;
using HoneyRaesAPI.Models;

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
        Emergency = true,
        DateCompleted = new DateTime()
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
        DateCompleted = new DateTime(2024, 07, 20)
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
        DateCompleted = new DateTime()
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

app.MapGet("/employee", () =>
{
    return employees;
});

app.MapGet("/employee/{id}", (int id) =>
{
    Employee employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    employee.ServiceTickets = serviceTickets.Where(st => st.EmployeeId == id).ToList();
    return Results.Ok(employee);
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

// always make sure this is at the end of the file:
app.Run();
