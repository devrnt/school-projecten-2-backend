namespace g16_dotnet.Models.Domain
{
    public class GroepLeerling
    {
        public int GroepId { get; set; }
        public int LeerlingId { get; set; }
        public Groep Groep { get; set; }
        public Leerling Leerling { get; set; }

        public GroepLeerling(Groep groep, Leerling leerling)
        {
            GroepId = groep.GroepId;
            LeerlingId = leerling.LeerlingId;
            Groep = groep;
            Leerling = leerling;
        }
    }
}
