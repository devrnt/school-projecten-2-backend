using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace g16_dotnet.Models.SessieViewModel
{
    public class SessieLijstViewModel
    {
        [HiddenInput]
        public int SessieCode { get; set; }
        [Display(Name = "Sessie naam")]
        public string SessieNaam { get; set; }
        public Klas Klas { get; set; }
        public string Omschrijving { get; set; }
        

        public SessieLijstViewModel(Sessie sessie)
        {
            SessieCode = sessie.SessieCode;
            SessieNaam = sessie.Naam;
            Klas = sessie.Klas;
            Omschrijving = sessie.Omschrijving;
        }
    }
}
