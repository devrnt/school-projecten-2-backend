using Newtonsoft.Json;
using System;

namespace g16_dotnet.Models.Domain
{
    public class Opdracht
    {
        #region Fields and Properties
        public int VolgNr { get; set; }
        public string ToegangsCode { get; set; }
        public bool IsVoltooid { get; set; }
        public int AantalPogingen { get; set; }
        public Oefening Oefening { get; set; }
        public GroepsBewerking GroepsBewerking { get; set; }
        #endregion

        #region Constructors
        public Opdracht(string toegangsCode, Oefening oefening, GroepsBewerking groepsBewerking)
        {
            ToegangsCode = toegangsCode;
            Oefening = oefening;
            GroepsBewerking = groepsBewerking;
            AantalPogingen = 0;
        }

        public Opdracht()
        {
            AantalPogingen = 0;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Controleert of de opgegeven code gelijk is aan de ingestelde ToegangsCode
        /// </summary>
        /// <param name="code">Mag niet null of leeg zijn</param>
        /// <returns>True indien gelijk, anders false</returns>
        /// <exception>ArgumentException indien code null of leeg is</exception>
        public bool ControleerToegangsCode(string code)
        {
            if (code == null || code.Trim().Length == 0)
                throw new ArgumentException("Je hebt geen toegangscode opgegeven. ");
            return code == ToegangsCode;
        }

        /// <summary>
        ///  Controleert of het opgegeven antwoord gelijk is aan het GroepsAntwoord
        ///  uit Oefening na toepassing van de GroepsBewerking
        /// </summary>
        /// <param name="antwoord">GroepsAntwoord gewijzigd door het toepassen van de GroepsBewerkingen</param>
        /// <returns>True indien het antwoord juist is, anders false</returns>
        public bool ControleerAntwoord(int antwoord)
        {
            int antwoordNaGroepsBewerking = Oefening.GroepsAntwoord;
            switch (GroepsBewerking.Operator)
            {
                case Operator.optellen:
                    antwoordNaGroepsBewerking += GroepsBewerking.Factor;
                    break;
                case Operator.aftrekken:
                    antwoordNaGroepsBewerking -= GroepsBewerking.Factor;
                    break;
                case Operator.vermeningvuldigen:
                    antwoordNaGroepsBewerking *= GroepsBewerking.Factor;
                    break;
                case Operator.delen:
                    antwoordNaGroepsBewerking /= GroepsBewerking.Factor;
                    break;               
            }
            return antwoord == antwoordNaGroepsBewerking;
        }

        #endregion
    }
}
