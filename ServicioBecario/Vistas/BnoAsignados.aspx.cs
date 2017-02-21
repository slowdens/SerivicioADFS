using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using ServicioBecario.Codigo;
namespace ServicioBecario.Vistas
{
    public partial class BnoAsignados : System.Web.UI.Page
    {

        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    loadPeriodo();
                    vermisosdeCampus();
                }
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }


        public void vermisosdeCampus()
        {
            query = "Sp_muestra_perifil_campus '" + Session["Usuario"].ToString() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["id_rol"].ToString() == "4")//Este es administrador multicampus
                {
                    hdfActivarRol.Value = "1";
                    ddlCampus.Visible = true;
                    laadCampus();
                    lblCampus.Visible = false;
                }
                else
                {
                    lblCampus.Text = dt.Rows[0]["Campus"].ToString();
                    hdfMostrarId.Value = dt.Rows[0]["Codigo_campus"].ToString();

                    hdfActivarRol.Value = "0";
                    ddlCampus.Visible = false;
                }
            }
            else
            {
                verModal("Alerta", "El usuario no se encontró registrado");
            }
        }
        public void loadPeriodo()
        {
            query = "select Periodo,Descripcion from cat_periodos where Activo=1";
            dt = db.getQuery(conexionBecarios,query);
            if (dt.Rows.Count > 0)
            {
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
        }

        public void laadCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus where Codigo_campus !='PRT' order by Nombre asc";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }


        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void btnInformacion_Click(object sender, EventArgs e)
        {
            try
            {
                mostrarInformacion();               

            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


        public void mostrarInformacion()
        {
            if(hdfActivarRol.Value=="1")
            {
                query = "sp_informe_no_asignados '" + ddlCampus.SelectedValue + "' , '" + ddlPeriodo.SelectedValue + "' ";
            }
            else
            {
                query = "sp_informe_no_asignados '" + hdfMostrarId.Value + "' , '" + ddlPeriodo.SelectedValue + "' ";
            }
            
            dt = db.getQuery(conexionBecarios,query);
            if (dt.Rows.Count > 0)
            {
                gvDatos.DataSource = dt;
                gvDatos.DataBind();
                ViewState["dt"] = dt;

            }
            else
            {
                gvDatos.DataSource = null;
                gvDatos.DataBind();
                ViewState["dt"] = null;
                verModal("Alerta","No existen registros");
            }
        }

        protected void IbtDescargar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                dt = (DataTable)ViewState["dt"];
                descargarReporte(dt);
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }


        public void descargarReporte(DataTable ds)
        {
            if (ds != null)
            {
                string attachment = "attachment; filename=Reportes_becarios_asignados.xls";
                string columnas = "", reglones = "", html = "";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "UTF-8";
                html = @"<table>
                            <tr>
                                <td colspan='6' style='text-align:center;font-size:20px;color:#113FB9'>
                                    REPORTE DE BECARIOS NO ASIGNADOS
                                </td>
                            </tr>
                            <tr>";
                foreach (DataColumn dc in ds.Columns)
                {
                    columnas += "<th>" + dc.ColumnName + "</th>";
                }
                html += "</tr>" + columnas;
                int i;
                foreach (DataRow dr in ds.Rows)
                {
                    reglones += "<tr>";
                    for (i = 0; i < ds.Columns.Count; i++)
                    {
                        reglones += "<td>" +HttpUtility.HtmlDecode( dr[i].ToString()) + "</td>";
                    }
                    reglones += "</tr>";

                }
                html += reglones + "</table>";
                Response.Write(html);
                Response.End();
            }
            else
            {
                verModal("Alerta", "No hay información para descargar");
            }
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                  gvDatos.PageIndex = e.NewPageIndex;
                mostrarInformacion();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
    }
}