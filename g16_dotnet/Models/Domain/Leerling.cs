namespace g16_dotnet.Models.Domain {

    public class Leerling {

        #region Fields and Properties
        public int LeerlingId { get; set; }
        public string Naam { get; set; }
        public string Voornaam { get; set; }
        public string Email { get; set; }
        #endregion

        #region Constructors
        public Leerling(string naam, string voornaam, string email) : this(naam, voornaam)
        {
            Email = email;
        }

        public Leerling(string naam, string voornaam)
        {
            Naam = naam;
            Voornaam = voornaam;
        }

        public Leerling()
        {

        }
        #endregion
    }
}