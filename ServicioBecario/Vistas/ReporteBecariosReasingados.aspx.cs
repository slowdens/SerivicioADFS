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
    public partial class ReporteBecariosReasingados : System.Web.UI.Page
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
                llenarNivel();
                llenarCampus();
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

        public void llenarNivel()
        {
            query = "select id_nivel_academico,Nivel_academico from cat_nivel_academico";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                ddlNivel.DataTextField = "Nivel_academico";
                ddlNivel.DataValueField = "id_nivel_academico";
                ddlNivel.DataSource = dt;
                ddlNivel.DataBind();
            }

        }

        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre as Campus from cat_campus";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                ddlCampus.DataTextField = "Campus";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void ddlNivel_DataBound(object sender, EventArgs e)
        {
            ddlNivel.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                llenarDatos();
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }


        public void llenarDatos()
        {
            if (ddlPeriodo.SelectedValue != "-1" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlCampus.SelectedValue =="-1" )//1
            {
                query = "sp_reporte_becarios_reasignados  "+ddlPeriodo.SelectedValue+",null,-1";
            }
            if (ddlPeriodo.SelectedValue == "-1" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlCampus.SelectedValue == "-1")//2
            {
                query = "sp_reporte_becarios_reasignados -1,'" + ddlNivel.SelectedItem.Text + "', -1";
            }
            if (ddlPeriodo.SelectedValue == "-1" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlCampus.SelectedValue != "-1")//3
            {
                query = "sp_reporte_becarios_reasignados -1,null," + ddlCampus.SelectedValue + "";
            }
            if (ddlPeriodo.SelectedValue != "-1" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlCampus.SelectedValue == "-1")//4
            {
                query = "sp_reporte_becarios_reasignados " + ddlPeriodo.SelectedValue + ",'" + ddlNivel.SelectedItem.Text + "',-1 ";
            }
            if (ddlPeriodo.SelectedValue != "-1" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlCampus.SelectedValue != "-1")//5
            {
                query = "sp_reporte_becarios_reasignados " + ddlPeriodo.SelectedValue + ",null," + ddlCampus.SelectedValue + "";
            }
            if (ddlPeriodo.SelectedValue == "-1" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlCampus.SelectedValue != "-1")//6
            {
                query = "sp_reporte_becarios_reasignados  -1, '" + ddlNivel.SelectedItem.Text + "'," + ddlCampus.SelectedValue + "";
            }
            if (ddlPeriodo.SelectedValue != "-1" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlCampus.SelectedValue != "-1")//7
            {
                query = "sp_reporte_becarios_reasignados  " + ddlPeriodo.SelectedValue + ", '" + ddlNivel.SelectedItem.Text + "'," + ddlCampus.SelectedValue + "";
            }
            if (ddlPeriodo.SelectedValue == "-1" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlCampus.SelectedValue == "-1")//8
            {
                query = "sp_reporte_becarios_reasignados -1,null,-1";
            }
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
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
            }
            
        }

        protected void btnImageDescargar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                dt =(DataTable) ViewState["dt"];
                descargarInforme(dt);
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }


        public void descargarInforme(DataTable ds)
        {
            if (ds != null)
            {
                string attachment = "attachment; filename=Reporte_becarios_reasignados.xls";
                string columnas, registros="", html="";
                Response.ClearContent();                
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "UTF-8";
                string tab = "";
                html = @"<table>
                    <tr>
                                <td colspan='6' style='text-align:center;font-size:20px;color:#113FB9'>
                                    REPORTE DE BECARIOS REASIGNADOS
                                </td>
                            <tr>";
                columnas = @"<tr>";
                foreach (DataColumn dc in ds.Columns)
                {                    
                    columnas += @"<th>" + dc.ColumnName + " </th>";
                }
                columnas += "</tr>";

                html += columnas;                
                int i;
                foreach (DataRow dr in ds.Rows)
                {                    
                    registros += "<tr>";
                    for (i = 0; i < ds.Columns.Count; i++)
                    {                        
                        registros += "<td>" + dr[i].ToString() + " </td>";
                    }
                    registros += "</tr>";                    
                }
                html += registros+"</table>";
                Response.Write(html);
                Response.End();                
            }
            else
            {
                verModal("Alerta", "No hay información para descargar");
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDatos.PageIndex = e.NewPageIndex;
                llenarDatos();
            }catch(Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

    }
}