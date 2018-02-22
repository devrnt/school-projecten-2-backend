using g16_dotnet.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.GroepViewModels
{
    public class GroepViewModel
    {
        public string Naam { get; set; }
        public GroepViewModel(Groep groep)
        {
            Naam = "testGroepInfo";
            //Naam = groep.Groepsnaam;
        }
    }
}
