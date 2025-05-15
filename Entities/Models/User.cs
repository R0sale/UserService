using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class User
    {
        [Required(ErrorMessage = "Id is a required field.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "FirstName is a required field.")]
        [MaxLength(20, ErrorMessage = "The max length of name field is 20.")]
        [MinLength(5, ErrorMessage = "The min length of name field is 5")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "LastName is a required field.")]
        [MaxLength(20, ErrorMessage = "The max length of name field is 20.")]
        [MinLength(5, ErrorMessage = "The min length of name field is 5")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "UserName is a required field.")]
        [MaxLength(20, ErrorMessage = "The max length of name field is 20.")]
        [MinLength(5, ErrorMessage = "The min length of name field is 5")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "UserName is a required field.")]
        public string? Role { get; set; }

        [Required(ErrorMessage = "UserName is a required field.")]
        public string? Email { get; set; }
    }
}
