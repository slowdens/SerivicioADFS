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
    public partial class EvaluacionSolicitante : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection();
        protected string[] cursor = {
            "SELECT *, (SELECT Codigo_campus FROM tbl_empleados t2 WHERE t1.Nomina = t2.Nomina) as solicitante_id_campus FROM tbl_solicitudes t1", // Solicitudes
            "SELECT id_preguntas_campus,Codigo_campus, Pregunta, id_examen_dirigido, id_componente, (SELECT Tipo_componente FROM cat_componentes t3 WHERE t3.id_componente = t2.id_componente) Tipo_componente FROM tbl_preguntas_campus t1 Inner Join tbl_preguntas t2 On t1.id_pregunta = t2.id_pregunta", // Preguntas
            "SELECT * FROM tbl_respuestas", // opciones
            "SELECT id_promedio_campus,Codigo_campus,t1.id_promedio,Menor_o_igual_que,Mayor_o_igual_que,t2.id_estatus_promedio,Estatus FROM tbl_promedio_campus t1 Join tbl_promedios t2 On t1.id_promedio = t2.id_promedio Join cat_estatus_promedios t3 On t2.id_estatus_promedio = t3.id_estatus_promedio", //Promedios
            "SELECT * FROM tbl_solicitudes_becarios" // Becarios en solicitud
        };
        int solicitudID;
        int alumnoID;
        int campusID;
        string idCampusAlterno;
        string idCampus;
        string idPeriodo;
        Evaluacion evaluacion = new Evaluacion();

        protected void Page_Load(object sender, EventArgs e)
        {
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
            connection.Open(); // Abertura de la conexion 
            try
            {
                if (EvaluacionAbierta())
                {
                    if (!ExisteEvaluacion(this.solicitudID, this.alumnoID))
                    {
                        loadSolicitud(); // Carga en memoria los datos de la solicitud
                        loadFormulario(); // Crea el formulario
                    }
                    else
                    {
                        verModal(" Información ", " Ya has evaluado al solicitante ");
                        LblComentarios.Visible = false;
                        TxtComentarios.Visible = false;
                        PanelFormulario.Visible = false;
                    }
                }
                else
                {
                    verModal(" Información ", " No se ha abierto el periodo de evaluaciones ");
                    LblComentarios.Visible = false;
                    TxtComentarios.Visible = false;
                    PanelFormulario.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Msg.Text = ex.ToString();
            }
            connection.Close(); // Cierre de la conexion     
        }
        protected void loadSolicitud()
        {
            SqlCommand solicitud = new SqlCommand(
                this.cursor[0]
                + " WHERE id_MiSolicitud = " + this.solicitudID.ToString().Trim(), connection
                );
            SqlDataReader drsolicitud = solicitud.ExecuteReader();
            while (drsolicitud.Read())
            {
                this.campusID = Convert.ToInt32(drsolicitud["solicitante_id_campus"].ToString());
            }
        }
        protected void loadFormulario()
        {
            SqlCommand cmmdPreguntas = new SqlCommand(
                this.cursor[1]
                + " WHERE id_examen_dirigido = 1 AND Codigo_campus = " + this.campusID.ToString()
                , connection);
            int index = 0;
            SqlDataReader drPreguntas = cmmdPreguntas.ExecuteReader();
            if (drPreguntas.HasRows)
            {
                while (drPreguntas.Read())
                {
                    index++;
                    // Preguntas
                    Pregunta reactivo = new Pregunta();
                    reactivo.ID = Convert.ToInt32(drPreguntas["id_preguntas_campus"].ToString());
                    reactivo.descripcion = drPreguntas["Pregunta"].ToString();
                    reactivo.tipo = drPreguntas["Tipo_componente"].ToString().Trim();

                    Label lblPregunta = new Label(); // Etiqueta para agregar al formulario
                    lblPregunta.Text = index.ToString().Trim() + ". " + reactivo.descripcion;
                    lblPregunta.CssClass = " control-label ";
                    PanelFormulario.Controls.Add(lblPregunta);
                    // Opciones
                    SqlCommand cmmdOpciones = new SqlCommand(
                        this.cursor[2]
                        + " WHERE id_pregunta_campus = " + reactivo.ID.ToString().Trim()
                        , connection);
                    SqlDataReader drOpciones = cmmdOpciones.ExecuteReader();
                    switch(reactivo.tipo){
                        case "<select>": // <select>
                            DropDownList dropDownList = new DropDownList();
                            dropDownList.ID = "ctrlEvaluacion" + reactivo.ID.ToString().Trim();

                            while (drOpciones.Read()){
                                Respuesta opcion = new Respuesta(); // Opciones de reactivo
                                opcion.ID = Convert.ToInt32(drOpciones["id_respuesta"].ToString());
                                opcion.descripcion = drOpciones["Respuesta"].ToString();
                                opcion.valor = float.Parse(drOpciones["Valor"].ToString());

                                dropDownList.Items.Add(new ListItem(opcion.descripcion, opcion.valor.ToString())); // Se agrega el control al panel
                                dropDownList.CssClass = " form-control";

                                reactivo.respuestas.Add(opcion); // Se agrega la opcion al reactivo
                            }
                            PanelFormulario.Controls.Add(new LiteralControl("<br>")); // Salto de linea
                            PanelFormulario.Controls.Add(dropDownList);
                            PanelFormulario.Controls.Add(new LiteralControl("<br>")); // Salto de linea
                            break;
                        case "<checkbox>": // <checkbox>
                            CheckBoxList checkBoxList = new CheckBoxList();
                            checkBoxList.ID = "ctrlEvaluacion" + reactivo.ID.ToString().Trim();
                            while (drOpciones.Read()){
                                Respuesta opcion = new Respuesta(); // Opciones de reactivo
                                opcion.ID = Convert.ToInt32(drOpciones["id_respuesta"].ToString());
                                opcion.descripcion = drOpciones["Respuesta"].ToString();
                                opcion.valor = float.Parse(drOpciones["Valor"].ToString());
                                checkBoxList.Items.Add(new ListItem(opcion.descripcion, opcion.valor.ToString()));
                                checkBoxList.CssClass = " input-group form-control ";
                                checkBoxList.RepeatDirection = RepeatDirection.Horizontal;
                                reactivo.respuestas.Add(opcion); // Se agrega la opcion al reactivo
                            }
                            PanelFormulario.Controls.Add(checkBoxList);
                            break;
                        case "<option>": // <option>
                            RadioButtonList radioButtonList = new RadioButtonList();
                            radioButtonList.ID = "ctrlEvaluacion" + reactivo.ID.ToString().Trim();
                            while (drOpciones.Read())
                            {
                                Respuesta opcion = new Respuesta(); // Opciones de reactivo
                                opcion.ID = Convert.ToInt32(drOpciones["id_respuesta"].ToString());
                                opcion.descripcion = drOpciones["Respuesta"].ToString();
                                opcion.valor = float.Parse(drOpciones["Valor"].ToString());
                                radioButtonList.Items.Add(new ListItem(opcion.descripcion, opcion.valor.ToString()));
                                radioButtonList.CssClass = " input-group form-control ";
                                radioButtonList.RepeatDirection = RepeatDirection.Horizontal;
                                reactivo.respuestas.Add(opcion); // Se agrega la opcion al reactivo
                            }
                            PanelFormulario.Controls.Add(radioButtonList);
                            break;
                    }
                    this.evaluacion.preguntas.Add(reactivo); // Se agrega la pregunta a la evaluación.
                    // Salto de linea
                    PanelFormulario.Controls.Add(new LiteralControl("<br>"));
                }
                // Configuración de pantalla
                LblComentarios.Visible = true;
                TxtComentarios.Visible = true;
                BtnEnviar.Visible = true;
            }
            else
            {
                // Configuración de pantalla
                LblComentarios.Visible = false;
                TxtComentarios.Visible = false;
                BtnEnviar.Visible = false;
            }
        }
        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            string error = "NO";
            foreach (Pregunta reactivo in this.evaluacion.preguntas)
            {
                string ctrlname = "ctrlEvaluacion" + reactivo.ID.ToString().Trim();
                switch (reactivo.tipo)
                {
                    case "<select>": // <select>
                        DropDownList ddp = (DropDownList)PanelFormulario.FindControl(ctrlname);
                        break;
                    case "<checkbox>": // <checkbox>
                        CheckBoxList cbl = (CheckBoxList)PanelFormulario.FindControl(ctrlname);
                        if (cbl.SelectedValue == "")
                        {                          
                            error = "SI";
                        }
                        break;
                    case "<option>": // <option>
                        RadioButtonList rbl = (RadioButtonList)PanelFormulario.FindControl(ctrlname);
                        if (rbl.SelectedValue == "")
                        {
                            error = "SI";
                        }
                        break;
                }
                if (error == "SI")
                {
                    break;
                }
            }
            if (error == "NO")
            {
                connection.Open();
                Msg.Text = "";
                // recorrer el objeto evaluacion para obtener los valores de los controles
                foreach (Pregunta reactivo in this.evaluacion.preguntas)
                {
                    string ctrlname = "ctrlEvaluacion" + reactivo.ID.ToString().Trim();
                    switch (reactivo.tipo)
                    {
                        case "<select>": // <select>
                            DropDownList ddp = (DropDownList)PanelFormulario.FindControl(ctrlname);
                            SetRespuestaReactivo(reactivo.descripcion, ddp.SelectedItem.Text, ddp.SelectedValue);
                            break;
                        case "<checkbox>": // <checkbox>
                            CheckBoxList cbl = (CheckBoxList)PanelFormulario.FindControl(ctrlname);
                            SetRespuestaReactivo(reactivo.descripcion, cbl.SelectedItem.Text, cbl.SelectedValue);
                            break;
                        case "<option>": // <option>
                            RadioButtonList rbl = (RadioButtonList)PanelFormulario.FindControl(ctrlname);
                            SetRespuestaReactivo(reactivo.descripcion, rbl.SelectedItem.Text, rbl.SelectedValue);
                            break;
                    }
                }
                SetCalificacionEvaluacion();// De acuerdo al puntaje actualiza la calificacion
                if (GetTotalBecariosEvaluacion(this.solicitudID) == GetTotalBecariosSolicitud(this.solicitudID)) // Determina si actualiza la cal. general
                {
                    SetCalificacionSolicitante();
                }
                verModal(" Información ", " La evaluación se ha enviado correctamente ");
                BtnEnviar.Visible = false;
                PanelFormulario.Visible = false;
                LblComentarios.Visible = false;
                TxtComentarios.Visible = false;
                connection.Close();
            }
            else
            {
                verModal("Alerta", "No se han seleccionado todos los campos");
            }
        }
        protected void SetRespuestaReactivo(string reactivo, string respuesta, string valor) // Guarda la respuesta al reactivo
        {
            SqlCommand cmmd = new SqlCommand(
                "INSERT INTO tbl_historial_preguntas_empleados ("
                    + "Pregunta,"
                    + "Respuesta,"
                    + "Valor_repuesta,"
                    + "idMiSolicitud,"
                    + "Matricula"
                + ") "
                + "VALUES ("
                    + "'" + reactivo.Trim() + "',"
                    + "'" + respuesta + "',"
                    + valor.ToString() + ","
                    + this.solicitudID + ","
                    + this.alumnoID
                + ")"
            , connection);
            try
            {
                cmmd.ExecuteNonQuery();
                // Contabiliza el valor de la pregunta
                this.evaluacion.puntaje += float.Parse(valor);
            }
            catch (Exception ex)
            {
                Msg.Text = "Error al insertar: " + ex.ToString();
            }
        }
        protected void SetCalificacionEvaluacion() // Guarda la calificación de la evaluación
        {
            string estatus = GetEstatusPromedio(this.evaluacion.puntaje); // Obtiene el estatus del promedio
            SqlCommand cmmd = new SqlCommand( // Actualiza los datos de la solicitud
                "UPDATE tbl_solicitudes_becarios "
                + "SET "
                    + "Solicitante_calificacion = '" + estatus + "',"
                    + "Solicitante_puntaje = " + this.evaluacion.puntaje.ToString().Trim()
                + " WHERE id_MiSolicitud = " + this.solicitudID.ToString().Trim()
                + " AND id_alumno = " + this.alumnoID.ToString().Trim()
                , connection);
            try
            {
                cmmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Msg.Text += "Error al actualizar: " + ex.ToString();
            }
        }
        protected void SetCalificacionSolicitante() // Calcula y guarda la calificacion general del solicitante
        {
            SqlCommand cmmd;
            SqlDataReader dr;
            // Calcula el puntaje
            float puntaje = 0;
            int totalBecarios = 0;
            cmmd = new SqlCommand(
                this.cursor[4] // Becarios en solicitud
                + " WHERE id_MiSolicitud = " + this.solicitudID.ToString().Trim()
                , connection);
            dr = cmmd.ExecuteReader();
            while (dr.Read())
            {
                if (dr["Becario_puntaje"].ToString().Trim() != "")
                {
                    puntaje += float.Parse(dr["Becario_puntaje"].ToString().Trim());
                    totalBecarios ++;
                } 
            }
            if(totalBecarios > 0){ // Calcula el puntaje promedio
                puntaje = puntaje/totalBecarios;
            }
            // Guarda la calificación
            string status = GetEstatusPromedio(puntaje);
            cmmd = new SqlCommand( // Actualiza los datos de la solicitud
                "UPDATE tbl_solicitudes "
                + "SET "
                    + "Empleado_calificacion = '" + status + "',"
                    + "Empleado_puntuaje = " + puntaje.ToString().Trim()
                + " WHERE id_MiSolicitud = " + this.solicitudID.ToString()
                , connection);
            try
            {
                cmmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Msg.Text += "Error al actualizar: " + ex.ToString();
            }
        }
        protected bool ExisteEvaluacion(int solicitud, int alumno) // Devuelve true si el becario ya realizó la evaluación
        {
            SqlCommand cmmd = new SqlCommand( // Consulta si ya existe alguna evaluacion con esta solicitud y alumno
                "SELECT DISTINCT "
                    + "idMiSolicitud,"
                    + "Matricula "
                + "FROM tbl_historial_preguntas_empleados "
                + "WHERE idMiSolicitud = " + solicitud.ToString().Trim() + " AND Matricula = " + alumno.ToString().Trim()
                , connection);
            SqlDataReader dr = cmmd.ExecuteReader();
            return dr.HasRows;
        }
        protected bool EvaluacionAbierta() // Devuelve true si el becario ya realizó la evaluación
        {
            string idEmpleado;
            bool periodoAbierto = false;
            //string matricula = Session["Matricula"].ToString();
            SqlCommand cmmd = new SqlCommand( // Consulta si ya existe alguna evaluacion con esta solicitud y alumno
                " SELECT id_Misolicitud, Matricula "
                + " FROM tbl_solicitudes_becarios "
                + " WHERE id_alumno = (Select id_alumno From Tbl_alumnos Where Matricula = '" + Session["Matricula"].ToString().Trim() + "' AND id_estatus_asignacion = 2) "
                , connection);   
            SqlDataReader drsolicitud = cmmd.ExecuteReader();
            if (drsolicitud.HasRows)
            {
                while (drsolicitud.Read())
                {
                    this.solicitudID = Convert.ToInt32(drsolicitud["id_Misolicitud"]);
                    this.alumnoID = Convert.ToInt32(drsolicitud["Matricula"]);
                }
            }
            cmmd = new SqlCommand( // Consulta si ya existe alguna evaluacion con esta solicitud y alumno
                "SELECT "
                    + " te.Nomina, id_campus_alterno, Codigo_campus, id_periodo "
                + "FROM tbl_solicitudes ts, tbl_empleados te "
                + "WHERE id_MiSolicitud = " + this.solicitudID.ToString().Trim() + " AND ts.id_empleado = te.id_empleado" 
                , connection);
            SqlDataReader dr = cmmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    idCampusAlterno = dr["id_campus_alterno"].ToString();
                    idEmpleado = dr["Nomina"].ToString();
                    idCampus = dr["Codigo_campus"].ToString();
                    idPeriodo = dr["Periodo"].ToString();
                }
                if (idCampusAlterno == "0" || idCampusAlterno == "")
                {
                    cmmd = new SqlCommand( // Consulta si ya existe alguna evaluacion con esta solicitud y alumno
                                        "SELECT "
                                            + " Fecha_inicio, Fecha_fin "
                                        + "FROM tbl_campus_periodo tcp, tbl_fechas_evaluacion tfe "
                                        + "WHERE tcp.Codigo_campus = " + idCampus.ToString().Trim() + " AND Periodo = " + idPeriodo.Trim() + " AND tfe.id_campus_periodo = tcp.id_campus_periodo "
                                        , connection);
                    SqlDataReader drfechas = cmmd.ExecuteReader();
                    if (drfechas.HasRows)
                    {
                        while (drfechas.Read())
                        {
                            if (DateTime.Today >= Convert.ToDateTime(drfechas["Fecha_inicio"]) && DateTime.Today <= Convert.ToDateTime(drfechas["Fecha_fin"]))
                                periodoAbierto = true;
                        }
                    }
                    else
                    {
                        verModal(" Información ", " No hay fechas de evaluación establecidas ");
                    }                    
                }
                else
                {
                    cmmd = new SqlCommand( // Consulta si ya existe alguna evaluacion con esta solicitud y alumno
                        "SELECT "
                            + " Fecha_inicio, Fecha_fin "
                        + "FROM tbl_campus_periodo tcp, tbl_fechas_evaluacion tfe "
                        + "WHERE tcp.Codigo_campus = " + idCampusAlterno.ToString().Trim() + " AND Periodo = " + idPeriodo.Trim() + " AND tfe.id_campus_periodo = tcp.id_campus_periodo "
                        , connection);
                    SqlDataReader drfechas = cmmd.ExecuteReader();
                    if (drfechas.HasRows)
                    {
                        while (drfechas.Read())
                        {
                            string inicio = drfechas["Fecha_inicio"].ToString();
                            string fin = drfechas["Fecha_fin"].ToString();
                            if (DateTime.Today >= Convert.ToDateTime(drfechas["Fecha_inicio"]) && DateTime.Today <= Convert.ToDateTime(drfechas["Fecha_fin"]))
                                periodoAbierto = true;
                        }
                    }
                    else
                    {
                        verModal(" Información ", " No hay fechas de evaluación establecidas ");
                    }
                }
            }
            return periodoAbierto;
        }
        protected string GetEstatusPromedio(float puntaje) // Obtiene el resultado de la evaluacion de acuerdo al puntaje
        {
            string status = "";
            SqlCommand cmmd = new SqlCommand( // Obtiene los promedios a comparar
                this.cursor[3]
                + " WHERE Codigo_campus = " + this.campusID.ToString().Trim()
                , connection);
            SqlDataReader sdr = cmmd.ExecuteReader();
            while (sdr.Read())
            {
                float lower = float.Parse(sdr["Mayor_o_igual_que"].ToString()); // Limite inferior
                float upper = float.Parse(sdr["Menor_o_igual_que"].ToString()); // Limite superior
                if (puntaje >= lower & puntaje <= upper)
                {
                    status = sdr["Estatus"].ToString().Trim(); // Estatus Promedio
                }
            }
            return status;
        }
        protected int GetTotalBecariosSolicitud(int solicitud) // Obtiene el No. de becarios incluidos en la solicitud
        {
            int totalBecarios = 0;
            SqlCommand cmmd = new SqlCommand(
                this.cursor[4] // Becarios en solicitud
                + " WHERE id_Misolicitud = " + solicitud.ToString().Trim()
                , connection);
            SqlDataReader reader = cmmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    totalBecarios ++; // Contabiliza el No. de registros
                }
            }
            return totalBecarios;
        }
        protected int GetTotalBecariosEvaluacion(int solicitud) // Obtiene el No. de becarios que han realizado la evaluación
        {
            int totalBecarios = 0;
            SqlCommand cmmd = new SqlCommand(
                "SELECT DISTINCT "
	                + "idMiSolicitud,"
	                + "Matricula "
                + "FROM tbl_historial_preguntas_empleados "
                + "WHERE idMiSolicitud = " + solicitud.ToString().Trim()
                , connection);
            SqlDataReader reader = cmmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    totalBecarios ++; // Contabiliza el No. de registros devueltos
                }
            }
            return totalBecarios;
        }
        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }
    }

}