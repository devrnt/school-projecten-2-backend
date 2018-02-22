using System.Collections;
using System.Collections.Generic;

namespace g16_dotnet.Models.Domain {
    public class Klas {
        #region Fields
        private ICollection _leerlingen;
        #endregion

        #region Constructor
        public Klas() {
            _leerlingen = new List<Leerling>();
        }
        #endregion

    }
}