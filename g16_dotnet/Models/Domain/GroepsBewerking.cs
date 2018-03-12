using Newtonsoft.Json;

namespace g16_dotnet.Models.Domain
{
    public class GroepsBewerking
    {
        #region Fields and Properties
        public int GroepsBewerkingId { get; set; }
        public string Omschrijving { get; private set; }
        public int Factor { get; set; }
        public Operator Operator { get; set; }
        #endregion

        #region Constructors
        public GroepsBewerking(string omschrijving, int factor, Operator op)
        {
            Omschrijving = omschrijving;
            Factor = factor;
            Operator = op;
        }

        public GroepsBewerking()
        {

        }
        #endregion
    }
}