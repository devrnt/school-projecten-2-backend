using Newtonsoft.Json;

namespace g16_dotnet.Models.Domain
{
    [JsonObject(MemberSerialization.OptIn)]

    public class Actie
    {
        #region Fields and Properties
        [JsonProperty]
        public int ActieId { get; set; }
        public string Omschrijving { get; set; }
        [JsonProperty]
        public bool IsUitgevoerd { get; set; }
        public Opdracht GelinkteOpdracht { get; set; }
        #endregion

        #region Constructors
        public Actie(string omschrijving) {
            Omschrijving = omschrijving;
        }
        public Actie(string omschrijving, Opdracht opdracht)
        {
            Omschrijving = omschrijving;
            GelinkteOpdracht = opdracht;
        }

        public Actie()
        {

        }
        #endregion
    }
}
