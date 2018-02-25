﻿using System.Collections.Generic;

namespace g16_dotnet.Models.Domain
{
    public class Sessie
    {
        #region Properties
        public int SessieId { get; set; }
        public string Naam { get; set; }
        public string Omschrijving { get; set; }
        public int Code { get; set; }
        public bool IsActief { get; set; }
        public IEnumerable<Groep> Groepen { get; set; }
        public Klas Klas { get; set; }
        #endregion

        #region Constructor
        public Sessie(string naam, string omschrijving, int code, IEnumerable<Groep> groepen, Klas klas)
        {
            Naam = naam;
            Omschrijving = omschrijving;
            Code = code;
            Groepen = groepen;
            Klas = klas;
        }

        public Sessie()
        {
            Groepen = new List<Groep>();
        }
        #endregion
    }
}
