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
    public partial class ReporteTotales : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LlenarGrid();
                LlenarPeriodos();
            }
        }

        protected void ListaPeriodos_DataBound(object sender, EventArgs e)
        {
            ListaPeriodos.Items.Insert(0, new ListItem("-- Todos los periodos --", "-1"));
        }

        protected void Filtrar_Click(object sender, EventArgs e)
        {
            LlenarGrid();
        }

        public void LlenarGrid()
        {
            try
            {
                
                query = "EXEC sp_Reportes_Totales_Filtro ";
                if (ListaPeriodos.SelectedValue != "-1") { query += " '" + ListaPeriodos.SelectedValue + "'"; }
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    gvDatos.DataSource = dt;
                    gvDatos.DataBind();
                }
                else
                {
                    gvDatos.DataSource = null;
                    gvDatos.DataBind();
                }
                ViewState["dt"] = dt;
            }catch(Exception ex)
            {
                verModal("Error"," No se pueden visualizar los datos");
            }
        }
        public void LlenarPeriodos()
        {
            query = "select Periodo,Descripcion from cat_periodos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ListaPeriodos.DataValueField = "Periodo";
                ListaPeriodos.DataTextField = "Descripcion";
                ListaPeriodos.DataSource = dt;
                ListaPeriodos.DataBind();
            }
        }
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
            gvDatos.PageIndex = e.NewPageIndex;
            LlenarGrid();
            }catch(Exception ex)
            {
                verModal("Error","No pudimos paginar");
            }
            
        }

        [WebMethod]
        public static string CampusDetalle(string Campus, string Periodo)
        {
            string body = "";
            ReporteTotales obj = new ReporteTotales();
            BasedeDatos db = new BasedeDatos();
            obj.query = "sp_Reportes_Totales_Filtro_Por_Campus '" + Campus + "'";
  
            if(Periodo!="" && Periodo !="0")
            {
                 obj.query += " '"+Periodo+"' ";
            }
            try
            {
                obj.dt = db.getQuery(obj.conexionBecarios,obj.query);
                if(obj.dt.Rows.Count>0)
                {
                    body += "<center><b>Solicitantes<b></center><table class='table' id='dtTable'>";
                    body += " <thead><tr><th>Nómina</th><th>Nombre</th><th>Periodo</th><th>Solicitud</th><th>Fecha de Solicitud</th><th>Estatus</th><th>Tipo</th></tr></thead><tbody>";
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
                    body += " </tbody><tfoot><tr><th>Nomina</th><th>Nombre</th><th>Periodo</th><th>Solicitud</th><th>Fecha de Solicitud</th><th>Estatus</th><th>Tipo</th></tr></tfoot></table>";
                    
                }
                else
                {
                    body += "<div class='alert alert-warning text-center'><b>Sin solicitantes</b></div>";
                }

                obj.query = "sp_Reportes_Totales_Filtro_Por_Campus_Becario '" + Campus + "'";
             
                if (Periodo != "" && Periodo != "0")
                {
                    obj.query += " '" + Periodo + "' ";
                }

                obj.dt = db.getQuery(obj.conexionBecarios, obj.query);
                if (obj.dt.Rows.Count > 0)
                {
                    body += "<center><b>Becarios<b></center><table class='table' id='dtTable2'>";
                    body += " <thead><tr><th>Matrícula</th><th>Nombre</th><th>Calificación</th><th>Programa</th><th>Nivel</th></tr></thead><tbody>";
                    foreach (DataRow Res in obj.dt.Rows)
                    {
                       
                            body += "<tr>";
                            body += "<td>" + Res["Matricula"] + "</td>";
                            body += "<td>" + Res["Nombre"] + "</td>";
                            body += "<td>" + Res["Becario_calificacion"] + "</td>";
                            body += "<td>" + Res["Nombre_programa_academico"] + "</td>";
                            body += "<td>" + Res["Nivel_academico"]+"</td>";
                     
                            body += "</tr>";
                        
                   
                    }
                    body += " </tbody><tfoot><tr><th>Matrícula</th><th>Nombre</th><th>Calificación</th><th>Programa</th><th>Nivel</th></tr></tfoot></table>";

                }
                else
                {
                    body += "<div class='alert alert-warning text-center'><b>Sin Becarios</b></div>";
                }



            }catch(Exception ex)
            {
                return ex.Message;
            }

            
            return body;
        }

        protected void btnimgnDescargar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                dt = (DataTable)ViewState["dt"];//Sacamos los dato del datable                
                descargarInfo(dt);//Mandamos la informacion a descargar.             
            }
            catch (Exception es)
            {
                string error = es.Message.ToString();
                verModal("Error", es.Message.ToString());
            }
        }
        public void descargarInfo(DataTable ds)
        {
            if (ds != null)
            {
                string html = "", columas = "", registros = "";
                string attachment = "attachment; filename=ReporteBecarios.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "UTF-8";
                string tab = "";
                html = @"<table>                             
                          <tr>
                                <td colspan='13' style='text-align:center;font-size:20px;color:#113FB9'>
                                    REPORTE GENERAL POR CAMPUS
                                </td>    
                          <tr>";
                foreach (DataColumn dc in ds.Columns)
                {

                    columas += "<th>" + dc.ColumnName + "</th>";
                }
                html += columas + @"</tr>
                        ";

                int i;

                foreach (DataRow dr in ds.Rows)
                {
                    tab = "";
                    registros += "<tr>";
                    for (i = 0; i < ds.Columns.Count; i++)
                    {
                        registros += "<td>" + dr[i].ToString() + "</td>";
                    }
                    registros += "</tr>";
                }
                html += registros + "</table>";
                Response.Write(html);
                Response.End();

            }
            else
            {
                verModal("Alerta", "No hay información para descargar");
            }
        }

    }
}