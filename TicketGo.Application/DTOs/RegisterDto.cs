﻿using DO_AN.Models;
using System.ComponentModel.DataAnnotations;

namespace TicketGo.Application.DTOs
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FullName { get; set; }
    }
}
