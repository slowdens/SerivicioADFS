using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace SolicitudBecario
{
    public partial class ResultadoEvaluacionBecario : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection();
        int alumnoID;
        int solicitudID;

        protected void Page_Load(object sender, EventArgs e)
        {
            //BtnCerrar.Attributes.Add("onClick", "javascript:history.back(); return false;"); // Scrip para regresar a pagina anterior
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();

            if (!this.IsPostBack)
            {
                LblMatricula.Text = Session["Matricula"].ToString();
                connection.Open(); // Abertura de la conexion

                try
                {
                    loadSolicitudID(); // Recupera el ID de solicitud en base al ID del alumno
                    if (LblSolicitud.Text != "")
                    {
                        loadDatosSolicitante(); // Recupera información del solicitante
                        loadDatosBecario(); // Recupera la informacion del becario
                    }
                }
                catch (Exception ex)
                {
                    Msg.Text = ex.ToString();
                }

                connection.Close();
            }
            /*
            if (Request.QueryString.ToString() != "")
            {
                this.alumnoID = Convert.ToInt32(Request.QueryString.ToString()); // Obtiene el id de solicitud
            }
            else
            {
                this.alumnoID = 0;
            }
            */
            
        }
        protected void loadSolicitudID()
        {
            SqlCommand cmmd = new SqlCommand(
                "SELECT Matricula, RTRIM(LTRIM(Nombre)) + ' ' + RTRIM(LTRIM(Apellido_paterno)) + ' ' + RTRIM(LTRIM(Apellido_materno)) Nombre FROM Tbl_alumnos Where Matricula = '" + LblMatricula.Text.Trim() + "' "
                , connection);

            SqlDataReader reader = cmmd.ExecuteReader();
            if (reader.HasRows) // Si encontró registros
            {
                while (reader.Read())
                {
                        LblIdAlumno.Text = reader["Matricula"].ToString();
                        LblNombreBecario.Text = reader["Nombre"].ToString().Trim();
                }
            }
            else
            {
                verModal("Información", " El becario no se encuentra registrado ");
            }
            if (LblIdAlumno.Text != "")
            {
                cmmd = new SqlCommand(
                    "SELECT TOP 1 "
                        + "id_Misolicitud Solicitud, id_alumno "
                    + "FROM tbl_solicitudes_becarios "
                    + "WHERE Matricula = " + LblIdAlumno.Text.Trim() + " "
                    + "AND id_estatus_asignacion = 2 "
                    + " ORDER BY id_Misolicitud DESC"
                    , connection);

                reader = cmmd.ExecuteReader();
                if (reader.HasRows) // Si encontró registros
                {
                    while (reader.Read())
                    {
                        if (reader["Solicitud"].ToString() != "")
                        {
                            LblSolicitud.Text = reader["Solicitud"].ToString();
                        }
                    }
                }
                else
                {
                    LblSolicitud.Text = "";
                    verModal("Información", " No has sido asignado a ninguna solicitud ");
                }
            }
        }
        protected void loadDatosSolicitante()
        {
            SqlCommand cmmd = new SqlCommand( // Datos del solicitante
                "SELECT "
                    + "RTRIM(LTRIM(emp.Nombre)) + ' ' + RTRIM(LTRIM(Apellido_paterno)) + ' ' + RTRIM(LTRIM(Apellido_materno)) Nombre, "
                    + "Nomina, "
                    + "Puesto, "
                    + "Departamento, "
                    + "Ubicacion_fisica, "
                    + "(SELECT Descripcion FROM cat_periodos WHERE id_periodo = sol.id_periodo) Periodo "
                + "FROM tbl_solicitudes sol "
                + "JOIN tbl_empleados emp ON sol.Matricula = emp.Matricula "
                + "WHERE id_MiSolicitud = " + LblSolicitud.Text.Trim()
                , connection);

            SqlDataReader reader = cmmd.ExecuteReader();
            if (reader.HasRows)// Si encontró registros
            {
                while (reader.Read())
                {
                    LblSolicitante.Text = reader["Nombre"].ToString();
                    LblNomina.Text = reader["Nomina"].ToString();
                    LblPuesto.Text = reader["Puesto"].ToString();
                    LblDepartamento.Text = reader["Departamento"].ToString();
                    LblUbicacionFisica.Text = reader["Ubicacion_fisica"].ToString();
                    LblPeriodo.Text = reader["Periodo"].ToString();
                }
            }
            else
            {
                LblSolicitante.Text = "N/D";
                LblNomina.Text = "N/D";
                LblPuesto.Text = "N/D";
                LblDepartamento.Text = "N/D";
                LblUbicacionFisica.Text = "N/D";
                LblPeriodo.Text = "N/D";
                verModal("Información", " No has sido asignado a ninguna solicitud ");
            }
        }
        protected void loadDatosBecario()
        {
            SqlCommand cmmd = new SqlCommand(
                "SELECT TOP 1 "
                    + "Matricula,"
                    + "rtrim(ltrim(t2.Nombre)) + ' ' + rtrim(ltrim(Apellido_paterno)) + ' ' + RTRIM(ltrim(Apellido_materno)) AS Nombre,"
                    + "Periodo_cursado Periodo,"
                    + "Becario_calificacion Resultado "
                + "FROM tbl_solicitudes_becarios t1 "
                + "JOIN tbl_alumnos t2 ON t2.Matricula = t1.Matricula "
                + "WHERE t1.id_alumno = " + LblIdAlumno.Text.Trim()
                + " ORDER BY t1.id_Misolicitud DESC"
                , connection);

            SqlDataReader dr = cmmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    LblMatricula.Text = dr["Matricula"].ToString().Trim();
                    LblNombreBecario.Text = dr["Nombre"].ToString().Trim();
                    LblCalificacion.Text = dr["Resultado"].ToString().Trim();
                }
            }
            else
            {
                LblMatricula.Text = "N/D";
                LblNombreBecario.Text = "N/D";
                LblCalificacion.Text = "N/D";
            }
            if (LblCalificacion.Text.Trim() == "PENDIENTE")
            {
                verModal("Información", " No has sido evaluado ");
            }
        }
        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }
    }
}