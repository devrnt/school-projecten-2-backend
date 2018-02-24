using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace g16_dotnet.Models.Domain
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Pad
    {
        #region Fields and Properties
        [JsonProperty]
        public int PadId { get; set; }
        public int? AantalOpdrachten { get { return Opdrachten?.Count(); } }
        public int? Voortgang { get { return Opdrachten?.Where(o => o.isVoltooid).Count(); } }
        public Opdracht HuidigeOpdracht { get { return Opdrachten?.First(o => !o.isVoltooid); } }
        public Actie HuidigeActie { get { return Acties?.First(a => !a.IsUitgevoerd); } }
        [JsonProperty]
        public IEnumerable<Opdracht> Opdrachten { get; set; }
        [JsonProperty]
        public IEnumerable<Actie> Acties { get; set; }
        #endregion

        #region Constructors
        public Pad(IEnumerable<Opdracht> opdrachten, IEnumerable<Actie> acties)
        {
            Opdrachten = opdrachten;
            Acties = acties;
        }

        public Pad()
        {

        }
        #endregion
    }
}
