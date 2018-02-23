using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public class Opdracht
    {
        #region Fields and Properties
        public int VolgNr { get; private set; }
        public string ToegangsCode { get; set; }
        public Oefening Oefening { get; private set; }
        public GroepsBewerking GroepsBewerking { get; private set; }
        #endregion

        #region Constructors
        public Opdracht(string toegangsCode, Oefening oefening, GroepsBewerking groepsBewerking)
        {
            ToegangsCode = toegangsCode;
            Oefening = oefening;
            GroepsBewerking = groepsBewerking;
        }
        #endregion

        #region Methods
        public bool ControleerToegangsCode(string code)
        {
            return ToegangsCode == code;
        }
        #endregion
    }
}
