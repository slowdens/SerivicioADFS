using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data;
using ServicioBecario.Codigo;
using System.Web.UI.WebControls;

namespace ServicioBecario.Vistas
{
    public partial class CampusIndicador : System.Web.UI.Page
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
                    llenarGridCampus();
                    llenarCampus();
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
            

        }

        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus order by Nombre asc ";
            dt = db.getQuery(conexionBecarios,query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }


        public void llenarGridCampus()
        {
            query = "sp_muestra_campus_inducador";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                gvCampus.DataSource = dt;
                gvCampus.DataBind();
            }
        }

        protected void gvCampus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCampus.PageIndex = e.NewPageIndex;
                llenarGridCampus();
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        

        protected void gvCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               pnlModificar.Visible = true;
               hdf_id_campus.Value = gvCampus.SelectedDataKey.Value.ToString();
               lblCampus.Text =gvCampus.SelectedRow.Cells[1].Text;
               pnlCampus.Visible = false;
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void ddlCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlCuenta.SelectedValue=="Si")
            {
                pnlGuardarCampus.Visible = false;
                query = "sp_quita_asignacion_campus_indicador " + hdf_id_campus.Value + "";
                db.getQuery(conexionBecarios, query);
                pnlCampus.Visible = true;
                llenarGridCampus();
                pnlModificar.Visible = false;
                
            }
            else
            {
                pnlGuardarCampus.Visible = true;
            }
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            try
            {
                query = "sp_agregarCampus_asignador '" + hdf_id_campus.Value + "'," + ddlCampus.SelectedValue + ",'" + ddlCuenta.SelectedValue + "'";
                dt = db.getQuery(conexionBecarios,query);
                verModal("Exito","Se asignó el campus asignador");
                pnlCampus.Visible = true;
                pnlModificar.Visible = false;
                llenarGridCampus();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                pnlModificar.Visible = false;
                pnlCampus.Visible = true;
                gvCampus.SelectedIndex = -1;
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }


    }
}