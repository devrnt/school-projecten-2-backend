using Newtonsoft.Json;
using System;

namespace g16_dotnet.Models.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Opdracht
    {
        #region Fields and Properties
        [JsonProperty]
        public int VolgNr { get; set; }
        public string ToegangsCode { get; set; }
        [JsonProperty]
        public bool isVoltooid { get; set; }
        public Oefening Oefening { get; set; }
        public GroepsBewerking GroepsBewerking { get; set; }
        #endregion

        #region Constructors
        public Opdracht(string toegangsCode, Oefening oefening, GroepsBewerking groepsBewerking)
        {
            ToegangsCode = toegangsCode;
            Oefening = oefening;
            GroepsBewerking = groepsBewerking;
        }

        public Opdracht()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Controleert of de opgegeven code gelijk is aan de ingestelde ToegangsCode
        /// </summary>
        /// <param name="code">Mag niet null of leeg zijn</param>
        /// <returns>True indien gelijk, anders false</returns>
        /// <exception>ArgumentException indien code null of leeg is</exception>
        public bool ControleerToegangsCode(string code)
        {
            if (code == null || code.Trim().Length == 0)
                throw new ArgumentException("Je hebt geen toegangscode opgegeven. ");
            return code == ToegangsCode;
        }

        #endregion
    }
}
