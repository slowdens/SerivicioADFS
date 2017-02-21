using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace ServicioBecario.Vistas
{
    public partial class MiAsignacion : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection();
        string matricula;
        int alumnoID;
        int solicitudID;

        protected void Page_Load(object sender, EventArgs e)
        {
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
            if (!this.IsPostBack)
            {
                this.matricula = Session["Matricula"].ToString();

                connection.Open(); // Abertura de la conexion
                try
                {
                    loadDatosBecario();
                }
                catch (Exception ex)
                {
                    Msg.Text = ex.ToString();
                }
                connection.Close();
            }
        }

        protected void loadDatosBecario()
        {
            SqlCommand cmmd = new SqlCommand(
                "SELECT TOP 1 "
	                + "t1.id_Misolicitud AS Solicitud,"	                
                    + "id_estatus_asignacion, "
	                + "Matricula,"
	                + "rtrim(ltrim(t2.Nombre)) + ' ' + rtrim(ltrim(Apellido_paterno)) + ' ' + RTRIM(ltrim(Apellido_materno)) AS Nombre,"
	                + "id_proyecto,"
                    + "(Select t4.Nombre From tbl_proyectos t4 Where t4.id_proyecto = t3.id_proyecto) AS Proyecto "
                + "FROM tbl_solicitudes_becarios t1 "
                + "JOIN tbl_alumnos t2 ON t2.Matricula = t1.Matricula "
                + "JOIN tbl_solicitudes t3 ON t3.id_Misolicitud = t1.id_MiSolicitud "
                + " WHERE t1.Matricula = (select Matricula From Tbl_alumnos Where Matricula = '" + this.matricula +"') AND id_estatus_asignacion = 2"
                //+ " WHERE t1.id_alumno = " + this.alumnoID
                + " ORDER BY t1.id_Misolicitud DESC"
                , connection);

            SqlDataReader dr = cmmd.ExecuteReader();
            if (dr.HasRows){
                while (dr.Read())
                {
                    LblMatricula.Text = dr["Matricula"].ToString();
                    LblNombre.Text = dr["Nombre"].ToString();
                    LBSolicitud.Text = dr["Solicitud"].ToString();
                    LblIdAlumno.Text = dr["Matricula"].ToString();
                    this.solicitudID = Convert.ToInt32(dr["Solicitud"].ToString()); // Guarda el ID de solicitud para link LBSolicitud

                    // Obtiene el nombre del proyecto
                    if (dr["id_proyecto"].ToString() != "")
                    {
                        LblProyecto.Text = dr["Proyecto"].ToString();
                    }
                    else
                    {
                        LblProyecto.Text = "N/A";
                    }
                }
            }
            else
            {
                cmmd = new SqlCommand( // Recupera información de las actividades a realizar
                    "SELECT Matricula, Nombre, Apellido_paterno, Apellido_materno "
                    + "FROM tbl_alumnos "
                    + "WHERE Matricula = '" + this.matricula.Trim() + "' "
                    , connection);

                dr = cmmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        LblNombre.Text = dr["Nombre"].ToString();
                        LblNombre.Text += " ";
                        LblNombre.Text += dr["Apellido_paterno"].ToString();
                        LblNombre.Text += " ";
                        LblNombre.Text += dr["Apellido_materno"].ToString();
                        LblMatricula.Text = dr["Matricula"].ToString();
                    }
                }
                
                LblProyecto.Text = "N/A";
                LBSolicitud.Text = "";
                verModal("Alerta", " No has sido asignado a ninguna solicitud ");
            }      
        }

        protected void LBSolicitud_Click(object sender, EventArgs e)
        {
            MiSolicitud.Visible = true;
            Asignacion.Visible = false;
            int proyectoID = 0;

            SqlCommand cmmd = new SqlCommand( // Datos del solicitante
                "SELECT "
                    + "id_MiSolicitud,"
                    + "id_proyecto,"
                    + "sol.Nomina,"
                    + "Nomina,"
                    + "RTRIM(LTRIM(emp.Nombre)) + ' ' + RTRIM(LTRIM(Apellido_paterno)) + ' ' + RTRIM(LTRIM(Apellido_materno)) Nombre,"
                    + "Puesto,"
                    + "(SELECT Nombre FROM cat_periodos WHERE Periodo = sol.Periodo) Periodo,"
                    + "(SELECT Fecha_inicio FROM cat_periodos WHERE Periodo = sol.Periodo) Fecha_inicio,"
                    + "(SELECT Fecha_fin FROM cat_periodos WHERE Periodo = sol.Periodo) Fecha_fin,"
                    + "(SELECT Nombre FROM cat_campus Where Codigo_campus = emp.Codigo_campus) Campus,"
                    + "Ubicacion_fisica,"
                    + "Correo_electronico,"
                    + "Extencion_telefonica,"
                    + "emp.Ubicacion_alterna Ubicacion_alterna "
                + "FROM tbl_solicitudes sol "
                + "JOIN tbl_empleados emp ON sol.Nomina = emp.Nomina "
                + "WHERE id_MiSolicitud = " + LBSolicitud.Text.Trim()
                , connection);
            connection.Open();
            SqlDataReader reader = cmmd.ExecuteReader();
            if (reader.HasRows)// Si encontró registros
            {
                while (reader.Read())
                {
                    LblCampus.Text = reader["Campus"].ToString();
                    LblNomina.Text = reader["Nomina"].ToString();
                    LblNombreDetalle.Text = reader["Nombre"].ToString();
                    LblPuesto.Text = reader["Puesto"].ToString();
                    LblUbicacionFisica.Text = reader["Ubicacion_fisica"].ToString();
                    LblEmail.Text = reader["Correo_electronico"].ToString();
                    //LblTelefono.Text = "N/A";
                    LblExtension.Text = reader["Extencion_telefonica"].ToString();

                    if (reader["Periodo"].ToString() != "") // Datos del periodo
                    {
                        LblPeriodo.Text = reader["Periodo"].ToString();
                        LblFechaInicio.Text = reader["Fecha_inicio"].ToString().Substring(0, 10);
                        LblFechaFin.Text = reader["Fecha_fin"].ToString().Substring(0, 10);
                    }
                    else
                    {
                        LblPeriodo.Text = "N/A";
                        LblFechaInicio.Text = "N/A";
                        LblFechaFin.Text = "N/A";
                    }

                    if (reader["Ubicacion_alterna"].ToString() != "") // Datos de ubicación alterna
                    {
                        LblUbicacionAlterna.Text = reader["emp.Ubicacion_alterna"].ToString();
                    }
                    else
                    {
                        LblUbicacionAlterna.Text = "N/A";
                    }

                    if (reader["id_proyecto"].ToString() != "") // Recupera id de proyecto
                    {
                        proyectoID = Convert.ToInt32(reader["id_proyecto"].ToString());
                    }
                }
            }

            if (proyectoID > 0) // Recupera informacion del proyecto
            {
                cmmd = new SqlCommand("SELECT * FROM tbl_proyectos WHERE id_proyecto = " + proyectoID.ToString().Trim(), connection);

                reader = cmmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProyectoAsignado.Text = reader["Nombre"].ToString();
                        LblObjetivo.Text = reader["Objetivo"].ToString();
                        LblJustificacion.Text = reader["Justificacion"].ToString();
                    }
                }
                else
                {
                    ProyectoAsignado.Text = "N/A";
                    LblObjetivo.Text = "N/A";
                    LblJustificacion.Text = "N/A";
                }
            }
            else
            {
                ProyectoAsignado.Text = "N/A";
                LblObjetivo.Text = "N/A";
                LblJustificacion.Text = "N/A";
            }


            cmmd = new SqlCommand( // Recupera información de las actividades a realizar
                "SELECT TOP 1 * "
                + "FROM tbl_solicitudes_becarios "
                + "WHERE Matricula = " + LblIdAlumno.Text.Trim()
                + " ORDER BY id_Misolicitud DESC"
                , connection);

            reader = cmmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    LblActividades.Text = reader["Becario_funciones"].ToString();
                }
            }
            else
            {
                LblActividades.Text = "N/A";
            }
            connection.Close();
        }
        protected void BtnCerrar_Click(object sender, EventArgs e)
        {
            MiSolicitud.Visible = false;
            Asignacion.Visible = true;
        }
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }
    }
}