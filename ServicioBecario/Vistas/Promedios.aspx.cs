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
    public partial class Promedios : System.Web.UI.Page
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
                    llenarCampus();
                    llenarEstatus();
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void llenarEstatus()
        {
            query = "select id_estatus_promedio,Estatus from cat_estatus_promedios";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlEstatus.DataTextField = "Estatus";
                ddlEstatus.DataValueField = "id_estatus_promedio";
                ddlEstatus.DataSource = dt;
                ddlEstatus.DataBind();
            }
        }

        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus where Codigo_campus !='PRT'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataTextField = "Nombre";
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

        protected void ddlCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlgrid.Visible = true;
                llenarGrid();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


        public void llenarGrid()
        {
            if (ddlCampus.SelectedValue != "")
            {
                query = "EXEC sp_catalogo_promedio_por_campus '" + ddlCampus.SelectedValue + "'";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    gvDatos.DataSource = dt;
                    gvDatos.DataBind();
                }
            }

        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void gvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hdi_id_promedio.Value = gvDatos.SelectedDataKey.Value.ToString();
                txtMayor.Text = gvDatos.SelectedRow.Cells[2].Text;
                txtMenor.Text = gvDatos.SelectedRow.Cells[3].Text;
                txtMayor.Text = txtMayor.Text.Replace(',', '.');
                txtMenor.Text = txtMenor.Text.Replace(',', '.');

                ddlEstatus.SelectedValue = ddlEstatus.Items.FindByText(gvDatos.SelectedRow.Cells[4].Text).Value;
                PnlModificacion.Visible = true;
                btnModificar.Visible = true;
                btnCancelar.Visible = true;
                ddlCampus.Enabled = false;


            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void ddlEstatus_DataBound(object sender, EventArgs e)
        {
            ddlEstatus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        
        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                query = "EXEC sp_actualiza_promedios " + hdi_id_promedio.Value + "," + txtMayor.Text + "," + txtMenor.Text + "";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                    {
                        //actualizamos el grid
                        llenarGrid();
                        PnlModificacion.Visible = false;
                        verModal("Exito", "Se modifico correctamente el promedio");
                        ddlCampus.Enabled = true;
                        btnCancelar.Visible = false;
                        btnModificar.Visible = false;
                    }
                    else
                    {
                        verModal("Error", "No se modifico el promedio");
                    }
                }

            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void limpiarcontroles()
        {
            txtMayor.Text = "";
            txtMenor.Text = "";
            ddlEstatus.SelectedValue = "";
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                btnModificar.Visible = false;
                btnCancelar.Visible = false;
                limpiarcontroles();
                PnlModificacion.Visible = false;
                gvDatos.SelectedIndex = -1;
                ddlCampus.Enabled = true;
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        
    }
}