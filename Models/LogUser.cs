using System;
using System.ComponentModel.DataAnnotations;

namespace MessageBoard.Models
{
    public class LogUser 
    {
        [Required(ErrorMessage="Please enter your email.")]
        [EmailAddress(ErrorMessage="Please enter a valid email address.")]
        [Display(Name="Email: ")]
        public string Email { get; set; }

        [Required(ErrorMessage="Password required.")]
        [DataType(DataType.Password)]
        [Display(Name="Password: ")]
        public string Password { get; set; }
    }
}