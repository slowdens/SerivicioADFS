using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ServicioBecario.Codigo;
using System.Data;
namespace ServicioBecario.Vistas
{
    public partial class ReasigBeca : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, mensaje;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenarPeriodo();
                llenarNivel();
                //llenarCampus();
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

        public void llenarPeriodo()
        {
            query = "select Periodo,Descripcion from cat_periodos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
        }

        public void llenarNivel()
        {
            query = "select Codigo_nivel_academico,Nivel_academico from cat_nivel_academico";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlNivel.DataTextField = "Nivel_academico";
                ddlNivel.DataValueField = "Codigo_nivel_academico";
                ddlNivel.DataSource = dt;
                ddlNivel.DataBind();
            }

        }

        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre as Campus from cat_campus order by Nombre";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
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
            ddlCampus.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                llenarDatos();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


        public void llenarDatos()
        {
            if (ddlPeriodo.SelectedValue != "-1" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlCampus.SelectedValue == "-1")//1
            {
                query = "sp_reporte_becarios_reasignados  " + ddlPeriodo.SelectedValue + ",null,-1";
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

            query = @"
                    select a.Matricula,
                    a.Nombre +' '+a.Apellido_paterno+' '+a.Apellido_materno as Becario,
                    ar.Nomina_desasocio as Deasocio,
                    p.Descripcion as Periodo,
                    c.Nombre as Campus,
                    --a.Codigo_nivel_academico,
	                na.Nivel_academico as Nivel
                    from tbl_alumno_reasignacion ar
                    inner join tbl_alumnos a on a.Matricula=ar.Matricula
                    inner join cat_periodos p on p.Periodo=ar.Periodo
                    inner join cat_campus c on c.Codigo_campus=a.Codigo_campus
	                inner join cat_nivel_academico na on na.Codigo_nivel_academico=a.Codigo_nivel_academico
	                where p.Periodo!='-1'   ";


            if (ddlPeriodo.SelectedValue != "-1") { query += " and p.Periodo='"+ddlPeriodo.SelectedValue+"'   "; }
            if (ddlNivel.SelectedValue != "-1") { query += " and na.Codigo_nivel_academico = " + ddlNivel.SelectedValue + " "; }
            if(hdfActivarRol.Value=="1")
            {
                if (ddlCampus.SelectedValue != "-1") { query += " and c.Codigo_campus= '" + ddlCampus.SelectedValue + "' "; }
            }
            else
            {
                query += " and c.Codigo_campus= '" + hdfMostrarId.Value + "' ";
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
                gvDatos.DataSource = null;
                gvDatos.DataBind();
                ViewState["dt"] = null;
            }

        }

        protected void btnImageDescargar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                dt = (DataTable)ViewState["dt"];
                descargarInforme(dt);
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


        public void descargarInforme(DataTable ds)
        {
            if (ds != null)
            {
                string attachment = "attachment; filename=Reporte_becarios_reasignados.xls";
                string columnas, registros = "", html = "";
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
                html += registros + "</table>";
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
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

    }
}