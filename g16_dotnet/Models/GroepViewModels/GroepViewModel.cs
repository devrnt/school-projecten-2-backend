using g16_dotnet.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.GroepViewModels
{
    public class GroepViewModel
    {

        [Display(Name = "Groep naam")]
        public string GroepNaam { get; set; }

        [Display(Name = "Groepsleden")]
        public Groep Groep { get; set; }
        
        public Sessie Sessie { get; set; }
        [Display(Name = "Beschikbare leerlingen")]
        public ICollection<Leerling> BeschikbareLeerlingen { get; set; }

        public GroepViewModel(Groep groep, Sessie sessie)
        {
            GroepNaam = groep.Groepsnaam;
            Groep = groep;
            Sessie = sessie;
            BeschikbareLeerlingen = new List<Leerling>();
            CheckBeschikbareLeerlingen();
        }

        public GroepViewModel()
        {

        }

        private void CheckBeschikbareLeerlingen()
        {
            foreach (var leerling in Sessie.Klas.Leerlingen)
            {
                bool antwoord = true;

                foreach (var groep in Sessie.Groepen)
                {
                    foreach (var groepLeerling in groep.Leerlingen)
                    {
                        if (leerling == groepLeerling)
                        {
                            antwoord = false;
                        }
                    }

                }
                if (antwoord)
                    BeschikbareLeerlingen.Add(leerling);

            }

        }
    }
}
