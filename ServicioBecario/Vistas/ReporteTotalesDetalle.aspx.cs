using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using ServicioBecario.Codigo;
using Microsoft.Web.Services3.Security.Tokens;
using System.Text.RegularExpressions;

namespace ServicioBecario.Vistas
{
    public partial class ReporteTotalesDetalle : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string Campus = Request.QueryString["Campus"];
                string Periodo = Request.QueryString["Periodo"];
                Tabla.Text = Detalle(Campus, Periodo);
            }

        }



        public string Detalle(string Campus, string Periodo = null)
        {
            string body = "";
            ReporteTotalesDetalle obj = new ReporteTotalesDetalle();
            BasedeDatos db = new BasedeDatos();
            obj.query = "sp_Reportes_Totales_Filtro_Por_Campus '" + Campus + "'";
            
            if(Periodo!="" && Periodo !="0" && Periodo!=null)
            {
                 obj.query += " ,'"+Periodo+"' ";
            }
            try
            {
                obj.dt = db.getQuery(obj.conexionBecarios,obj.query);
                if(obj.dt.Rows.Count>0)
                {
                    body += "<table class='table' id='dtTable'>";
                    body += " <thead><tr><th>Nomina</th><th>Nombre</th><th>Periodo</th><th>Solicitud</th><th>Fecha de Solicitud</th><th>Estatus</th><th>Tipo</th></tr></thead><tbody>";
                    foreach(DataRow Res in obj.dt.Rows)
                    {
                        body += "<tr>";
                        body += "<td>" + Res["Nomina"] + "</td>";
                        body += "<td>" + Res["Nombre"] + "</td>";
                        body += "<td>" + Res["Periodo"] + "</td>";
                        body += "<td>" + Res["id_MiSolicitud"] + "</td>";
                        body += "<td>" + Res["Fecha_solicitud"].ToString().Substring(0,10) + "</td>";
                        body += "<td>" + Res["Solicitud_estatus"] + "</td>";
                        body += "<td>" + Res["Tipo"] + "</td>";
                        body += "</tr>";
                    }
                    body += " </tbody><tfoot><tr><th>Nomina</th><th>Nombre</th><th>Solicitud</th><th>Periodo</th><th>Fecha de Solicitud</th><th>Estatus</th><th>Tipo</th></tr></tfoot></table>";
                    
                }
                else
                {
                    return "<div class='alert alert-warning text-center'><b>Campus Vacío</b></div>";
                }

            }catch(Exception ex)
            {
                return ex.Message;
            }

            
            return body;
        }
        
    }
}