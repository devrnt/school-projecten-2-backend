using Newtonsoft.Json;

namespace g16_dotnet.Models.Domain
{

    public class Actie
    {
        #region Fields and Properties
        public int ActieId { get; set; }
        public string Omschrijving { get; set; }
        #endregion

        #region Constructors
        public Actie(string omschrijving) {
            Omschrijving = omschrijving;
        }

        public Actie()
        {

        }
        #endregion
    }
}
