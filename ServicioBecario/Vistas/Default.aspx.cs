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
            hola+="Nombre = "+huesped.Nombre+" <br/>";
            hola+="Imss = "+huesped.Imss+ " <br/>";
            hola += "campus = " + huesped.ClaveCampus + " <br/>";
            hola += "ApellidoMaterno = " + huesped.ApellidoMaterno + " <br/>";
            hola += "ApellidoPaterno = " + huesped.ApellidoPaterno + " <br/>";
            hola += "AreaPersonal = " + huesped.AreaPersonal + " <br/>";
            hola += "ClaveCampus = " + huesped.ClaveCampus + " <br/>";
            hola += "ClaveCoas = " + huesped.ClaveCoas + " <br/>";
            hola += "ClaveContrato = " + huesped.ClaveContrato + " <br/>";
            hola += "ClaveInstitucion = " + huesped.ClaveInstitucion + " <br/>";
            hola += "ClavePuesto  = " + huesped.ClavePuesto + " <br/>";
            hola += "ClaveRectoria  = " + huesped.ClaveRectoria + " <br/>";
            hola += "ClaveUnidadOrg  = " + huesped.ClaveUnidadOrg + " <br/>";
            hola += "CURP  = " + huesped.CURP + " <br/>";
            hola += "DescCampus   = " + huesped.DescCampus + " <br/>";
            hola += "DescContrato   = " + huesped.DescContrato + " <br/>";
            hola += "DescPuesto   = " + huesped.DescPuesto + " <br/>";
            hola += "DescRectoria   = " + huesped.DescRectoria + " <br/>";
            hola += "DescUnidadOrg   = " + huesped.DescUnidadOrg + " <br/>";
            hola += "Division   = " + huesped.Division + " <br/>";
            hola += "Email   = " + huesped.Email + " <br/>";
            hola += "Estatus   = " + huesped.Estatus + " <br/>";
            hola += "FechaGraciaAcad   = " + huesped.FechaGraciaAcad + " <br/>";
            hola += "FechaGraciaAdmv   = " + huesped.FechaGraciaAdmv + " <br/>";
            hola += "FechaNacimiento   = " + huesped.FechaNacimiento + " <br/>";
            hola += "GrupoPersonal   = " + huesped.GrupoPersonal + " <br/>";
                Label1.Text=hola;

        }
    }
}