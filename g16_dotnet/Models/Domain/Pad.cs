﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    //[JsonObject(MemberSerialization.OptIn)]
    public class Pad
    {
        #region Fields and Properties
        public int AantalOpdrachten { get { return Opdrachten.Count(); } }
        public int Voortgang { get { return Opdrachten.Where(o => o.isVoltooid).Count(); } }
        //[JsonProperty]
        public IEnumerable<Opdracht> Opdrachten { get; set; }
        //[JsonProperty]
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
