using Newtonsoft.Json;

namespace g16_dotnet.Models.Domain
{
    [JsonObject(MemberSerialization.OptOut)]
    public class GroepsBewerking
    {
        #region Fields and Properties
        public int GroepsBewerkingId { get; set; }
        public string Omschrijving { get; private set; }
        #endregion

        #region Constructors
        public GroepsBewerking(string omschrijving)
        {
            Omschrijving = omschrijving;
        }

        public GroepsBewerking()
        {

        }
        #endregion
    }
}