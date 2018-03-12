﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g16_dotnet.Models.Domain
{
    public abstract class PadState
    {
        public string StateName { get; set; }

        protected PadState(string name)
        {
            StateName = name;
        }

        public virtual bool ControleerAntwoord(Pad pad, int antwoord)
        {
            throw new InvalidOperationException("Je kan nu geen antwoord ingeven!");
        }

        public virtual bool ControleerToegangsCode(Pad pad, string toegangscode)
        {
            throw new InvalidOperationException("Je kan nu geen actie uitvoeren!");
        }

    }
}