using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioBecario.Codigo
{
    public class Usuario
    {
        private string nomina;

        public string Nomina
        {
            get
            {
                return this.nomina;
            }
            set
            {
                this.nomina = value;
            }
        }
    }
}