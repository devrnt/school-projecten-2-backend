using Newtonsoft.Json;

namespace g16_dotnet.Models.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Oefening
    {
        #region Fields and Properties
        [JsonProperty]
        public int OefeningId { get; set; }
        [JsonProperty]
        public string Opgave { get; set; }
        [JsonProperty]
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
        public bool ControleerAntwoord(string antwoord)
        {
            return antwoord == GroepsAntwoord;
        } 
        #endregion
    }
}
