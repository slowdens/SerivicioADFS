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
    public partial class ReporteProyectos : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, mensaje;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                llenarPeriodo();
                //llenarCampus();
                vermisosdeCampus();
            }
        }

        public void llenarPeriodo()
        {
            query = "select Periodo,Descripcion from cat_periodos";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
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
                    llenarCampus();
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

        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus order by Nombre";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                llenarGrid();
            }catch(Exception es )
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void llenarGrid()
        {

            if(hdfActivarRol.Value=="1")
            {
                if (ddlPeriodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && ddlValoracion.SelectedValue == "-1")//1
                {
                    query = "sp_reporte_Proyectos  '" + ddlPeriodo.SelectedValue + "','-1' ,-1";
                }
                if (ddlPeriodo.SelectedValue == "-1" && ddlCampus.SelectedValue != "-1" && ddlValoracion.SelectedValue == "-1")//2
                {
                    query = "sp_reporte_Proyectos '-1' , '" + ddlCampus.SelectedValue + "',-1";
                }
                if (ddlPeriodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlValoracion.SelectedValue != "-1")//3
                {
                    query = "sp_reporte_Proyectos  '-1','-1'," + ddlValoracion.SelectedValue + "";
                }
                if (ddlPeriodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlValoracion.SelectedValue == "-1")//4
                {
                    query = "sp_reporte_Proyectos '" + ddlPeriodo.SelectedValue + "', '" + ddlCampus.SelectedValue + "',-1";
                }
                if (ddlPeriodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && ddlValoracion.SelectedValue != "-1")//5
                {
                    query = "sp_reporte_Proyectos '" + ddlPeriodo.SelectedValue + "','-1'," + ddlValoracion.SelectedValue + "";
                }
                if (ddlPeriodo.SelectedValue == "-1" && ddlCampus.SelectedValue != "-1" && ddlValoracion.SelectedValue != "-1")//6
                {
                    query = "sp_reporte_Proyectos '-1', '" + ddlCampus.SelectedValue + "'," + ddlValoracion.SelectedValue + "";
                }
                if (ddlPeriodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlValoracion.SelectedValue != "-1")//7
                {
                    query = "sp_reporte_Proyectos '" + ddlPeriodo.SelectedValue + "','" + ddlCampus.SelectedValue + "'," + ddlValoracion.SelectedValue + "";
                }
                if (ddlPeriodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlValoracion.SelectedValue == "-1")//8
                {
                    query = "sp_reporte_Proyectos '-1','-1',-1";
                }
            }
            else
            {
                if (ddlPeriodo.SelectedValue != "-1" && hdfMostrarId.Value == "-1" && ddlValoracion.SelectedValue == "-1")//1
                {
                    query = "sp_reporte_Proyectos  '" + ddlPeriodo.SelectedValue + "','-1' ,-1";
                }
                if (ddlPeriodo.SelectedValue == "-1" && hdfMostrarId.Value != "-1" && ddlValoracion.SelectedValue == "-1")//2
                {
                    query = "sp_reporte_Proyectos '-1' , '" + hdfMostrarId.Value + "',-1";
                }
                if (ddlPeriodo.SelectedValue == "-1" && hdfMostrarId.Value == "-1" && ddlValoracion.SelectedValue != "-1")//3
                {
                    query = "sp_reporte_Proyectos  '-1','-1'," + ddlValoracion.SelectedValue + "";
                }
                if (ddlPeriodo.SelectedValue != "-1" && hdfMostrarId.Value != "-1" && ddlValoracion.SelectedValue == "-1")//4
                {
                    query = "sp_reporte_Proyectos '" + ddlPeriodo.SelectedValue + "', '" + hdfMostrarId.Value + "',-1";
                }
                if (ddlPeriodo.SelectedValue != "-1" && hdfMostrarId.Value == "-1" && ddlValoracion.SelectedValue != "-1")//5
                {
                    query = "sp_reporte_Proyectos '" + ddlPeriodo.SelectedValue + "','-1'," + ddlValoracion.SelectedValue + "";
                }
                if (ddlPeriodo.SelectedValue == "-1" && hdfMostrarId.Value != "-1" && ddlValoracion.SelectedValue != "-1")//6
                {
                    query = "sp_reporte_Proyectos '-1', '" + hdfMostrarId.Value + "'," + ddlValoracion.SelectedValue + "";
                }
                if (ddlPeriodo.SelectedValue != "-1" && hdfMostrarId.Value != "-1" && ddlValoracion.SelectedValue != "-1")//7
                {
                    query = "sp_reporte_Proyectos '" + ddlPeriodo.SelectedValue + "','" + hdfMostrarId.Value + "'," + ddlValoracion.SelectedValue + "";
                }
                if (ddlPeriodo.SelectedValue == "-1" && hdfMostrarId.Value == "-1" && ddlValoracion.SelectedValue == "-1")//8
                {
                    query = "sp_reporte_Proyectos '-1','-1',-1";
                }
            }
            
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                GvDatos.DataSource = dt;
                GvDatos.DataBind();
                ViewState["dt"] = dt;
            }
            else
            {
                GvDatos.DataSource = null;
                GvDatos.DataBind();
                ViewState["dt"] = null;
                verModal("Alerta","No se encontro la informacion filtrada");
            }
            
        }



        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void imgbtnDescargar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                dt=(DataTable)ViewState["dt"];
                descargarReporte(dt);
            }catch(Exception es ){
                verModal("Error",es.Message.ToString());

            }
        }

       public void descargarReporte(DataTable ds)
       {
           if (ds != null)
           {
               string attachment = "attachment; filename=Reportes_por_proyecto.xls";
               string columnas = "", reglones = "", html = "";
               Response.ClearContent();
               Response.AddHeader("content-disposition", attachment);
               Response.ContentType = "application/vnd.ms-excel";
               Response.Charset = "UTF-8";
               html = @"<table>
                            <tr>
                                <td colspan='6' style='text-align:center;font-size:20px;color:#113FB9'>
                                    REPORTE DE PROYECTOS
                                </td>
                            </tr>
                            <tr>";
               foreach (DataColumn dc in ds.Columns)
               {
                   columnas += "<th>" + dc.ColumnName + "</th>";
               }
               html += "</tr>"+columnas;               
               int i;               
               foreach (DataRow dr in ds.Rows)
               {                  
                   reglones += "<tr>";
                   for (i = 0; i < ds.Columns.Count; i++)
                   {                    
                       reglones += "<td>" + dr[i].ToString() + "</td>";                       
                   }
                   reglones += "</tr>";
                   
               }
               html += reglones+ "</table>";
               Response.Write(html);
               Response.End();
           }
           else
           {
               verModal("Alerta", "No hay información para descargar");
           }
       }

       protected void GvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
       {
           try
           {
               GvDatos.PageIndex = e.NewPageIndex;
               llenarGrid();
           }catch(Exception es)
           {
               verModal("Error",es.Message.ToString());
           }
       }



    }
}