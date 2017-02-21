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
    public partial class TableroTramite : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string matricula = Request.QueryString["mar"];
                string periodo = Request.QueryString["per"];
                if (!String.IsNullOrEmpty(matricula) && !String.IsNullOrEmpty(periodo))
                {
                    if (!IsPostBack)
                    {
                        mostrarDatosGenerales(matricula, periodo);
                    }
                }
                else
                {
                    verModal("Alerta", "No hay información que mostrar");
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }

        }

        public void mostrarDatosGenerales(string matricula, string periodo)
        {
            query = "sp_tablero_tramites '" + matricula + "' , '" + periodo + "' ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvDatosGenerales.DataSource = dt;
                GvDatosGenerales.DataBind();
            }
            else
            {
                verModal("Alerta", "No existen datos para mostrar");
                
            }
        }



        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void btnMostrarHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                string periodo = sacarPeriodo(btn);
                string matricula = sacarMatricula(periodo, btn);
                pnlHistoriaAsignacion.Visible = true;
                mostrarHistorialReasignacion(matricula, periodo);
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public string sacarPeriodo(Button mds)
        {
            string periodo = mds.CommandName;
            periodo = periodo.Substring(0, periodo.IndexOf('!'));
            return periodo;
        }
        public string sacarMatricula(string periodo, Button btn)
        {
            string matricula = btn.CommandName;
            matricula = matricula.Replace(periodo + "!", "");
            return matricula.Trim();
        }



        public void mostrarHistorialReasignacion(string matricula, string periodo)
        {
            query = "sp_muestra_historial_asignacion '" + matricula + "','" + periodo + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvHistorialAsignacion.DataSource = dt;
                GvHistorialAsignacion.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay información de reasignaciones para el periodo: " + periodo + "   y matrícula: " + matricula + "");
            }
        }

        protected void btnDesAsignacion_Click(object sender, EventArgs e)
        {
            try
            {
                Button bnt = (Button)sender;
                string periodo = sacarPeriodo(bnt);
                string matricula = sacarMatricula(periodo, bnt);

                //string urlSharepoint = System.Configuration.ConfigurationManager.AppSettings["urlsharepoint"];
                //urlSharepoint = urlSharepoint.Replace("**", "&");
                //Response.Redirect("DesAsigna.aspx?" + urlSharepoint + "&per=" + periodo + "&mar=" + matricula + "");

                Response.Redirect("DesAsigna.aspx?per=" + periodo + "&mar=" + matricula + "");
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void Volver_Click(object sender, EventArgs e)
        {
            Response.Redirect("../vistas/SeguimientoAsignacion.aspx");
        }


    }
}