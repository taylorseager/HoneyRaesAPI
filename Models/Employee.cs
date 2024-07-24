using System;
namespace HoneyRaesAPI.Models
{
    public class Employee
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? Specialty { get; set; }
        public List<ServiceTicket>? ServiceTickets { get; set; }

        //public Employee(int id, string firstName, string specialty, List<ServiceTicket> serviceTickets)
        //{
        //    Id = id;
        //    FirstName = firstName;
        //    Specialty = specialty;
        //    ServiceTickets = serviceTickets;
        //}
    }
}
