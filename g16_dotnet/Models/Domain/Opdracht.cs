using Newtonsoft.Json;

namespace g16_dotnet.Models.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Opdracht
    {
        #region Fields and Properties
        [JsonProperty]
        public int VolgNr { get; set; }
        [JsonProperty]
        public string ToegangsCode { get; private set; }
        [JsonProperty]
        public bool isVoltooid { get; set; }
        [JsonProperty]
        public Oefening Oefening { get; private set; }
        [JsonProperty]
        public GroepsBewerking GroepsBewerking { get; private set; }
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
        public bool ControleerToegangsCode(string code)
        {
            return code == ToegangsCode;
        }

        #endregion
    }
}
