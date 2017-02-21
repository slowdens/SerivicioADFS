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
    public partial class DesAsigna : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            string matricula = Request.QueryString["mar"];
            string periodo = Request.QueryString["per"];
            if (!String.IsNullOrEmpty(matricula) && !String.IsNullOrEmpty(periodo))
            {
                if (!IsPostBack)
                {
                    llenarInformacionAsignacion(periodo, matricula);
                }
            }
            else
            {
                verModal("Alerta", "No hay datos disponibles");
            }


        }

        public void llenarInformacionAsignacion(string periodo, string matricula)
        {
            query = "sp_muestra_datos_a_designar '" + matricula + "','" + periodo + "' ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                lblMatricula.Text = dt.Rows[0]["Matricula"].ToString();
                lblNombreBecario.Text = dt.Rows[0]["Nombre alumno"].ToString();
                lblPeriodo.Text = dt.Rows[0]["Periodo"].ToString();
                lblProyecto.Text = dt.Rows[0]["Proyecto"].ToString();
                lblNivelEstudios.Text = dt.Rows[0]["Nivel academico"].ToString();
                lblCampus.Text = dt.Rows[0]["Campus Becario"].ToString();

                //Datos del responsable
                lblNomina.Text = dt.Rows[0]["Nomina"].ToString();
                lblNombreSb.Text = dt.Rows[0]["Nombre Solicitante"].ToString();
                lblPuesto.Text = dt.Rows[0]["Puesto"].ToString();
                lblDepartamento.Text = dt.Rows[0]["Departamento"].ToString();
                lblUbicacionFisica.Text = dt.Rows[0]["Ubicacion fisica"].ToString();

            }
            else
            {
                verModal("Alerta", "No hay registros que mostrar");
            }
        }

        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void btnDesAsignar_Click(object sender, EventArgs e)
        {
            try
            {
                guardarDes_Asignacion();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }



        public void guardarDes_Asignacion()
        {
            query = "sp_guarda_des_asignacion '" + lblMatricula.Text + "','" + lblPeriodo.Text + "','" + txtJustificacion.Text + "','" + Session["Usuario"].ToString() + "','" + lblNomina.Text + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Exito", "La información se guardo con éxito");
                }
            }
        }

    }
}