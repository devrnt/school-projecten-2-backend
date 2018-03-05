using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Het email adres moet ingevuld zijn")]
        [Display]
        [EmailAddress(ErrorMessage ="Geen geldig email adres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Het wachtwoord moet ingevuld zijn")]
        [DataType(DataType.Password, ErrorMessage ="Geen geldig wachtwoord")]
        public string Password { get; set; }

        [Display(Name = "Ingelogd blijven")]
        public bool RememberMe { get; set; }
    }
}
