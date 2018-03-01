namespace g16_dotnet.Models.Domain
{
    public class Oefening
    {
        #region Fields and Properties
        public int OefeningId { get; set; }
        public string Opgave { get; set; }
        public int GroepsAntwoord { get; set; }
        #endregion

        #region Constructors
        public Oefening(string opgave, int groepsAntwoord)
        {
            Opgave = opgave;
            GroepsAntwoord = groepsAntwoord;
        }
        // EF
        public Oefening() {

        }
        #endregion

        #region Methods
        
        #endregion
    }
}
