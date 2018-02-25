using Newtonsoft.Json;
using System;

namespace g16_dotnet.Models.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Oefening
    {
        #region Fields and Properties
        [JsonProperty]
        public int OefeningId { get; set; }
        public string Opgave { get; set; }
        public string GroepsAntwoord { get; set; }
        #endregion

        #region Constructors
        public Oefening(string opgave, string groepsAntwoord)
        {
            Opgave = opgave;
            GroepsAntwoord = groepsAntwoord;
        }
        // EF
        public Oefening() {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Controleert of de paramter antwoord gelijk is aan het ingestelde GroepsAntwoord
        /// </summary>
        /// <param name="antwoord">Mag niet leeg of null zijn</param>
        /// <returns>True indien gelijk, false indien niet gelijk</returns>
        /// <exception>ArgumentException indien antwoord null of leeg is</exception>
        public bool ControleerAntwoord(string antwoord)
        {
            if (antwoord == null || antwoord.Trim().Length == 0)
                throw new ArgumentException("Je hebt geen antwoord opgegeven. ");
            return antwoord == GroepsAntwoord;
        } 
        #endregion
    }
}
