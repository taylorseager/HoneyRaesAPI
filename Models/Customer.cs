﻿using System;
namespace HoneyRaesAPI.Models
{
	public class Customer
    {
		public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? Address  { get; set; }
        public List<ServiceTicket>? ServiceTickets { get; set; }
    }
}

