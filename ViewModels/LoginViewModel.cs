﻿namespace Dashboard.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        [Required(ErrorMessage = "UserName is required!" )]
        
        public string UserName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]

        public string Password { get; set; } = string.Empty;
    }

}
