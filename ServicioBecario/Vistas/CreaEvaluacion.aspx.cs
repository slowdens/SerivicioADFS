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
    public partial class CreaEvaluacion : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, idcomponente,iddirigido,idcampus;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    llenarCampus();
                    llenarDirigido();
                    llenarComponentes();
                    llenaFiltroComponente();
                    filtrarDirigidos();
                    llenarfiltroCampus();
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void llenaFiltroComponente()
        {
            query = "select id_componente,  LTRIM (Tipo_componente) as Tipo_componente from cat_componentes";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlfiltrarComponentes.DataTextField = "Tipo_componente";
                ddlfiltrarComponentes.DataValueField = "id_componente";
                ddlfiltrarComponentes.DataSource = dt;
                ddlfiltrarComponentes.DataBind();
            }
        }
        public void agregarPregunta()
        {
            query = "sp_guardar_pregunta '" + ddlCampus.SelectedValue + "'," + ddlDirigido.SelectedValue + ",'" + txtPregunta.Text.Trim() + "'," + ddlComponente.SelectedValue + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                //Esto es para salida de los mensaje de preguntas
                switch (dt.Rows[0]["Mensaje"].ToString())
                {
                    case "Ok":
                        verModal("Exito", "Tu información ha sido guardada con éxito");

                        break;
                    case "Exite":
                        verModal("Alerta", "Ya existe la pregunta registrada");
                        break;
                    case "No":
                        verModal("Alerta", "No se puede guardar la información");
                        break;
                }
                //Tomamos le id de la pregunta
                hdfId_pregunta.Value = dt.Rows[0]["Preguntaid"].ToString();
                //Esto es para los mensajes de la respuesta
                switch (dt.Rows[0]["MensajeRespuesta"].ToString())
                {
                    case "NoR":
                        pnlCapturaRespuesta.Visible = true;
                        lblPregunta.Text = txtPregunta.Text;
                        break;
                    case "YesR":
                        pnlCapturaRespuesta.Visible = false;
                        verModal("Alerta", "La pregunta ya contiene respuesta y no se puede sobre escribir, si desea vaya a mostrar pregunta y seleccione respuesta ");
                        break;
                }

            }

        }
        protected void btnGuardarPregunta_Click(object sender, EventArgs e)
        {
            try
            {
                pnlCapturaPregunta.Visible = false;
                agregarPregunta();
                pnlCapturaRespuesta.Visible = true;
               // tpCaptara.HeaderText = "Captura de respuesta";

            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }
        public void filtrarDirigidos()
        {
            query = "select id_examen_dirigido,Dirigido_a from cat_examen_dirigos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarDirigido.DataValueField = "id_examen_dirigido";
                ddlFiltrarDirigido.DataTextField = "Dirigido_a";
                ddlFiltrarDirigido.DataSource = dt;
                ddlFiltrarDirigido.DataBind();
            }
        }
        protected void btbAgregarRespuesta_Click(object sender, EventArgs e)
        {
            try
            {
                pnlCapturaRespuesta.Visible = false;
                guardarPreguntas();
                pnlCapturaPregunta.Visible = true;
                //tpCaptara.HeaderText = "Crear pregunta";
                limpiarCapturapregunta();
                limpiarCapturaRespuesta();
                mostrarinformacionEnGrid();
                //tpVista.Width 
               // TabContainer1.ActiveTabIndex = 1;//Activa la segunda ventanita
                


            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void limpiarCapturaRespuesta()
        {
            txtRespuesta1.Text = "";
            txtRespuesta2.Text = "";
            txtRespuesta3.Text = "";
            txtRespuesta4.Text ="";
            txtRespuesta5.Text ="";
            txtValor1.Text = "";
            txtValor2.Text = "";
            txtValor3.Text = "";
            txtValor4.Text = "";
            txtValor5.Text = "";
        }

        public void limpiarCapturapregunta()
        {
            txtPregunta.Text = "";
            ddlCampus.SelectedValue = "";
            ddlDirigido.SelectedValue = "";
            ddlComponente.SelectedValue = "";
        }
        public void guardarPreguntas()
        {
            string respuesta = txtRespuesta1.Text.Trim() + "!" + txtRespuesta2.Text.Trim() + "!" + txtRespuesta3.Text.Trim() + "!" + txtRespuesta4.Text.Trim() + "!" + txtRespuesta5.Text.Trim() + "!";
            string valor = txtValor1.Text.Trim() + "!" + txtValor2.Text.Trim() + "!" + txtValor3.Text.Trim() + "!" + txtValor4.Text.Trim() + "!" + txtValor5.Text.Trim();
            query = "sp_guardar_repuestas '" + respuesta + "','" + valor + "'," + hdfId_pregunta.Value + ",'!'," + ddlCampus.SelectedValue + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Exito", "Las respuestas fueron agregadas correctamente");
                }
                else
                {
                    verModal("Error", "Se encontró con un error al guardar las respuestas");
                }
            }

        }


        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre as Campus from cat_campus order by Nombre asc";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataTextField = "Campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        public void llenarDirigido()
        {
            query = "select id_examen_dirigido,Dirigido_a from cat_examen_dirigos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlDirigido.DataTextField = "Dirigido_a";
                ddlDirigido.DataValueField = "id_examen_dirigido";
                ddlDirigido.DataSource = dt;
                ddlDirigido.DataBind();
            }
        }

        protected void ddlDirigido_DataBound(object sender, EventArgs e)
        {
            ddlDirigido.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        public void llenarComponentes()
        {
            query = "select id_componente,Tipo_componente from cat_componentes";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlComponente.DataTextField = "Tipo_componente";
                ddlComponente.DataValueField = "id_componente";
                ddlComponente.DataSource = dt;
                ddlComponente.DataBind();
            }
        }

        public void llenarfiltroCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus where Codigo_campus!='PRT' ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltraCampus.DataValueField = "Codigo_campus";
                ddlFiltraCampus.DataTextField = "Nombre";
                ddlFiltraCampus.DataSource = dt;
                ddlFiltraCampus.DataBind();
            }
        }
        protected void ddlComponente_DataBound(object sender, EventArgs e)
        {
            ddlComponente.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        public bool mostrarRespuestas(string id, string idCampus)
        {
            bool bandara = false;
            query = "sp_mostrar_respuestas " + id + "," + idCampus + " ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvMostrarRespuestas.DataSource = dt;
                GvMostrarRespuestas.DataBind();
                bandara = true;
            }
            else
            {
                verModal("Alerta", "La pregunta seleccionada no contiene respuestas");
            }
            return bandara;
        }

        protected void ddlfiltrarComponentes_DataBound(object sender, EventArgs e)
        {
            ddlfiltrarComponentes.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void ddlFiltrarDirigido_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarDirigido.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void ddlFiltraCampus_DataBound(object sender, EventArgs e)
        {
            ddlFiltraCampus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void btnFiltrarPregunta_Click(object sender, EventArgs e)
        {
            try
            {
                mostrarinformacionEnGrid();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void mostrarinformacionEnGrid()
        {
            if (string.IsNullOrEmpty(ddlfiltrarComponentes.SelectedValue))
            {
                idcomponente = "-1";
            }
            else
            {
                idcomponente = ddlfiltrarComponentes.SelectedValue;
            }
            if (string.IsNullOrEmpty(ddlFiltrarDirigido.SelectedValue))
            {
                iddirigido = "-1";
            }
            else
            {
                iddirigido = ddlFiltrarDirigido.SelectedValue;
            }
            if (string.IsNullOrEmpty(ddlFiltraCampus.SelectedValue))
            {
                idcampus = "-1";
            }
            else
            {
                idcampus = ddlFiltraCampus.SelectedValue;
            }

            query = "sp_mostra_pregunta_campos_dirigido '" + txtFiltrarPregunas.Text.ToString().Trim() + "'," + idcomponente+ "," + iddirigido+ "," + idcampus + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvMostrarPregutas.DataSource = dt;
                GvMostrarPregutas.DataBind();
            }
            else
            {
                verModal("Alerta", "No se encontró información en la búsqueda");
                GvMostrarPregutas.DataSource = null;
                GvMostrarPregutas.DataBind();
            }
        }
        protected void lnkVista_click(object sender, EventArgs e)
        {
            try
            {
                /* cuando se deja activada la edicion de pregunta se restablece*/
                btnFiltrarPregunta.Visible = true;
                btnActulizarPreguntas.Visible = false;
                btnCancelarEdicionPregunta.Visible = false;
                limpiarcomponentesFiltrosPregunta();
                GvMostrarPregutas.SelectedIndex = -1;
                /**/
                LinkButton btn = (LinkButton)(sender);
                //string id_pregunta = btn.CommandArgument;
                //string id_campus = btn.ToolTip;
                string combinado = btn.CommandArgument;
                string idpregunta = combinado.Substring(0, combinado.LastIndexOf("!"));
                combinado = combinado.Replace(idpregunta + "!", "");
                lblRespuestaFiltrada.Text = btn.CommandName;
                hdfid_pregunta_.Value = idpregunta;
                hdfid_campus_.Value = combinado;
                if (mostrarRespuestas(idpregunta, combinado))
                {
                    pnlMostrarPreguntas.Visible = false;
                    PnlVerRespuestas.Visible = true;
                }
                lblCampusFiltrado.Text = ddlCampus.Items.FindByValue(hdfid_campus_.Value).Text;
              //  tpVista.HeaderText = "Ver respuestas";
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void GvMostrarRespuestas_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlActualizarRespuestas.Visible = true;//Activamos nuestro panel
            hdid_respuesta.Value = GvMostrarRespuestas.SelectedDataKey.Value.ToString();

            txtRepuestaActulizar.Text = GvMostrarRespuestas.SelectedRow.Cells[1].Text;
            txtValorActualizar.Text = GvMostrarRespuestas.SelectedRow.Cells[2].Text;
            txtValorActualizar.Text = txtValorActualizar.Text.Replace(',', '.');
           
        }

        protected void GvMostrarPregutas_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdfId_pregunta.Value = GvMostrarPregutas.SelectedDataKey.Value.ToString();
            string s = GvMostrarPregutas.SelectedRow.Cells[1].Text;
            LinkButton contenedor = GvMostrarPregutas.Rows[GvMostrarPregutas.SelectedIndex].FindControl("lnkVista") as LinkButton;
            txtFiltrarPregunas.Text = contenedor.CommandName;
            ddlFiltrarDirigido.SelectedValue = ddlFiltrarDirigido.Items.FindByText(GvMostrarPregutas.SelectedRow.Cells[3].Text).Value;
            ddlfiltrarComponentes.SelectedValue = ddlfiltrarComponentes.Items.FindByText(HttpUtility.HtmlDecode(GvMostrarPregutas.SelectedRow.Cells[2].Text).Trim()).Value;
            ddlFiltraCampus.SelectedValue = ddlFiltraCampus.Items.FindByText(HttpUtility.HtmlDecode(GvMostrarPregutas.SelectedRow.Cells[4].Text)).Value;
            hdfId_campus.Value = ddlFiltraCampus.SelectedValue;
            btnFiltrarPregunta.Visible = false;
            btnActulizarPreguntas.Visible = true;
            //deshabilitarCampusCliente();//con esta funcion desabilito el cliente
            btnCancelarEdicionPregunta.Visible = true;
            ddlFiltraCampus.Enabled = false;
            
        }
        protected void btnActulizarPreguntas_click(object sender, EventArgs e)
        {
            try
            {
                
                actualizarPregunta();
                limpiarcomponentesFiltrosPregunta();
                mostrarinformacionEnGrid();
                btnFiltrarPregunta.Visible = true;
                btnActulizarPreguntas.Visible = false;
                btnCancelarEdicionPregunta.Visible = false;
                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void actualizarPregunta()
        {
            query = "sp_actualiza_pregunta " + hdfId_pregunta.Value + ",'" + txtFiltrarPregunas.Text + "'," + ddlfiltrarComponentes.SelectedValue + "," + ddlFiltrarDirigido.SelectedValue + "," + hdfId_pregunta.Value + " ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Exito", "Se a actualizo la información");
                }
            }
        }
        protected void GvMostrarPregutas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GvMostrarPregutas.PageIndex = e.NewPageIndex;
                mostrarinformacionEnGrid();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void deshabilitarCampusCliente()
        {
            ClientScript.RegisterStartupScript(GetType(), "Mostrar", "desavilitaCampus();", true);
        }


        protected void btnActulizarRespuestas_Click(object sender, EventArgs e)
        {
            try
            {
                actulizarRespuestas();
                mostrarRespuestas(hdfid_pregunta_.Value, hdfid_campus_.Value);
                pnlActualizarRespuestas.Visible = false;
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void actulizarRespuestas()
        {
            query = "sp_actualiza_respuestas " + hdid_respuesta.Value + ",'" + txtRepuestaActulizar.Text + "'," + txtValorActualizar.Text + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Exito", "La transacción se realizó correctamente");
                }
                else
                {
                    verModal("Alerta", "No se realizo a transacción!!");
                }
            }
        }

        protected void btnCancelarEdicionPregunta_Click(object sender, EventArgs e)
        {
            try
            {
                btnFiltrarPregunta.Visible = true;
                btnActulizarPreguntas.Visible = false;
                btnCancelarEdicionPregunta.Visible = false;
                limpiarcomponentesFiltrosPregunta();
                GvMostrarPregutas.SelectedIndex = -1;

            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }


        public void limpiarcomponentesFiltrosPregunta()
        {
            txtFiltrarPregunas.Text = "";
            ddlfiltrarComponentes.SelectedValue = "";
            ddlFiltrarDirigido.SelectedValue = "";
            ddlFiltraCampus.SelectedValue = "";
            ddlFiltraCampus.Enabled = true;
        }

        protected void btnCancelarRespuesta_Click(object sender, EventArgs e)
        {
            try
            {
                pnlActualizarRespuestas.Visible = false;
                GvMostrarRespuestas.SelectedIndex = -1;

            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            try
            {
                pnlMostrarPreguntas.Visible = true;
                PnlVerRespuestas.Visible = false;
                GvMostrarRespuestas.SelectedIndex = -1;
                pnlActualizarRespuestas.Visible = false;
               // tpVista.HeaderText = "Ver preguntas";
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }
    }
}