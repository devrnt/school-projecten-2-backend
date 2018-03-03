namespace g16_dotnet.Models.Domain
{
    public class PadOpdracht
    {
        #region Fields and Properties
        public int OpdrachtId { get; set; }
        public int PadId { get; set; }
        public Opdracht Opdracht { get; set; }
        public Pad Pad { get; set; }
        #endregion

        #region Constructors
        public PadOpdracht(Pad pad, Opdracht opdracht)
        {
            Opdracht = opdracht;
            Pad = pad;
            PadId = pad.PadId;
            OpdrachtId = opdracht.VolgNr;
        }

        public PadOpdracht()
        {

        }
        #endregion
    }
}
