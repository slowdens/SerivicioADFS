using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace ServicioBecario.Vistas
{
    public partial class AsistenciaBecario : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection();
        string empleadoID
        {
            get
            {
                return ViewState["EmpleadoId"] == null ? "0" : ViewState["EmpleadoId"].ToString();
            }
            set
            {
                ViewState["EmpleadoId"] = value;
            }
        }
        int solicitudID
        {
            get
            {
                return ViewState["SolicitudId"] == null ? 0 : Convert.ToInt32(ViewState["SolicitudId"].ToString());
            }
            set
            {
                ViewState["SolicitudId"] = value;
            }
        }

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();

            if (!this.IsPostBack)
            {
                connection.Open(); // Abertura de la conexion

                this.empleadoID = GetEmpleadoIdFromNomina(Session["Usuario"].ToString());

                try
                {
                    loadSolicitudes(); // Recupera y muestra las solicitudes en las que aparece el empleado
                    loadDatosSolicitante(); // Recupera y muestra los datos del empleado
                }
                catch (Exception ex)
                {
                    verModal("Alerta", ex.ToString());
                }

                connection.Close();
            }
            else
            {
                connection.Open(); // Abertura de la conexion

                loadBecarios(this.solicitudID);

                connection.Close(); // Cierre de la conexion
            }
        }

        protected void BtnGuardarAsistencia_Click(object sender, EventArgs e)
        {
            connection.Open(); // abertura de la conexion

            foreach (GridViewRow row in GrdBecario.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("cbAsistencia");
                if (cb.Checked)
                {
                    SetAsistenciaBecario(this.solicitudID, row.Cells[1].Text, 1);
                }
                else
                {
                    SetAsistenciaBecario(this.solicitudID, row.Cells[1].Text, 0);
                }
            }

            connection.Close();

            divSolicitudes.Visible = true;
            divAsistencia.Visible = false;

            verModal("Alerta", "Asistencia guardada 1");
        }

        protected void BtnGuardarAsistenciaG_Click(object sender, EventArgs e)
        {
            connection.Open(); // Abertura de la conexion

            foreach (GridViewRow row in GrdBecario.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("cbAsistencia");
                if (cb.Checked)
                {
                    SetAsistenciaBecario(this.solicitudID, row.Cells[1].Text, 1);
                }
                else
                {
                    SetAsistenciaBecario(this.solicitudID, row.Cells[1].Text, 0);
                }
            }

            connection.Close(); // Cierre de la conexion

            divSolicitudes.Visible = true;
            divAsistencia.Visible = false;

            verModal("Alerta", "Asistencia guardada!");
        }

        protected void BtnRegresar1_Click(object sender, EventArgs e)
        {
            divSolicitudes.Visible = true;
            divAsistencia.Visible = false;
        }

        protected void BtnRegresar2_Click(object sender, EventArgs e)
        {
            divSolicitudes.Visible = true;
            divDetalleAsistencia.Visible = false;
        }
        
        #endregion

        #region Becarios

        protected void loadBecarios(int solicitudId)
        {
            bool readOnly = false; // Variable para determinar si solo se puede consultar la información

            SqlCommand cmmd = new SqlCommand(
                "SELECT "
                    + "t1.Matricula AlumnoId,"
                    + "t2.Matricula,"
                    + "rtrim(ltrim(t2.Nombre)) + ' ' + rtrim(ltrim(Apellido_paterno)) + ' ' + RTRIM(ltrim(Apellido_materno)) Nombre,"
                    + "Asistencia,"
                    + "Asistencia_fecha Fecha "
                + "FROM tbl_solicitudes_becarios t1 "
                + "JOIN tbl_alumnos t2 ON t2.Matricula = t1.Matricula "
                + "WHERE id_Misolicitud = " + solicitudId + " and t1.id_estatus_asignacion = 2"
                , connection);
            

            SqlDataAdapter da = new SqlDataAdapter(cmmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Agrega la columna de asistencia
            dt.Columns.Add("Asistió");

            GrdBecario.DataSource = dt;
            GrdBecario.DataBind();

            if(dt.Rows.Count<=0)
            {
                readOnly = true;
            }


            // Agrega los controles de checkbox por cada becario
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                // Control que recuperar la asistencia en BD
                CheckBox cbAsistencia = (CheckBox)GrdBecario.Rows[i].Cells[3].Controls[0];

                // control para capturar la asistencia
                CheckBox cb = new CheckBox();
                cb.ID = "cbAsistencia";
                cb.Checked = cbAsistencia.Checked;
                string variavble = GrdBecario.Rows[i].Cells[0].Text;
                if(ExisteTomaAsistenciaBecario(solicitudId, GrdBecario.Rows[i].Cells[0].Text))
                {
                    cb.Enabled = false;
                    readOnly = true;
                }

                GrdBecario.Rows[i].Cells[5].Controls.Add(cb); // Agrega el control al gridview
            }

            // Determina si la asistencia solo es de consulta
            if (readOnly)
            {
                BtnGuardarAsistenciaG.Visible = false;
            }
            else
            {
                BtnGuardarAsistenciaG.Visible = true;
            }
        }

        protected void SetAsistenciaBecario(int solicitudId, string matricula, int asistencia)
        {
            string alumnoId = "0";

            SqlCommand cmmd = new SqlCommand( // Busca el id del alumno en base a la matricula
                "SELECT "
                    + "Matricula "
                + "FROM tbl_alumnos "
                + "WHERE Matricula = '" + matricula + "'"
            , connection);

            SqlDataReader dr = cmmd.ExecuteReader();
            while (dr.Read())
            {
                alumnoId = dr["Matricula"].ToString();
            }

            if (alumnoId != "0")
            {
                /* Opcion 1
                cmmd = new SqlCommand("INSERT INTO tbl_solicitudes_becarios_asistencia (" // Inserta la asistencia
                                                    + "id_Misolicitud, "
                                                    + "id_alumno, "
                                                    + "Fecha_asistencia, "
                                                    + "Asistencia ) "
                                                    + "VALUES ("
                                                    + "@idmisolicitud, "
                                                    + "@idalumno, "
                                                    + "@fechaasistencia, "
                                                    + "@asistencia )", connection);
                cmmd.Parameters.Clear();
                cmmd.Parameters.AddWithValue("@idmisolicitud", solicitudId);
                cmmd.Parameters.AddWithValue("@idalumno", alumnoId);
                cmmd.Parameters.AddWithValue("@asistencia", asistencia);
                cmmd.Parameters.AddWithValue("@fechaasistencia", DateTime.Today);
                */

                // Opcion 2
                cmmd = new SqlCommand(// Actualiza el registro de asistencia
                    "UPDATE tbl_solicitudes_becarios "
                    + "SET " 
                        + "Asistencia = @asistencia,"
                        + "Asistencia_fecha = @fechaasistencia "
                    + "WHERE id_Misolicitud = @idmisolicitud  "
                    + "AND Matricula = @idalumno "
                , connection);

                cmmd.Parameters.Clear();
                cmmd.Parameters.AddWithValue("@idmisolicitud", solicitudId);
                cmmd.Parameters.AddWithValue("@idalumno", alumnoId);
                cmmd.Parameters.AddWithValue("@asistencia", asistencia);
                cmmd.Parameters.AddWithValue("@fechaasistencia", DateTime.Today);

                // Msg.Text += solicitudId.ToString() + " - " + alumnoId.ToString() + " - " + asistencia.ToString() + " - " + DateTime.Today.ToString();

                try
                {
                    cmmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    verModal("Alerta", ex.ToString());
                }
            }
        }

        protected bool ExisteTomaAsistenciaBecario(int solicitudId, string alumnoId)// Consulta si ya existe un registro de asistencia
        {
            bool existe = false;

            SqlCommand cmmd = new SqlCommand(
                "SELECT"
                    + " * "
                + "FROM tbl_solicitudes_becarios "
                + "WHERE id_Misolicitud = @idmisolicitud  "
                    + "AND Asistencia_fecha IS NULL "
                    + "AND Matricula = @idalumno "
            , connection);

            cmmd.Parameters.Clear();
            cmmd.Parameters.AddWithValue("@idmisolicitud", solicitudId);
            cmmd.Parameters.AddWithValue("@idalumno", alumnoId);

            SqlDataReader dr = cmmd.ExecuteReader();

            if (!dr.HasRows)
            {
                existe = true;
            }

            return existe;
        }

        #endregion

        #region Solicitudes

        protected void loadSolicitudes()
        {
            SqlCommand cmmd = new SqlCommand(
                "SELECT "
                    + "id_MiSolicitud Solicitud,"
                    + "id_tipo_solicitud TipoID,"
                    + "(SELECT Nombre FROM cat_tipo_solicitudes tip WHERE tip.id_tipo_solicitud = sol.id_tipo_solicitud) TipoDescripcion "
                + "FROM tbl_solicitudes sol "
                + "JOIN cat_periodos per ON per.Periodo = sol.Periodo "
                + "WHERE per.Activo = 1 AND Nomina =  '"+this.empleadoID.ToString().Trim()+"'"
            , connection);

            SqlDataAdapter da = new SqlDataAdapter(cmmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GrdSolicitudes.DataSource = dt;
            GrdSolicitudes.DataBind();

            if (dt.Rows.Count == 0 && this.solicitudID > 0)
            {
                verModal("Alerta", "No existen solicitudes para este empleado");
            }
        }

        protected void DeleteSolicitudId(int solicitud)
        {
            SqlCommand cmmd;
            SqlDataReader dr;
            int proyectoID = 0; // Por si el proyecto esta relacionado con algun proyecto

            cmmd = new SqlCommand( // Obtiene el id del proyecto si este existe
                "SELECT id_proyecto FROM tbl_solicitudes WHERE id_MiSolicitud = " + solicitud.ToString().Trim()
                , connection);

            dr = cmmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    proyectoID = Convert.ToInt32(dr["id_proyecto"].ToString());
                }
            }

            try
            {
                // Borrado de evaluacion del solicitante
                cmmd = new SqlCommand("DELETE FROM tbl_historial_preguntas_empleados WHERE idMiSolicitud = " + solicitud.ToString().Trim(), connection);
                cmmd.ExecuteNonQuery();
 
                // Borrado de la evaluacion de los becarios
                cmmd = new SqlCommand("DELETE FROM tbl_historial_preguntas_becarios WHERE idMiSolicitud = " + solicitud.ToString().Trim(), connection);
                cmmd.ExecuteNonQuery();

                // Borrado de asistencias de becarios
                cmmd = new SqlCommand("DELETE FROM tbl_solicitudes_becarios_asistencia WHERE id_Misolicitud = " + solicitud.ToString().Trim(), connection);
                cmmd.ExecuteNonQuery();

                // Borrado de becarios
                cmmd = new SqlCommand("DELETE FROM tbl_solicitudes_becarios WHERE id_Misolicitud = " + solicitud.ToString().Trim(), connection);
                cmmd.ExecuteNonQuery();

                // Borrado de solicitud
                cmmd = new SqlCommand("DELETE FROM tbl_solicitudes WHERE id_MiSolicitud = " + solicitud.ToString().Trim(), connection);
                cmmd.ExecuteNonQuery();

                // Borrado de proyecto
                cmmd = new SqlCommand("DELETE FROM tbl_proyectos WHERE id_proyecto = " + proyectoID.ToString().Trim(), connection);
                cmmd.ExecuteNonQuery();

            }catch (Exception ex){
                verModal("Alerta", ex.ToString());
            }
            
            //
        }

        #endregion

        #region LoadData

        protected void loadDatosSolicitante()
        {
            SqlCommand cmmd = new SqlCommand( // Datos del solicitante
                "SELECT "
                    + "RTRIM(LTRIM(tbl_empleados.Nombre)) + ' ' + RTRIM(LTRIM(Apellido_paterno)) + ' ' + RTRIM(LTRIM(Apellido_materno)) Nombre,"
                    + "Nomina,"
                    + "Puesto,"
                    + "Departamento,"
                    + "Ubicacion_fisica, "
                    + "Division, "
                    + "cat_campus.Nombre Campus "
                + "FROM tbl_empleados, cat_campus "
                + "WHERE Nomina = '" + this.empleadoID.ToString().Trim() + "' and tbl_empleados.Codigo_campus = cat_campus.Codigo_campus "
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
                    DatosDivision.Text = reader["Division"].ToString();
                    DatosCampus.Text = reader["Campus"].ToString();
                }
            }
            else
            {
                LblSolicitante.Text = "N/A";
                LblNomina.Text = "N/A";
                LblPuesto.Text = "N/A";
                LblDepartamento.Text = "N/A";
                LblUbicacionFisica.Text = "N/A";
            }
        }

        protected string  GetEmpleadoIdFromNomina(string nomina)
        {
            string empleadoId = "0";
            SqlCommand cmmd = new SqlCommand(
                "SELECT "
                    + "Nomina "
                + "FROM tbl_empleados "
                + "WHERE Nomina = '" + nomina + "'"
            , connection);

            SqlDataReader dr = cmmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    empleadoId = dr["Nomina"].ToString();
                }
            }

            return empleadoId; // Regreso de valor
        }

        #endregion

        #region GrdSolictudes

        protected void GrdSolicitudes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument); // index
            GridViewRow row = GrdSolicitudes.Rows[index]; // row
            int solicitudId = Convert.ToInt32(row.Cells[0].Text); // solicitud
            this.solicitudID = solicitudId; // Setea como solicitud seleccionada

            connection.Open(); // Abertura de conexion

            if(e.CommandName == "Asistencia"){
                loadBecarios(solicitudId);

                divSolicitudes.Visible = false;
                divAsistencia.Visible = true;
            }

            if(e.CommandName == "Detalle"){
                SqlCommand command = new SqlCommand(string.Format(
                    "SELECT  "
                        + "(tbl_alumnos.Nombre + ' ' + Apellido_Paterno + ' ' + Apellido_Materno) as Becario,"
                        + "CONVERT (char(10),"
                        + "Fecha_asistencia, 103) as Fecha, Asistencia "
                    + "From tbl_solicitudes_becarios_asistencia, tbl_alumnos "
                    + "Where (id_Misolicitud = @idmisolicitud) "
                          + "and tbl_solicitudes_becarios_asistencia.Matricula = tbl_alumnos.Matricula ")
                , connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@idmisolicitud", solicitudId.ToString().Trim());

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GrdAsistenciaGeneral.DataSource = dt;
                GrdAsistenciaGeneral.DataBind();

                divSolicitudes.Visible = false;
                divDetalleAsistencia.Visible = true;
            }
            /*
            if(e.CommandName == "Modificar"){

                int tipo = Convert.ToInt32(row.Cells[1].Text); // en este te basas para llamar el tipo de solicitud
                switch (tipo)
                {
                    case 1:
                        Response.Redirect("SolicitudEspecifica.aspx?" + solicitudId.ToString());
                        break;
                    case 2:
                        Response.Redirect("SolicitudporProyectos.aspx?" + solicitudId.ToString());
                        break;
                    case 3:
                        Response.Redirect("SolicitudEspecial.aspx?" + solicitudId.ToString());
                        break;
                }

            }

            if(e.CommandName == "Eliminar"){
                DeleteSolicitudId(solicitudId); // Borrado de la solicitud

                Msg.Text = "La solicitud " + solicitudId.ToString().Trim() + " se ha eliminado correctamente!";

                // Actualización de GridView de solicitudes
                loadSolicitudes();
            }
            */
            connection.Close(); // cierre de la conexion
        }

        protected void GrdSolicitudes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // reference the Delete ImageButton
                ImageButton deleteButton = (ImageButton)e.Row.Cells[6].Controls[0];
                deleteButton.OnClientClick = "if (!window.confirm('Esta seguro de que desa eliminar esta solicitud ?')) return false;";
            }
            */
            // hidden columns
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false; // Tipo de solicitud
                e.Row.Cells[4].Visible = false; // Columna Detalle
            }
          
        }

        #endregion   
     
        #region GrdBecario

        protected void GrdBecario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // reference the Delete ImageButton
                ImageButton deleteButton = (ImageButton)e.Row.Cells[6].Controls[0];
                deleteButton.OnClientClick = "if (!window.confirm('Esta seguro de que desa eliminar esta solicitud ?')) return false;";
            }
            */
            // hidden columns
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; // AlumnoId
                e.Row.Cells[3].Visible = false; // Asistencia
                e.Row.Cells[4].Visible = false; // Fecha
            }

        }

        #endregion

        #region Utilerias

        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        #endregion

    }
}