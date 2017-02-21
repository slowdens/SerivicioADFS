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
    public partial class Proyectos : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        Int32 valorotroalumno = 0;
        string idalumno;
        Int32 numerobecarios = 0;
        Int32 idsolicitud = 0;
        Int32 totalbecariosseleccionados = 0;
        DataTable dt;
        string query;
        BasedeDatos dbs = new BasedeDatos();
        Correo mail = new Correo();
        static string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
            command.Connection = connection;
            if (Session["Solicitud"] != null)//(Request.QueryString.ToString() != "")
            {
                this.idsolicitud = Convert.ToInt32(Session["Solicitud"].ToString()); //this.idsolicitud = Convert.ToInt32(Request.QueryString.ToString()); // Obtiene el id de solicitud
                if (!IsPostBack)
                {
                    TxtSolicitud.Text = idsolicitud.ToString();
                    //TblCaptura.Visible = true;
                    BtnDatos.Visible = true;
                    //TblAlta.Visible = true;
                    TxtEstatusSolicitud.Text = "MODIFICACION";
                    loaddata();
                    loaddatamodificacion();
                    loaddatabecarios();
                    Session.Remove("Solicitud");
                }
            }
            else
            {
                if (!IsPostBack)
                {
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

        private void loaddata()
        {
            // Datos del solicitante
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
            command.Parameters.AddWithValue("@nomina", Session["Usuario"].ToString());

            connection.Open(); // Abertura de la conexion

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
                DatosCorreo1.Text = dr["Correo_electronico"].ToString();
                DatosDivision.Text = dr["Division"].ToString();
                DatosCampus.Text = dr["Campus"].ToString();
                solicitanteCampus.Text = dr["cuenta_con_alumnos"].ToString();
                IdCampus.Text = dr["Codigo_campus"].ToString();
            }
            //Verificar que no tenga otra
            command = new SqlCommand(string.Format(
                "SELECT ts.id_MiSolicitud, ts.Periodo, cp.Activo, ts.Nomina "
                    + "FROM tbl_solicitudes as ts, cat_periodos as cp, Tbl_empleados as te "
                    + "Where (ts.Nomina = @nomina) and (Activo = 1 and id_tipo_solicitud = 2 and ts.Nomina = te.Nomina) "
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
            connection.Close(); // Cierre de la conexion

            if (duplicado == "NO")
            {
                // Periodos
                command = new SqlCommand(string.Format("SELECT Periodo, Descripcion FROM cat_periodos "
                                                      + "Where (Activo = 1) "), connection);
                command.Parameters.Clear();

                connection.Open();

                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    CbPeriodos.Items.Add(new ListItem(dr["Descripcion"].ToString(), dr["Periodo"].ToString()));
                }

                connection.Close();
/***********************************************************************************************************************************************************************************************/
                //command = new SqlCommand(string.Format(
                //        "SELECT "
                //            + "id_grado_cursado,"
                //            + "grado "
                //        + "FROM cat_grado_cursado "), connection);

                //connection.Open(); // Abertura de la conexión

                //dr = command.ExecuteReader();
                //CbPeriodo.Items.Clear();
                //CbPeriodo.Items.Add(new ListItem("--NA--", ""));
                //while (dr.Read())
                //{
                //    CbPeriodo.Items.Add(new ListItem(dr["grado"].ToString(), dr["id_grado_cursado"].ToString())); // Control de periodos en captura de becario
                //}
                //CbPeriodo.SelectedIndex = 0;
                //connection.Close(); // Cierre de la conexión
/***********************************************************************************************************************************************************************************************/                
                LoadListPeriodo();
            }
            else
            {
                BtnDatos.Enabled = false;
                CbPeriodos.Enabled = false;
                TxtBoxNombre.Enabled = false;
                TxtBoxJustificacion.Enabled = false;
                TxtBoxObjetivo.Enabled = false;
                TxtBoxFunciones.Enabled = false;
                TxtBoxTotalBecarios.Enabled = false;
                verModal("Alerta", " Ya has dado de alta una solicitud ");
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
                bool hasError = false;

                if (Matricula.Text != "")
                {
                    if (GrdBecarios.Rows.Count == Convert.ToInt64(TxtBoxTotalBecarios.Text))
                    {
                        hasError = true;
                        //ErrViewer.Text = "Ya se han capturado todos los becarios";
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "message", "showMessage(\"Error!\",\"Ya se han capturado todos los becarios\");", true);
                        //TblBecariosProgramas.Visible = true;
                        //TblCaptura.Visible = false;
                    }
                }

                if (!hasError)
                {
                    string idalumno = "0";
                    SqlCommand command;

                    connection.Open(); // Abertura de la conexión

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
                                + "WHERE id_Misolicitud = '" + TxtSolicitud.Text.Trim() + "' and Matricula = '" + idalumno.ToString().Trim() + "'"
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

                        command = new SqlCommand(string.Format("INSERT INTO tbl_solicitudes_becarios ("
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
                            valorotroalumno = 0;
                            /*
                            command.Parameters.AddWithValue("@idmisolicitud", TxtSolicitud.Text.Trim());
                            command.Parameters.AddWithValue("@idalumno", idalumno);
                            command.Parameters.AddWithValue("@idprograma", '\0');
                            command.Parameters.AddWithValue("@idperiodocursado", '\0');
                            command.Parameters.AddWithValue("@otroalumno", valorotroalumno);
                            command.Parameters.AddWithValue("@becariocalificacion", "PENDIENTE");
                            //command.Parameters.AddWithValue("@becariopuntaje", 0);
                            command.Parameters.AddWithValue("@becariofunciones", Funciones.Text);
                            command.Parameters.AddWithValue("@idestatusasignacion", 1);
                            command.Parameters.AddWithValue("@asistencia", 0);
                            command.Parameters.AddWithValue("@correoavisoasignacion", 0);
                            command.Parameters.AddWithValue("@nivelacademico", 0);
                            //command.Parameters.AddWithValue("@asistenciafecha", DateTime.Today);
                             */
                        }
                        else
                        {
                            valorotroalumno = 1;
                        }
                        command.Parameters.AddWithValue("@idmisolicitud", TxtSolicitud.Text.Trim());

                        if (idalumno == "0")
                        {
                            command.Parameters.AddWithValue("@idalumno", DBNull.Value);
                            if (CbPeriodo.SelectedItem.Text == "--NA--")
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
                            }
                            else
                            {
                                //Aqui llenamos la informacion                                
                                //command.Parameters.AddWithValue("@idprograma", CbPrograma.SelectedItem.Value);
                                command.Parameters.AddWithValue("@idprograma", getCodogoProgama(CbPrograma.SelectedItem.Text));
                            }
                            if (CbNivel.SelectedItem.Text == "--Seleccione--")
                            {
                                command.Parameters.AddWithValue("@nivelacademico", 0);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@nivelacademico", CbNivel.SelectedItem.Value);
                            }
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@idalumno", idalumno);
                            command.Parameters.AddWithValue("@idperiodocursado", "N/A");
                            command.Parameters.AddWithValue("@idprograma", "N/A");
                            command.Parameters.AddWithValue("@nivelacademico", 0);
                        }

                        command.Parameters.AddWithValue("@otroalumno", valorotroalumno);
                        command.Parameters.AddWithValue("@becariocalificacion", "PENDIENTE");
                        //command.Parameters.AddWithValue("@becariopuntaje", 0);
                        command.Parameters.AddWithValue("@becariofunciones", Funciones.Text);
                        command.Parameters.AddWithValue("@idestatusasignacion", 1);
                        command.Parameters.AddWithValue("@asistencia", 0);
                        command.Parameters.AddWithValue("@correoavisoasignacion", 0);
                        //command.Parameters.AddWithValue("@asistenciafecha", DateTime.Today);
                        //}
                        try
                        {
                            command.ExecuteNonQuery();

                            //ErrViewer.Text = "Registro creado correctamente";

                            Matricula.Text = "";
                            Funciones.Text = "";
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            ErrViewer.Text = "Insert Error: " + ex.Message;
                        }

                        connection.Close();

                        loaddatabecarios(); // Recarga GridView
                    }
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

        public void llenarSemestreCursado(string programaAcademcio)
        {
            query = @"
                    select   distinct a.Grado_academico
                    from tbl_alumnos a  inner join cat_programa_acedemico pa on pa.Codigo_programa_academico=a.Codigo_programa_academico 
                    where 
                    a.Codigo_campus='"+IdCampus.Text+@"' and  a.Periodo='"+CbPeriodos.SelectedValue+@"'
                    and a.Clave_apoyo in (select s.Clave_apoyo from tbl_apoyo_financiero s 
		                    where Periodo='"+CbPeriodos.SelectedValue+@"' 
		                    and Codigo_campus='" + IdCampus.Text + @"')
                    and pa.Codigo_nivel_academico= "+CbNivel.SelectedValue+@"
                    and pa.Nombre_programa_academico = '"+programaAcademcio+@"'";

            dt = dbs.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                CbPeriodo.Items.Clear();
                CbPeriodo.Items.Add(new ListItem("No aplica", ""));
                foreach(DataRow ro in dt.Rows)
                {
                    CbPeriodo.Items.Add(new ListItem(ro["Grado_academico"].ToString(), ro["Grado_academico"].ToString())); // Control de periodos en captura de becario
                }
                   CbPeriodo.SelectedIndex = 0;
            }
            else
            {
                verModal("Alerta","No hay información en los semestre cursado");
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
                dt = dbs.getQuery(conexionBecarios, query);
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

        private void loaddatabecarios()
        {
            SqlCommand command = new SqlCommand(
                string.Format(
                //"SELECT "
                //    + "id_consecutivo Consecutivo,"
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
                //    + "case when Otro_alumno = 0 then 'NO' else 'SI' END AS Otro, "
                //    + "Programa,"
                //    + "Periodo_cursado as Periodo,"
                //    + "Codigo_nivel_academico as Nivel "
                //+ "FROM tbl_solicitudes_becarios sol "
                //+ "WHERE (id_Misolicitud = @idmisolicitud)"), connection);
                    @"
SELECT
sol.id_consecutivo Consecutivo,  
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
case when Otro_alumno = 0 then 'NO' else 'SI' END AS Otro,  
case when cp.Nombre_programa_academico is null then 'N/A' else cp.Nombre_programa_academico end as Programa,

Periodo_cursado as 'Periodo', 

CASE WHEN na.Codigo_nivel_academico=0 THEN 'N/A' ELSE na.Nivel_academico END AS Nivel

FROM tbl_solicitudes_becarios sol  
left JOIN cat_programa_acedemico cp on cp.Codigo_programa_academico= sol.Codigo_programa_academico
inner join cat_nivel_academico na on na.Codigo_nivel_academico=sol.Codigo_nivel_academico
WHERE (sol.id_Misolicitud =  @idmisolicitud)"), connection);
     

            command.Parameters.Clear();
            command.Parameters.AddWithValue("@idmisolicitud", TxtSolicitud.Text.Trim());

            connection.Open();

            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
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
                //    + "Codigo_nivel_academico as Nivel "
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

                    CASE WHEN na.Codigo_nivel_academico=0 THEN 'N/A' ELSE na.Nivel_academico END AS Nivel

                    FROM tbl_solicitudes_becarios sol  
                    left JOIN cat_programa_acedemico cp on cp.Codigo_programa_academico= sol.Codigo_programa_academico
                    inner join cat_nivel_academico na on na.Codigo_nivel_academico=sol.Codigo_nivel_academico
                    WHERE (sol.id_Misolicitud = @idmisolicitud)"), connection);

            command.Parameters.Clear();
            command.Parameters.AddWithValue("@idmisolicitud", TxtSolicitud.Text.Trim());

            da = new SqlDataAdapter(command);
            dt = new DataTable();
            da.Fill(dt);

            GrdResultado.DataSource = dt;
            GrdResultado.DataBind();

            connection.Close();

            numerobecarios = (dt.Rows.Count) + 1;
            TxtblkNumeroBecarios.Text = (dt.Rows.Count).ToString();
            totalbecariosseleccionados = (Convert.ToInt32(TxtBoxTotalBecarios.Text));

            if (dt.Rows.Count > 0)
            {
                if (numerobecarios <= totalbecariosseleccionados)
                {
                    //ErrViewer.Text = "Ok";
                    NoBecarios.Text = "Becario " + numerobecarios.ToString() + " / " + TxtBoxTotalBecarios.Text;
                    Agregar.Enabled = true;
                    otroalumno.Enabled = true;
                    Enviar.Enabled = false;
                    ChkAcuerdo.Enabled = false;
                    Matricula.Enabled = true;
                    Funciones.Enabled = true;
                    CbPrograma.Enabled = true;
                    CbPeriodo.Enabled = true;
                    CbNivel.Enabled = true;
                }
                else
                {
                    NoBecarios.Text = "Todos los becarios fueron capturados ";
                    Agregar.Enabled = false;
                    otroalumno.Enabled = false;
                    Enviar.Enabled = false;
                    ChkAcuerdo.Enabled = false;
                    Matricula.Enabled = false;
                    Funciones.Enabled = false;
                    CbPrograma.Enabled = false;
                    CbPeriodo.Enabled = false;
                    CbNivel.Enabled = false;
                    if ((numerobecarios - 1) == totalbecariosseleccionados)
                    {
                        Enviar.Enabled = true;
                        ChkAcuerdo.Enabled = true;
                    }
                }
            }
            else
            {
                NoBecarios.Text = "Becario 1 / " + TxtBoxTotalBecarios.Text;
                ErrViewer.Text = "La consulta no regreso registros";
                Agregar.Enabled = true;
                otroalumno.Enabled = true;
                Enviar.Enabled = false;
                ChkAcuerdo.Enabled = false;
                Matricula.Enabled = true;
                Funciones.Enabled = true;
                CbPrograma.Enabled = true;
                CbPeriodo.Enabled = true;
                CbNivel.Enabled = true;
            }
        }
        private void loaddatamodificacion()
        {

            SqlCommand command = new SqlCommand(string.Format("SELECT  Becarios_total, id_proyecto, Ubicacion_alterna, Acepto, Nomina, Periodo "
                                                  + "From tbl_solicitudes "
                                                  + "Where id_Misolicitud = @idmisolicitud "), connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@idmisolicitud", idsolicitud.ToString().Trim());
            connection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                TxtBoxTotalBecarios.Text = dr["Becarios_total"].ToString();
                totalbecariosseleccionados = Convert.ToInt32(dr["Becarios_total"].ToString());
                TxtBoxProyecto.Text = dr["id_proyecto"].ToString();
                TxtUbicacionAlterna.Text = dr["Ubicacion_alterna"].ToString();
                TxtIdEmpleado.Text = dr["Nomina"].ToString();
                CbPeriodos.SelectedValue = dr["Periodo"].ToString();
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
            //CbTotalBecarios.SelectedIndex = -1;
            //CbTotalBecarios.Items.FindByText(TotalBecariosBd.ToString()).Selected = true;
            command = new SqlCommand(string.Format("SELECT Nombre, Justificacion, Objetivo, Proyecto_funciones  "
                                                  + "From tbl_proyectos "
                                                  + "Where id_proyecto = @idproyecto "), connection);
            command.Parameters.Clear();

            command.Parameters.AddWithValue("@idproyecto", TxtBoxProyecto.Text.Trim());
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                TxtBoxNombre.Text = dr["Nombre"].ToString();
                TxtBoxJustificacion.Text = dr["Justificacion"].ToString();
                TxtBoxObjetivo.Text = dr["Objetivo"].ToString();
                TxtBoxFunciones.Text = dr["Proyecto_funciones"].ToString();
                nombreProyecto2.Text = "<B>Nombre del Proyecto: </B>" + TxtBoxNombre.Text.Trim();
                NoBecarios.Text = "Becario 1 / " + TxtBoxTotalBecarios.Text;
                //totalbecariosseleccionados = Convert.ToInt32(dr["Becarios_total"].ToString());
                //TxtBoxProyecto.Text = dr["id_proyecto"].ToString();
            }
            connection.Close();
        }

        protected void BtnDatos_Click(object sender, EventArgs e)
        {
            string query = @"select fs.Fecha_inicio,fs.Fecha_fin
                                from tbl_campus_periodo cp inner join tbl_fechas_solicitudes fs on cp.id_campus_periodo=fs.id_campus_periodo
                                inner join tbl_empleados e on e.Codigo_campus=cp.Codigo_campus

                                where Nomina='" + Session["usuario"].ToString() + "' and cp.Periodo='" + CbPeriodos.SelectedValue + "'";
            dt = dbs.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (DateTime.Today >= Convert.ToDateTime(dt.Rows[0]["Fecha_inicio"]) && DateTime.Today <= Convert.ToDateTime(dt.Rows[0]["Fecha_fin"]))
                {
                    llenarNivel();
                    totalbecariosseleccionados = (Convert.ToInt32(TxtBoxTotalBecarios.Text));
                    if (totalbecariosseleccionados == 0)
                    {
                        verModal("Alerta", " No hay becarios asignados ");
                    }
                    else
                    {
                        connection.Open(); // Abertura de la conexión

                        if (TxtEstatusSolicitud.Text == "MODIFICACION")
                        {
                            // Actualiza el toal de becarios agregados hasta el momento
                            command = new SqlCommand(string.Format("UPDATE tbl_solicitudes SET "
                                    + "Becarios_total = @becariostotal "
                                    + "WHERE id_miSolicitud = @idmisolicitud "), connection);

                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@idmisolicitud", TxtSolicitud.Text);
                            command.Parameters.AddWithValue("@becariostotal", TxtBoxTotalBecarios.Text);

                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (System.Data.SqlClient.SqlException ex)
                            {
                                ErrViewer.Text = "Insert Error: " + ex.Message;
                            }

                            // Actualiza los datos del proyecto
                            command = new SqlCommand(string.Format("UPDATE tbl_proyectos SET "
                                    + "Nombre = @nombre, "
                                    + "Justificacion = @justificacion, "
                                    + "Objetivo = @objetivo, "
                                    + "Proyecto_funciones = @proyectofunciones "
                                    + "WHERE id_proyecto = @idproyecto "), connection);

                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@idproyecto", TxtBoxProyecto.Text.Trim());
                            command.Parameters.AddWithValue("@nombre", TxtBoxNombre.Text.Trim());
                            command.Parameters.AddWithValue("@justificacion", TxtBoxJustificacion.Text.Trim());
                            command.Parameters.AddWithValue("@objetivo", TxtBoxObjetivo.Text.Trim());
                            command.Parameters.AddWithValue("@proyectofunciones", TxtBoxFunciones.Text.Trim());
                            try
                            {
                                command.ExecuteNonQuery();
                                connection.Close();
                                TblCaptura.Visible = true;
                                TblAlta.Visible = false;
                                loaddatabecarios();
                            }
                            catch (System.Data.SqlClient.SqlException ex)
                            {
                                ErrViewer.Text = "Insert Error: " + ex.Message;
                            }
                        }
                        else
                        {
                            ///alta proyecto
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
                            command.Parameters.AddWithValue("@aprobado", 0);
                            command.Parameters.AddWithValue("@objetivo", TxtBoxObjetivo.Text.Trim());
                            command.Parameters.AddWithValue("@idtipoproyecto", 1);
                            command.Parameters.AddWithValue("@proyectofunciones", TxtBoxFunciones.Text.Trim());

                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (System.Data.SqlClient.SqlException ex)
                            {
                                ErrViewer.Text = "Insert Error: " + ex.Message;
                            }

                            command = new SqlCommand(string.Format("SELECT @@Identity"), connection);
                            command.ExecuteNonQuery();

                            idsolicitud = Convert.ToInt32(command.ExecuteScalar());

                            //ErrViewer.Text = "Ok  ";
                            TxtBoxProyecto.Text = idsolicitud.ToString();

                            ///Alta de la solicitud
                            command = new SqlCommand(string.Format("INSERT INTO tbl_solicitudes ("
                                                                        + "Periodo, "
                                                                        + "Nomina, "
                                                                        + "id_proyecto, "
                                                                        + "id_tipo_solicitud, "
                                                                        + "id_solicitud_estatus, "
                                                                        + "Fecha_solicitud, "
                                                                        + "Empleado_calificacion, "
                                //+ "Empleado_puntuaje, "
                                                                        + "Becarios_total ) "
                                                                        + "VALUES ("
                                                                        + "@idperiodo, "
                                                                        + "(SELECT Nomina FROM tbl_empleados WHERE Nomina = @nomina),"
                                                                        + "@idproyecto, "
                                                                        + "@idtiposolicitud, "
                                                                        + "@idsolicitudestatus,"
                                                                        + "@fechasolicitud, "
                                                                        + "@empeladocalificacion, "
                                //+ "@empladopuntaje, "
                                                                        + "@becariostotal) "), connection);

                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@idperiodo", CbPeriodos.SelectedValue);
                            command.Parameters.AddWithValue("@nomina", Session["Usuario"].ToString());
                            command.Parameters.AddWithValue("@idproyecto", Convert.ToInt32(TxtBoxProyecto.Text.Trim()));
                            command.Parameters.AddWithValue("@idtiposolicitud", 2);
                            command.Parameters.AddWithValue("@idsolicitudestatus", 1);
                            command.Parameters.AddWithValue("@fechasolicitud", DateTime.Today);
                            command.Parameters.AddWithValue("@empeladocalificacion", "PENDIENTE");
                            //command.Parameters.AddWithValue("@empladopuntaje", 0);
                            command.Parameters.AddWithValue("@becariostotal", TxtBoxTotalBecarios.Text);

                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (System.Data.SqlClient.SqlException ex)
                            {
                                ErrViewer.Text = "Insert Error: " + ex.Message;
                            }
                            command = new SqlCommand(string.Format("SELECT @@Identity"), connection);
                            command.ExecuteNonQuery();

                            idsolicitud = Convert.ToInt32(command.ExecuteScalar());

                            //ErrViewer.Text += "Ok  ";
                            TxtSolicitud.Text = idsolicitud.ToString();

                            TblCaptura.Visible = true;
                            TblAlta.Visible = false;
                            nombreProyecto2.Text = "<B>Nombre del Proyecto: </B>" + TxtBoxNombre.Text.Trim();
                            NoBecarios.Text = "Becario 1 / " + TxtBoxTotalBecarios.Text;
                            Session["Periodo"] = CbPeriodos.SelectedValue;
                        }
                    }
                }
                else
                {
                    if (DateTime.Today > Convert.ToDateTime(dt.Rows[0]["Fecha_fin"]))
                    {
                        verModal("Alerta", "Ya pasaron las fechas de solicitudes para el periodo "+CbPeriodos.SelectedItem.Text+"");
                    }
                    if (DateTime.Today < Convert.ToDateTime(dt.Rows[0]["Fecha_inicio"]))
                    {
                        verModal("Alerta", "Aún no son las fechas de solicitudes en el periodo  "+CbPeriodos.SelectedItem.Text+"");
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
                connection.Open();


                if (GrdResultado.Rows.Count >= Convert.ToInt32(TxtBoxTotalBecarios.Text))
                {
                    SqlCommand command = new SqlCommand(string.Format("UPDATE tbl_solicitudes SET "
                                                        + "Ubicacion_alterna = @ubicacionalterna, "
                                                        + "Acepto = @acepto "
                                                        + "Where (id_Misolicitud = @idmisolicitud) "), connection);

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@idmisolicitud", TxtSolicitud.Text.Trim());
                    command.Parameters.AddWithValue("@ubicacionalterna", TxtUbicacionAlterna.Text);
                    command.Parameters.AddWithValue("@acepto", "SI");
                    /*
                    if (ChkAcuerdo.Checked == false)
                    {
                        command.Parameters.AddWithValue("@acepto", 0);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@acepto", 1);
                    }
                    */
                    try
                    {
                        command.ExecuteNonQuery();

                        TblCaptura.Visible = false;
                        TblResultado.Visible = true;
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        ErrViewer.Text = "Insert Error: " + ex.Message;
                    }

                    connection.Close();// Cierre de la conexion

                    if (TxtEstatusSolicitud.Text == "MODIFICACION")
                    {
                        NoSolicitud.Text += " " + TxtSolicitud.Text + " (Modificación)";
                    }
                    else
                    {
                        NoSolicitud.Text += " " + TxtSolicitud.Text;
                    }

                    if (Convert.ToInt32(TxtBoxTotalBecarios.Text) > 1)
                    {
                        TotalRegistrados.Text = "con " + TxtBoxTotalBecarios.Text + " becarios";
                    }
                    else
                    {
                        TotalRegistrados.Text = "con " + TxtBoxTotalBecarios.Text + " becario";
                    }

                    ErrViewer.Text = "";
                    if (TxtEstatusSolicitud.Text == "MODIFICACION") // Mensaje si es modificación
                    {
                        command = new SqlCommand(string.Format(
                        "SELECT "
                            + "Asunto,"
                            + "Cuerpo "
                        + "FROM tbl_cuerpo_correo t1 "
                        + "INNER JOIN cat_correos t2 "
                        + "ON t1.id_correo = t2.id_correo "
                        + "WHERE rtrim(ltrim(Tipo_correo)) = 'Modificación a la solicitud(Por Proyecto)'"
                        + "AND Codigo_campus = (Select Codigo_campus FROM tbl_empleados WHERE Nomina = '{0}')",
                            Session["Usuario"].ToString()), connection); ;
                    }
                    else // Mensaje si es una nueva solicitud 
                    {
                        /*<-- Envío de correo*/
                        //Mail mail; // Configuración de envío de correo
                        command = new SqlCommand(string.Format(
                                "SELECT Asunto, Cuerpo "
                                + "FROM tbl_cuerpo_correo t1 "
                                + "INNER JOIN cat_correos t2 "
                                + "ON t1.id_correo = t2.id_correo "
                                + "WHERE rtrim(ltrim(Tipo_correo)) = 'Confirmación de registro(Por proyecto)'"
                                    + "AND Codigo_campus = (Select Codigo_campus FROM tbl_empleados WHERE Nomina = '{0}')",
                                    Session["Usuario"].ToString()), connection);


                    }
                    connection.Open(); // Abertura de la conexión
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        /*
                        mail = new Mail();
                        MailAddress recipient = new MailAddress(DatosCorreo1.Text.Trim());
                        mail.Recipients.Add(recipient);
                        mail.Subject = dr["Asunto"].ToString();
                        // Eliminación de palabras reservadas del body
                        mail.Body = reemplazarClaves(dr["Cuerpo"].ToString());
                        mail.send();
                        ErrViewer.Text = mail.ErrDescription;
                         */
                        //MailMessage message = new MailMessage("servicio.becario@itesm.mx", DatosCorreo1.Text.Trim());
                        //message.Subject = dr["Asunto"].ToString();
                        //message.Body = reemplazarClaves(dr["Cuerpo"].ToString()) + db.noEnvio();
                        //message.IsBodyHtml = true;

                        
                        //mail.MandarCorreo(message);

                        mandarCorreo(reemplazarClaves(dr["Cuerpo"].ToString()), dr["Asunto"].ToString(), DatosCorreo1.Text.Trim());

                    }
                    connection.Close();
                    /* Envío de correo --> */
                }
                else
                {
                    ErrViewer.Text = "No se han capturado todos los becarios";
                }
            }
            else
            {
                verModal("Alerta", " No has aceptado las condiciones del reglamento ");
            }
        }


        public void mandarCorreo(string cuerpo, string asunto, string correo)
        {
            string ambiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
            string ipPruebas = System.Configuration.ConfigurationManager.AppSettings["IpPruebas"];
            string ipDesarrollo = System.Configuration.ConfigurationManager.AppSettings["IpDesarrollo"];

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
                //                MessageBox.Show("Mensaje Enviado Correctamente", "Correo C#", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //              sw = false;

            }
            catch (System.Net.Mail.SmtpException e)
            {

                //Response.Write(e);
                verModal("Error", e.ToString());
            }

        }

        protected void EliminarBecario(int becarioId)
        {
            SqlCommand cmmd = new SqlCommand(
                "DELETE FROM tbl_solicitudes_becarios WHERE id_consecutivo = " + becarioId.ToString().Trim()
                , connection);

            cmmd.ExecuteNonQuery();
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
                Int64 total = Convert.ToInt64(TxtBoxTotalBecarios.Text.Trim());// + Convert.ToInt64(TxtNumeroProgramas.Text.Trim());
                cadena = cadena.Replace("!Cantidad!", total.ToString());
            }
            if (cadena.IndexOf("!Matricula!") != -1)
            {
                substring = "";
                command = new SqlCommand(string.Format(
                    /* original
                    "SELECT "
                        + "ROW_NUMBER() over (Order By id_consecutivo) as #,"
                        + "Matricula,"
                        + "Nombre + ' ' + Apellido_paterno + ' ' + Apellido_materno as Nombre,"
                        + "Otro_alumno as Otro "
                    + "FROM tbl_solicitudes_becarios t1 "
                    + "INNER JOIN tbl_alumnos t2 "
                    + "ON t1.id_alumno = t2.id_alumno "
                    + "WHERE id_Misolicitud = {0} AND (t1.id_alumno <> '4' AND '$' + rtrim(ltrim(t1.id_alumno)) + '$' <> '$$')", TxtSolicitud.Text.Trim()

                    **/

                     /*"SELECT "
                        + "ROW_NUMBER() over (Order By id_consecutivo) as #,"
                        + "Matricula,"
                        + "Nombre + ' ' + Apellido_paterno + ' ' + Apellido_materno as Nombre,"
                        + "Otro_alumno as Otro "
                    + "FROM tbl_solicitudes_becarios t1 "
                    + "INNER JOIN tbl_alumnos t2 "
                    + "ON t1.Matricula = t2.Matricula"
                    + "WHERE id_Misolicitud = {0} ", TxtSolicitud.Text.Trim()



                     */

                     "SELECT "
                        + "ROW_NUMBER() over (Order By id_consecutivo) as #,"
                        + "t2.Matricula,"
                        + "Nombre + ' ' + Apellido_paterno + ' ' + Apellido_materno as Nombre,"
                        + "Otro_alumno as Otro "
                    + "FROM tbl_solicitudes_becarios t1 "
                    + "INNER JOIN tbl_alumnos t2 "
                    + "ON t1.Matricula = t2.Matricula"
                    + " WHERE id_Misolicitud = "+TxtSolicitud.Text.Trim()+""



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
                cadena = cadena.Replace("!Nombre!", NombreSolicitante.Text.Trim());
            }
            if (cadena.IndexOf("!Programa!") != -1)
            {
                substring = "";
                command = new SqlCommand(string.Format(
                    /*   //original
                       "SELECT "
                           + "ROW_NUMBER() over (Order By id_consecutivo) as #,"
                           + "Programa,"
                           + "Periodo_cursado as Periodo,"
                           + "Otro_alumno as Otro, "
                           + "Nivel_academico "
                       + "FROM tbl_solicitudes_becarios "
                       + "WHERE id_alumno is null AND id_Misolicitud = {0} AND '$' + rtrim(ltrim(Programa)) + '$' <> '$$'", TxtSolicitud.Text.Trim()
                    */
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
                     WHERE Matricula is null AND id_Misolicitud = {0} ", TxtSolicitud.Text.Trim()


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

        #region GrdBecarios

        protected void GrdBecarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument); // index
            GridViewRow row = GrdBecarios.Rows[index]; // row
            int becarioId = Convert.ToInt32(row.Cells[0].Text); // Consecutivo en la solicitud

            if (e.CommandName == "Eliminar")
            {
                connection.Open(); // Abertura de conexion

                EliminarBecario(becarioId); // Quita al becario de la solicitud

                connection.Close(); // cierre de la conexion

                // Actualización de GridView de solicitudes
                loaddatabecarios();
            }

        }

        /*protected void GrdBecarios_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //Just changed the index of cells based on your requirements
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Visible = false;
            }
        }*/

        protected void GrdBecarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // reference the Delete ImageButton
                ImageButton deleteButton = (ImageButton)e.Row.Cells[7].Controls[0];
                deleteButton.OnClientClick = "if (!window.confirm(' ¿Esta seguro que desea cancelar este becario? ')) return false;";
            }

            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                // hide de column ID
                e.Row.Cells[0].Visible = false;
            }
        }

        protected void GrdResultado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                // hide de column ID
                e.Row.Cells[0].Visible = false;
            }
        }

        #endregion
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void CbNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nivelID = Convert.ToInt32(CbNivel.SelectedValue);
            LoadListPrograma(nivelID);
        }

        protected void LoadListNivel() // Llena Drop Down List de Nivel
        {
            SqlCommand command = new SqlCommand(string.Format(
                "SELECT  "
                    + "Codigo_nivel_academico,"
                    + "Nivel_academico "
                + "FROM cat_nivel_academico "
            + " "), connection);

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
            dt = dbs.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                CbNivel.Items.Clear();
                CbNivel.Items.Add(new ListItem("--Seleccione--", "0"));
                foreach (DataRow row in dt.Rows)
                {
                    CbNivel.Items.Add(new ListItem(row["Nivel_academico"].ToString(), row["Codigo_nivel_academico"].ToString()));
                }
                CbNivel.SelectedIndex = 0;
                LoadListPrograma(Convert.ToInt32(CbNivel.SelectedValue));


            }
            else
            {
                verModal("Alerta", "No hay Información en el nivel");
            }



        }




        protected void LoadListPrograma(int nivelID) // Llena Drop Down List de Programas
        {
            string campus = IdCampus.Text;

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
                    CbPrograma.Items.Add(new ListItem(dr["Nombre_programa_academico"].ToString(), dr["Nombre_programa_academico"].ToString()));
                }
                CbPrograma.SelectedIndex = 0;
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

            SqlCommand command = new SqlCommand(string.Format(
                "SELECT "
                    + "Periodo,"
                    + "Descripcion "
                + "FROM cat_periodos "
                + "WHERE (Activo = 1) ")
            , connection);

            connection.Open(); // Abertura de la conexión
            CbPeriodos.Items.Add(new ListItem("--Seleccione--", ""));
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                CbPeriodos.Items.Add(new ListItem(dr["Descripcion"].ToString(), dr["Periodo"].ToString())); // Control de periodos al inicio de la solicitud
            }
            connection.Close(); // Cierre de la conexión
            CbPeriodos.SelectedIndex = 0;
        }


        [WebMethod]
        public static string tomaLink(string colection)
        {

            string querys = "select link from tbl_reglamento";
            string variable = "No hay link";
            DataTable dtr = getQuery(conexionBecarios, querys);
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