using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServicioBecario.Codigo;

namespace ServicioBecario.Vistas
{
    public partial class NotificacionSoli : System.Web.UI.Page
    {

        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, caracter;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenarPeriodo();
            }
        }
        public void llenarPeriodo()
        {
            query = "select Periodo,Descripcion from cat_periodos where Activo=1";
            dt = db.getQuery(conexionBecarios,query);
            if (dt.Rows.Count > 0)
            {
                ddlperiodo.DataTextField = "Descripcion";
                ddlperiodo.DataValueField = "Periodo";
                ddlperiodo.DataSource = dt;
                ddlperiodo.DataBind();
            }
        }

        protected void btnVerificar_Click(object sender, EventArgs e)
        {
            try
            {
                query = "sp_mostrar_notificacion_solocitante_evaluacion " + ddlperiodo.SelectedValue + ",'" + Session["Usuario"].ToString() + "'";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    if(dt.Rows[0]["Mensaje"].ToString()=="Ok")
                    {
                        lblResultado.Text = dt.Rows[0]["Cuerpo"].ToString();
                    }
                    else
                    {
                        verModal("Alerta","No hay notificaciones");
                    }
                }
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }



        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void ddlperiodo_DataBound(object sender, EventArgs e)
        {
            ddlperiodo.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }



    }
}