using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServicioBecario.Codigo;
using System.Data;
using System.Configuration;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using mx.itesm.portales.libs.identidad;


namespace ServicioBecario.Vistas
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, caracter;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        Trabajadores tr = new Trabajadores();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //Session["usuario"] = "L00000002";
            string url = obtenerUrl(HttpContext.Current.Request.Url.AbsoluteUri);
            string[] pantalla = { "Envio.aspx",  "ReporteProyectos.aspx", "ReporteBecariosReasingados.aspx", "SolicitudEspeciales.aspx", "SbNoEvaluados.aspx", "EspecificaIndividual.aspx", "AccesoDenegado.aspx", "TableroTramite.aspx", "DesAsigna.aspx", "Mostrar.aspx", "Default.aspx" };



            var mail = Request.Cookies["MailUserPortal"].Value;
            mx.itesm.portales.libs.identidad.Usuario huesped = Autentica.AutenticaUsuario(mail, "NuevaNomina");
            Response.Write("El valor de la cookies es = " + mail);



            if(!string.IsNullOrEmpty(Session["usuario"].ToString()))
            {
                lblNominaMaster.Text = Session["Usuario"].ToString();
                Hdfusuario.Value = Session["Usuario"].ToString();      
                query = "sp_rol_empledo  '" + Hdfusuario.Value + "'";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    lblRol.Text = "<label >Rol &nbsp;&nbsp;&nbsp;</label> " + dt.Rows[0]["Rol"].ToString();
                }
                if (!Page.IsPostBack)
                {

                    int s = Array.IndexOf(pantalla, url);
                    if (!verPermiso(url, Session["Usuario"].ToString()) && Array.IndexOf(pantalla, url) == -1)
                    {
                        Response.Redirect("/vistas/AccesoDenegado.aspx");
                        //Response.Redirect("/vistas/AccesoDenegado.aspx?" + urlSharepoint);
                    }

                }
            }
            else
            {
               // Response.Redirect("/Login.aspx");
            }
            


        }
        public string obtenerUrl(string cadena)
        {
            cadena = cadena.Substring(cadena.LastIndexOf('/') + 1);
            if (cadena.Contains("?"))
            {
                cadena = cadena.Substring(0, cadena.IndexOf('?'));
            }

            return cadena;
        }
        public bool verPermiso(string url, string usuario)
        {
            bool bandera = false;
            
           //query = "Pagina_permitida '" + usuario + "','" + url + "' ";
           query = "Pagina_permitida_nueva '" + usuario + "','" + url + "' ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Si")
                {
                    bandera = true;
                }
            }

            return bandera;
        }

        protected void btnCerrar_Click(object sender, ImageClickEventArgs e)
        {
            Session.Abandon();
            Response.Redirect("/Login.aspx");
        }
    }
}