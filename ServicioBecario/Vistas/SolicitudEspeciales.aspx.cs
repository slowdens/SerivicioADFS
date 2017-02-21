using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Data;
using System.Net.Mail;
using ServicioBecario.Codigo;

namespace ServicioBecario.Vistas
{
    public partial class SolicitudEspeciales : System.Web.UI.Page
    {

        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        //http://www.microsoft.com/es-es/download/confirmation.aspx?id=23734
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        int valorotroalumno;
        int numerobecarios;
        int totalbecariosseleccionados;
        DataTable dtPrincipal,dt;
        String query;
        BasedeDatos db = new BasedeDatos();
        Correo mail = new Correo();

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
            command.Connection = connection;

            if (Session["Solicitud"] != null)//(Request.QueryString.ToString() != "") // Modificación de solicitud
            {
                if (!IsPostBack)
                {
                    TxtBoxSolicitud.Text = Session["Solicitud"].ToString(); //TxtBoxSolicitud.Text = Request.QueryString.ToString(); // Obtiene el id de solicitud
                    BtnDatos.Visible = true;
                    estatus.Text = "MODIFICACION";
                    connection.Open(); // Apertura de la conexión
                    Session.Remove("Solicitud");
                    loaddata();
                    loaddatamodificacion();
                }

                connection.Close(); // Cierre de la conexión
            }
            else // Nueva solicitud
            {
                if (!IsPostBack)
                {
                    connection.Open(); // Apertura de la conexión
                    Session.Remove("Solicitud");
                    loaddata(); // Carga de encabezado y combo box
                    connection.Close(); // Cierre de la conexión
                }
            }

            // Validaciones
            /*
            if (otroalumno.Checked)
            {
                Matricula.Attributes.Remove("data-validation-engine");
            }
            else
            {
                Matricula.Attributes.Add("data-validation-engine", "validate[required]");
            }
            */
            if (TblCaptura.Visible)
            {
                Funciones.Attributes.Add("data-validation-engine", "validate[required]");
            }
            else
            {
                Funciones.Attributes.Remove("data-validation-engine");
            }
        }

        protected void Agregar_Click(object sender, EventArgs e)
        {
            string error = "NO";
            string tipo = "NO";
            if (Matricula.Text.Length > 0 && Matricula.Text.Length < 10)
            {
                error = "SI";
            }
            else
            {
                if (Matricula.Text.Length == 0 && otroalumno.Checked)
                {
                    error = "NO";
                }
                else
                {
                    error = "NO";
                    /*
                    connection.Open();
                    command = new SqlCommand( // Recupera el id de alumno
                            "SELECT "
                                + "id_alumno "
                            + "FROM tbl_alumnos "
                            + "WHERE Matricula = '" + Matricula.Text.Substring(0, 9) + "'"
                        , connection);

                    SqlDataReader dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        error = "NO";
                    }
                    else
                    {
                        error = "SI";
                    }
                    connection.Close();
                     */
                }
            }
            
            if (CbNivel.SelectedValue == "0" && Matricula.Text == "")
            {
                error = "SI";
                tipo = "NIVEL";
            }
            if (error == "NO")
            {
                Boolean hasError = false;

                if (Matricula.Text != "")
                {
                    if (0 == Convert.ToInt64(totalMatricula.Text))
                    {
                        hasError = true;
                        verModal("Alerta", "No puedes agregar becarios por matrícula");
                        TblBecariosProgramas.Visible = true;
                        TblCaptura.Visible = false;
                    }
                    else
                    {
                        if (GrdBecarios.Rows.Count == Convert.ToInt64(totalMatricula.Text))
                        {
                            hasError = true;
                            verModal("Alerta", "Ya se han capturado todos los becarios");

                            //TblBecariosProgramas.Visible = true;
                            //TblCaptura.Visible = false;
                        }
/*
                        else
                            
                        {
                            if (otroalumno.Checked)
                            {
                                hasError = true;

                                verModal("Alerta", "Debe capturar la matricula");
                            }
                            
                        }
                             */
                    }
                }
                else
                {
                    if (0 != Convert.ToInt64(totalPeriodo.Text))
                    {
                        if ((GrdProgramas.Rows.Count == Convert.ToInt64(totalPeriodo.Text)) & otroalumno.Checked)
                        {
                            hasError = true;
                            verModal("Alerta", "Ya se han capturado todos los programas");

                            TblBecariosProgramas.Visible = true;
                            TblCaptura.Visible = false;
                        }
                    }
                    else
                    {
                        hasError = true;
                        verModal("Alerta", "No puedes capturar becarios por programa");
                        TblBecariosProgramas.Visible = true;
                        TblCaptura.Visible = false;
                    }


                }



                if (!hasError) // Alta becario
                {
                    string idalumno = "";
                    SqlCommand command;

                    connection.Open(); // Abertura de la conexion
                    if (Matricula.Text != "")
                    {
                        command = new SqlCommand( // Recupera el id de alumno
                            "SELECT "
                                + "Matricula "
                            + "FROM tbl_alumnos "
                            + "WHERE Matricula = '" + Matricula.Text.Substring(0, 9) + "'"
                        , connection);

                        SqlDataReader dr = command.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                idalumno = dr["Matricula"].ToString();
                            }
                        }
                    }

                    ///Verificar la duplicidad
                    command = new SqlCommand( // Recupera el id de alumno
                                "SELECT "
                                    + "Matricula "
                                + "FROM tbl_solicitudes_becarios "
                                + "WHERE id_Misolicitud = '" + TxtBoxSolicitud.Text.Trim() + "' and Matricula = '" + idalumno.ToString().Trim() + "'  and   '$'+ RTRIM( LTRIM(Matricula)) +'$' <> '$$'   "
                            , connection);
                    SqlDataReader dr2 = command.ExecuteReader();
                    if (dr2.HasRows)
                    {
                        verModal("Alerta", " Ya se registro el alumno ");
                        Matricula.Text = "";
                        Funciones.Text = "";
                    }
                    else
                    {
                        command = new SqlCommand(string.Format("INSERT INTO tbl_solicitudes_becarios (" // Inserta becario
                                                                          + "id_Misolicitud, "
                                                                          + "Matricula, "
                                                                          + "Codigo_programa_academico, "
                                                                          + "Periodo_cursado, "
                                                                          + "Otro_alumno, "
                                                                          + "Becario_calificacion, "
                            //+ "Becario_puntaje, "
                                                                          + "Becario_funciones, "
                                                                          + "id_estatus_asignacion, "
                                                                          + "Asistencia, "
                                                                          + "Correo_aviso_asignacion, "
                                                                          + "Codigo_nivel_academico) "
                            //+ "Asistencia_fecha) "
                                                                          + "VALUES ("
                                                                          + "@idmisolicitud, "
                                                                          + "@idalumno, "
                                                                          + "@idprograma,  "
                                                                          + "@idperiodocursado, "
                                                                          + "@otroalumno,  "
                                                                          + "@becariocalificacion,  "
                            //+ "@becariopuntaje, "
                                                                          + "@becariofunciones,  "
                                                                          + "@idestatusasignacion, "
                                                                          + "@asistencia, "
                                                                          + "@correoavisoasignacion, "
                                                                          + "@nivelacademico) "), connection);
                        //+ "@asistenciafecha) "), connection);
                        command.Parameters.Clear();
                        if (otroalumno.Checked == false)
                        {
                            this.valorotroalumno = 0;
                            /*
                            command.Parameters.AddWithValue("@idmisolicitud", TxtBoxSolicitud.Text);
                            command.Parameters.AddWithValue("@idalumno", idalumno);
                            command.Parameters.AddWithValue("@idprograma", '\0');
                            command.Parameters.AddWithValue("@idperiodocursado", '\0');
                            command.Parameters.AddWithValue("@otroalumno", this.valorotroalumno);
                            command.Parameters.AddWithValue("@becariocalificacion", "PENDIENTE");
                            //command.Parameters.AddWithValue("@becariopuntaje", 0);
                            command.Parameters.AddWithValue("@becariofunciones", Funciones.Text);
                            command.Parameters.AddWithValue("@idestatusasignacion", 1);
                            command.Parameters.AddWithValue("@asistencia", 0);
                            command.Parameters.AddWithValue("@correoavisoasignacion", 0);
                            command.Parameters.AddWithValue("@nivelacademico", 0);
                            //command.Parameters.AddWithValue("@asistenciafecha", DateTime.Today);

                            verModal("Alerta", "Registro creado correctamente");

                            Matricula.Text = "";
                            Funciones.Text = "";
                             */
                        }
                        else
                        {
                            this.valorotroalumno = 1;
                        }
                        command.Parameters.AddWithValue("@idmisolicitud", TxtBoxSolicitud.Text);

                        if (idalumno == "")
                        {
                            command.Parameters.AddWithValue("@idalumno", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@idalumno", idalumno);
                        }
                        if (CbPeriodo.SelectedItem.Text == "--NA--")
                        {
                            command.Parameters.AddWithValue("@idperiodocursado", "N/A");
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@idperiodocursado", CbPeriodo.SelectedItem.Value);
                        }
                        if (CbPrograma.SelectedItem.Text == "--NA--")
                        {
                            command.Parameters.AddWithValue("@idprograma", "N/A");
                        }
                        else
                        {
                            //Aqui va nuestro codigo

                            command.Parameters.AddWithValue("@idprograma", getCodogoProgama( CbPrograma.SelectedItem.Text));
                        }
                        if (CbNivel.SelectedItem.Text == "--Seleccione--")
                        {
                            command.Parameters.AddWithValue("@nivelacademico",0);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@nivelacademico", Convert.ToInt32(CbNivel.SelectedItem.Value));
                        }
                        //command.Parameters.AddWithValue("@idprograma", CbPrograma.SelectedItem.Text);
                        //command.Parameters.AddWithValue("@idperiodocursado", CbPeriodo.SelectedItem.Text);
                        command.Parameters.AddWithValue("@otroalumno", this.valorotroalumno);
                        command.Parameters.AddWithValue("@becariocalificacion", "PENDIENTE");
                        //command.Parameters.AddWithValue("@becariopuntaje", 0);
                        command.Parameters.AddWithValue("@becariofunciones", Funciones.Text);
                        command.Parameters.AddWithValue("@idestatusasignacion", 1);
                        command.Parameters.AddWithValue("@asistencia", 0);
                        command.Parameters.AddWithValue("@correoavisoasignacion", 0);
                        //command.Parameters.AddWithValue("@nivelacademico", CbNivel.SelectedItem.Text);
                        //command.Parameters.AddWithValue("@asistenciafecha", DateTime.Today);

                        verModal("Alerta", "Registro creado correctamente");

                        Matricula.Text = "";
                        Funciones.Text = "";
                        //}
                        try
                        {
                            command.ExecuteNonQuery();

                            TblCaptura.Visible = false;
                            TblBecariosProgramas.Visible = true;
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            verModal("Alerta", "Insert Error: " + ex.Message);
                        }

                        loadGridViewData();

                        connection.Close();
                    }
                }
            }
            else
            {
                if (tipo == "NIVEL")
                {
                   // verModal("Alerta", " No has seleccionado el nivel ");
                    verModal("Nivel", "El nivel es:" + CbNivel.SelectedValue);
                    CbNivel.Focus();
                }
                else
                {
                    verModal("Alerta", " La matrícula no existe ");
                }
            }
        }


        public string getCodogoProgama(string programaAcademico)
        {
            string codigoprograma = "";
            string query = "";

            string campus = IdCampus.Text;


            if (programaAcademico != "")
            {
                query = " sp_pogramaAcademico_aleatorio '" + programaAcademico + "','" + CbPeriodos.SelectedValue + "','" + campus + "'," + CbNivel.SelectedValue + " ";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    codigoprograma = dt.Rows[0]["codigo_programa_academico"].ToString();
                }
            }
            else
            {
                codigoprograma = "N/A";
            }
            return codigoprograma;
        }

        public string getCodogoProgamaMasiva(string programaAcademico)
        {
            string codigoprograma = "";
            string query = "";

            string campus = IdCampus.Text;


            if (programaAcademico != "")
            {
                query = " sp_pogramaAcademico_aleatorio '" + programaAcademico + "','" + CbPeriodos.SelectedValue + "','" + campus + "'," + DropDownListNivel.SelectedValue + " ";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    codigoprograma = dt.Rows[0]["codigo_programa_academico"].ToString();
                }
            }
            else
            {
                codigoprograma = "N/A";
            }
            return codigoprograma;
        }




        protected void BtnDatos_Click(object sender, EventArgs e)
        {
            string gquery = @"select fs.Fecha_inicio,fs.Fecha_fin
                                from tbl_campus_periodo cp inner join tbl_fechas_solicitudes fs on cp.id_campus_periodo=fs.id_campus_periodo
                                inner join tbl_empleados e on e.Codigo_campus=cp.Codigo_campus

                                where Nomina='" + Session["usuario"].ToString() + "' and cp.Periodo='" + CbPeriodos.SelectedValue + "'";
            dt = db.getQuery(conexionBecarios,gquery);
            if (dt.Rows.Count > 0)
            {
                if (DateTime.Today >= Convert.ToDateTime(dt.Rows[0]["Fecha_inicio"]) && DateTime.Today <= Convert.ToDateTime(dt.Rows[0]["Fecha_fin"]))
                {
                    llenarNivel();
                    connection.Open(); // Apertura de conexion

                    if (estatus.Text == "MODIFICACION")
                    {
                        /// Actualiza los datos de la solicitud
                        command = new SqlCommand(string.Format("UPDATE tbl_solicitudes SET "
                                + "Becarios_total = @becariostotal, "
                                + "Becarios_periodo = @becariosperiodo "
                                + "WHERE id_miSolicitud = @idmisolicitud "), connection);
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idmisolicitud", TxtBoxSolicitud.Text);
                        command.Parameters.AddWithValue("@becariostotal", totalMatricula.Text);
                        command.Parameters.AddWithValue("@becariosperiodo", totalPeriodo.Text);

                        command.ExecuteNonQuery();

                        /// Actualiza los datos del proyecto
                        command = new SqlCommand(string.Format("UPDATE tbl_proyectos SET "
                                + "Nombre = @nombre, "
                                + "Justificacion = @justificacion, "
                                + "Objetivo = @objetivo, "
                                + "Proyecto_funciones = @proyectofunciones "
                                + "WHERE id_proyecto = @idproyecto "), connection);
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idproyecto", TxtBoxProyecto.Text);
                        command.Parameters.AddWithValue("@nombre", TxtBoxNombre.Text.Trim());
                        command.Parameters.AddWithValue("@justificacion", TxtBoxJustificacion.Text.Trim());
                        command.Parameters.AddWithValue("@objetivo", TxtBoxObjetivo.Text.Trim());
                        command.Parameters.AddWithValue("@proyectofunciones", TxtBoxFunciones.Text.Trim());

                        command.ExecuteNonQuery();

                        loadGridViewData(); // Actualiza los GridViews
                        TblAlta.Visible = false;
                        TblBecariosProgramas.Visible = true;
                    }
                    else
                    {

                        Int32 aux; // Variable para recuperar id de registro insertado

                        /// Alta del proyecto
                        command = new SqlCommand(string.Format("INSERT INTO tbl_proyectos ("
                                                                    + "Nombre, "
                                                                    + "Justificacion, "
                                                                    + "Fecha_registro, "
                                                                    + "Aprobado, "
                                                                    + "Objetivo, "
                                                                    + "id_tipo_proyecto, "
                                                                    + "Proyecto_funciones) "
                                                                    + "VALUES ("
                                                                    + "@nombre, "
                                                                    + "@justificacion, "
                                                                    + "@fecharegistro,  "
                                                                    + "@aprobado, "
                                                                    + "@objetivo,  "
                                                                    + "@idtipoproyecto, "
                                                                    + "@proyectofunciones) "), connection);

                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@nombre", TxtBoxNombre.Text.Trim());
                        command.Parameters.AddWithValue("@justificacion", TxtBoxJustificacion.Text.Trim());
                        command.Parameters.AddWithValue("@fecharegistro", DateTime.Today);
                        command.Parameters.AddWithValue("@aprobado", 1);
                        command.Parameters.AddWithValue("@objetivo", TxtBoxObjetivo.Text.Trim());
                        command.Parameters.AddWithValue("@idtipoproyecto", 2); // Especial
                        command.Parameters.AddWithValue("@proyectofunciones", TxtBoxFunciones.Text.Trim());
                        try
                        {
                            command.ExecuteNonQuery();

                            command = new SqlCommand(string.Format("SELECT @@Identity"), connection);
                            command.ExecuteNonQuery();

                            aux = Convert.ToInt32(command.ExecuteScalar());
                            TxtBoxProyecto.Text = aux.ToString();
                            //ErrViewer.Text = "Ok  ";
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            verModal("Alerta", "Insert Error: " + ex.Message);
                        }

                        /// Alta de la solicitud
                        command = new SqlCommand(string.Format("INSERT INTO tbl_solicitudes ("
                                                                    + "Periodo, "
                                                                    + "Nomina, "
                                                                    + "id_proyecto, "
                                                                    + "id_tipo_solicitud, "
                                                                    + "id_solicitud_estatus, "
                                                                    + "Fecha_solicitud, "
                                                                    + "Empleado_calificacion, "
                                                                    + "Becarios_total, "
                                                                    + "Becarios_periodo) "
                            //+ "Empleado_puntuaje) "
                                                                    + "VALUES ("
                                                                    + "@idperiodo, "
                                                                    + "@idempleado, "
                                                                    + "@idproyecto, "
                                                                    + "@idtiposolicitud, "
                                                                    + "@idsolicitudestatus,"
                                                                    + "@fechasolicitud, "
                                                                    + "@empeladocalificacion, "
                                                                    + "@becariostotal, "
                                                                    + "@becariosperiodo) "), connection);
                        //+ "@empladopuntaje) "), connection);
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@idperiodo", CbPeriodos.SelectedValue);
                        command.Parameters.AddWithValue("@idempleado", lblIdEmpleadoSolicitud.Text);
                        command.Parameters.AddWithValue("@idproyecto", TxtBoxProyecto.Text);
                        command.Parameters.AddWithValue("@idtiposolicitud", 3);
                        command.Parameters.AddWithValue("@idsolicitudestatus", 2);
                        command.Parameters.AddWithValue("@fechasolicitud", DateTime.Today);
                        command.Parameters.AddWithValue("@empeladocalificacion", "PENDIENTE");
                        command.Parameters.AddWithValue("@becariostotal", Convert.ToInt32(totalMatricula.Text.Trim()));
                        command.Parameters.AddWithValue("@becariosperiodo", Convert.ToInt32(totalPeriodo.Text.Trim()));
                        // command.Parameters.AddWithValue("@empladopuntaje", 0);
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            verModal("Alerta", "Insert Error: " + ex.Message);
                        }

                        command = new SqlCommand(string.Format("SELECT @@Identity"), connection);
                        command.ExecuteNonQuery();

                        aux = Convert.ToInt32(command.ExecuteScalar());
                        TxtBoxSolicitud.Text = aux.ToString();

                        //ErrViewer.Text = "Ok";

                        loadGridViewData(); // Actualiza los GridViews

                        TblAlta.Visible = false;
                        TblBecariosProgramas.Visible = true;
                        nombreProyecto2.Text = "<B>Nombre del Proyecto: </B>" + TxtBoxNombre.Text.Trim();
                        Session["Periodo"] = CbPeriodos.SelectedValue;
                    }
                }
                else
                {
                    if (DateTime.Today > Convert.ToDateTime(dt.Rows[0]["Fecha_fin"]))
                    {
                        verModal("Alerta", "Ya pasaron las fechas de solicitudes para el periodo " + CbPeriodos.SelectedItem.Text + "");
                    }
                    if (DateTime.Today < Convert.ToDateTime(dt.Rows[0]["Fecha_inicio"]))
                    {
                        verModal("Alerta", "Aún no son las fechas de solicitudes para el periodo " + CbPeriodos.SelectedItem.Text + "");
                    }
                }

            }
            else
            {
                verModal("Alerta", "No existen fechas de solicitud en el periodo");
            }
            

        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            BasedeDatos db = new BasedeDatos();

            numerobecarios = GrdBecarios.Rows.Count + GrdProgramas.Rows.Count;
            if (GrdBecarios.Rows.Count < Convert.ToInt32(totalMatricula.Text))
            {
                verModal("Alerta", "No has capturado todos los becarios por matrícula");
            }
            else
            {
                if (GrdProgramas.Rows.Count < Convert.ToInt32(totalPeriodo.Text))
                {
                    verModal("Alerta", "No has capturado todos los becarios por periodo");
                }
                else
                {
                    /*<-- Guardado de la ubicacion alterna */
                    SqlCommand command = new SqlCommand(string.Format("UPDATE tbl_solicitudes "
                                                                                + "SET "
                                                                                + "Ubicacion_alterna = '{0}', "
                                                                                + "Acepto = '{1}' "
                                                                                + "Where id_MiSolicitud = {2}",
                                                                                TxtUbicacionAlterna.Text.Trim(), "SI",
                                                                                TxtBoxSolicitud.Text), connection);
                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                    /* Guardado de la ubicacion alterna --> */

                    lblRegistro.Text = "Se ha registrado el proyecto " + TxtBoxProyecto.Text.ToString() + " en la solicitud con el folio " + TxtBoxSolicitud.Text.ToString();
                    if (estatus.Text == "MODIFICACION")
                    {
                        lblRegistro.Text += " (Modificación) ";
                    }
                    TotalRegistrados.Text = "con un total de " + numerobecarios.ToString() + " becarios. ";
                    TblBecariosProgramas.Visible = false;
                    TblResultado.Visible = true;

                    /*<-- Envío de correo*/
                    //Mail mail; // Configuración de envío de correo
                    if (estatus.Text == "MODIFICACION") // Mensaje si es modificación
                    {
                        command = new SqlCommand(string.Format(
                        "SELECT "
                            + "Asunto,"
                            + "Cuerpo "
                        + "FROM tbl_cuerpo_correo t1 "
                        + "INNER JOIN cat_correos t2 "
                        + "ON t1.id_correo = t2.id_correo "
                        + "WHERE rtrim(ltrim(Tipo_correo)) = 'Modificación a la solicitud(Por Proyecto Especial)'"
                        + "AND Codigo_campus = (Select Codigo_campus FROM tbl_empleados WHERE Nomina = '{0}')",
                            TxtBoxNominaEmpleado.Text.Trim()), connection);
                    }
                    else // Mensaje si es una nueva solicitud 
                    {
                        command = new SqlCommand(string.Format(
                                "SELECT Asunto, Cuerpo "
                                + "FROM tbl_cuerpo_correo t1 "
                                + "INNER JOIN cat_correos t2 "
                                + "ON t1.id_correo = t2.id_correo "
                                + "WHERE rtrim(ltrim(Tipo_correo)) = 'Confirmación de registro(Por proyecto Especial)'"
                                    + "AND Codigo_campus = (Select Codigo_campus From tbl_empleados Where Nomina = '{0}')",
                                    TxtBoxNominaEmpleado.Text.Trim()), connection);
                    }
                    connection.Open();

                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        //MailMessage message = new MailMessage("servicio.becario@itesm.mx", DatosCorreo1.Text.Trim());
                        //message.Subject = dr["Asunto"].ToString();
                        //message.Body = reemplazarClaves(dr["Cuerpo"].ToString()) + db.noEnvio();
                        //message.IsBodyHtml = true;

                     
                        //mail.MandarCorreo(message);


                        mandarCorreo(reemplazarClaves(dr["Cuerpo"].ToString()) + db.noEnvio(), dr["Asunto"].ToString(), DatosCorreo1.Text.Trim());
                    }
                    connection.Close();
                    /* Envío de correo --> */
                }
            }
        }

        public bool mandarCorreo(string cuerpo, string asunto, string correo)
        {
            bool bandera = false;
            string ambiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
            string ipDesarrollo = System.Configuration.ConfigurationManager.AppSettings["IpDesarrollo"];
            string ipPruebas = System.Configuration.ConfigurationManager.AppSettings["IpPruebas"];
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
                    client = new SmtpClient(ipDesarrollo, 587);
                    break;
                case "prod":
                    //Es la direcciion de pruebas
                    client = new SmtpClient(ipPruebas, 587);
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



        protected void CbNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nivelID = Convert.ToInt32(CbNivel.SelectedValue);

            //connection.Open(); //Abertura de la conexión

            LoadListPrograma(nivelID);

            //connection.Close(); // Cierre de la conexión
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string connectionString = "";
            if (FileUpload1.HasFile)
            {
                ErrViewer.Text = "Cargando archivo...";

                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string fileLocation = Server.MapPath("~/App_Data/" + fileName);
                FileUpload1.SaveAs(fileLocation);

                if (fileExtension == ".xls")
                {
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                      fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                else if (fileExtension == ".xlsx")
                {
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                      fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }

                OleDbConnection con = new OleDbConnection(connectionString);
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = con;

                OleDbDataAdapter dAdapter = new OleDbDataAdapter(cmd);
                DataTable dtExcelRecords = new DataTable();
                con.Open();

                DataTable dtExcelSheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string getExcelSheetName = dtExcelSheetName.Rows[0]["Table_Name"].ToString();
                cmd.CommandText = "SELECT * FROM [" + getExcelSheetName + "]";

                dAdapter.SelectCommand = cmd;
                dAdapter.Fill(dtExcelRecords);
                
                if (TextBoxTipoCargaExcel.Text.Trim() == "MATRICULA")
                {
                    if (dtExcelRecords.Columns.Contains("Matricula"))
                    {
                        if ((dtExcelRecords.Rows.Count + Convert.ToInt64(TxtNumeroBecarios.Text)) <= Convert.ToInt64(totalMatricula.Text))
                        {
                            foreach (DataRow row in dtExcelRecords.Rows)
                            {

                                /// Alta de los becarios solicitados - por matricula
                                SqlCommand command = new SqlCommand(string.Format("INSERT INTO tbl_solicitudes_becarios ("
                                                                            + "id_Misolicitud, "
                                                                            + "Matricula,"
                                                                            + "Otro_alumno, "
                                                                            + "Becario_calificacion, "
                                                                            + "Asistencia, "
                                                                            + "Correo_aviso_asignacion, "
                                                                            + "id_estatus_asignacion, "
                                                                            + "Codigo_programa_academico ,"
                                                                            + "Codigo_nivel_academico ,"
                                                                            + "Periodo_cursado )"

                                                                            + "VALUES ("
                                                                            + "@idsolicitud, "
                                                                            + "@matriculaalumno,"
                                                                            + "@otro, "
                                                                            + "@becariocalificacion, "
                                                                            + "@asistencia, "
                                                                            + "@correoavisoasignacion, "
                                                                            + "@estatusasignacion,"
                                                                            + "@codigo_programa_academico, "
                                                                            + "@codigo_nivel_academico, "
                                                                            + "@semestre_cursado )"), connection);

                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@idsolicitud", TxtBoxSolicitud.Text);
                                command.Parameters.AddWithValue("@matriculaalumno", row["Matricula"].ToString());
                                command.Parameters.AddWithValue("@becariocalificacion", "PENDIENTE");
                                command.Parameters.AddWithValue("@asistencia", 0);
                                command.Parameters.AddWithValue("@correoavisoasignacion", 0);
                                command.Parameters.AddWithValue("@estatusasignacion", 1);
                                command.Parameters.AddWithValue("@codigo_programa_academico", "N/A");
                                command.Parameters.AddWithValue("@codigo_nivel_academico", 0);
                                command.Parameters.AddWithValue("@semestre_cursado", "N/A");


                                if (row["Otro"].ToString() == "S")
                                {
                                    valorotroalumno = 1;
                                }
                                else
                                {
                                    valorotroalumno = 0;
                                }

                                command.Parameters.AddWithValue("@otro", valorotroalumno);

                                connection.Open();

                                command.ExecuteNonQuery();

                                connection.Close();

                                verModal("Alerta", "Archivo cargado");
                                ErrViewer.Text = "";

                                TblCargaMasiva.Visible = false;
                                TblBecariosProgramas.Visible = true;
                            }
                        }
                        else
                        {
                            verModal("Alerta", "El número de becarios sobrepasa al indicado, verificar");
                        }
                    }
                    else
                    {
                        verModal("Alerta", "No es un archivo correcto ");
                    }
                }
                else // Programas
                {
                    if (dtExcelRecords.Columns.Contains("Programa"))
                    {
                        if ((dtExcelRecords.Rows.Count + Convert.ToInt64(TxtNumeroProgramas.Text)) <= Convert.ToInt64(totalPeriodo.Text))
                        {
                            foreach (DataRow row in dtExcelRecords.Rows)
                            {
                                // Alta de los becarios solicitados - por programas
                                SqlCommand command = new SqlCommand(string.Format("INSERT INTO tbl_solicitudes_becarios ("
                                                                            + "id_Misolicitud, "
                                                                            + "Codigo_programa_academico,"
                                                                            + "Periodo_cursado,"
                                                                            + "Otro_alumno,"
                                                                            + "Codigo_nivel_academico, "
                                                                            + "Becario_calificacion, "
                                                                            + "Asistencia, "
                                                                            + "Correo_aviso_asignacion, "
                                                                            + "id_estatus_asignacion "
                                                                            + ")"

                                                                            + "VALUES ("
                                                                            + "@idsolicitud, "
                                                                            + "@programa,"
                                                                            + "@periodo,"
                                                                            + "@otro,"
                                                                            + "@nivel, "
                                                                            + "@becariocalificacion, "
                                                                            + "@asistencia, "
                                                                            + "@correoavisoasignacion, "
                                                                            + "@estatusasignacion)"), connection);

                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@idsolicitud", TxtBoxSolicitud.Text);
                                
                                //command.Parameters.AddWithValue("@programa", row["Programa"].ToString());
                                command.Parameters.AddWithValue("@programa", getCodogoProgamaMasiva(row["Programa"].ToString()));
                                command.Parameters.AddWithValue("@periodo", row["Periodo Cursado"].ToString());
                                command.Parameters.AddWithValue("@nivel", DropDownListNivel.SelectedValue);
                                command.Parameters.AddWithValue("@becariocalificacion", "PENDIENTE");
                                command.Parameters.AddWithValue("@asistencia", 0);
                                command.Parameters.AddWithValue("@correoavisoasignacion", 0);
                                command.Parameters.AddWithValue("@estatusasignacion", 1);


                                if (row["Otro"].ToString() == "S")
                                {
                                    valorotroalumno = 1;
                                }
                                else
                                {
                                    valorotroalumno = 0;
                                }

                                command.Parameters.AddWithValue("@otro", valorotroalumno);

                                connection.Open();

                                command.ExecuteNonQuery();

                                connection.Close();

                                verModal("Alerta", " Archivo cargado");
                                ErrViewer.Text = "";
                                TblCargaMasiva.Visible = false;
                                TblBecariosProgramas.Visible = true;
                            }
                        }
                        else
                        {
                            verModal("Alerta", "El número de programas sobrepasa al indicado, verificar");
                        }
                    }
                    else
                    {
                        verModal("Alerta", "No es un archivo correcto ");
                    }
                }
                con.Close();
                loadGridViewData();
            }
        }

        protected void BtnVerificarNomina_Click(object sender, EventArgs e)
        {
            string nominaObtenida = Regex.Replace(TxtBoxNominaEmpleado.Text, @"[^\d]", "");
            TxtBoxNominaEmpleado.Text = "L" + nominaObtenida.Trim();
            //connection.Open();
            loadDatosEmpleado(); // Carga la información del empleado
            //connection.Close();
            BtnDatos.Focus();
        }

        protected void btnAgregarMatricula_Click(object sender, EventArgs e)
        {
            TblBecariosProgramas.Visible = false;
            TblCaptura.Visible = true;
            Matricula.Text = "";
            Funciones.Text = "";
            // Agrega la validación al control de funciones
            otroalumno.Visible = true;
            otroalumno.Checked = true;
            textoOtroAlumno.Visible = true;
            TblOtro.Visible = false;
            Matricula.Visible = true;
            matriculaInstruccion.Visible = true;
            CbNivel.SelectedIndex = 0;
            CbPeriodo.SelectedIndex = 0;
            CbPrograma.SelectedIndex = 0;
            Funciones.Attributes.Add("data-validation-engine", "validate[required]");

        }
        protected void btnAgregarPrograma_Click(object sender, EventArgs e)
        {
            TblBecariosProgramas.Visible = false;
            TblCaptura.Visible = true;
            Matricula.Text = "";
            Funciones.Text = "";
            Matricula.Visible = false;
            TblOtro.Visible = true;
            otroalumno.Checked = true;
            otroalumno.Visible = true;
            textoOtroAlumno.Visible = true;
            matriculaInstruccion.Visible = false;
            // Agrega la validación al control de funciones
            Funciones.Attributes.Add("data-validation-engine", "validate[required]");

        }
        protected void btnCargaMasivaMatricula_Click(object sender, EventArgs e)
        {
                TextBoxTipoCargaExcel.Text = "MATRICULA";
                TblBecariosProgramas.Visible = false;
                TblCargaMasiva.Visible = true;
                lblCarga.Text = "Carga masiva por Matrícula";
        }

        protected void btnCargaMasivaPrograma_Click(object sender, EventArgs e)
        {
            if (DropDownListNivel.SelectedItem.Text == "--Seleccione--")
            {
                verModal("Alerta", "Para continuar debes seleccionar un nivel");
            }
            else
            {
                TextBoxTipoCargaExcel.Text = "PROGRAMA";
                TblBecariosProgramas.Visible = false;
                TblCargaMasiva.Visible = true;
                lblCarga.Text = "Carga masiva por Programa / Semestre";
            }

        }

        protected void BtnCancelarMasiva_Click(object sender, EventArgs e)
        {
            TextBoxTipoCargaExcel.Text = "";
            TblBecariosProgramas.Visible = true;
            TblCargaMasiva.Visible = false;
            ErrViewer.Text = "";
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Matricula.Text = "0";
            Funciones.Text = "0";
            TblCaptura.Visible = false;
            TblBecariosProgramas.Visible = true;
        }

        #endregion

        #region Load's

        private void loaddata()
        {
            /*<-- Datos de encabezado*/
            SqlCommand command = new SqlCommand(
                "SELECT  "
                    + "Nomina,"
                    + "tbl_empleados.Nombre,"
                    + "Apellido_paterno,"
                    + "Apellido_materno,"
                    + "Ubicacion_fisica,"
                    + "Puesto,"
                    + "Departamento,"
                    + "Correo_electronico, "
                    + "tbl_empleados.Codigo_campus, "
                    + "cuenta_con_alumnos, "
                    + "Division, "
                    + "cat_campus.Nombre Campus "
                + "FROM tbl_empleados, cat_campus "
                + "Where (Nomina = @nomina) and tbl_empleados.Codigo_campus = cat_campus.Codigo_campus "
                //+ "Where Nomina = '" + Session["Usuario"].ToString() + "'"
            , connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@nomina", Session["Usuario"].ToString());

            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                NombreSolicitante.Text = dr["Nombre"].ToString();
                NombreSolicitante.Text += " ";
                NombreSolicitante.Text += dr["Apellido_paterno"].ToString();
                NombreSolicitante.Text += " ";
                NombreSolicitante.Text += dr["Apellido_materno"].ToString();
                DatosUbicacion.Text = dr["Ubicacion_fisica"].ToString();
                DatosNomina.Text = dr["Nomina"].ToString();
                DatosPuesto.Text = dr["Puesto"].ToString();
                DatosCorreo.Text = dr["Correo_electronico"].ToString();
                DatosDepartamento.Text = dr["Departamento"].ToString();
                DatosDivision.Text = dr["Division"].ToString();
                DatosCampus.Text = dr["Campus"].ToString();
                solicitanteCampus.Text = dr["cuenta_con_alumnos"].ToString();
                IdCampus.Text = dr["Codigo_campus"].ToString();
            }
            // Periodos
            command = new SqlCommand(string.Format("SELECT Periodo, Descripcion FROM cat_periodos "
                                                  + "Where (Activo = 1) "), connection);
            command.Parameters.Clear();

            CbPeriodos.Items.Add(new ListItem("--Seleccione--", "0"));
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                CbPeriodos.Items.Add(new ListItem(dr["Descripcion"].ToString(), dr["Periodo"].ToString()));
            }
            CbPeriodos.SelectedIndex = 0;
            /* Datos de encabezado --> */

            /* <-- Nivel Academico */
            //LoadListNivel();
            /* Nivel Academico --> */

            /* <-- Periodo Academico */
            LoadListPeriodo();
            /* Periodo Academico --> */
        }


        public void llenarNivel()
        {
            string campus = IdCampus.Text;

            string query = @"select 
                                distinct na.Nivel_academico,
                                na.Codigo_nivel_academico
                                from tbl_alumnos a
                                inner join cat_nivel_academico na on  na.Codigo_nivel_academico=a.Codigo_nivel_academico
                                where a.Codigo_campus='" + campus + @"' and a.Periodo='" + CbPeriodos.SelectedValue + @"' 
                                and a.Clave_apoyo in (
	                            (select s.Clave_apoyo from tbl_apoyo_financiero s 
		                            where Periodo='" + CbPeriodos.SelectedValue + @"' 
		                            and Codigo_campus='" + campus + @"'
                                ))";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                CbNivel.Items.Clear();
                CbNivel.Items.Add(new ListItem("--Seleccione--", "0"));
                foreach (DataRow row in dt.Rows)
                {
                    CbNivel.Items.Add(new ListItem(row["Nivel_academico"].ToString(), row["Codigo_nivel_academico"].ToString()));
                    DropDownListNivel.Items.Add(new ListItem(row["Nivel_academico"].ToString(), row["Codigo_nivel_academico"].ToString())); // Nivel para la captura individual.
                }
                CbNivel.SelectedIndex = 0;
                DropDownListNivel.SelectedIndex = 0;
                LoadListPrograma(Convert.ToInt32(CbNivel.SelectedValue));


            }
            else
            {
                verModal("Alerta", "No hay Información en el nivel");
            }



        }


        private void loaddatamodificacion()
        {
            SqlCommand command = new SqlCommand( // Recupera los datos de la solicitud
                "SELECT "
                    + "Becarios_total,"
                    + "Becarios_periodo,"
                    + "id_proyecto,"
                    + "Ubicacion_alterna,"
                    + "Nomina, "
                    + "Periodo, "
                    + "(SELECT Nomina FROM tbl_empleados emp WHERE emp.Nomina = sol.Nomina) Nomina "
                + "FROM tbl_solicitudes sol "
                + "Where id_Misolicitud = " + TxtBoxSolicitud.Text
            , connection);

            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                totalMatricula.Text = dr["Becarios_total"].ToString();
                totalPeriodo.Text = dr["Becarios_periodo"].ToString();
                totalbecariosseleccionados = Convert.ToInt32(dr["Becarios_total"].ToString());
                TxtUbicacionAlterna.Text = dr["Ubicacion_alterna"].ToString();
                CbPeriodos.SelectedValue = dr["Periodo"].ToString();

                lblIdEmpleadoSolicitud.Text = dr["Nomina"].ToString(); // Recupera el ID del empleado
                TxtBoxProyecto.Text = dr["id_proyecto"].ToString(); // Recupera el ID del proyecto
                TxtBoxNominaEmpleado.Text = dr["Nomina"].ToString(); // Recupera la Nomina del empleado de la solicitud
            }

            loadDatosEmpleado(); // Recupera la informacion del empleado

            command = new SqlCommand( // Recupera la información del proyecto
                "SELECT "
                    + "Nombre,"
                    + "Justificacion,"
                    + "Objetivo,"
                    + "Proyecto_funciones "
                + "From tbl_proyectos "
                + "Where id_proyecto = " + TxtBoxProyecto.Text
            , connection);

            dr = command.ExecuteReader();
            while (dr.Read())
            {
                TxtBoxNombre.Text = dr["Nombre"].ToString();
                TxtBoxJustificacion.Text = dr["Justificacion"].ToString();
                TxtBoxObjetivo.Text = dr["Objetivo"].ToString();
                nombreProyecto2.Text = "<B>Nombre del Proyecto: </B>" + TxtBoxNombre.Text.Trim();
                TxtBoxFunciones.Text = dr["Proyecto_funciones"].ToString();
            }
        }

        private void LoadListNivel()
        {
            SqlCommand command = new SqlCommand(string.Format(
                "SELECT  "
                    + "Codigo_nivel_academico,"
                    + "Nivel_academico "
                + "FROM cat_nivel_academico "
            + " "), connection);

            SqlDataReader dr = command.ExecuteReader();
            DropDownListNivel.Items.Add(new ListItem("--Seleccione--", "0"));
            CbNivel.Items.Add(new ListItem("--Seleccione--", "0"));
            while (dr.Read())
            {
                CbNivel.Items.Add(new ListItem(dr["Nivel_academico"].ToString(), dr["Codigo_nivel_academico"].ToString())); // Nivel para la captura individual
                DropDownListNivel.Items.Add(dr["Nivel_academico"].ToString()); // Nivel para la captura individual.
            }
            CbNivel.SelectedIndex = 0;
            DropDownListNivel.SelectedIndex = 0;
            // Actualiza los controles que dependen del listbox
            LoadListPrograma(Convert.ToInt32(CbNivel.SelectedValue));
        }

        private void LoadListPrograma(int nivelID)
        {
            connection.Open();
            string campus = IdCampus.Text;
            SqlCommand command = new SqlCommand(
                //"SELECT "
                //    + "Codigo_programa_academico,"
                //    + "Nombre_programa_academico "
                //+ "FROM cat_programa_acedemico "
                //+ "WHERE Codigo_nivel_academico = '" + nivelID.ToString().Trim()+"'"



                @"/*Muestra el programa Academico*/
                    select distinct  pr.Nombre_programa_academico 
                    from tbl_alumnos a 
                    inner join cat_programa_acedemico pr on a.Codigo_programa_academico = pr.Codigo_programa_academico
                    where a.Codigo_campus='" + campus + "' and  a.Periodo='" + CbPeriodos.SelectedValue + @"'
                    and a.Clave_apoyo in (select s.Clave_apoyo from tbl_apoyo_financiero s 
		                    where Periodo='" + CbPeriodos.SelectedValue + @"' 
		                    and Codigo_campus='" + campus + @"')
                    and pr.Codigo_nivel_academico=" + nivelID + ""
            , connection);

            SqlDataReader dr = command.ExecuteReader();
            if (dr.HasRows)
            {
                CbPrograma.Items.Clear();
                CbPrograma.Items.Add(new ListItem("--NA--", ""));
                while (dr.Read())
                {
                    CbPrograma.Items.Add(new ListItem(dr["Nombre_programa_academico"].ToString(), dr["Nombre_programa_academico"].ToString()));
                }
                CbPrograma.SelectedIndex = 0;
            }
            else
            {
                CbPrograma.Items.Clear();
                CbPrograma.Items.Add(new ListItem("--NA--", ""));
            }
            connection.Close();
        }

        private void LoadListPeriodo()
        {
            CbPeriodo.Items.Clear();

            SqlCommand command = new SqlCommand(string.Format(
                "SELECT "
                    + "id_grado_cursado,"
                    + "grado "
                + "FROM cat_grado_cursado "), connection);

            SqlDataReader dr = command.ExecuteReader();
            CbPeriodo.Items.Add(new ListItem("--NA--", ""));
            while (dr.Read())
            {
                CbPeriodo.Items.Add(new ListItem(dr["grado"].ToString(), dr["id_grado_cursado"].ToString()));
            }
            CbPeriodo.SelectedIndex = 0;
        }

        private void loadGridViewData()
        {
            // Grid por Matricula
            SqlCommand command = new SqlCommand(string.Format(
                "SELECT "
                    + "ROW_NUMBER() over (Order By id_consecutivo) as Consecutivo,"
                    + "id_consecutivo ID,"
                    + "t1.Matricula,"
                    + "Nombre + ' ' + Apellido_paterno + ' ' + Apellido_materno as Nombre,"
                    + "case when Otro_alumno = 0 then 'NO' else 'SI' END AS Otro "
                    //+ "Otro_alumno as Otro "
                + "FROM tbl_solicitudes_becarios t1 "
                + "INNER JOIN tbl_alumnos t2 "
                + "ON t1.MAtricula = t2.Matricula "
                + "WHERE id_Misolicitud = {0} AND  t1.Matricula is not null ", TxtBoxSolicitud.Text
                ), connection);

            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GrdBecarios.DataSource = dt;
            GrdBecarios.DataBind();

            numerobecarios = (dt.Rows.Count) + 1; // Actualización del contador
            TxtNumeroBecarios.Text = (dt.Rows.Count).ToString();

            // Grid por programas
            command = new SqlCommand(string.Format(
                /*
                "SELECT "
                    + "ROW_NUMBER() over (Order By id_consecutivo) as Consecutivo,"
                    + "id_consecutivo ID,"
                    + "Programa,"
                    + "Periodo_cursado as Periodo,"
                    + "Otro_alumno as Otro "
                + "FROM tbl_solicitudes_becarios "
                + "WHERE id_Misolicitud = {0} AND '$' + rtrim(ltrim(Programa)) + '$' <> '$$'", TxtBoxSolicitud.Text
                */



                //"SELECT "
                //    + "ROW_NUMBER() over (Order by id_consecutivo) as Consecutivo, "
                //    + "id_consecutivo ID, "
                //    + "Programa, "
                //    + "Periodo_cursado as Periodo, "
                //    + "case when Otro_alumno = 0 then 'NO' else 'SI' END AS Otro, "
                //    + "Nivel_academico as Nivel "
                //    + "FROM tbl_solicitudes_becarios "
                //    + "WHERE id_Misolicitud = {0} "
                //    + "AND '$' + rtrim(ltrim(Codigo_programa_academico)) + '$' <> '$$' "
                //    + "AND Matricula is null ", TxtBoxSolicitud.Text), connection);



                @"select  ROW_NUMBER() over (Order by id_consecutivo) as Consecutivo,  
                    id_consecutivo ID,  
                    --Programa,  
                    case when cp.Nombre_programa_academico is null then 'N/A' else cp.Nombre_programa_academico end as Programa,
                    Periodo_cursado as Periodo,  
                    case when Otro_alumno = 0 then 'NO' else 'SI' END AS Otro,  
                    --Nivel_academico as Nivel  
                    CASE WHEN na.Codigo_nivel_academico=0 THEN 'N/A' ELSE na.Nivel_academico END AS Nivel
                    FROM tbl_solicitudes_becarios  sol
                    left JOIN cat_programa_acedemico cp on cp.Codigo_programa_academico= sol.Codigo_programa_academico
                    inner join cat_nivel_academico na on na.Codigo_nivel_academico=sol.Codigo_nivel_academico

                    WHERE id_Misolicitud = {0}  AND Matricula is null ", TxtBoxSolicitud.Text), connection);



            da = new SqlDataAdapter(command);
            dt = new DataTable();
            da.Fill(dt);

            GrdProgramas.DataSource = dt;
            GrdProgramas.DataBind();
            numerobecarios = (dt.Rows.Count) + 1; // Actualización del contador
            TxtNumeroProgramas.Text = (dt.Rows.Count).ToString();
            NoBecarios.Text = "Becarios Matrícula capturados <B>" + TxtNumeroBecarios.Text.ToString() + " / " + totalMatricula.Text.Trim() + "</B> -- Becarios Programa / Semestre capturados <B>" + TxtNumeroProgramas.Text.ToString() + " / " + totalPeriodo.Text.Trim() + "</B>";
            if (TxtNumeroBecarios.Text.ToString() == totalMatricula.Text.Trim())
            {
                btnAgregarMatricula.Enabled = false;
                btnCargaMasivaMatricula.Enabled = false;
            }
            else
            {
                btnAgregarMatricula.Enabled = true;
                btnCargaMasivaMatricula.Enabled = true;
            }
            if (TxtNumeroProgramas.Text.ToString() == totalPeriodo.Text.Trim())
            {
                btnAgregarPrograma.Enabled = false;
                btnCargaMasivaPrograma.Enabled = false;
            }
            else
            {
                btnAgregarPrograma.Enabled = true;
                btnCargaMasivaPrograma.Enabled = true;
            }
        }


        public void llenarSemestreCursado(string programaAcademcio)
        {
            query = @"
                    select   distinct a.Grado_academico
                    from tbl_alumnos a  inner join cat_programa_acedemico pa on pa.Codigo_programa_academico=a.Codigo_programa_academico 
                    where 
                    a.Codigo_campus='" + IdCampus.Text + @"' and  a.Periodo='" + CbPeriodos.SelectedValue + @"'
                    and a.Clave_apoyo in (select s.Clave_apoyo from tbl_apoyo_financiero s 
		                    where Periodo='" + CbPeriodos.SelectedValue + @"' 
		                    and Codigo_campus='" + IdCampus.Text + @"')
                    and pa.Codigo_nivel_academico= " + CbNivel.SelectedValue + @"
                    and pa.Nombre_programa_academico = '" + programaAcademcio + @"'";

            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                CbPeriodo.Items.Clear();
                CbPeriodo.Items.Add(new ListItem("--NA--", ""));
                foreach (DataRow ro in dt.Rows)
                {
                    CbPeriodo.Items.Add(new ListItem(ro["Grado_academico"].ToString(), ro["Grado_academico"].ToString())); // Control de periodos en captura de becario
                }
                CbPeriodo.SelectedIndex = 0;
            }
            else
            {
                verModal("Alerta", "No hay información en los semestre cursado");
            }
        }

        protected void loadDatosEmpleado()
        {
            try
            {
                TxtBoxNominaEmpleado.ForeColor = System.Drawing.Color.Black;
                query = "SELECT   Nomina, tbl_empleados.Nombre, Apellido_paterno, Apellido_materno, "
                                                                  + "Ubicacion_fisica, Puesto, Departamento, Correo_electronico, Extencion_telefonica, "
                                                                  + "Division, cat_campus.Nombre Campus "
                                                                  + "FROM tbl_empleados, cat_campus "
                                                                  + "Where (Nomina = '" + TxtBoxNominaEmpleado.Text + "') and tbl_empleados.Codigo_campus = cat_campus.Codigo_campus";

                dtPrincipal = db.getQuery(conexionBecarios, query);

                //SqlCommand command = new SqlCommand(string.Format("SELECT  id_empleado, Nomina, tbl_empleados.Nombre, Apellido_paterno, Apellido_materno, " 
                //                                                  + "Ubicacion_fisica, Puesto, Departamento, Correo_electronico, Extencion_telefonica, "
                //                                                  + "Division, cat_campus.Nombre Campus "
                //                                                  + "FROM tbl_empleados, cat_campus "
                //                                                  + "Where (Nomina = @nominaEmpleado) and tbl_empleados.id_campus = cat_campus.id_campus"), connection);
                //command.Parameters.Clear();
                //command.Parameters.AddWithValue("@nominaEmpleado", TxtBoxNominaEmpleado.Text.Trim());

                //SqlDataReader dr = command.ExecuteReader();
                if (dtPrincipal.Rows.Count > 0)
                {
                    lblIdEmpleadoSolicitud.Text = dtPrincipal.Rows[0]["Nomina"].ToString();// dr["id_empleado"].ToString(); // Id del empleado solicitante
                    NombreSolicitante1.Text = dtPrincipal.Rows[0]["Nombre"].ToString(); //dr["Nombre"].ToString();
                    NombreSolicitante1.Text += " ";
                    NombreSolicitante1.Text += dtPrincipal.Rows[0]["Apellido_paterno"].ToString(); //dr["Apellido_paterno"].ToString();
                    NombreSolicitante1.Text += " ";
                    NombreSolicitante1.Text += //dr["Apellido_materno"].ToString();
                    DatosUbicacion1.Text = dtPrincipal.Rows[0]["Ubicacion_fisica"].ToString();// dr["Ubicacion_fisica"].ToString();
                    DatosNomina1.Text = //dr["Nomina"].ToString();
                    DatosPuesto1.Text = dtPrincipal.Rows[0]["Puesto"].ToString(); //dr["Puesto"].ToString();
                    DatosDepartamento1.Text = dtPrincipal.Rows[0]["Departamento"].ToString(); //dr["Departamento"].ToString();
                    DatosCorreo1.Text = dtPrincipal.Rows[0]["Correo_electronico"].ToString();//dr["Correo_electronico"].ToString();
                    DatosExtension1.Text = dtPrincipal.Rows[0]["Extencion_telefonica"].ToString();// dr["Extencion_telefonica"].ToString();
                    DatosDivision1.Text = dtPrincipal.Rows[0]["Division"].ToString(); //dr["Division"].ToString();
                    DatosCampus1.Text = dtPrincipal.Rows[0]["Campus"].ToString();//dr["Campus"].ToString();
                    BtnDatos.Enabled = true;
                    TblAsignado.Visible = true;

                }
                else
                {
                    DataTable dtNomina = new DataTable();
                    BasedeDatos dbNomina = new BasedeDatos();
                    dtNomina = dbNomina.infoEmpleados(TxtBoxNominaEmpleado.Text);
                    DataTable dtr;

                    if (dtNomina.Rows.Count > 0)
                    {
                        if (dtNomina.Rows[0]["Nomina"].ToString() != "Nada")
                        {
                            int  valor = int.Parse(dtNomina.Rows[0]["Campus"].ToString());
                            query = "select Nombre from cat_campus where Codigo_sap_campus = " + valor;
                            DataTable dt = dbNomina.getQuery(conexionBecarios, query);
                            //dtNomina.Rows[0]["Estatus"].ToString();                       
                            //dtNomina.Rows[0]["NombreCompleto"].ToString();


                            NombreSolicitante1.Text = dtNomina.Rows[0]["Nombres"].ToString();
                            NombreSolicitante1.Text += " ";
                            NombreSolicitante1.Text += dtNomina.Rows[0]["Apaterno"].ToString();
                            NombreSolicitante1.Text += " ";
                            NombreSolicitante1.Text += dtNomina.Rows[0]["Amaterno"].ToString();
                            DatosUbicacion1.Text = dtNomina.Rows[0]["UFisica"].ToString();
                            DatosNomina1.Text = dtNomina.Rows[0]["Nomina"].ToString();
                            DatosPuesto1.Text = dtNomina.Rows[0]["Puesto"].ToString();
                            DatosDepartamento1.Text = dtNomina.Rows[0]["Departamento"].ToString();
                            DatosCorreo1.Text = dtNomina.Rows[0]["Correo"].ToString();
                            DatosExtension1.Text = dtNomina.Rows[0]["Extencion"].ToString();
                            DatosDivision1.Text = dtNomina.Rows[0]["Divicion"].ToString();
                            DatosCampus1.Text = dt.Rows[0]["Nombre"].ToString();  //dtNomina.Rows[0]["Campus"].ToString();
                            BtnDatos.Enabled = true;
                            TblAsignado.Visible = true;


                            /* //daniel
                            int idCampus = 0;
                            command = new SqlCommand(string.Format("SELECT id_campus FROM cat_campus WHERE Codigo_sap_campus = " + dtNomina.Rows[0]["Campus"].ToString(), connection);
                            try
                            {
                                SqlDataReader drCampus = command.ExecuteReader();
                                while (drCampus.Read())
                                {
                                    idCampus = drCampus[id_campus].
                                }
                                CbPeriodo.SelectedIndex = 0;
                            }
                            catch (System.Data.SqlClient.SqlException ex)
                            {
                                ErrViewer.Text = "Insert Error: " + ex.Message;
                            }
                             //daniel
                            */
                            //command = new SqlCommand(string.Format("INSERT INTO tbl_empleados ("
                            //                                                 + "Nomina, "
                            //                                                 + "Nombre, "
                            //                                                 + "Apellido_paterno, "
                            //                                                 + "Apellido_materno, "
                            //                                                 + "Extencion_telefonica, "
                            //                                                 + "Correo_electronico, "
                            //                                                 + "Division, "
                            //                                                 + "Ubicacion_fisica, "
                            //                                                 + "Puesto, "
                            //                                                 + "Estatus, "
                            //                                                 + "Ubicacion_alterna, "
                            //                                                 + "id_campus, "
                            //                                                 + "Departamento,  "
                            //                                                 + "Grupo_personal, "
                            //                                                 + "Area_personal ) "
                            //                                                 + "VALUES ("
                            //                                                 + "@Nomina, "
                            //                                                 + "@Nombre, "
                            //                                                 + "@Apellido_paterno, "
                            //                                                 + "@Apellido_materno, "
                            //                                                 + "@Extencion_telefonica, "
                            //                                                 + "@Correo_electronico, "
                            //                                                 + "@Division, "
                            //                                                 + "@Ubicacion_fisica, "
                            //                                                 + "@Puesto, "
                            //                                                 + "@Estatus, "
                            //                                                 + "@Ubicacion_alterna, "
                            //                                                 + "@id_campus, "
                            //                                                 + "@Departamento,"
                            //                                                 + "@Grupo_personal, "
                            //                                                 + "@Area_personal)"), connection);
                            //    command.Parameters.Clear();
                            //    command.Parameters.AddWithValue("@Nomina", dtNomina.Rows[0]["Nomina"].ToString());
                            //    command.Parameters.AddWithValue("@Nombre", dtNomina.Rows[0]["Nombres"].ToString());
                            //    command.Parameters.AddWithValue("@Apellido_paterno", dtNomina.Rows[0]["Apaterno"].ToString());
                            //    command.Parameters.AddWithValue("@Apellido_materno", dtNomina.Rows[0]["Amaterno"].ToString());
                            //    command.Parameters.AddWithValue("@Extencion_telefonica", dtNomina.Rows[0]["Extencion"].ToString());
                            //    command.Parameters.AddWithValue("@Correo_electronico", dtNomina.Rows[0]["Correo"].ToString());
                            //    command.Parameters.AddWithValue("@Division", dtNomina.Rows[0]["Divicion"].ToString());
                            //    command.Parameters.AddWithValue("@Ubicacion_fisica", dtNomina.Rows[0]["UFisica"].ToString());
                            //    command.Parameters.AddWithValue("@Puesto", dtNomina.Rows[0]["Puesto"].ToString());
                            //    if (dtNomina.Rows[0]["Estatus"].ToString() == "Activo")
                            //    {
                            //        command.Parameters.AddWithValue("@Estatus", 1);
                            //    }
                            //    else
                            //    {
                            //        command.Parameters.AddWithValue("@Estatus", 0);
                            //    }
                            //    command.Parameters.AddWithValue("@Ubicacion_alterna", "");

                            //    DataTable dts = dbNomina.getQuery(conexionBecarios, "select id_campus from cat_campus where Codigo_sap_campus=" + Convert.ToInt16(dtNomina.Rows[0]["Campus"]) + "");
                            //    command.Parameters.AddWithValue("@id_campus", int.Parse(dts.Rows[0]["id_campus"].ToString()));
                            //    command.Parameters.AddWithValue("@Departamento", dtNomina.Rows[0]["Departamento"].ToString());
                            //    command.Parameters.AddWithValue("@Grupo_personal", int.Parse(dtNomina.Rows[0]["Grupo"].ToString()));
                            //    command.Parameters.AddWithValue("@Area_personal", int.Parse(dtNomina.Rows[0]["Area"].ToString()));
                            //    //command.Parameters.AddWithValue("@Grupo_personal"
                            //    //command.Parameters.AddWithValue("@Area_personal" 
                            //try
                            //{
                            //    command.ExecuteNonQuery();
                            //    //ErrViewer.Text = "Registro creado correctamente";
                            //}
                            //catch (System.Data.SqlClient.SqlException ex)
                            //{
                            //    ErrViewer.Text = "Insert Error: " + ex.Message;
                            //}
                            //command = new SqlCommand(string.Format("SELECT @@Identity"), connection);
                            //command.ExecuteNonQuery();
                            //lblIdEmpleadoSolicitud.Text = command.ExecuteScalar().ToString();

                            try
                            {
                                //Para insertar los datos en el empleado
                                int activo;
                                if (dtNomina.Rows[0]["Estatus"].ToString() == "Activo")
                                {
                                    activo = 1;
                                }
                                else
                                {
                                    activo = 0;
                                }

                                query = "sp_guarda_empleado_especiales '" + dtNomina.Rows[0]["Nomina"].ToString() + "','" + dtNomina.Rows[0]["Nombres"].ToString() + "','" + dtNomina.Rows[0]["Apaterno"].ToString() + "','" + dtNomina.Rows[0]["Amaterno"].ToString() + "','" + dtNomina.Rows[0]["Extencion"].ToString() + "','" + dtNomina.Rows[0]["Correo"].ToString() + "','" + dtNomina.Rows[0]["Divicion"].ToString() + "','" + dtNomina.Rows[0]["UFisica"].ToString() + "','" + dtNomina.Rows[0]["Puesto"].ToString() + "'," + activo + "," + dtNomina.Rows[0]["Campus"] + ",'" + dtNomina.Rows[0]["Departamento"].ToString() + "'," + dtNomina.Rows[0]["Grupo"].ToString() + "," + dtNomina.Rows[0]["Area"].ToString() + "";
                                dtr = dbNomina.getQuery(conexionBecarios, query);
                                lblIdEmpleadoSolicitud.Text = dtr.Rows[0]["Nomina"].ToString();

                            }
                            catch (Exception es)
                            {
                                ErrViewer.Text = "Insert Error: " + es.Message;
                            }


                        }
                        else
                        {
                            verModal("Alerta", "No existe informacion disponible para la nómina " + TxtBoxNominaEmpleado.Text);
                            TxtBoxNominaEmpleado.ForeColor = System.Drawing.Color.Green;
                            TxtBoxNominaEmpleado.Text = "No Existe";
                            BtnDatos.Enabled = false;
                            TblAsignado.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ess)
            {
                ErrViewer.Text = ess.Message.ToString();
            }

            
        }

        #endregion

        #region Utilerias

        protected string reemplazarClaves(string cadena)
        {
            string  substring = ""; // Inicializo variable auxiliar
            //SqlCommand command;

            if(cadena.IndexOf("!Proyecto!") != -1)
            {
                cadena = cadena.Replace("!Proyecto!", TxtBoxNombre.Text.Trim());
            }

            if(cadena.IndexOf("!Justificacion!") != -1)
            {
                cadena = cadena.Replace("!Justificacion!", TxtBoxJustificacion.Text.Trim());
            }

            if (cadena.IndexOf("!Cantidad!") != -1)
            {
                Int64 total = Convert.ToInt64(TxtNumeroBecarios.Text.Trim()) + Convert.ToInt64(TxtNumeroProgramas.Text.Trim());
                cadena = cadena.Replace("!Cantidad!", total.ToString());
            }

            if (cadena.IndexOf("!Matricula!") != -1)
            {
                substring = "";

                command = new SqlCommand(string.Format(
                     
                /**
                    //original
                    "Select "
                    + "ROW_NUMBER() over (Order By id_consecutivo) as #,"
                    + "Matricula,"
                    + "Nombre + ' ' + Apellido_paterno + ' ' + Apellido_materno as Nombre,"
                    + "Otro_alumno as Otro "

                + "From tbl_solicitudes_becarios t1 "
                + "Inner Join tbl_alumnos t2 "
                + "On t1.id_alumno = t2.id_alumno "
                + "Where id_Misolicitud = {0} AND (t1.id_alumno <> '4' AND '$' + rtrim(ltrim(t1.id_alumno)) + '$' <> '$$')", TxtBoxSolicitud.Text.Trim()

                */
                    //original
                    "Select "
                    + "ROW_NUMBER() over (Order By id_consecutivo) as #,"
                    + "Matricula,"
                    + "Nombre + ' ' + Apellido_paterno + ' ' + Apellido_materno as Nombre,"
                    + "Otro_alumno as Otro "

                + "From tbl_solicitudes_becarios t1 "
                + "Inner Join tbl_alumnos t2 "
                + "On t1.Matricula = t2.Matricula "
                + "Where id_Misolicitud = {0} ", TxtBoxSolicitud.Text.Trim()


                ), connection);

                substring = "<table>"
                    + "<tr>"
                        + "<td>#</td>"
                        + "<td>Matricula</td>"
                        + "<td>Nombre</td>"
                    + "</tr>";

                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    substring += 
                        "<tr>"
                            + "<td>" + dr["#"].ToString() + "</td>"
                            + "<td>" + dr["Matricula"].ToString() + "</td>"
                            + "<td>" + dr["Nombre"].ToString() + "</td>"
                        + "</tr>";
                }

                substring += "</table>";

                cadena = cadena.Replace("!Matricula!", substring);
            }

            if (cadena.IndexOf("!Nombre!") != -1)
            {
                cadena = cadena.Replace("!Nombre!", NombreSolicitante1.Text.Trim());
            }

            if (cadena.IndexOf("!Programa!") != -1)
            {
                substring = "";

                command = new SqlCommand(string.Format(
                    
                    /**    
                    //original
                    "Select "
                    + "ROW_NUMBER() over (Order By id_consecutivo) as #,"
                    + "Programa,"
                    + "Periodo_cursado as Periodo,"
                    + "Otro_alumno as Otro,"
                    + "Nivel_academico "
                + "From tbl_solicitudes_becarios "
                + "Where id_alumno is null   AND  id_Misolicitud = {0} AND '$' + rtrim(ltrim(Programa)) + '$' <> '$$'", TxtBoxSolicitud.Text.Trim()
                ), connection);

                    */
                     "Select "
                    + "ROW_NUMBER() over (Order By id_consecutivo) as #,"
                    + "Programa,"
                    + "Periodo_cursado as Periodo,"
                    + "Otro_alumno as Otro,"
                    + "Nivel_academico "
                + "From tbl_solicitudes_becarios "
                + "Where Matricula is null   AND  id_Misolicitud = {0} AND '$' + rtrim(ltrim(Programa)) + '$' <> '$$'", TxtBoxSolicitud.Text.Trim()
                ), connection);






                substring = "<table>"
                    + "<tr>"
                        + "<td>#</td>"
                        + "<td>Nivel</td>"
                        + "<td>Programa</td>"
                        + "<td>Semestre</td>"
                    + "</tr>";

                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    substring +=
                        "<tr>"
                            + "<td>" + dr["#"].ToString() + "</td>"
                            + "<td>" + dr["Nivel_academico"].ToString() + "</td>"
                            + "<td>" + dr["Programa"].ToString() + "</td>"
                            + "<td>" + dr["Periodo"].ToString() + "</td>"
                        + "</tr>";
                }

                substring += "</table>";

                cadena = cadena.Replace("!Programa!", substring);
            }

            if (cadena.IndexOf("!Periodo!") != -1)
            {
                cadena = cadena.Replace("!Periodo!", CbPeriodos.SelectedItem.Text);
            }

            return cadena;
        }

        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        #endregion

        #region GrdBecarios

        protected void GrdBecarios_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //Just changed the index of cells based on your requirements
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;
            }
        }

        protected void GrdBecarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument); // index
            GridViewRow row = GrdBecarios.Rows[index]; // row
            int rowID = Convert.ToInt32(row.Cells[1].Text);

            if (e.CommandName == "Eliminar")
            {
                connection.Open(); // Abertura de la conexión

                EliminarBecario(rowID);

                connection.Close(); // Cierre de la conexión

                loadGridViewData(); // Refresca la informacion de los grid.
            }
        }

        protected void EliminarBecario(int rowID)
        {
            SqlCommand cmmd = new SqlCommand(
                "DELETE FROM tbl_solicitudes_becarios "
                + "WHERE id_consecutivo = " + rowID.ToString().Trim()
                , connection);
            cmmd.ExecuteNonQuery();
        }

        #endregion

        #region GrdProgramas

        protected void GrdProgramas_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //Just changed the index of cells based on your requirements
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;
            }
        }

        protected void GrdProgramas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument); // index
            GridViewRow row = GrdProgramas.Rows[index]; // row
            int rowID = Convert.ToInt32(row.Cells[1].Text);

            if (e.CommandName == "Eliminar")
            {
                connection.Open(); // Abertura de la conexión

                EliminarBecario(rowID);

                connection.Close(); // Cierre de la conexión

                loadGridViewData(); // Refresca la informacion de los grid.
            }
        }

        #endregion

        protected void CbPrograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                llenarSemestreCursado(CbPrograma.SelectedItem.Text);
            }
            catch (Exception es)
            {
                verModal("Alerta",es.Message.ToString());
            }
        }

        

    }
}