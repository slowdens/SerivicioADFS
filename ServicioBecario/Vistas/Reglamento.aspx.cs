using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ServicioBecario.Codigo;
namespace ServicioBecario.Vistas
{
    public partial class Reglamento : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
    
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!IsPostBack)
                {
                    llenarGrid();
                }
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                modificarLink();
                limpiarTExto();

            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        public void llenarGrid()
        {
            query = "sp_muestra_reglamento_link";
            dt = db.getQuery(conexionBecarios,query);
            if (dt.Rows.Count > 0)
            {
                Gvdatos.DataSource = dt;
                Gvdatos.DataBind();
            }
            else
            {
                Gvdatos.DataSource =null;
                Gvdatos.DataBind();
                verModal("Alerta","No hay registros");
            }
        }

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void Gvdatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               txturl.Text = Gvdatos.SelectedRow.Cells[0].Text;
               pnlmodificar.Visible = true;
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        public void modificarLink()
        {
            if (!txturl.Text.Contains("http"))
            {
                txturl.Text ="http://"+ txturl.Text.Trim();
            }
            query = "sp_modifica_link '" + txturl.Text.Trim() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                verModal("Exito", "Se modifico correctamente el registro");
                llenarGrid();
                limpiarTExto();
                pnlmodificar.Visible = false;
            }
            else
            {
                verModal("Alerta", "Error al modificar");
            }
        }

        public void limpiarTExto()
        {
            txturl.Text = "";
        }

    }
}