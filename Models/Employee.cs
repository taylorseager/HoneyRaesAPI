using System;
namespace HoneyRaesAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Specialty { get; set; }


        public Employee(int id, string firstName, string specialty)
        {
            Id = id;
            FirstName = firstName;
            Specialty = specialty;
        }
    }
}
