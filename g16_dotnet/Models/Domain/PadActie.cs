namespace g16_dotnet.Models.Domain
{
    public class PadActie
    {
        #region Fields and Properties
        public int PadId { get; set; }
        public int ActieId { get; set; }
        public Pad Pad { get; set; }
        public Actie Actie { get; set; }
        public bool IsUitgevoerd { get; set; }
        #endregion

        #region Constructors
        public PadActie(Pad pad, Actie actie)
        {
            Pad = pad;
            Actie = actie;
            PadId = pad.PadId;
            ActieId = actie.ActieId;
        }

        public PadActie()
        {

        }
        #endregion
    }
}
