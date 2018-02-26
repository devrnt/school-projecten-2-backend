using g16_dotnet.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace g16_dotnet.Models.SessieViewModel
{
    public class SessieLijstViewModel
    {
        [HiddenInput]
        public int SessieId { get; set; }
        [Display(Name = "Sessie naam")]
        public string SessieNaam { get; set; }
        public string Omschrijving { get; set; }

        public SessieLijstViewModel(Sessie sessie)
        {
            SessieId = sessie.SessieId;
            SessieNaam = sessie.Naam;
            Omschrijving = sessie.Omschrijving;
        }
    }
}
