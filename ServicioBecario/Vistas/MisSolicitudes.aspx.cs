using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net.Mail;
using ServicioBecario.Codigo;

namespace ServicioBecario.Vistas
{
    public partial class MisSolicitudes : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        SqlConnection connection = new SqlConnection();
        int existe;
        BasedeDatos db = new BasedeDatos();
        Correo msj = new Correo();
        string empleadoID //int empleadoID
        {
            get
            {
                return ViewState["EmpleadoId"] == null ? "0" : ViewState["EmpleadoId"].ToString(); //Convert.ToInt32(ViewState["EmpleadoId"].ToString());
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
            connection .ConnectionString = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();

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
                    Msg.Text = ex.ToString();
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

            Msg.Text = "";
            foreach (GridViewRow row in GrdBecario.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("cbAsistencia");
                if (cb.Checked)
                {
                    SetAsistenciaBecario(this.solicitudID, row.Cells[0].Text, 1);
                }
                else
                {
                    SetAsistenciaBecario(this.solicitudID, row.Cells[0].Text, 0);
                }
            }

            connection.Close();

            divSolicitudes.Visible = true;
            divAsistencia.Visible = false;

            Msg.Text = "Asistencia guardada";
        }

        protected void BtnGuardarAsistenciaG_Click(object sender, EventArgs e)
        {
            connection.Open(); // Abertura de la conexion

            /*
            SqlCommand command = new SqlCommand(string.Format("UPDATE tbl_solicitudes_becarios SET "
                                                    + "Asistencia = 1 "
                                                    + "Where (id_Misolicitud = @idmisolicitud) "), connection);

            command.Parameters.Clear();
            command.Parameters.AddWithValue("@idmisolicitud", this.solicitudID);
            
            try
            {
                command.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Msg.Text = "Insert Error: " + ex.Message;
            }
                
            */

            foreach (GridViewRow row in GrdBecario.Rows)
            {
                SetAsistenciaBecario(this.solicitudID, row.Cells[0].Text, 1);
            }

            connection.Close(); // Cierre de la conexion

            divSolicitudes.Visible = true;
            divAsistencia.Visible = false;

            Msg.Text = "Asistencia guardada";
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
            SqlCommand cmmd = new SqlCommand(
                "SELECT "
                    + "t1.Matricula,"
                    + "rtrim(ltrim(t2.Nombre)) + ' ' + rtrim(ltrim(Apellido_paterno)) + ' ' + RTRIM(ltrim(Apellido_materno)) Nombre "
                //+ "Asistencia Asistio "
                + "FROM tbl_solicitudes_becarios t1 "
                + "JOIN tbl_alumnos t2 ON t2.Matricula = t1.Matricula "
                + "WHERE id_Misolicitud = " + solicitudId
                , connection);

            SqlDataAdapter da = new SqlDataAdapter(cmmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Agrega la columna de asistencia
            dt.Columns.Add("Asistio");

            GrdBecario.DataSource = dt;
            GrdBecario.DataBind();

            // Agrega los controles de checkbox por cada becario
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CheckBox cb = new CheckBox();
                cb.ID = "cbAsistencia";

                GrdBecario.Rows[i].Cells[2].Controls.Add(cb);
            }
        }

        protected void SetAsistenciaBecario(int solicitudId, string matricula, int asistencia)
        {
            string alumnoId = "0";

            SqlCommand cmmd = new SqlCommand( // Busca el id del alumno en base a la matricula
                "SELECT "
                    + "id_alumno "
                + "FROM tbl_alumnos "
                + "WHERE Matricula = '" + matricula + "'"
            , connection);

            SqlDataReader dr = cmmd.ExecuteReader();
            while (dr.Read())
            {
               // alumnoId = Convert.ToInt32(dr["id_alumno"].ToString());
                alumnoId = dr["Matricula"].ToString();
            }

            if (alumnoId != "0")
            {
                // Consulta si ya existe un registro de asistencia
                cmmd = new SqlCommand("SELECT id_consecutivo FROM tbl_solicitudes_becarios_asistencia WHERE "
                                 + "id_Misolicitud = @idmisolicitud  "
                                 + "AND Matricula = @idalumno "
                                 + "AND Fecha_asistencia = @fechaasistencia ", connection);
                cmmd.Parameters.Clear();
                cmmd.Parameters.AddWithValue("@idmisolicitud", this.solicitudID.ToString().Trim());
                cmmd.Parameters.AddWithValue("@idalumno", alumnoId.ToString().Trim());
                cmmd.Parameters.AddWithValue("@fechaasistencia", DateTime.Today);
                dr = cmmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        existe = Convert.ToInt32(dr["id_consecutivo"].ToString());
                    }
                }
                else
                {
                    existe = -1;
                }

                if (existe == -1)
                {
                    cmmd = new SqlCommand("INSERT INTO tbl_solicitudes_becarios_asistencia (" // Inserta la asistencia
                                                     + "id_Misolicitud, "
                                                     + "Matricula, "
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
                }
                else
                {
                    cmmd = new SqlCommand("UPDATE tbl_solicitudes_becarios_asistencia SET " // Actualiza el registro de asistencia
                                                 + "Asistencia = @asistencia WHERE "
                                                 + "id_Misolicitud = @idmisolicitud  "
                                                 + "AND Matricula = @idalumno "
                                                 + "AND Fecha_asistencia = @fechaasistencia ", connection);
                    cmmd.Parameters.Clear();
                    cmmd.Parameters.AddWithValue("@idmisolicitud", solicitudId);
                    cmmd.Parameters.AddWithValue("@idalumno", alumnoId);
                    cmmd.Parameters.AddWithValue("@asistencia", asistencia);
                    cmmd.Parameters.AddWithValue("@fechaasistencia", DateTime.Today);
                }

                try
                {
                    cmmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Msg.Text += ex.ToString();
                }
            }
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
                + "WHERE per.Activo = 1 AND Nomina = '"+this.empleadoID.ToString().Trim()+"'"  //" + this.empleadoID.ToString().Trim(
                , connection);

            SqlDataAdapter da = new SqlDataAdapter(cmmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GrdSolicitudes.DataSource = dt;
            GrdSolicitudes.DataBind();

            if (dt.Rows.Count == 0 && this.solicitudID > 0)
            {
                Msg.Text = "No existen solicitudes para este empleado";
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
                    if (dr["id_proyecto"] == DBNull.Value)
                    {
                        proyectoID = 0;
                    }
                    else
                    {
                    proyectoID = Convert.ToInt32(dr["id_proyecto"].ToString());
                    }
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

                if (proyectoID != 0)
                {
                    // Borrado de proyecto
                    cmmd = new SqlCommand("DELETE FROM tbl_proyectos WHERE id_proyecto = " + proyectoID.ToString().Trim(), connection);
                    cmmd.ExecuteNonQuery();
                }
            }catch (Exception ex){
                Msg.Text = ex.ToString();
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
                    + "Correo_electronico, "
                    + "cat_campus.Nombre Campus "
                + "FROM tbl_empleados, cat_campus "
                + "WHERE Nomina = '" + this.empleadoID.ToString().Trim() + "'  and tbl_empleados.Codigo_campus = cat_campus.Codigo_campus "
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
                    LblEmail.Text = reader["Correo_electronico"].ToString();
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

        protected string GetEmpleadoIdFromNomina(string nomina)
        {
            string  empleadoId = "0";
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
                    //empleadoId = Convert.ToInt32(dr["id_empleado"].ToString());
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
            /*
            if(e.CommandName == "Asistencia"){
                loadBecarios(solicitudId);

                divSolicitudes.Visible = false;
                divAsistencia.Visible = true;

                Msg.Text = "";
            }

            if(e.CommandName == "Detalle"){
                SqlCommand command = new SqlCommand(string.Format(
                    "SELECT  "
                        + "(tbl_alumnos.Nombre + ' ' + Apellido_Paterno + ' ' + Apellido_Materno) as Becario,"
                        + "CONVERT (char(10),"
                        + "Fecha_asistencia, 103) as Fecha, Asistencia "
                    + "From tbl_solicitudes_becarios_asistencia, tbl_alumnos "
                    + "Where (id_Misolicitud = @idmisolicitud) "
                          + "and tbl_solicitudes_becarios_asistencia.id_alumno = tbl_alumnos.id_alumno ")
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

                Msg.Text = "";

            }
            */
            if (e.CommandName == "Modificar")
            {

                int tipo = Convert.ToInt32(row.Cells[1].Text); // en este te basas para llamar el tipo de solicitud
                Session["Solicitud"] = solicitudId.ToString();
                /*
                string urlSharepoint = System.Configuration.ConfigurationManager.AppSettings["urlsharepoint"];
                urlSharepoint = urlSharepoint.Replace("**", "&");
                */
                //Verificar la fecha de modificación
                SqlCommand command = new SqlCommand(string.Format("SELECT id_Misolicitud, tbl_solicitudes.Periodo, tbl_solicitudes.Nomina, "
                                                                + "tbl_empleados.Codigo_campus, tbl_campus_periodo.id_campus_periodo, Fecha_inicio, Fecha_fin  "
                                                                + "From tbl_solicitudes, tbl_empleados, tbl_campus_periodo, tbl_fechas_solicitudes "
                                                                + "Where id_MiSolicitud = @idmisolicitud "
                                                                + "and tbl_solicitudes.Nomina = tbl_empleados.Nomina "
                                                                + "and tbl_empleados.Codigo_campus = tbl_campus_periodo.Codigo_campus "
                                                                + "and tbl_campus_periodo.Periodo = tbl_solicitudes.Periodo "
                                                                + "and tbl_fechas_solicitudes.id_campus_periodo = tbl_campus_periodo.id_campus_periodo "), connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@idmisolicitud", solicitudId.ToString().Trim());
                //connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (DateTime.Today >= Convert.ToDateTime(dr["Fecha_inicio"]) && DateTime.Today <= Convert.ToDateTime(dr["Fecha_fin"]))
                        {
                            switch (tipo)
                            {
                                case 1:
                                    Response.Redirect("EspecificaIndividual.aspx");
                                    //Response.Redirect("EspecificaIndividual.aspx?" + urlSharepoint);
                                    break;
                                case 2:
                                    Response.Redirect("Proyectos.aspx");
                                    //Response.Redirect("Proyectos.aspx?" + urlSharepoint);
                                    break;
                                case 3:
                                    Response.Redirect("SolicitudEspeciales.aspx");
                                    //Response.Redirect("SolicitudEspeciales.aspx?" + urlSharepoint);
                                    break;
                            }

                        }
                        else
                        {
                            verModal("Alerta", " La solicitud, no se puede modificar (esta fuera de periodo) ");
                        }
                    }
                }
                else
                {
                    verModal("Alerta", " No hay fechas de solicitudes registradas ");
                }
            }

            if(e.CommandName == "Eliminar"){

                query = "select * from tbl_solicitudes_becarios where id_MiSolicitud="+this.solicitudID+" and  id_estatus_asignacion=2 ";

                dt = db.getQuery(conexionBecarios, query);
                if(dt.Rows.Count<=0)//si fue menor o igual a cero  se puede eliminar
                {
                    correoEliminacion(solicitudId);//Mandamos correo de que se elimino la solicitud
                    DeleteSolicitudId(solicitudId); // Borrado de la solicitud

                    Msg.Text = "La solicitud " + solicitudId.ToString().Trim() + " se ha eliminado correctamente!";

                    // Actualización de GridView de solicitudes
                    loadSolicitudes();
                }
                else
                {
                    verModal("Alerta", "No se permite eliminar la solicitud tienes matrículas asignadas");
                }
                
                
            }

            connection.Close(); // cierre de la conexion
        }
        public void correoEliminacion(int solicitud)
        {
            query = "sp_Correos_cancelaciones_solicitudes "+solicitud+"";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(), dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["Correo"].ToString());
            }

        }


       public bool mandarCorreo2(string cuerpo, string asunto, string correo)
        {
            bool bandera = false;
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(correo, ""));
            msg.From = new MailAddress("servicio.becario@itesm.mx", "Servicio Becario");
            msg.Subject = asunto;
            msg.Body = cuerpo + db.noEnvio();
            msg.IsBodyHtml = true;
            try
            {
                
                msj.MandarCorreo(msg);
                bandera = true;
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }

            return bandera;
        }




       public bool mandarCorreo(string cuerpo, string asunto, string correo)
       {
           bool bandera = false;
           string ambiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
           string IpDesarrollo = System.Configuration.ConfigurationManager.AppSettings["IpDesarrollo"];
           string IpPruebas = System.Configuration.ConfigurationManager.AppSettings["IpPruebas"];
           string to = correo;
           string from = "servicio.becario@itesm.mx";
           string subject = asunto;
           string body = cuerpo;

           MailMessage message = new MailMessage(from, to, subject, body);
           message.IsBodyHtml = true;
           SmtpClient client = null;
           switch (ambiente)
           {
               case "pprd":
                   //Direccion de desarrollo
                   client = new SmtpClient(IpDesarrollo, 587);
                   break;
               case "prod":
                   //Es la direcciion de pruebas
                   client = new SmtpClient(IpPruebas, 587);
                   break;
           }



           client.UseDefaultCredentials = false;

           try
           {
               client.Send(message);//Enviamos el mensaje
               bandera = true;



           }
           catch (System.Net.Mail.SmtpException e)
           {

               //Response.Write(e);
               //verModal("Error", e.ToString());
           }


           return bandera;
       }





        protected void GrdSolicitudes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // reference the Delete ImageButton
                ImageButton deleteButton = (ImageButton)e.Row.Cells[4].Controls[0];
                deleteButton.OnClientClick = "if (!window.confirm('Esta seguro de que desa eliminar esta solicitud ?')) return false;";
            }

            // hidden columns
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;
            }
          
        }

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        #endregion        
    }
}