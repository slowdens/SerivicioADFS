using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using ServicioBecario.Codigo;

namespace ServicioBecario.Vistas
{
    public partial class MostrarSolicitudDatos : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        DataTable dt;
        string query;
        BasedeDatos db = new BasedeDatos();

        protected void Page_Load(object sender, EventArgs e)
        {
            try{
                if(!IsPostBack)
                {
                    string id_solicitud = Request.QueryString["ds"];
                    string tipoSolicitud = Request.QueryString["soli"];
                    if (!String.IsNullOrEmpty(id_solicitud) && !String.IsNullOrEmpty(tipoSolicitud))
                    {
                        switch (tipoSolicitud)
                        {
                                //Seleccionamos el tipo de solicitud
                            case "Especifica individual":
                                //mostramos en pantalla la informacion que tiene que como titulo
                                lblTipoSolicitud.Text = "Solicitud especifica individual";
                                //Sacamos los nombre del solicitante
                                query = "sp_solititante_solicitud "+id_solicitud+"";

                                dt = db.getQuery(conexionBecarios,query);
                                if(dt.Rows.Count>0)
                                {
                                    lblSolicitud.Text = dt.Rows[0]["Solicitud"].ToString();
                                    lblNombreSolicitante.Text = dt.Rows[0]["NombreSolicitante"].ToString();
                                    lblPeriodo.Text = dt.Rows[0]["Periodo"].ToString();
                                }

                            

                                //sacar los resultados de la asignacion de la base de datos;
                                query = "sp_consulta_tipo_solicitud_especifica " + id_solicitud + "";
                                dt = db.getQuery(conexionBecarios,query);
                                if(dt.Rows.Count>0)
                                {
                                    GVSolicitudIndividual.DataSource = dt;
                                    GVSolicitudIndividual.DataBind();
                                }
                                pnlEspecifica.Visible = true;
                                verModal("Hola ","Aqui estoy");
                                break;

 /*******************************************************************************Por proyecto***************************************************************************************************/                       
                            case "Por proyecto especial":
                            case "Por proyecto":
                                if (tipoSolicitud == "Por proyecto especial")
                                {
                                    lblTipoSolicitud.Text = "Solicitud por proyecto especial";
                                }
                                if (tipoSolicitud == "Por proyecto")
                                {
                                    lblTipoSolicitud.Text = "Solicitud por proyecto";
                                }

                                pnlPorProyecto.Visible = true;


                                query = "sp_solititante_solicitud " + id_solicitud + "";
                                dt = db.getQuery(conexionBecarios,query);
                                if(dt.Rows.Count>0)
                                {
                                    lblSolicitanteP.Text = dt.Rows[0]["NombreSolicitante"].ToString();
                                    lblPeriodoP.Text = dt.Rows[0]["Periodo"].ToString();
                                    lblTipoSolicitudP.Text = dt.Rows[0]["Solicitud"].ToString();
                                }
                                //sacar los resultados de la asignacion de la base de datos;
                                query = "sp_consulta_tipo_solicitud_especifica " + id_solicitud + "";
                                dt = db.getQuery(conexionBecarios, query);
                                if (dt.Rows.Count > 0)
                                {
                                    gvVistasProyecto.DataSource = dt;
                                    gvVistasProyecto.DataBind();
                                }
                                



                                break;                                
                            case "Masiva por sorteo":
                                    lblTipoSolicitud.Text = "Masiva por sorteo";
                                break;
                        }
                    }
                    else
                    {
                        verModal("Alerta","No hay información");
                    }
                }
            }catch(Exception  es)
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

        protected void imbEliminar_click(object sender, EventArgs e)
        {
            try
            {
                ImageButton boton = (ImageButton)sender;
                string id = boton.CommandArgument;
                query = "sp_elimina_alumno_solicitud "+id+"";
                dt = db.getQuery(conexionBecarios,query);
                if(dt.Rows.Count>0)
                {
                    if(dt.Rows[0]["Mensaje"].ToString()=="Ok")
                    {
                        verModal("Exito", "La información se eliminó correctamente");
                    }
                }
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }
    /*******************************************************************************************************************************************************/
        protected void imgActualizarPorProyecto_Click(object sender, EventArgs e)
        {
            try
            {
                pnlActualizarProyectoBecarios.Visible = true;
                //buton que permite actualizar la informacición con respecto al becario.
                ImageButton buton = (ImageButton)sender;
                //Tomamos el id del proyecto
                string id = buton.CommandName;
                //mostramos nuestro panel para que se visualze
                
                //Llenamos los combobox
                llenarNivelAcademico(ddlNivelEstudosPorproyecto);
                llenarProgramaAcademico(ddlprogramaAcademicoPorProyecto);
                llenarGradoCursado(ddlPeriodoCursadoPorproyecto);
                //Sacams la información del alumno

                //llenarCamposSolicitudIndivual(id);

                llenarCamposSolicitudPorProyecto(id);

            }catch(Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

/***************************************************************************************************************************************************************/
        protected void imgActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                //buton que permite actualizar la informacición con respecto al becario.
                ImageButton buton = (ImageButton)sender;
                //Tomamos el id del proyecto
                string id = buton.CommandName;
                //mostramos nuestro panel para que se visualze
                pnlActualizarEspecifica.Visible = true;
                //Llenamos los combobox
                llenarNivelAcademico(ddlAnivelEstudios);
                llenarProgramaAcademico(ddlAProgramaAcademico);
                llenarGradoCursado(ddlAPeriodoCursado);
                //Sacams la información del alumno

                llenarCamposSolicitudIndivual(id);




            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


        public void llenarGradoCursado(DropDownList dll)
        {
            query = "select id_grado_cursado,grado from cat_grado_cursado";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                dll.DataTextField = "grado";
                dll.DataValueField = "id_grado_cursado";
                dll.DataSource = dt;
                dll.DataBind();
            }
            

        }


        public void llenarCamposSolicitudIndivual(string id)
        {
            query = "sp_saca_informacion_alumno "+id+"";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                txtAmatricula.Text = dt.Rows[0]["Matricula"].ToString();
                lblNombre.Text = dt.Rows[0]["NombreAlumno"].ToString();
                ddlAnivelEstudios.SelectedValue = ddlAnivelEstudios.Items.FindByText(dt.Rows[0]["Nivel_academico"].ToString()).Value;
                ddlAProgramaAcademico.SelectedValue = ddlAProgramaAcademico.Items.FindByText(dt.Rows[0]["Programa"].ToString()).Value;
                //ddlAPeriodoCursado.SelectedValue = ddlAPeriodoCursado.Items.FindByText(dt.Rows[0]["Periodo_cursado"].ToString()).Value;
                txtAFunciones.Text = dt.Rows[0]["Becario_funciones"].ToString();
                hdfid_alumno.Value = dt.Rows[0]["id_consecutivo"].ToString();
            }
            else
            {
                verModal("Alerta", "No se puede relacionar la información");
            }
        }

        public void  llenarNivelAcademico(DropDownList lis)
        {
            query = @"select id_nivel_academico,Nivel_academico from cat_nivel_academico";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                lis.DataTextField = "Nivel_academico";
                lis.DataValueField = "id_nivel_academico";
                lis.DataSource = dt;
                lis.DataBind();
            }
        }

        protected void ddlAnivelEstudios_DataBound(object sender, EventArgs e)
        {
            ddlAnivelEstudios.Items.Insert(0, new ListItem("--Seleccione --", ""));
            ddlAnivelEstudios.Items.Insert(1, new ListItem("N/A", "0"));
        }

        public void llenarProgramaAcademico(DropDownList lis)
        {

            query = "select id_programa_academico , Nombre_programa_academico from cat_programa_acedemico";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                lis.DataTextField = "Nombre_programa_academico";
                lis.DataValueField = "id_programa_academico";
                lis.DataSource = dt;
                lis.DataBind();
            
            }
        }
        


        protected void ddlAProgramaAcademico_DataBound(object sender, EventArgs e)
        {
            ddlAProgramaAcademico.Items.Insert(0, new ListItem("--Seleccione --", ""));
            ddlAProgramaAcademico.Items.Insert(1, new ListItem("N/A", "0"));
        }

        protected void txtAmatricula_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string matricula = txtAmatricula.Text.ToUpper();
                if (!matricula.Contains("A"))
                {
                    matricula = "A" + matricula;
                    txtAmatricula.Text = matricula;
                }
                else
                {
                    txtAmatricula.Text = matricula.ToUpper();
                }
                sacarNombreAlumno(txtAmatricula.Text);


            }catch(Exception es )
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void sacarNombreAlumno(string matricula)
        {
            lblNombre.Text = "";
            query = "select Nombre +' ' + Apellido_paterno +' '+ Apellido_materno As Dato from tbl_alumnos where Matricula='" + matricula + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                lblNombre.Text = dt.Rows[0]["Dato"].ToString();
                lblAlumnoPorProyecto.Text = dt.Rows[0]["Dato"].ToString();
            }
            else
            {
                verModal("Alerta", "Lo sentimos pero no se encontró el alumno");
            }
        }

        protected void ddlAPeriodoCursado_DataBound(object sender, EventArgs e)
        {
            ddlAPeriodoCursado.Items.Insert(0, new ListItem("--Seleccione --", ""));
            ddlAPeriodoCursado.Items.Insert(1, new ListItem("N/A", "0"));
        }

        protected void btnAActualizarInfor_Click(object sender, EventArgs e)
        {
            try
            {

                string prueba = "";
                query = "sp_actualiza_tablero_becarios  " + hdfid_alumno.Value + " ,'" + txtAmatricula.Text + "','" + ddlAnivelEstudios.SelectedItem.Text + "', '" + ddlAProgramaAcademico.SelectedItem.Text + "','" + ddlAPeriodoCursado.SelectedItem.Text + "','" + txtAFunciones.Text + "','" + chkDeacuerdo.Checked + "'";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                    {
                        query = "sp_historial_cambio_solicitud  '" + Session["Usuario"].ToString() + "', " + hdfid_alumno.Value + "";
                        db.getQuery(conexionBecarios,query);
                        verModal("Exito", "Se actualizo la información del alumno");
                    }
                }
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void ddlNivelEstudosPorproyecto_DataBound(object sender, EventArgs e)
        {
            ddlNivelEstudosPorproyecto.Items.Insert(0, new ListItem("--Seleccione --", ""));
            ddlNivelEstudosPorproyecto.Items.Insert(1, new ListItem("N/A", "0"));
        }

        protected void ddlprogramaAcademicoPorProyecto_DataBound(object sender, EventArgs e)
        {
            ddlprogramaAcademicoPorProyecto.Items.Insert(0, new ListItem("--Seleccione --", ""));
            ddlprogramaAcademicoPorProyecto.Items.Insert(1, new ListItem("N/A", "0"));
        }

        protected void ddlPeriodoCursadoPorproyecto_DataBound(object sender, EventArgs e)
        {
            ddlPeriodoCursadoPorproyecto.Items.Insert(0, new ListItem("--Seleccione --", ""));
            ddlPeriodoCursadoPorproyecto.Items.Insert(1, new ListItem("N/A", "0"));
        }

        protected void txtMatriculaPorproyecto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string matricula = txtMatriculaPorproyecto.Text.ToUpper();
                if (!matricula.Contains("A"))
                {
                    matricula = "A" + matricula;
                    txtAmatricula.Text = matricula;
                }
                else
                {
                    txtMatriculaPorproyecto.Text = matricula.ToUpper();
                }
                sacarNombreAlumno(txtMatriculaPorproyecto.Text);


            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void llenarCamposSolicitudPorProyecto(string id)
        {
            query = "sp_saca_informacion_alumno " + id + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                txtMatriculaPorproyecto.Text = dt.Rows[0]["Matricula"].ToString();
                lblAlumnoPorProyecto.Text = dt.Rows[0]["NombreAlumno"].ToString();
                ddlNivelEstudosPorproyecto.SelectedValue = ddlNivelEstudosPorproyecto.Items.FindByText(dt.Rows[0]["Nivel_academico"].ToString()).Value;
                ddlprogramaAcademicoPorProyecto.SelectedValue = ddlprogramaAcademicoPorProyecto.Items.FindByText(dt.Rows[0]["Programa"].ToString()).Value;
                //ddlPeriodoCursadoPorproyecto.SelectedValue = ddlPeriodoCursadoPorproyecto.Items.FindByText(dt.Rows[0]["Periodo_cursado"].ToString()).Value;
                txtFuncionesPorProyecto.Text = dt.Rows[0]["Becario_funciones"].ToString();
                hdfid_alumno.Value = dt.Rows[0]["id_consecutivo"].ToString();
            }
            else
            {
                verModal("Alerta", "No se puede relacionar la información");
            }
        }

        protected void btnActualizarAlumnoPorProyecto_Click(object sender, EventArgs e)
        {
            try {
                //Asegir programando despue de  junta
                query = "sp_actualiza_tablero_becarios  " + hdfid_alumno.Value + " ,'" + txtMatriculaPorproyecto.Text + "','" + ddlNivelEstudosPorproyecto.SelectedItem.Text + "', '" + ddlprogramaAcademicoPorProyecto.SelectedItem.Text + "','" + ddlPeriodoCursadoPorproyecto.SelectedItem.Text + "','" + txtFuncionesPorProyecto.Text + "','" + chkDeacuerdo.Checked + "'";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                    {
                        query = "sp_historial_cambio_solicitud  '" + Session["Usuario"].ToString() + "', " + hdfid_alumno.Value + "";
                        db.getQuery(conexionBecarios,query);
                        verModal("Exito", "Se actualizo la información del alumno");
                    }
                }
            }catch(Exception es){
                verModal("Error",es.Message.ToString());
            }
        }

        protected void btnporProyectoEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton boton = (ImageButton)sender;
                string id = boton.CommandArgument;
                query = "sp_elimina_alumno_solicitud " + id + "";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                    {
                        verModal("Exito", "La información se eliminó correctamente");
                    }
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btncancelar_Click(object sender, EventArgs e)
        {
            
            pnlActualizarEspecifica.Visible = false;
            pnlActualizarPorProyecto.Visible = false;
            btncancelar.Visible = false;
            
        }

        protected void btncancelarproyecto_Click(object sender, EventArgs e)
        {
            try
            {
                pnlActualizarProyectoBecarios.Visible = false;
                btncancelarproyecto.Visible = false;
                btncancelar.Visible = false;
                btnActualizarAlumnoPorProyecto.Visible = false;
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

    }
}