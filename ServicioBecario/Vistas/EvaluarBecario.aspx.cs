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
    public partial class EvaluarBecario : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection();
        List<Pregunta> preguntas
        {
            get {
                if (ViewState["ListPreguntas"] == null) {
                    ViewState["ListPreguntas"] = new List<Pregunta>();
                }
                return (List<Pregunta>)(ViewState["ListPreguntas"]);
            }
            set
            {
                ViewState["ListPreguntas"] = value;
            }
        }
        List<Alumno> alumnos
        {
            get {
                if (ViewState["ListAlumnos"] == null) {
                    ViewState["ListAlumnos"] = new List<Alumno>();
                }
                return (List<Alumno>)(ViewState["ListAlumnos"]);
            }
            set
            {
                ViewState["ListAlumnos"] = value;
            }
        }
        string  solicitanteID;
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
        string campusID;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();

            connection.Open(); // Abertura de la conexion

            // Recupera el ID del empleado
            this.solicitanteID = GetEmpleadoIdFromNomina(Session["Usuario"].ToString());
            this.campusID = GetCampusID(this.solicitanteID);

            /*if (this.solicitanteID > 0)
            {
                BtnEnviar.Visible = true;
                TxtComentarios.Visible = true;
                LblComentarios.Visible = true;
            }*/
            LoadDatosSolicitante();
            LoadGrdSolicitudes(this.solicitanteID);

            LoadEvaluacion();

            connection.Close(); // Cierre de la conexión
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            string ctrlName;
            int rowIndex;
            int columnIndex;
            string error = "NO";
            ///Verifica que los controles esten llenos
            for (rowIndex = 0; rowIndex < alumnos.Count; rowIndex++)
            {
                for (columnIndex = 1; columnIndex < preguntas.Count + 1; columnIndex++)
                {
                    ctrlName = "ctrlEvaluacion" + columnIndex.ToString().Trim(); // Obtiene el nombre del control a buscar

                    Pregunta reactivo = preguntas[columnIndex - 1];
                    switch (reactivo.tipo)
                    {
                        case "<select>": // <select>
                            break;

                        case "<checkbox>": // <checkbox>
                            CheckBoxList cbl = (CheckBoxList)GridEvaluacion.Rows[rowIndex].FindControl(ctrlName);

                            if (cbl.SelectedValue == "")
                            {
                                verModal("Alerta", "No se han seleccionado todos los campos");
                                error = "SI";
                            }
                            break;

                        case "<option>": // <option>
                            RadioButtonList rbl = (RadioButtonList)GridEvaluacion.Rows[rowIndex].FindControl(ctrlName);
                            if (rbl.SelectedValue == "")
                            {
                                verModal("Alerta", "No se han seleccionado todos los campos");
                                error = "SI";
                            }
                            break;
                    }
                }
            }

            if (error == "NO")
            {
                connection.Open(); // Abertura de conexion
                for (rowIndex = 0; rowIndex < alumnos.Count; rowIndex++)
                {
                    for (columnIndex = 1; columnIndex < preguntas.Count + 1; columnIndex++)
                    {
                        ctrlName = "ctrlEvaluacion" + columnIndex.ToString().Trim(); // Obtiene el nombre del control a buscar

                        Pregunta reactivo = preguntas[columnIndex - 1];
                        switch (reactivo.tipo)
                        {
                            case "<select>": // <select>
                                DropDownList ddl = (DropDownList)GridEvaluacion.Rows[rowIndex].FindControl(ctrlName);

                                SetRespuestaReactivo(alumnos[rowIndex].Matricula, reactivo.descripcion, ddl.SelectedItem.Text, ddl.SelectedValue);
                                break;

                            case "<checkbox>": // <checkbox>
                                CheckBoxList cbl = (CheckBoxList)GridEvaluacion.Rows[rowIndex].FindControl(ctrlName);

                                if (cbl.SelectedValue == "")
                                {
                                    verModal("No se contesto 1", "No se contesto 2");
                                }
                                else
                                {
                                    SetRespuestaReactivo(alumnos[rowIndex].Matricula, reactivo.descripcion, cbl.SelectedItem.Text, cbl.SelectedValue);
                                }
                                break;

                            case "<option>": // <option>
                                RadioButtonList rbl = (RadioButtonList)GridEvaluacion.Rows[rowIndex].FindControl(ctrlName);
                                SetRespuestaReactivo(alumnos[rowIndex].Matricula, reactivo.descripcion, rbl.SelectedItem.Text, rbl.SelectedValue);
                                break;
                        }
                    }

                    // Guardado de comentarios de la evaluacion del becario
                    ctrlName = "ctrlEvaluacion" + columnIndex.ToString().Trim(); // Obtiene el nombre del control a buscar
                    TextBox textbox = (TextBox)GridEvaluacion.Rows[rowIndex].FindControl(ctrlName);
                    SetComentariosBecario(alumnos[rowIndex].Matricula, textbox.Text);

                    // Actualización de la calificacion del becario
                    // nef  quiito SetCalificacionEvaluacion(alumnos[rowIndex].ID);
                    SetCalificacionEvaluacion(alumnos[rowIndex].Matricula);
                }

                // Guardado de comentarios generales de la evaluación
                SetComentariosGenerales(TxtComentarios.Text);

                connection.Close();

                // Si todo sale bien oculta formulario y envia mensaje al usuario
                formulario.Visible = false;
                BtnEnviar.Visible = false;

                Msg.Text = "La evaluación se ha enviado correctamente";
                verModal("Alerta", "La evaluación se ha enviado correctamente");
            }
        }
            
        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            divEvaluacion.Visible = false;
            divSolicitudes.Visible = true;
        }

        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        #endregion

        #region Evaluacion

        protected List<Pregunta> loadPreguntas()
        {
            List<Pregunta> preguntas = new List<Pregunta>();
            SqlCommand cmmd = new SqlCommand( 
                "SELECT "
	                + "id_preguntas_campus,"
	                + "Codigo_campus,"
	                + "Pregunta,"
	                + "id_examen_dirigido,"
	                + "id_componente,"
	                + "("
		                + "SELECT "
			                + "Tipo_componente "
		                + "FROM cat_componentes t3 "
		                + "WHERE t3.id_componente = t2.id_componente"
	                + ") Tipo_componente "
                + "FROM tbl_preguntas_campus t1 "
                + "Inner Join tbl_preguntas t2 "
                + "On t1.id_pregunta = t2.id_pregunta "
                + "WHERE id_examen_dirigido = 2 AND Codigo_campus =  '"+this.campusID.ToString()+"'" 
                , connection);

            SqlDataReader dr = cmmd.ExecuteReader();
            while (dr.Read())
            {
                Pregunta reactivo = new Pregunta();// Nueva pregunta
                reactivo.ID = Convert.ToInt32(dr["id_preguntas_campus"].ToString());
                reactivo.descripcion = dr["Pregunta"].ToString();
                reactivo.tipo = dr["Tipo_componente"].ToString().Trim();

                // Carga las opciones
                SqlCommand cmmdOpciones = new SqlCommand(
                        "SELECT "
                            + "* "
                        + "FROM tbl_respuestas "
                        + "WHERE id_pregunta_campus = " + reactivo.ID.ToString().Trim()
                        , connection);
                SqlDataReader drOpciones = cmmdOpciones.ExecuteReader();
                while (drOpciones.Read())
                {
                    Respuesta opcion = new Respuesta(); // Opciones de reactivo
                    opcion.ID = Convert.ToInt32(drOpciones["id_respuesta"].ToString());
                    opcion.descripcion = drOpciones["Respuesta"].ToString();
                    opcion.valor = float.Parse(drOpciones["Valor"].ToString());

                    reactivo.respuestas.Add(opcion); 
                }

                preguntas.Add(reactivo); // Se agrega la pregunta
            }

            return preguntas;
        }

        private List<Alumno> loadAlumnos()
        {
            List<Alumno> alumnos = new List<Alumno>();

            SqlCommand cmmd = new SqlCommand(
                    "SELECT "
                        + "id_Misolicitud,"
                        + "t1.Matricula AlumnoId,"
                        + "t2.Matricula,"
                        + "rtrim(ltrim(t2.Nombre)) + ' ' + rtrim(ltrim(Apellido_paterno)) + ' ' + RTRIM(ltrim(Apellido_materno)) as Nombre "
                    + "FROM tbl_solicitudes_becarios t1 "
                    + "JOIN tbl_alumnos t2 ON t2.Matricula = t1.Matricula  "
                    + "WHERE id_Misolicitud = " + this.solicitudID.ToString().Trim() + " and t1.id_estatus_asignacion = 2", connection);

            SqlDataReader dr = cmmd.ExecuteReader();
            while (dr.Read())
            {
                Alumno alumno = new Alumno();
                //alumno.ID = Convert.ToInt32(dr["Matricula"].ToString());
                alumno.Matricula = dr["Matricula"].ToString().Trim();
                alumno.Nombre = dr["Nombre"].ToString().Trim();

                alumnos.Add(alumno);
            }

            return alumnos;
        }
  
        protected void SetRespuestaReactivo(string alumno, string reactivo, string respuesta, string valor) // Guarda la respuesta al reactivo
        {
            SqlCommand cmmd = new SqlCommand(
                "INSERT INTO tbl_historial_preguntas_becarios ("
                    + "Pregunta,"
                    + "Repuesta,"
                    + "Valor_respuesta,"
                    + "idMiSolicitud,"
                    + "Matricula"
                + ") "
                + "VALUES ("
                    + "'" + reactivo.Trim() + "',"
                    + "'" + respuesta + "',"
                    + valor.ToString() + ","
                    + this.solicitudID + ", '" + alumno.ToString() + "' " 
                + ")"
            , connection);

            try
            {
                cmmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                verModal("Alerta", "Error al insertar: " + ex.ToString());
            }
        }
        
        protected void SetComentariosBecario(string alumno, string comentarios) // Guarda los comentarios de la evaluacion al becario
        {
            SqlCommand cmmd = new SqlCommand( // Actualiza los datos de la solicitud
                "UPDATE tbl_solicitudes_becarios "
                + "SET "
                    + "Comentarios = '" + comentarios + "' "
                + " WHERE id_MiSolicitud = " + this.solicitudID.ToString()
                + " AND Matricula = '" + alumno.ToString() + "'"  
                , connection);

            try
            {
                cmmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                verModal("Alerta", "Error al actualizar: " + ex.ToString());
            }
        }
        
        protected void SetComentariosGenerales(string comentarios) // Guarda los comentarios generales de la evaluación
        {
            SqlCommand cmmd = new SqlCommand( // Actualiza los datos de la solicitud
                "UPDATE tbl_solicitudes "
                + "SET "
                    + "Comentarios_generales = '" + comentarios + "' "
                + " WHERE id_MiSolicitud = " + this.solicitudID.ToString()
                , connection);

            try
            {
                cmmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                verModal("Alerta", "Error al actualizar: " + ex.ToString());
            }
        }
        
        protected void SetCalificacionEvaluacion(string alumno) // Guarda la calificación de la evaluación
        {
            float puntaje = 0;// Obtiene el puntaje del becario
            SqlCommand cmmd = new SqlCommand(
                "SELECT "
                    + "SUM(Valor_respuesta) Puntaje FROM tbl_historial_preguntas_becarios "
                + "WHERE idMiSolicitud = " + this.solicitudID.ToString()
                    + " AND Matricula = '" + alumno.ToString() + "' "  
                , connection);

            SqlDataReader dr = cmmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    puntaje = float.Parse(dr["Puntaje"].ToString());
                }
            }

            string estatus = GetEstatusPromedio(puntaje); // Obtiene el estatus del promedio
            cmmd = new SqlCommand( // Actualiza los datos de la solicitud
                "UPDATE tbl_solicitudes_becarios "
                + "SET "
                    + "Becario_calificacion = '" + estatus + "',"
                    + "Becario_puntaje = " + puntaje.ToString().Trim()
                + " WHERE id_MiSolicitud = " + this.solicitudID.ToString().Trim()
                + " AND Matricula = '"+alumno.ToString().Trim()+"'"
                , connection);

            try
            {
                cmmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                verModal("Alerta", "Error al actualizar: " + ex.ToString());
            }
        }
        
        protected string GetEstatusPromedio(float puntaje) // Obtiene el resultado de la evaluacion de acuerdo al puntaje
        {
            string status = "";

            SqlCommand cmmd = new SqlCommand( // Obtiene los promedios a comparar
                "SELECT "
                    + "id_promedio_campus,"
                    + "Codigo_campus,"
                    + "t1.id_promedio,"
                    + "Menor_o_igual_que,"
                    + "Mayor_o_igual_que,"
                    + "t2.id_estatus_promedio,"
                    + "Estatus " 
                + "FROM tbl_promedio_campus t1 "
                + "Join tbl_promedios t2 On t1.id_promedio = t2.id_promedio "
                + "Join cat_estatus_promedios t3 On t2.id_estatus_promedio = t3.id_estatus_promedio " //Promedios
                + "WHERE Codigo_campus = '"+this.campusID.ToString().Trim()+"' " 
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
        
        protected bool ExisteEvaluacion(int solicitud) // Devuelve true si el becario ya realizó la evaluación
        {
            SqlCommand cmmd = new SqlCommand( // Consulta si ya existe alguna evaluacion con esta solicitud
                "SELECT DISTINCT "
                    + "idMiSolicitud "
                + "FROM tbl_historial_preguntas_becarios "
                + "WHERE idMiSolicitud = " + solicitud.ToString().Trim()
                , connection);

            SqlDataReader dr = cmmd.ExecuteReader();

            return dr.HasRows;
        }

        protected void LoadEvaluacion()
        {
            this.preguntas = loadPreguntas(); // Carga los nombres de las preguntas
            if (this.preguntas.Count > 0)
            {
                DataTable table = new DataTable(); // Llenado de Tabla temporal

                table.Columns.Add("Alumno"); // Primera columna
                for (int index = 0; index < this.preguntas.Count; index++)
                {
                    table.Columns.Add(this.preguntas[index].descripcion);
                }
                table.Columns.Add("Comentarios"); // Ultima columna

                // Busqueda y carga de los alumnos de la solicitud.
                this.alumnos = loadAlumnos();

                for (int index = 0; index < this.alumnos.Count; index++)
                {
                    DataRow row = table.NewRow();
                    row["Alumno"] = alumnos[index].Nombre.Trim();
                    table.Rows.Add(row);
                    BtnEnviar.Visible = true;
                }

                // Carga el datatable en el GridView
                GridEvaluacion.DataSource = table;
                GridEvaluacion.DataBind();
                
                // Agrega los controles al grid
                int rowIndex;
                int columnIndex;
                for (rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
                {
                    for (columnIndex = 1; columnIndex < table.Columns.Count - 1; columnIndex++)
                    {
                        string ctrlname = "ctrlEvaluacion" + columnIndex.ToString().Trim(); // identificador de control

                        Pregunta reactivo = this.preguntas[columnIndex - 1];
                        switch (reactivo.tipo)
                        {
                            case "<select>": // <select>
                                DropDownList dropDownList = new DropDownList();
                                dropDownList.ID = ctrlname;
                                dropDownList.CssClass = " form-control ";
                                foreach (Respuesta opcion in reactivo.respuestas)
                                {
                                    dropDownList.Items.Add(new ListItem(opcion.descripcion, opcion.valor.ToString()));
                                }
                                GridEvaluacion.Rows[rowIndex].Cells[columnIndex].Controls.Add(dropDownList);
                                break;

                            case "<checkbox>": // <checkbox>
                                CheckBoxList checkBoxList = new CheckBoxList();
                                checkBoxList.ID = ctrlname;
                                checkBoxList.CssClass = " input-group form-control ";
                                int i = 1;
                                foreach (Respuesta opcion in reactivo.respuestas)
                                {
                                    if (i == 1)
                                    {
                                        ListItem li1 = new ListItem(opcion.descripcion, opcion.valor.ToString());
                                        li1.Selected = true; 
                                        checkBoxList.Items.Add(li1);
                                        i = 0;
                                    }
                                    else
                                    {
                                        checkBoxList.Items.Add(new ListItem(opcion.descripcion, opcion.valor.ToString()));
                                    }
                                }
                                GridEvaluacion.Rows[rowIndex].Cells[columnIndex].Controls.Add(checkBoxList);
                                break;

                            case "<option>": // <option>
                                RadioButtonList radioButtonList = new RadioButtonList();
                                radioButtonList.ID = ctrlname;
                                radioButtonList.CssClass = " form-control ";
                                radioButtonList.RepeatDirection = RepeatDirection.Horizontal;

                                foreach (Respuesta opcion in reactivo.respuestas)
                                {
                                    radioButtonList.Items.Add(new ListItem(opcion.descripcion, opcion.valor.ToString()));
                                }

                                GridEvaluacion.Rows[rowIndex].Cells[columnIndex].Controls.Add(radioButtonList);

                                break;
                        }
                    }

                    // Agrega columna de comentarios
                    TextBox textBox = new TextBox();
                    textBox.ID = "ctrlEvaluacion" + columnIndex.ToString().Trim(); // identificador de control //"ctrlEvaluacion" + columnIndex.ToString().Trim();
                    textBox.CssClass = " form-control ";
                    GridEvaluacion.Rows[rowIndex].Cells[columnIndex].Controls.Add(textBox);
                }
            }
        }

        #endregion

        #region Solicitante

        protected string GetEmpleadoIdFromNomina(string nomina)
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
                    //empleadoId = Convert.ToInt32(dr["Nomina"].ToString());
                    empleadoId = dr["Nomina"].ToString();
                }
            }

            return empleadoId; // Regreso de valor
        }

        protected string GetCampusID(string solicitanteId)
        {
            string  campusID = "0";

            SqlCommand cmmd = new SqlCommand(
                "SELECT "
                    + "Codigo_campus "
                + "FROM tbl_empleados "
                + "WHERE Nomina = '"+solicitanteId+"' "  
            , connection);

            SqlDataReader dr = cmmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    //campusID = Convert.ToInt32(dr["Codigo_campus"].ToString());
                    campusID = dr["Codigo_campus"].ToString();
                }
            }

            return campusID; // Regreso de valor

        }

        #endregion

        #region GrdSolicitudes

        protected void LoadGrdSolicitudes(string solicitanteId)
        {
            SqlCommand cmmd = new SqlCommand(
                "SELECT "
                    + "id_MiSolicitud SolicitudID,"
	                + "(SELECT Nombre FROM cat_tipo_solicitudes tip WHERE tip.id_tipo_solicitud = sol.id_tipo_solicitud) TipoDescripcion "
                + "FROM tbl_solicitudes sol "
                + "JOIN cat_periodos per ON per.Periodo = sol.Periodo "
                + "WHERE sol.Nomina =  '" + solicitanteId.ToString() + "' AND  per.Activo = 1"
            , connection);

            SqlDataAdapter da = new SqlDataAdapter(cmmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GrdSolicitudes.DataSource = dt;
            GrdSolicitudes.DataBind();

            if (dt.Rows.Count == 0)
            {
                verModal("Alerta", "No existen solicitudes para evaluar");
            }
        }

        protected void GrdSolicitudes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument); // index
            GridViewRow row = GrdSolicitudes.Rows[index]; // row
            this.solicitudID = Convert.ToInt32(row.Cells[0].Text); // Setea como solicitud seleccionada

            connection.Open(); // Abertura de conexion
           
            if (e.CommandName == "Evaluar")
            {
                if (!ExisteEvaluacion(this.solicitudID))
                {
                    //Verificar la fecha de modificación
                    SqlCommand command = new SqlCommand(string.Format("SELECT id_Misolicitud, tbl_solicitudes.Periodo, tbl_solicitudes.Nomina, "
                                                + "tbl_empleados.Codigo_campus, tbl_campus_periodo.id_campus_periodo, Fecha_inicio, Fecha_fin  "
                                                + "From tbl_solicitudes, tbl_empleados, tbl_campus_periodo, tbl_fechas_evaluacion "
                                                + "Where id_MiSolicitud = @idmisolicitud "
                                                + "and tbl_solicitudes.Nomina = tbl_empleados.Nomina "
                                                + "and tbl_empleados.Codigo_campus = tbl_campus_periodo.Codigo_campus "
                                                + "and tbl_campus_periodo.Periodo = tbl_solicitudes.Periodo "
                                                + "and tbl_fechas_evaluacion.id_campus_periodo = tbl_campus_periodo.id_campus_periodo "), connection);
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@idmisolicitud", this.solicitudID.ToString().Trim());
                    //connection.Open();
                    SqlDataReader dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (DateTime.Today >= Convert.ToDateTime(dr["Fecha_inicio"]) && DateTime.Today <= Convert.ToDateTime(dr["Fecha_fin"]))
                            {
                                divSolicitudes.Visible = false;
                                divEvaluacion.Visible = true;
                                LoadEvaluacion();
                                TxtComentarios.Text = "";
                                // Muestra los controles de la evaluación
                                formulario.Visible = true;
                                //BtnEnviar.Visible = true;
                                TxtComentarios.Visible = true;
                                LblComentarios.Visible = true;
                            }
                            else
                            {
                                verModal("Alerta", " La solicitud, no se puede evaluar (esta fuera de periodo) ");
                            }
                        }
                    }
                    else
                    {
                        verModal("Alerta", " No hay fechas de evaluación registradas ");
                    }

                }
                else
                {
                    verModal("Alerta", "Usted ya ha evaluado a los becarios de esta solicitud");

                    // Oculta los controles de la evaluación
                    BtnEnviar.Visible = false;
                    TxtComentarios.Visible = false;
                    LblComentarios.Visible = false;
                }
            }

            connection.Close(); // cierre de la conexion
        }

        protected void GrdSolicitudes_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        #endregion

        protected void LoadDatosSolicitante()
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
                + "WHERE Nomina = '" + this.solicitanteID.ToString().Trim() + "' and tbl_empleados.Codigo_campus = cat_campus.Codigo_campus "
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
                    DatosCorreo.Text = reader["Correo_electronico"].ToString();
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

    }
}