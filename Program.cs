using System.Net;
using HoneyRaesAPI.Models;

var builder = WebApplication.CreateBuilder(args);

List<Customer> customers = new List<Customer>()
{
    new Customer(id: 1, firstName: "Rachel", address: "123 Main St, NYC"),
    new Customer(id: 2, firstName: "Monica", address: "146 First St, NYC"),
    new Customer(id: 3, firstName: "Chandler", address: "231 Central Perk Dr, NYC"),

};

List<Employee> employees = new List<Employee>
{
    new Employee(id: 1, firstName: "Joey", specialty: "Acting"),
    new Employee(id: 2, firstName: "Ross", specialty: "History"),
};

List<ServiceTicket> serviceTickets = new List<ServiceTicket>
{
    new ServiceTicket(id: 123, customerId: 1, employeeId: 2, description: "Customer is struggling with WW2 topics.", emergency: true, dateCompleted: null),
    new ServiceTicket(id: 124, customerId: 2, employeeId: 1, description: "Needs acting classes", emergency: false, dateCompleted: "07/14/2024"),
    new ServiceTicket(id: 125, customerId: 3, employeeId: 0, description: "Customer is struggling with WW2 topics.", emergency: false, dateCompleted: "07/20/2024"),
    new ServiceTicket(id: 126, customerId: 1, employeeId: 1, description: "Customer is struggling with WW2 topics.", emergency: false, dateCompleted: "07/21/2024"),
    new ServiceTicket(id: 127, customerId: 2, employeeId: 0, description: "Customer is struggling with WW2 topics.", emergency: true, dateCompleted: null),

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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
// BELOW CREATES AN ENDPOINT - ROUTE AND HANDLER (FUNCTION THAT DETERMINES THE LOGIC FOR
// WHAT TO DO WHEN A REQUEST IS MADE TO THAT ROUTE
app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/hello", () =>
{
    return "hello";
});

// always make sure this is at the end of the file:
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
