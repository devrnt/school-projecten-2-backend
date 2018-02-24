using Newtonsoft.Json;

namespace g16_dotnet.Models.Domain
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Oefening
    {
        #region Fields and Properties
        public int OefeningId { get; set; }
        public string Opgave { get; private set; }
        public string GroepsAntwoord { get; private set; }
        #endregion

        #region Constructors
        public Oefening(string opgave, string groepsAntwoord)
        {
            Opgave = opgave;
            GroepsAntwoord = groepsAntwoord;
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
