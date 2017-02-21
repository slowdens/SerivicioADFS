using mx.itesm.portales.libs.identidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServicioBecario.Vistas
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string hola;

            var mail = Request.Cookies["MailUserPortal"].Value;
            hola = "MailusertPorta " + mail+"<br/>";
            
            mx.itesm.portales.libs.identidad.Usuario huesped = Autentica.AutenticaUsuario(mail, "NuevaNomina");
            //Response.Write("El valor de la cookies es = " + mail + " <br/>");
            //Response.Write(" La nomina es := " + huesped.Nomina + " <br/>");
            hola += "Nomina = " + huesped.Nomina + "<br/>";
                Label1.Text=hola;

        }
    }
}