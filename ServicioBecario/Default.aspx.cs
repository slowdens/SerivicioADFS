using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using System.Configuration;
using ServicioBecario.Codigo;
using mx.itesm.portales.libs;
using mx.itesm.portales.libs.identidad;
using ServicioBecario.Codigo;
using System.Data;


namespace ServicioBecario
{
    public partial class Default : System.Web.UI.Page
    {
        Trabajadores tr = new Trabajadores();
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        

        protected void Page_Load(object sender, EventArgs e)
        {
            try {

                if (Request.Cookies["MailUserPortal"] == null)
                {
                    Response.Write("No existe la cookie");
                    
                }
                else
                {
                    //Exite la cookie
                    var mail = Request.Cookies["MailUserPortal"].Value;                    
                    string area="",grupo="";
                    int campus;
                    mx.itesm.portales.libs.identidad.Usuario huesped = Autentica.AutenticaUsuario(mail, "NuevaNomina");
                    Response.Write("El valor de la cookies es = "+mail);
                    if(string.IsNullOrEmpty(huesped.AreaPersonal))
                    {
                        area = "0";
                    }
                    else
                    {
                        area = huesped.AreaPersonal;
                    }
                    if(string.IsNullOrEmpty(huesped.GrupoPersonal))
                    {
                        grupo = "0";
                    }
                    else
                    {
                        grupo = huesped.GrupoPersonal;
                    }

                    //Agregamos el campus
                    if (tr.Campus != "" && !string.IsNullOrEmpty(tr.Campus))
                    {
                        campus = int.Parse(tr.Campus);
                    }
                    else
                    {
                        campus = 1;
                    }                    
                    query = "sp_guarda_empleado_nuevos '"+huesped.Nomina+"','"+huesped.Email+"','"+huesped.DescUnidadOrg+"', '"+campus+"','"+ huesped.Division +"', '0','"+huesped.Nombre+"','"+huesped.ApellidoPaterno+"','"+huesped.ApellidoMaterno+"','"+huesped.DescPuesto+"','N/A',"+grupo+" ,"+area+"" ;

                    dt = db.getQuery(conexionBecarios,query);

                    Session["usuario"] = huesped.Nomina;

                    Response.Redirect("/vistas/Default.aspx");
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    //Response.Write("Nomian " + huesped.Nomina +"<br/>");
                    //Response.Write("Paterno " + huesped.ApellidoPaterno +"<br/>" );
                    //Response.Write("Materno " + huesped.ApellidoMaterno +"<br/>");
                    //Response.Write("AreaPersonal " + huesped.AreaPersonal+"<br/>");
                    //Response.Write("ClaveCampus " + huesped.ClaveCampus+"<br/>");
                    //Response.Write("ClaveCoas" + huesped.ClaveCoas+"<br/>");
                    //Response.Write("ClaveInstitucion" + huesped.ClaveInstitucion+"<br/>");
                    //Response.Write("ClaveCoas " + huesped.ClaveCoas+"<br/>");
                    //Response.Write("Clavepuesto " + huesped.ClavePuesto+"<br/>");
                    //Response.Write("Descripcion de campus " + huesped.DescCampus+"<br/>");
                    //Response.Write("DescContrato " + huesped.DescContrato+"<br/>");
                    //Response.Write("DescPuesto " + huesped.DescPuesto + "<br/>");
                    //Response.Write("Division " + huesped.Division + "<br/>");
                    //Response.Write("Correo " + huesped.Email + "<br/>");
                    //Response.Write("Grupo perosonal " + huesped.GrupoPersonal + "<br/>");
                    //Response.Write("Sub divicion " + huesped.SubDivision + "<br/>");
                    //Response.Write("Fecha nacimiento " + huesped.FechaNacimiento + "<br/>");
                    //Response.Write("Rfc " + huesped.RFC + "<br/>");




                
                    


                    
                
                }
                
            }catch(Exception ess)
            {
                Response.Write(ess.Message.ToString());
            }
            
                
            
        
        }
    }
}