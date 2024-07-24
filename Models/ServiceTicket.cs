using System;
namespace HoneyRaesAPI.Models
{
	public class ServiceTicket
	{
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public string? Description { get; set; }
        public bool Emergency { get; set; }
        public DateTime? DateCompleted { get; set; }
        public Employee? Employee { get; set; }
        public Customer? Customer { get; set; }

  //      public ServiceTicket(int id, int customerId, int employeeId, string description, bool emergency, string dateCompleted)
		//{
  //          Id = id;
  //          CustomerId = customerId;
  //          EmployeeId = employeeId;
  //          Description = description;
  //          Emergency = false;
  //          DateCompleted = dateCompleted;
  //      }
	}
}

