using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Net.Mail;
using ServicioBecario.Codigo;
using System.Web.Services;
namespace ServicioBecario.Vistas
{
    public partial class EspecificaIndividual : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        BasedeDatos db = new BasedeDatos();
        Int32 idsolicitud = 0;        
        Int32 valorotroalumno = 0;
        string idalumno;
        Int32 numerobecarios = 0;
        Int32 totalbecariosseleccionados = 0;
        string TotalBecariosBd;
        static string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        public string cadenaConexion = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        DataTable dt;
        Correo mail = new Correo();
        
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
            command.Connection = connection;
            TxtBoxNominaEmpleado.Text = Session["Usuario"].ToString();
            //Request.QueryString.ToString()); // Obtiene el id de solicitud

            if (Session["Solicitud"] != null)//(Request.QueryString.ToString() != "")
            {
                
                //this.idsolicitud = Convert.ToInt32(Session["Usuario"].ToString());//Request.QueryString.ToString()); // Obtiene el id de solicitud
                if (!IsPostBack)
                {
                    this.idsolicitud = Convert.ToInt32(Session["Solicitud"].ToString());
                    TxtSolicitud.Text = idsolicitud.ToString();
                    TblCaptura.Visible = false;
                    BtnDatos.Visible = true;
                    TblAlta.Visible = true;
                    TxtEstatusSolicitud.Text = "MODIFICACION";
                    loaddata();
                    loaddatamodificacion();
                    loaddatabecarios();
                    Session.Remove("Solicitud");
                    Session["Periodo"] = "0";
                    /*eSTO LO PUESE NEF*/
                    Session["Periodo"] = CbPeriodos.SelectedValue;
                    
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    this.idsolicitud = 0;
                    Session.Remove("Solicitud");
                    // Carga de combo box
                    loaddata();
                    
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
            if (Agregar.Enabled)
            {
                Funciones.Attributes.Add("data-validation-engine", "validate[required]");
            }
            else
            {
                Funciones.Attributes.Remove("data-validation-engine");
            }
        }

        public string getCodogoProgama(string programaAcademico)
        {
            string codigoprograma = "";
            string query="";

            string campus = "";
            /*Verificamos el campus asignador*/
            if (solicitanteCampus.Text == "False")
            {
                campus = campusAlterno.SelectedValue;
            }
            else
            {
                campus = IdCampus.Text;
            }


            if(programaAcademico!="")
            {
               query=" sp_pogramaAcademico_aleatorio '"+programaAcademico+"','"+CbPeriodos.SelectedValue+"','"+campus+"',"+CbNivel.SelectedValue+" ";
               dt = db.getQuery(conexionBecarios,query);
               if(dt.Rows.Count>0)
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


        public void llenarSemestreCursado(string programaAcademcio)
        {
            string campus = "";
            /*Verificamos el campus asignador*/
            if (solicitanteCampus.Text == "False")
            {
                campus = campusAlterno.SelectedValue;
            }
            else
            {
                campus = IdCampus.Text;
            }

            string query = @"
                    select   distinct a.Grado_academico
                    from tbl_alumnos a  inner join cat_programa_acedemico pa on pa.Codigo_programa_academico=a.Codigo_programa_academico 
                    where 
                    a.Codigo_campus='" + campus + @"' and  a.Periodo='" + CbPeriodos.SelectedValue + @"'
                    and a.Clave_apoyo in (select s.Clave_apoyo from tbl_apoyo_financiero s 
		                    where Periodo='" + CbPeriodos.SelectedValue + @"' 
		                    and Codigo_campus='" + campus + @"')
                    and pa.Codigo_nivel_academico= " + CbNivel.SelectedValue + @"
                    and pa.Nombre_programa_academico = '" + programaAcademcio + @"'";

            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                CbPeriodo.Items.Clear();
                CbPeriodo.Items.Add(new ListItem("No aplica", ""));
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


        protected void Agregar_Click(object sender, EventArgs e)
        {
            string error = "NO";
            string tipo = "";
            string query = "";
            string codigo_programa_academico = "";
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
                string  idalumno = "0";

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
                            //idalumno = Convert.ToInt32(dr["id_alumno"].ToString());
                        }
                    }

                }
         

                ///Verificar la duplicidad
                command = new SqlCommand( // Recupera el id de alumno
                            "SELECT "
                                + "Matricula "
                            + "FROM tbl_solicitudes_becarios "
                            + "WHERE id_Misolicitud = '" + TxtSolicitud.Text.Trim() + "' and Matricula = '" + idalumno + "'" 
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
                        command = new SqlCommand( // Query para insertar el becario
                        string.Format("INSERT INTO tbl_solicitudes_becarios ("
                                                                        + "id_Misolicitud, "
                                                                        + "Matricula, "
                                                                        + "Programa, "
                                                                        + "Periodo_cursado, "
                                                                        + "Otro_alumno, "
                                                                        + "Becario_calificacion, "
                        //+ "Becario_puntaje, "
                                                                        + "Becario_funciones, "
                                                                        + "id_estatus_asignacion, "
                                                                        + "Asistencia, "
                                                                        + "Correo_aviso_asignacion, "
                                                                        + "Nivel_academico, "
                                                                        + "Codigo_programa_academico, "
                                                                        + "Codigo_nivel_academico )"
                        //+ "Asistencia_fecha) "                        +
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
                                                                        + "@nivelacademico, " 
                                                                        + "@iCodigo_programa_academico, "
                                                                        + "@iCodigo_nivel_academico ) "), connection);
                    //+ "@asistenciafecha) "), connection);



                    command.Parameters.Clear();

                    if (otroalumno.Checked == false)
                    {
                        valorotroalumno = 0;                       
                    }
                    else
                    {
                        valorotroalumno = 1;
                    }
                    command.Parameters.AddWithValue("@idmisolicitud", TxtSolicitud.Text.Trim());

                    if (idalumno == "0")
                    {
                        command.Parameters.AddWithValue("@idalumno", DBNull.Value);
                        if (CbPeriodo.SelectedItem.Text == "No aplica")
                        {
                            command.Parameters.AddWithValue("@idperiodocursado", "N/A");
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@idperiodocursado", CbPeriodo.SelectedItem.Text);
                        }
                        if (CbPrograma.SelectedItem.Text == "No aplica")
                        {
                            command.Parameters.AddWithValue("@idprograma", "N/A");
                            command.Parameters.AddWithValue("@iCodigo_programa_academico", "N/A");
                        }
                        else
                        {
                            codigo_programa_academico = getCodogoProgama(CbPrograma.SelectedItem.Text);
                            command.Parameters.AddWithValue("@idprograma", CbPrograma.SelectedItem.Text);
                            //command.Parameters.AddWithValue("@iCodigo_programa_academico", CbPrograma.SelectedValue);
                            command.Parameters.AddWithValue("@iCodigo_programa_academico", codigo_programa_academico);
                        }
                        if (CbNivel.SelectedItem.Text == "--Seleccione--")
                        {
                            command.Parameters.AddWithValue("@nivelacademico", "N/A");
                            command.Parameters.AddWithValue("@iCodigo_nivel_academico", 0);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@nivelacademico", CbNivel.SelectedItem.Text);
                            command.Parameters.AddWithValue("@iCodigo_nivel_academico", CbNivel.SelectedValue);
                        }


                    }
                    else
                    {
                        command.Parameters.AddWithValue("@idalumno", Matricula.Text.Substring(0, 9).Trim());
                        command.Parameters.AddWithValue("@idperiodocursado", "N/A");
                        command.Parameters.AddWithValue("@idprograma", "N/A");
                        command.Parameters.AddWithValue("@nivelacademico", "N/A");
                        command.Parameters.AddWithValue("@iCodigo_programa_academico", "N/A");
                        command.Parameters.AddWithValue("@iCodigo_nivel_academico", 0);
                    }

                    command.Parameters.AddWithValue("@otroalumno", valorotroalumno);
                    command.Parameters.AddWithValue("@becariocalificacion", "PENDIENTE");
                    command.Parameters.AddWithValue("@becariopuntaje", 0);
                    command.Parameters.AddWithValue("@becariofunciones", Funciones.Text);
                    command.Parameters.AddWithValue("@idestatusasignacion", 1);
                    command.Parameters.AddWithValue("@asistencia", 0);
                    command.Parameters.AddWithValue("@correoavisoasignacion", 0);
                    command.Parameters.AddWithValue("@asistenciafecha", DateTime.Today);
                    //}

                    try
                    {
                        command.ExecuteNonQuery();

                        Matricula.Text = "";
                        Funciones.Text = "";
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        ErrViewer.Text = "Insert Error: " + ex.Message;
                    }

                    connection.Close(); // Cierre de la conexión

                    loaddatabecarios(); // recarga el grid de becarios agregados
                }
            }
            else
            {
                if (tipo == "NIVEL")
                {
                    verModal("Alerta", " No has seleccionado el nivel ");
                    CbNivel.Focus();
                }
                else
                {
                    verModal("Alerta", " La matrícula no existe ");
                }
            }
        }
 
        protected void BtnDatos_Click(object sender, EventArgs e)
        {
            string query = @"select fs.Fecha_inicio,fs.Fecha_fin
                                from tbl_campus_periodo cp inner join tbl_fechas_solicitudes fs on cp.id_campus_periodo=fs.id_campus_periodo
                                inner join tbl_empleados e on e.Codigo_campus=cp.Codigo_campus

                                where Nomina='" + Session["usuario"].ToString() + "' and cp.Periodo='" + CbPeriodos.SelectedValue + "'";
            dt = db.getQuery(cadenaConexion,query);
            if(dt.Rows.Count>0)
            {
               //verifica las fechas 
               if (DateTime.Today >= Convert.ToDateTime(dt.Rows[0]["Fecha_inicio"]) && DateTime.Today <= Convert.ToDateTime(dt.Rows[0]["Fecha_fin"]))
               {
                   if (CbPeriodos.SelectedValue == "")
                   {
                       verModal("Alerta", " No se ha seleccionado un periodo ");
                   }
                   else
                   {
                       totalbecariosseleccionados = (Convert.ToInt32(CbTotalBecarios.SelectedValue));
                       if (totalbecariosseleccionados == 0)
                       {
                           verModal("Alerta", " No hay becarios asignados ");
                       }
                       else
                       {
                           //aqui tiene que ir lo del nivel
                           llenarNivel();
                           if (TxtEstatusSolicitud.Text == "MODIFICACION")
                           {
                               totalbecariosseleccionados = (Convert.ToInt32(CbTotalBecarios.SelectedValue));
                               TblCaptura.Visible = true;
                               BtnDatos.Visible = false;
                               TblAlta.Visible = false;
                           }
                           else
                           {
                               
                               TblCaptura.Visible = true;
                               BtnDatos.Visible = false;
                               TblAlta.Visible = false;
                               string fechasolicitud = DateTime.Today.ToShortDateString();
                               SqlCommand command = new SqlCommand(string.Format("INSERT INTO tbl_solicitudes ("
                                                                                   + "Periodo, "
                                                                                   + "Nomina, "
                                                                                   + "id_tipo_solicitud, "
                                                                                   + "id_solicitud_estatus, "
                                                                                   + "Fecha_solicitud, "
                                                                                   + "Empleado_calificacion, "
                                                                                   + "id_campus_alterno, "
                                                                                   + "Becarios_total ) "
                                                                                   + "VALUES ("
                                                                                   + "@idperiodo, "
                                                                                   + "(SELECT Nomina FROM tbl_empleados WHERE Nomina = @nomina),"
                                                                                   + "@idtiposolicitud, "
                                                                                   + "@idsolicitudestatus,"
                                                                                   + "@fechasolicitud, "
                                                                                   + "@empeladocalificacion, "
                                                                                   + "@idcampusalterno, "
                                                                                   + "@becariostotal) "), connection);
                               command.Parameters.Clear();
                               command.Parameters.AddWithValue("@idperiodo", CbPeriodos.SelectedValue);
                               command.Parameters.AddWithValue("@nomina", TxtBoxNominaEmpleado.Text);
                               command.Parameters.AddWithValue("@idtiposolicitud", 1);
                               command.Parameters.AddWithValue("@idsolicitudestatus", 2);
                               command.Parameters.AddWithValue("@fechasolicitud", DateTime.Today);
                               command.Parameters.AddWithValue("@empeladocalificacion", "PENDIENTE");
                               if (solicitanteCampus.Text == "False")
                               {
                                   command.Parameters.AddWithValue("@idcampusalterno", campusAlterno.SelectedValue);
                                   Session["CampusAlterno"] = campusAlterno.SelectedValue.ToString();
                               }
                               else
                               {
                                   command.Parameters.AddWithValue("@idcampusalterno", System.Data.SqlTypes.SqlInt32.Null);
                                   Session.Remove("CampusAlterno");
                               }
                               command.Parameters.AddWithValue("@becariostotal", CbTotalBecarios.SelectedValue);
                               //nef
                               Session["Periodo"] = CbPeriodos.SelectedValue;
                               try
                               {
                                   connection.Open();
                                   command.ExecuteNonQuery();
                               }
                               catch (System.Data.SqlClient.SqlException ex)
                               {
                                   ErrViewer.Text = "Insert Error: " + ex.Message;
                               }
                               finally
                               {
                                   command = new SqlCommand(string.Format("SELECT @@Identity"), connection);
                                   command.ExecuteNonQuery();
                                   idsolicitud = Convert.ToInt32(command.ExecuteScalar());
                                   //ErrViewer.Text = "Ok  ";
                                   TxtSolicitud.Text = idsolicitud.ToString();
                                   connection.Close();
                               }
                           }
                           loaddatabecarios();
                       }
                   }
               }
               else
               {
                   if(DateTime.Today>Convert.ToDateTime(dt.Rows[0]["Fecha_fin"]))
                   {
                       verModal("Alerta", "Ya pasaron las fechas de solicitudes para el periodo "+CbPeriodos.SelectedItem.Text+"");
                   }
                   if(DateTime.Today<Convert.ToDateTime(dt.Rows[0]["Fecha_inicio"]))
                   {
                       verModal("Alerta", "Aún no son las fechas de solicitudes para el periodo "+CbPeriodos.SelectedItem.Text+"");
                   }
               }
            }
            else
            {
                verModal("Alerta", "No existen fechas de solicitud en el periodo");
            }
            
            
        }

        protected void Enviar_Click(object sender, EventArgs e)
        {
            if (ChkAcuerdo.Checked)
            {
                BasedeDatos db = new BasedeDatos();

                if (GrdResultado.Rows.Count >= Convert.ToInt32(CbTotalBecarios.SelectedValue))
                {
                    SqlCommand command = new SqlCommand(string.Format("UPDATE tbl_solicitudes SET "
                                                        + "Ubicacion_alterna = @ubicacionalterna, "
                                                        + "Acepto = @acepto, "
                                                        + "Becarios_total = @becariostotal, "
                                                        + "id_campus_alterno = @idcampusalterno "
                                                        + "Where (id_MiSolicitud = @idmisolicitud) "), connection);

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@idmisolicitud", TxtSolicitud.Text.Trim());
                    command.Parameters.AddWithValue("@ubicacionalterna", TxtUbicacionAlterna.Text);
                    command.Parameters.AddWithValue("@becariostotal", CbTotalBecarios.SelectedValue);
                    command.Parameters.AddWithValue("@idcampusalterno", campusAlterno.SelectedValue);
                    command.Parameters.AddWithValue("@acepto", "SI");

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        ErrViewer.Text = "Insert Error: " + ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                    TblCaptura.Visible = false;
                    TblAlta.Visible = false;
                    TblResultado.Visible = true;
                    if (TxtEstatusSolicitud.Text == "MODIFICACION")
                    {
                        NoSolicitud.Text += TxtSolicitud.Text + " (Modificación)";
                    }
                    else
                    {
                        NoSolicitud.Text += TxtSolicitud.Text;
                    }
                    if (Convert.ToInt32(CbTotalBecarios.SelectedValue.ToString()) > 1)
                    {
                        TotalRegistrados.Text = "con " + TxtNumeroBecarios.Text + " becarios";
                    }
                    else
                    {
                        TotalRegistrados.Text = "con " + TxtNumeroBecarios.Text + " becario";
                    }
                    ErrViewer.Text = "";

                    /*<-- Envío de correo*/
                    //Mail mail; // Configuración de envío de correo
                    if (TxtEstatusSolicitud.Text == "MODIFICACION") // Mensaje si es modificación
                    {
                        command = new SqlCommand(string.Format(
                        "SELECT "
                            + "Asunto,"
                            + "Cuerpo "
                        + "FROM tbl_cuerpo_correo t1 "
                        + "INNER JOIN cat_correos t2 "
                        + "ON t1.id_correo = t2.id_correo "
                        + "WHERE rtrim(ltrim(Tipo_correo)) = 'Modificación a la solicitud(Individual)' "
                        + "AND t1.Codigo_campus = (Select Codigo_campus FROM tbl_empleados WHERE Nomina = '{0}')",
                            TxtBoxNominaEmpleado.Text.Trim()), connection);
                    }
                    else // Mensaje si es una nueva solicitud 
                    {
                        command = new SqlCommand(string.Format(
                            "SELECT "
                                + "Asunto,"
                                + "Cuerpo "
                            + "FROM tbl_cuerpo_correo t1 "
                            + "INNER Join cat_correos t2 "
                            + "ON t1.id_correo = t2.id_correo "
                            + "WHERE rtrim(ltrim(Tipo_correo)) = 'Confirmación de registro(Individual)' "
                            + "AND t1.Codigo_campus = (Select Codigo_campus From tbl_empleados WHERE Nomina = '{0}')",
                                TxtBoxNominaEmpleado.Text.Trim()), connection);
                    }

                    connection.Open();
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        
                        //MailMessage message = new MailMessage("servicio.becario@itesm.mx", DatosCorreo.Text.Trim());
                        //message.Subject = dr["Asunto"].ToString();
                        //message.Body = reemplazarClaves(dr["Cuerpo"].ToString()) + db.noEnvio();
                        //message.IsBodyHtml = true;

                        
                        //mail.MandarCorreo(message);


                        mandarCorreo(reemplazarClaves(dr["Cuerpo"].ToString()) + db.noEnvio(), dr["Asunto"].ToString(), DatosCorreo.Text.Trim());

                    }
                    connection.Close();
                    /* Envío de correo --> */
                }
                else
                {
                    verModal("Alerta", " No se han capturado todos los becarios ");
                    //ErrViewer.Text = "No se han capturado todos los becarios";
                }
            }
            else
            {
                verModal("Alerta", " No has aceptado el Reglamento de Servicio Becario ");
            }
        }

        /*
        protected void ChkAcuerdo_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkAcuerdo.Checked)
            {
                ChkAcuerdo.Attributes.Remove("data-validation-engine");
            }
            else
            {
                ChkAcuerdo.Attributes.Add("data-validation-engine", "validate[required]");
            }
        }
        */


        public void mandarCorreo(string cuerpo, string asunto, string correo)
        {
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
            switch(ambiente)
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
                //                MessageBox.Show("Mensaje Enviado Correctamente", "Correo C#", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //              sw = false;

            }
            catch (System.Net.Mail.SmtpException e)
            {
                
                //Response.Write(e);
                verModal("Error", e.ToString());
            }

        }
        protected void CbNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nivelID = Convert.ToInt32(CbNivel.SelectedValue);

            LoadListPrograma(nivelID);
        }

        #endregion

        #region Load's

        private void loaddata()
        {
            string duplicado = "NO";
            SqlCommand command = new SqlCommand(string.Format(
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
                + "Where (Nomina = @nomina) and tbl_empleados.Codigo_campus = cat_campus.Codigo_campus"
                ), connection);

            command.Parameters.Clear();
            command.Parameters.AddWithValue("@nomina", TxtBoxNominaEmpleado.Text);
            connection.Open();

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
                DatosDepartamento.Text = dr["Departamento"].ToString();
                DatosCorreo.Text = dr["Correo_electronico"].ToString();
                DatosDivision.Text = dr["Division"].ToString();
                DatosCampus.Text = dr["Campus"].ToString();
                solicitanteCampus.Text = dr["cuenta_con_alumnos"].ToString();
                IdCampus.Text = dr["Codigo_campus"].ToString();
            }
            //Verificar que no tenga otra
            command = new SqlCommand(string.Format(
                "SELECT id_misolicitud, ts.Periodo, Activo, te.Nomina "
                    + "FROM tbl_solicitudes as ts, cat_periodos as cp, Tbl_empleados as te "
                    + "Where (te.Nomina = @nomina) and (Activo = 1 and id_tipo_solicitud = 1 and ts.Nomina = te.Nomina) "
            ), connection);

            command.Parameters.Clear();
            command.Parameters.AddWithValue("@nomina", Session["Usuario"].ToString());

            dr = command.ExecuteReader();
            while (dr.Read())
            {
                duplicado = "SI";
                if (TxtSolicitud.Text != "")
                {
                    duplicado = "NO";
                }
            }

            connection.Close();
            if (duplicado == "NO")
            {
                if (solicitanteCampus.Text == "False")
                {
                    TblCampusAlterno.Visible = true;
                    command = new SqlCommand(string.Format(
                        "SELECT  cat_campus.Codigo_campus, cat_campus.Nombre "
                        + "FROM cat_campus, cat_campus_asignacion "
                    + "WHERE cat_campus_asignacion.Codigo_campus = @idcampus  and cat_campus.Codigo_campus = cat_campus_asignacion.id_sub_campus "), connection);

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@nomina", TxtBoxNominaEmpleado.Text);
                    command.Parameters.AddWithValue("@idcampus", IdCampus.Text);
                    connection.Open(); // Abertura de la conexión

                    dr = command.ExecuteReader();
                    CbNivel.Items.Clear();
                    while (dr.Read())
                    {
                        campusAlterno.Items.Add(new ListItem(dr["Nombre"].ToString(), dr["Codigo_campus"].ToString()));
                    }
                    connection.Close();
                }


                // Cantidad de Becarios
                int x = 0;
                int i = 0;
                command = new SqlCommand(string.Format(
                    "SELECT "
                        + "Cantidad_becario "
                    + "FROM tbl_nomina_especifico "
                    + "WHERE (Nomina = @nomina) "), connection);

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@nomina", TxtBoxNominaEmpleado.Text);

                connection.Open(); // Abertura de la conexion

                dr = command.ExecuteReader();


                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        i = Convert.ToInt32(dr["Cantidad_becario"].ToString());
                    }
                }
                else
                {
                    command = new SqlCommand(
                        "SELECT "
                            + "Cantidad_becarios "
                        + "FROM tbl_detalles_puestos pue, tbl_empleados emp "
                        + "WHERE pue.Grupo_personal = emp.Grupo_personal "
                            + "AND pue.Area_personal = emp.Area_personal "
                            + "AND pue.Codigo_campus = emp.Codigo_campus "
                            + "AND Nomina = '" + TxtBoxNominaEmpleado.Text + "'"
                        , connection);

                    dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            i = Convert.ToInt32(dr["Cantidad_becarios"].ToString());
                        }
                    }
                    else
                    {
                        //i = 0; // El empleado no puede solicitar becarios
                        command = new SqlCommand(
                        "SELECT "
                            + "Cantidad_becarios "
                        + "FROM tbl_detalles_puestos pue, tbl_empleados emp "
                        + "WHERE pue.Grupo_personal = 0 "
                            + "AND pue.Area_personal = 0 "
                            + "AND pue.Codigo_campus = emp.Codigo_campus "
                            + "AND Nomina = '" + TxtBoxNominaEmpleado.Text + "'"
                        , connection);

                        dr = command.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                i = Convert.ToInt32(dr["Cantidad_becarios"].ToString());
                            }
                        }
                        else
                        {
                            i = 0; // El empleado no puede solicitar becarios
                        }
                    }
                }

                connection.Close(); // Cierre de la conexion
                CbTotalBecarios.Items.Add(new ListItem("--Seleccione--", "0"));
                if (i == 0)
                {
                    CbTotalBecarios.Items.Add("0");
                }
                else
                {
                    for (x = 1; x <= i; x++)
                    {
                        CbTotalBecarios.Items.Add(x.ToString());
                    }
                }
                CbTotalBecarios.SelectedIndex = 0;
                connection.Close(); // Cierre de la conexión

                //Nivel
                //LoadListNivel();

                //Periodo
                LoadListPeriodo();
            }
            else
            {
                BtnDatos.Enabled = false;
                CbPeriodos.Enabled = false;
                CbTotalBecarios.Enabled = false;
                verModal("Notificación", " Ya has dado de alta una solicitud ");
            }
        }

        private void loaddatabecarios()
        {
            SqlDataAdapter da;
            DataTable dt;

            connection.Open(); // Abertura de conexion
 
            // Grid de edicion de becarios
            SqlCommand command = new SqlCommand(
                string.Format(
                    "SELECT "
                        + "id_consecutivo ConsecutivoId,"
                        + "(	SELECT "
                            + "Matricula "
                            + "FROM tbl_alumnos alm "
                            + "WHERE alm.Matricula = sol.Matricula"
                        + ") as Matricula,"
                        + "(	SELECT "
                                + "Nombre + ' ' + Apellido_Paterno + ' ' + Apellido_Materno "
                            + "FROM tbl_alumnos alm "
                            + "WHERE alm.Matricula = sol.Matricula"
                        + ") as Becario,"
                        + "case when Otro_alumno = 0 then 'NO' else 'SI' END AS Otro, "
                        + "Programa,"
                        + "Periodo_cursado as Periodo,"
                        + "Nivel_academico as Nivel "
                    + "FROM tbl_solicitudes_becarios sol "
                    + "WHERE (id_Misolicitud = @idmisolicitud)"), connection);

            command.Parameters.Clear();
            command.Parameters.AddWithValue("@idmisolicitud", TxtSolicitud.Text.Trim());

            da = new SqlDataAdapter(command);
            dt = new DataTable();
            da.Fill(dt);

            GrdBecarios.DataSource = dt;
            GrdBecarios.DataBind();

            // Grid de becarios final
            command = new SqlCommand(
                string.Format(
                //"SELECT "
                //    + "(	SELECT "
                //        + "Matricula "
                //        + "FROM tbl_alumnos alm "
                //        + "WHERE alm.Matricula = sol.Matricula"
                //    + ") as Matricula,"
                //    + "(	SELECT "
                //            + "Nombre + ' ' + Apellido_Paterno + ' ' + Apellido_Materno "
                //        + "FROM tbl_alumnos alm "
                //        + "WHERE alm.Matricula = sol.Matricula"
                //    + ") as Becario,"
                //    + "case when Otro_alumno = 0 then 'NO' else 'SI' END AS 'Otro Alumno', "
                //    + "Programa,"
                //    + "Periodo_cursado as 'Semestre Cursado',"
                //    + "Nivel_academico as Nivel "

                    //+ "FROM tbl_solicitudes_becarios sol "
                //+ "WHERE (id_Misolicitud = @idmisolicitud)"), connection);
                    @"SELECT  
(	SELECT  
    Matricula  
    FROM tbl_alumnos alm  
    WHERE alm.Matricula = sol.Matricula 
) as Matricula, 
(	SELECT  
        Nombre   +' '+   Apellido_Paterno  + ' '   +Apellido_Materno  
    FROM tbl_alumnos alm  
    WHERE alm.Matricula = sol.Matricula 
) as Becario, 
case when Otro_alumno = 0 then 'NO' else 'SI' END AS 'Otro Alumno',  
case when cp.Nombre_programa_academico is null then 'N/A' else cp.Nombre_programa_academico end as Programa,

Periodo_cursado as 'Semestre Cursado', 
--sol.Nivel_academico as Nivel  
--na.Nivel_academico as Nivel,
CASE WHEN na.Codigo_nivel_academico=0 THEN 'N/A' ELSE na.Nivel_academico END AS Nivel

FROM tbl_solicitudes_becarios sol  
left JOIN cat_programa_acedemico cp on cp.Codigo_programa_academico= sol.Codigo_programa_academico
inner join cat_nivel_academico na on na.Codigo_nivel_academico=sol.Codigo_nivel_academico
                    WHERE (sol.id_Misolicitud = @idmisolicitud) "), connection);

            command.Parameters.Clear();
            command.Parameters.AddWithValue("@idmisolicitud", TxtSolicitud.Text.Trim());

            da = new SqlDataAdapter(command);
            dt = new DataTable();
            da.Fill(dt);

            GrdResultado.DataSource = dt;
            GrdResultado.DataBind();

            connection.Close(); // Cierre de la conexion

            numerobecarios = (dt.Rows.Count) + 1;
            TxtNumeroBecarios.Text = (dt.Rows.Count).ToString();
            totalbecariosseleccionados = (Convert.ToInt32(CbTotalBecarios.SelectedValue));
            //Enviar.Enabled = false;
            //ErrViewer.Text = totalbecariosseleccionados.ToString();

            if (dt.Rows.Count > 0)
            {
                if (numerobecarios <= totalbecariosseleccionados)
                {
                    NoBecarios.Text = "Becario " + numerobecarios.ToString() + " / " + totalbecariosseleccionados.ToString();
                    Agregar.Enabled = true;
                    otroalumno.Enabled = true;
                    Enviar.Enabled = false;
                    ChkAcuerdo.Enabled = false;
                    Matricula.Enabled = true;
                    CbPrograma.Enabled = true;
                    CbPeriodo.Enabled = true;
                    CbNivel.Enabled = true;
                    Funciones.Enabled = true;

                }
                else
                {
                    NoBecarios.Text = "Todos los becarios fueron capturados ";
                    Agregar.Enabled = false;
                    otroalumno.Enabled = false;
                    Enviar.Enabled = false;
                    ChkAcuerdo.Enabled = false;
                    CbPrograma.Enabled = false;
                    CbPeriodo.Enabled = false;
                    CbNivel.Enabled = false;
                    Funciones.Enabled = false;
                    if ((numerobecarios - 1) == totalbecariosseleccionados)
                    {
                        Enviar.Enabled = true;
                        ChkAcuerdo.Enabled = true;
                        //Matricula.Text = "Todos los becarios fueron capturados";
                        Matricula.Enabled = false;
                    }
                }
            }
            else
            {
                //ErrViewer.Text = " La consulta no regreso registros";

                NoBecarios.Text = "Becario 1" + " / " + totalbecariosseleccionados.ToString();
                Agregar.Enabled = true;
                otroalumno.Enabled = true;
                Enviar.Enabled = false;
                ChkAcuerdo.Enabled = false;
                Matricula.Enabled = true;
                CbPrograma.Enabled = true;
                CbPeriodo.Enabled = true;
                CbNivel.Enabled = true;
                Funciones.Enabled = true;
            }

            // Validaciones
            /*
            if (Agregar.Enabled)
            {
                Funciones.Attributes.Add("data-validation-engine", "validate[required]");
                ChkAcuerdo.Attributes.Remove("data-validation-engine");
            }
            else
            {
                Funciones.Attributes.Remove("data-validation-engine");
                ChkAcuerdo.Attributes.Add("data-validation-engine", "validate[required]");
            }
             * */
        }

        private void loaddatamodificacion()
        {
            SqlCommand command = new SqlCommand(string.Format("SELECT  Becarios_total, id_proyecto, Ubicacion_alterna, Acepto, Nomina, Periodo "
                                                  + "From tbl_solicitudes "
                                                  + "Where (id_MiSolicitud = @idmisolicitud) "), connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@idmisolicitud", idsolicitud.ToString().Trim());
            connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                TotalBecariosBd = dr["Becarios_total"].ToString();
                totalbecariosseleccionados = Convert.ToInt32(dr["Becarios_total"].ToString());
                TxtUbicacionAlterna.Text = dr["Ubicacion_alterna"].ToString();
                CbPeriodos.SelectedValue = dr["Periodo"].ToString();
                //TxtBoxNominaEmpleado.Text = dr["Id_empleado"].ToString();
                /*
                if (dr["Acepto"].ToString() == "SI")
                {
                    ChkAcuerdo.Checked = true;
                }
                else
                {
                    ChkAcuerdo.Checked = false;
                }
                 */ 
            }
            CbTotalBecarios.SelectedIndex = -1;
            CbTotalBecarios.Items.FindByText(TotalBecariosBd.ToString()).Selected = true;
            connection.Close();
        }

        

        protected void LoadListNivel() // Llena Drop Down List de Nivel
        {
            /*Vieja seleccion de codigo_nivel_academico*/
            SqlCommand command = new SqlCommand(string.Format(
                "SELECT  "
                    + "Codigo_nivel_academico,"
                    + "Nivel_academico "
                + "FROM cat_nivel_academico "

            + " "), connection);

           

            connection.Open(); // Abertura de la conexión


            connection.Open(); // Abertura de la conexión

            SqlDataReader dr = command.ExecuteReader();
            CbNivel.Items.Clear();
            CbNivel.Items.Add(new ListItem("--Seleccione--", "0"));
            while (dr.Read())
            {
                CbNivel.Items.Add(new ListItem(dr["Nivel_academico"].ToString(), dr["Codigo_nivel_academico"].ToString()));
            }
            CbNivel.SelectedIndex = 0;
            connection.Close(); // Cierre de la conexión

            // Actualiza los controles que dependen del listbox
            LoadListPrograma(Convert.ToInt32(CbNivel.SelectedValue));
        }


        public void llenarNivel()
        {
            string campus = "";
            /*Verificamos el campus asignador*/
            if(solicitanteCampus.Text == "False")
            {
                campus = campusAlterno.SelectedValue;
            }
            else
            {
                campus = IdCampus.Text;
            }
            
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
            dt = db.getQuery(conexionBecarios,query);
            if (dt.Rows.Count > 0)
            {
                CbNivel.Items.Clear();
                CbNivel.Items.Add(new ListItem("--Seleccione--", "0"));
                foreach(DataRow row in dt.Rows)
                {
                    CbNivel.Items.Add(new ListItem(row["Nivel_academico"].ToString(), row["Codigo_nivel_academico"].ToString()));
                }
                CbNivel.SelectedIndex = 0;
                LoadListPrograma(Convert.ToInt32(CbNivel.SelectedValue));


            }
            else
            {
                verModal("Alerta","No hay Información en el nivel");
            }
                


        }


        protected void LoadListPrograma(int nivelID) // Llena Drop Down List de Programas
        {

            string campus = "";
            /*Verificamos el campus asignador*/
            if (solicitanteCampus.Text == "False")
            {
                campus = campusAlterno.SelectedValue;
            }
            else
            {
                campus = IdCampus.Text;
            }



            SqlCommand command = new SqlCommand(
                //"SELECT "
                //    + "Codigo_programa_academico,"
                //    + "Nombre_programa_academico "
                //+ "FROM cat_programa_acedemico "
                //+ "WHERE Codigo_nivel_academico = " + nivelID.ToString().Trim()


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

            connection.Open(); // Abertura de la conexión

            SqlDataReader dr = command.ExecuteReader();
            if (dr.HasRows)
            {
                CbPrograma.Items.Clear();
                CbPrograma.Items.Add(new ListItem("No aplica", ""));
                while (dr.Read())
                {
                    //CbPrograma.Items.Add(new ListItem(dr["Nombre_programa_academico"].ToString() + " " + dr["Codigo_programa_academico"].ToString(), dr["Codigo_programa_academico"].ToString()));
                    CbPrograma.Items.Add(new ListItem(dr["Nombre_programa_academico"].ToString(), dr["Nombre_programa_academico"].ToString()));                    
                }
                CbPrograma.SelectedIndex = 0;
                CbPeriodo.Items.Add(new ListItem("No aplica", ""));
            }
            else
            {
                CbPrograma.Items.Clear();
                CbPrograma.Items.Add(new ListItem("No aplica", ""));
                CbPeriodo.Items.Add(new ListItem("No aplica", ""));
            }

            connection.Close(); // Cierre de la conexion
        }

        protected void LoadListPeriodo() // Llena los Drop Down List de Periodo
        {
            CbPeriodos.Items.Clear();
            CbPeriodo.Items.Clear();
            CbPeriodos.Items.Add(new ListItem("--Seleccione--", ""));
            SqlCommand command = new SqlCommand(string.Format(
                "SELECT "
                    + "Periodo,"
                    + "Descripcion "
                + "FROM cat_periodos "
                + "WHERE (Activo = 1) ")
            , connection);

            connection.Open(); // Abertura de la conexión

            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                CbPeriodos.Items.Add(new ListItem(dr["Descripcion"].ToString(), dr["Periodo"].ToString())); // Control de periodos al inicio de la solicitud
            }
            CbPeriodos.SelectedIndex = 0;
            connection.Close(); // Cierre de la conexión

            //command = new SqlCommand(string.Format(
            //        "SELECT "
            //            + "id_grado_cursado,"
            //            + "grado "
            //        + "FROM cat_grado_cursado "), connection);

            //connection.Open(); // Abertura de la conexión
            //CbPeriodo.Items.Add(new ListItem("--NA--", ""));
            //dr = command.ExecuteReader();
            //while (dr.Read())
            //{
            //    CbPeriodo.Items.Add(new ListItem(dr["grado"].ToString(), dr["id_grado_cursado"].ToString())); // Control de periodos en captura de becario
            //}
            //CbPeriodo.SelectedIndex = 0;
            //connection.Close(); // Cierre de la conexión
        }

        #endregion

        #region Utilerias

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }
        protected string reemplazarClaves(string cadena)
        {
            string substring = ""; // Inicializo variable auxiliar
            //SqlCommand command;
            if (cadena.IndexOf("!Proyecto!") != -1)
            {
                cadena = cadena.Replace("!Proyecto!", TxtBoxNombre.Text.Trim());
            }
            if (cadena.IndexOf("!Justificacion!") != -1)
            {
                cadena = cadena.Replace("!Justificacion!", TxtBoxJustificacion.Text.Trim());
            }
            if (cadena.IndexOf("!Cantidad!") != -1)
            {
                Int64 total = Convert.ToInt64(TxtNumeroBecarios.Text.Trim());// + Convert.ToInt64(TxtNumeroProgramas.Text.Trim());
                cadena = cadena.Replace("!Cantidad!", total.ToString());
            }
            if (cadena.IndexOf("!Matricula!") != -1)
            {
                substring = "";
                command = new SqlCommand(string.Format(
                     "SELECT "
                        + "ROW_NUMBER() over (Order By id_consecutivo) as #,"
                        + "T2.Matricula,"
                        + "Nombre + ' ' + Apellido_paterno + ' ' + Apellido_materno as Nombre,"
                        + "Otro_alumno as Otro "
                    + "FROM tbl_solicitudes_becarios t1 "
                    + "INNER JOIN tbl_alumnos t2 "
                    + "ON t1.Matricula = t2.Matricula "
                    + "WHERE id_Misolicitud = {0} ", TxtSolicitud.Text.Trim()

                    /*
                    "SELECT "
                        + "ROW_NUMBER() over (Order By id_consecutivo) as #,"
                        + "Matricula,"
                        + "Nombre + ' ' + Apellido_paterno + ' ' + Apellido_materno as Nombre,"
                        + "Otro_alumno as Otro "
                    + "FROM tbl_solicitudes_becarios t1 "
                    + "INNER JOIN tbl_alumnos t2 "
                    + "ON t1.id_alumno = t2.id_alumno "
                    + "WHERE id_Misolicitud = {0} AND (t1.id_alumno <> '4' AND '$' + rtrim(ltrim(t1.id_alumno)) + '$' <> '$$')", TxtSolicitud.Text.Trim()
                        */


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
                //connection.Close();
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
                     
                    
                     @"SELECT  
                          ROW_NUMBER() over (Order By id_consecutivo) as #,                         
						  pc.Nombre_programa_academico as Programa,
                          g.Descripcion as Periodo, 
                          Otro_alumno as Otro,  
						  n.Nivel_academico                         
                      FROM tbl_solicitudes_becarios sb 
					  INNER JOIN cat_nivel_academico n on sb.Codigo_nivel_academico = n.Codigo_nivel_academico
					  INNER JOIN cat_programa_acedemico pc on sb.Codigo_programa_academico = pc.Codigo_programa_academico
					  inner join cat_grado_cursado g on sb.periodo_cursado = g.grado
                      WHERE Matricula is null AND  id_Misolicitud = {0} ", TxtSolicitud.Text.Trim()

                    

                ), connection);
                //connection.Open();
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
                //connection.Close();
                substring += "</table>";
                cadena = cadena.Replace("!Programa!", substring);
            }

            if (cadena.IndexOf("!Periodo!") != -1)
            {
                cadena = cadena.Replace("!Periodo!", CbPeriodos.SelectedItem.Text);
            }
            return cadena;
        }

        #endregion

        #region GrdBecarios

        protected void GrdBecarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // reference the Delete ImageButton
                ImageButton deleteButton = (ImageButton)e.Row.Cells[7].Controls[0];
                deleteButton.OnClientClick = "if (!window.confirm(' ¿Esta seguro de que desea cancelar este becario? ')) return false;";
                CheckBox chkOtroAlumno = (CheckBox)e.Row.Cells[4].FindControl("chkOtroAlumno");
            }

            // Ocultal columna de ID
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
            }
        }

        protected void GrdBecarios_RowCommand(object sender, GridViewCommandEventArgs e)

        {
            int index = Convert.ToInt32(e.CommandArgument); // index
            GridViewRow row = GrdBecarios.Rows[index]; // row
            int consecutivoId = Convert.ToInt32(row.Cells[0].Text); // Consecutivo ID


            if (e.CommandName == "Eliminar")
            {
                connection.Open(); // Abertura de conexion

                EliminarBecario(consecutivoId); // Eliminación del becario de la solicitud

                connection.Close(); // cierre de la conexion

                // Actualización de GridView de becarios
                loaddatabecarios();
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

        [WebMethod]
        public static string tomaLink(string colection)
        {
            
            string querys = "select link from tbl_reglamento";
            string variable = "No hay link";
            DataTable dtr = getQuery(conexionBecarios,querys);
            if (dtr.Rows.Count > 0)
            {
                variable = dtr.Rows[0]["link"].ToString();
            }
            return variable;
        }


        public static DataTable getQuery(string conexion, string query)
        {
            //Se crea el datatable
            DataTable dt = new DataTable();
            //Creamos la conexion
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            //Llenanos nuestro  data table
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();
            //Retorno mi data table
            return dt;
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
                verModal("Alerta", es.Message.ToString());
            }
        }
    }
}