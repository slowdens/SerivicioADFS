using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using ServicioBecario.Codigo;


namespace ServicioBecario.Vistas
{
    public partial class Solitantenoev : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, mensaje;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenarfiltrosPeriodo();
                
                //llenarFiltrosCampus();
                vermisosdeCampus();
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
                    ddlFiltrarCampus.Visible = true;
                    llenarFiltrosCampus();
                    lblCampus.Visible = false;
                }
                else
                {
                    lblCampus.Text = dt.Rows[0]["Campus"].ToString();
                    hdfMostrarId.Value = dt.Rows[0]["Codigo_campus"].ToString();

                    hdfActivarRol.Value = "0";
                    ddlFiltrarCampus.Visible = false;
                }
            }
            else
            {
                verModal("Alerta", "El usuario no se encontró registrado");
            }
        }

        public void llenarfiltrosPeriodo()
        {
            query = "select Periodo,Descripcion from cat_periodos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarPeriodo.DataTextField = "Descripcion";
                ddlFiltrarPeriodo.DataValueField = "Periodo";
                ddlFiltrarPeriodo.DataSource = dt;
                ddlFiltrarPeriodo.DataBind();
            }

        }

        protected void ddlFiltrarPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarPeriodo.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }
       

       
        public void llenarFiltrosCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus order by Nombre";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarCampus.DataTextField = "Nombre";
                ddlFiltrarCampus.DataValueField = "Codigo_campus";
                ddlFiltrarCampus.DataSource = dt;
                ddlFiltrarCampus.DataBind();
            }
        }

        protected void ddlFiltrarCampus_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarCampus.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }


        public void descarExcel(DataTable dts)
        {
            if (dts != null)
            {
                string attachment = "attachment; filename=SBNoEvaluados.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "UTF-8";
                string tab = "";
                string html, columnas = "", registros = "";

                html = @"<table border='0'>
                            <tr>
                                <td colspan='6' style='text-align:center;font-size:20px;color:#113FB9'>
                                    REPORTE DE SOLICITANTES NO EVALUADOS
                                </td>
                            <tr>";

                foreach (DataColumn dc in dts.Columns)
                {
                    columnas += @"<th>" + dc.ColumnName + "   </th>";
                }

                html += columnas + @"</tr>";
                int i;
                foreach (DataRow dr in dts.Rows)
                {
                    tab = "";
                    registros += "<tr>";
                    for (i = 0; i < dts.Columns.Count; i++)
                    {
                        registros += "<td>" + dr[i].ToString() + " </td>";
                    }
                    registros += "</tr>";
                }

                html += registros + @"</table>";
                Response.Write(html);
                Response.End();
            }
            else
            {
                verModal("Alerta", "No hay información para descargar");
            }
        }



        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                llenarGrid();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void llenarGrid()
        {
            if(hdfActivarRol.Value=="1")
            {
                query = "sp_solicitante_no_evaluados_reporte '" + ddlFiltrarPeriodo.SelectedValue + "','" + ddlFiltrarCampus.SelectedValue + "'";
            }
            else
            {
                query = "sp_solicitante_no_evaluados_reporte '" + ddlFiltrarPeriodo.SelectedValue + "','" + hdfMostrarId.Value + "'";
            }
            

            
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                gvDatos.DataSource = dt;
                gvDatos.DataBind();
                ViewState["dt"] = dt;

            }
            else
            {
                verModal("Alerta", "No se encontro la información");
                gvDatos.DataSource = null;
                gvDatos.DataBind();
                ViewState["dt"] = null;

            }
        }


        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void btnimgDescargar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                dt = (DataTable)ViewState["dt"];
                descarExcel(dt);
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDatos.PageIndex = e.NewPageIndex;
                llenarGrid();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


        
    }
}