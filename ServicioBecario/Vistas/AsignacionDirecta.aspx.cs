using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using ServicioBecario.Codigo;
using System.Net.Mail;

namespace ServicioBecario.Vistas
{
    public partial class AsignacionDirecta : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        Correo msj = new Correo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                llenarPeriodo();
            }
        }

        public void llenarPeriodo()
        {
            query = "select Descripcion,Periodo from cat_periodos where Activo=1";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
        }

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione--", ""));
        }

        protected void txtMatricula_TextChanged(object sender, EventArgs e)
        {
            string matricula = txtMatricula.Text.ToUpper();
            if (!string.IsNullOrEmpty(matricula))
            {
                if (!matricula.Contains("A"))
                {
                    matricula = "A" + matricula;
                    txtMatricula.Text = matricula;
                }
                else
                {
                    txtMatricula.Text = matricula.ToUpper();
                }
                sacarNombreAlumno(txtMatricula.Text.Trim());
                regresoDefault();
            }
            else
            {
                btnverAsignacion.Visible = false;
                regresoDefault();
                lblMatricula.Text = "";
            }
            
        }

        public void sacarNombreAlumno(string matricula)
        {
            if(ddlPeriodo.SelectedValue!="")
            {
                lblMatricula.Text = "";
                if(txtMatricula.Text!="")
                {
                    query = "select Nombre +' ' + Apellido_paterno +' '+ Apellido_materno As Dato from tbl_alumnos where Matricula='" + matricula + "' and periodo='" + ddlPeriodo.SelectedValue + "'";
                    dt = db.getQuery(conexionBecarios, query);
                    if (dt.Rows.Count > 0)
                    {
                        lblMatricula.Text = dt.Rows[0]["Dato"].ToString();
                        btnverAsignacion.Visible = true;
                    }
                    else
                    {
                        verModal("Alerta", "Lo sentimos pero no se encontró el alumno en el periodo " + ddlPeriodo.SelectedItem.Text + "");
                        btnverAsignacion.Visible = false;
                    }
                }
                else
                {
                    btnverAsignacion.Visible = false;
                }
                
            }
            else
            {
                verModal("Alerta","No has seleccionado el periodo");
            }
            
        }


        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void btnverAsignacion_Click(object sender, EventArgs e)
        {

            try
            {
                    query = "sp_mostrar_solicitante_de_la_matricula '" + txtMatricula.Text.Trim() + "',"+ddlPeriodo.SelectedValue+"";
                    dt = db.getQuery(conexionBecarios, query);                             
                    if (dt.Rows.Count > 0)//Berificamos que tenga una asignacion
                    {
                        /*Mostrar los datos  */
                        pnlConAsignacion.Visible = true;
                        lblAsignadoNomina.Text = dt.Rows[0]["Nomina"].ToString();
                        lblAsignadoSolicitante.Text = dt.Rows[0]["Nombre"].ToString();
                        lblAsignadopueto.Text = dt.Rows[0]["Puesto"].ToString();
                        lblAsignadoDepartamento.Text = dt.Rows[0]["Departamento"].ToString();
                        lblAsignadoUbicacionFisica.Text = dt.Rows[0]["Ubicacion_fisica"].ToString();
                        lblAsignadoproyecto.Text = dt.Rows[0]["Proyecto"].ToString();

                    }
                    else
                    {
                        /*********Aqui debemos de establecer este bloque es para cuando no tiene asignacion*/
                        pnlAsignar.Visible = true;
                        pnlConAsignacion.Visible = false;
                    }         
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btnDesasignar_Click(object sender, EventArgs e)
        {
            try
            {
                desasignar();
                limpiarComponentesDesasignacion();
                pnlConAsignacion.Visible = false;
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        public void desasignar()
        {
            string consecutivo;
            query = "sp_guarda_des_asignacion '" + txtMatricula.Text.Trim() + "','" + ddlPeriodo.SelectedItem.Text + "','" + txtDJustificacion.Text + "','" + Session["Usuario"].ToString() + "','" + lblAsignadoNomina.Text + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    //Necesitamos enviar correos de desasociacion pero al solicitante
                    query = "sp_toma_aviso_designacion '" + txtMatricula.Text.Trim() + "','" + lblAsignadoNomina.Text + "'," + dt.Rows[0]["id_consecutivo"].ToString() + ",'"+ddlPeriodo.SelectedItem.Text+"' ";
                    consecutivo = dt.Rows[0]["id_consecutivo"].ToString();
                    dt = db.getQuery(conexionBecarios,query);
                    if(dt.Rows.Count>0)//validamos que tenga informacion el correo
                    {
                        if(mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(),dt.Rows[0]["Asunto"].ToString(),dt.Rows[0]["CorreoSolicitante"].ToString()))
                        {
                            //mandamos un correo al alumo becario
                            query = "sp_guarda_des_asignacion_al_becario '" + txtMatricula.Text.Trim() + "','" + lblAsignadoNomina.Text + "'," + consecutivo + ",'"+ddlPeriodo.SelectedItem.Text+"'";
                            dt = db.getQuery(conexionBecarios,query);
                            if(dt.Rows.Count>0)
                            {
                                if (mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(), dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["CorreoAlumno"].ToString()))
                                {
                                    pnlAsignar.Visible = true;
                                    verModal("Éxito", "La información se guardó con éxito y se mandara un correo de aviso");
                                    
                                }
                            }
                            
                        }
                    }                 
                    
                }
            }
        }


        public void limpiarComponentesDesasignacion()
        {
            lblAsignadoproyecto.Text = "";
            lblAsignadoNomina.Text = "";
            lblAsignadoSolicitante.Text = "";
            lblAsignadopueto.Text = "";
            lblAsignadoDepartamento.Text = "";
            lblAsignadoUbicacionFisica.Text = "";
            txtDJustificacion.Text = "";

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


        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtfuncionesBecario.Text!="")
                {
                    //Asignamos la nomina con la matricula
                    if (txtProyecto.Text != "")
                    {
                        query = "sp_asignar_nomina_matricula_directa '" + txtMatricula.Text + "','" + txtAsignarNomina.Text + "'," + ddlPeriodo.SelectedValue + ",'" + txtProyecto.Text + "'";
                    }
                    else
                    {
                        query = "sp_asignar_nomina_matricula_directa '" + txtMatricula.Text + "','" + txtAsignarNomina.Text + "'," + ddlPeriodo.SelectedValue + ",null";
                    }

                    dt = db.getQuery(conexionBecarios, query);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                        {
                            //Tomamos el cuerpo y asunto del corro
                            query = "sp_mostrar_cuerpo_asignacion_directa_solicitante  '" + txtAsignarNomina.Text.Trim() + "','" + ddlPeriodo.SelectedItem.Text + "','" + txtMatricula.Text.Trim() + "'";
                            dt = db.getQuery(conexionBecarios, query);
                            if (dt.Rows.Count > 0)
                            {
                                //Manda correo a los solicitantes diciendo su asignacion
                                if (mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(), dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["Correo"].ToString()))
                                {

                                    query = "sp_mostrar_cuerpo_asignacion_directa_becario '" + txtAsignarNomina.Text + "','" + ddlPeriodo.SelectedItem.Text + "','" + txtMatricula.Text + "'";
                                    dt = db.getQuery(conexionBecarios, query);
                                    if (dt.Rows.Count > 0)
                                    {
                                        if (mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(), dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["Correo"].ToString()))
                                        {
                                            verModal("Éxito", "Se asigno la matrícula: " + txtMatricula.Text + " a la nómina: " + txtAsignarNomina.Text);
                                            //Limpiamos los comtroles                                                
                                            limpiarDatos();


                                            pnlDatoSolicitante.Visible = false;
                                            //Limpiamos los controles donde se encuetra la nomina y el proyecto 
                                            limpiarproyects();

                                            pnlAsignar.Visible = false;
                                            limpiarDatosPrimeros();
                                        }
                                    }
                                    //mandar correo al alumno becario de su asignacion                                
                                }
                            }



                        }
                    }
                }    
                else
                {
                    verModal("Alerta","No has agregadó las funciones del becario");
                }

                

            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
           

        }

        public void regresoDefault()
        {
            lblAsignadoproyecto.Text = "";
            lblAsignadoproyecto.Text = "";
            lblAsignadoNomina.Text = "";
            lblAsignadoSolicitante.Text = "";
            lblAsignadopueto.Text = "";
            lblAsignadoDepartamento.Text = "";
            lblAsignadoUbicacionFisica.Text = "";
            txtfuncionesBecario.Text = "";
            pnlConAsignacion.Visible = false;
            pnlAsignar.Visible = false;
            txtAsignarNomina.Text = "";
            txtProyecto.Text = "";
        }

        protected void btnVerficanombreAsigador_Click(object sender, EventArgs e)
        {

            DataTable dtr;
           
            //Mandamos llamar el ws para inserte la nomina dentro
            
            //dt = db.infoEmpleados(txtAsignarNomina.Text);
            //query = "sp_inserta_nomina_directa '" + dt.Rows[0]["Nomina"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Nombres"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Apaterno"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Amaterno"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Correo"].ToString().Trim() + "','" + dt.Rows[0]["Divicion"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["UFisica"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Puesto"].ToString().Trim() + "','" + dt.Rows[0]["Campus"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Extencion"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Estatus"].ToString().Trim() + "','" + dt.Rows[0]["Departamento"].ToString().Trim() + "'," + dt.Rows[0]["Grupo"].ToString().Trim() + "," + dt.Rows[0]["Area"].ToString().Trim() + "";
            //dt = db.getQuery(conexionBecarios,query);
            //if(dt.Rows.Count>0)
            //{
            //    query = "sp_muestra_informacion_nomina '" + txtAsignarNomina.Text + "'";
            //    dt = db.getQuery(conexionBecarios,query);
            //    if(dt.Rows.Count>0)
            //    {
            //        //Mostramos la informacion del solicitante
            //        pnlDatoSolicitante.Visible = true;

            //        lblNominaAsignar.Text = dt.Rows[0]["Nomina"].ToString();
            //        lblNombreSolicitanteAsignar.Text = dt.Rows[0]["NombreCompleto"].ToString();
            //        lblPuestoAsignar.Text = dt.Rows[0]["Puesto"].ToString();
            //        lblDepartamentoAsignar.Text = dt.Rows[0]["Departamento"].ToString();
            //        lbUbicacionFisicaAsignar.Text = dt.Rows[0]["Ubicacion_fisica"].ToString();
            //    }
            //}
            /*********************Esto es lo nuevo********************************/
            try
            {
                //dt = db.AgregarNomina(txtAsignarNomina.Text.Trim(), conexionBecarios);
                //if (dt.Rows.Count > 0)
                //{
                //    pnlDatoSolicitante.Visible = true;

                //    lblNominaAsignar.Text = db.formatoEscritura(dt.Rows[0]["Nomina"].ToString());
                //    lblNombreSolicitanteAsignar.Text = db.formatoEscritura( dt.Rows[0]["NombreCompleto"].ToString());
                //    lblPuestoAsignar.Text =db.formatoMayusculaInicial( dt.Rows[0]["Puesto"].ToString());
                //    lblDepartamentoAsignar.Text =db.formatoMayusculaInicial( dt.Rows[0]["Departamento"].ToString());
                //    lbUbicacionFisicaAsignar.Text =db.formatoMayusculaInicial( dt.Rows[0]["UFisica"].ToString());
                //}
                //else
                //{
                //    verModal("Alerta", "No existe la nómina");
                //}
                string ambiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                int campus = 0;
                query = "sp_saca_informacion_del_empleado '" + txtAsignarNomina.Text + "'";
                dt = db.getQuery(conexionBecarios,query);
                if (dt.Rows[0]["Mensaje"].ToString() != "No esta")
                {
                    
                    lblNominaAsignar.Text = db.formatoEscritura(dt.Rows[0]["Nomina"].ToString());
                    lblNombreSolicitanteAsignar.Text = db.formatoEscritura( dt.Rows[0]["NombreEmpleado"].ToString());
                    lblPuestoAsignar.Text =db.formatoMayusculaInicial( dt.Rows[0]["Puesto"].ToString());
                    lblDepartamentoAsignar.Text =db.formatoMayusculaInicial( dt.Rows[0]["Departamento"].ToString());
                    lbUbicacionFisicaAsignar.Text =db.formatoMayusculaInicial( dt.Rows[0]["Ubicacion_fisica"].ToString());
                    pnlDatoSolicitante.Visible = true;
                }
                else
                {
                    switch(ambiente)
                    {
                        case "pprd"://Para prueba
                            dtr = db.infoEmpleados(txtAsignarNomina.Text.Trim());
                            if (dtr.Rows[0]["Nomina"].ToString() != "Nada")
                            {
                                campus = int.Parse(dtr.Rows[0]["Campus"].ToString().Trim());
                                query = "sp_inserta_nomina_directa_nuevo '" + db.formatoEscritura(dtr.Rows[0]["Nomina"].ToString()) + "','" + db.formatoEscritura(dtr.Rows[0]["Nombres"].ToString()) + "','" + db.formatoEscritura(dtr.Rows[0]["Apaterno"].ToString()) + "','" + db.formatoEscritura(dtr.Rows[0]["Amaterno"].ToString()) + "','" + dtr.Rows[0]["Correo"].ToString().Trim() + "','" + db.formatoEscritura(dtr.Rows[0]["Divicion"].ToString()) + "','" + dtr.Rows[0]["UFisica"].ToString() + "','" + db.formatoEscritura(dtr.Rows[0]["Puesto"].ToString()) + "','" + campus + "','" + dtr.Rows[0]["Extencion"].ToString() + "','" + dtr.Rows[0]["Estatus"].ToString().Trim() + "','" + db.formatoEscritura(dtr.Rows[0]["Departamento"].ToString()) + "'," + dtr.Rows[0]["Grupo"].ToString().Trim() + "," + dtr.Rows[0]["Area"].ToString().Trim() + "";
                                dt = db.getQuery(conexionBecarios, query);
                                if (dt.Rows.Count > 0)
                                {
                                    string mensaje = dt.Rows[0]["Mensaje"].ToString();
                                    switch(dt.Rows[0]["Mensaje"].ToString())
                                     {
                                         case "Ok":
                                            query = "sp_saca_informacion_del_empleado '" + txtAsignarNomina.Text + "'";
                                            dt= db.getQuery(conexionBecarios,query);
                                            if(dt.Rows.Count>0)
                                            {
                                                lblNominaAsignar.Text = db.formatoEscritura(dt.Rows[0]["Nomina"].ToString());
                                                lblNombreSolicitanteAsignar.Text = db.formatoEscritura(dt.Rows[0]["NombreEmpleado"].ToString());
                                                lblPuestoAsignar.Text = db.formatoMayusculaInicial(dt.Rows[0]["Puesto"].ToString());
                                                lblDepartamentoAsignar.Text = db.formatoMayusculaInicial(dt.Rows[0]["Departamento"].ToString());
                                                lbUbicacionFisicaAsignar.Text = db.formatoMayusculaInicial(dt.Rows[0]["Ubicacion_fisica"].ToString());
                                                pnlDatoSolicitante.Visible = true;

                                            }
                                            else
                                            {
                                                verModal("Alerta", "La nómina no se encontro");
                                            }

                                             break;

                                         case "Existe":
                                             break;


                                         default:
                                             verModal("Alerta",mensaje);
                                             break;
                                     }
                                }
                            }
                            break;
                        case "prod":
                            dtr = db.informacionEmpleadosProduccion(txtAsignarNomina.Text.Trim());
                            if (dtr.Rows[0]["Nomina"].ToString() != "Nada")
                            {
                                campus = int.Parse(dtr.Rows[0]["Campus"].ToString().Trim());
                                query = "sp_inserta_nomina_directa_nuevo '" + db.formatoEscritura(dtr.Rows[0]["Nomina"].ToString()) + "','" + db.formatoEscritura(dtr.Rows[0]["Nombres"].ToString()) + "','" + db.formatoEscritura(dtr.Rows[0]["Apaterno"].ToString()) + "','" + db.formatoEscritura(dtr.Rows[0]["Amaterno"].ToString()) + "','" + dtr.Rows[0]["Correo"].ToString().Trim() + "','" + db.formatoEscritura(dtr.Rows[0]["Divicion"].ToString()) + "','" + dtr.Rows[0]["UFisica"].ToString() + "','" + db.formatoEscritura(dtr.Rows[0]["Puesto"].ToString()) + "','" + campus + "','" + dtr.Rows[0]["Extencion"].ToString() + "','" + dtr.Rows[0]["Estatus"].ToString().Trim() + "','" + db.formatoEscritura(dtr.Rows[0]["Departamento"].ToString()) + "'," + dtr.Rows[0]["Grupo"].ToString().Trim() + "," + dtr.Rows[0]["Area"].ToString().Trim() + "";
                                dt = db.getQuery(conexionBecarios, query);
                                if (dt.Rows.Count > 0)
                                {
                                    string mensaje = dt.Rows[0]["Mensaje"].ToString();
                                    switch(dt.Rows[0]["Mensaje"].ToString())
                                     {
                                         case "Ok":
                                            query = "sp_saca_informacion_del_empleado '" + txtAsignarNomina.Text + "'";
                                            dt= db.getQuery(conexionBecarios,query);
                                            if(dt.Rows.Count>0)
                                            {
                                                lblNominaAsignar.Text = db.formatoEscritura(dt.Rows[0]["Nomina"].ToString());
                                                lblNombreSolicitanteAsignar.Text = db.formatoEscritura(dt.Rows[0]["NombreEmpleado"].ToString());
                                                lblPuestoAsignar.Text = db.formatoMayusculaInicial(dt.Rows[0]["Puesto"].ToString());
                                                lblDepartamentoAsignar.Text = db.formatoMayusculaInicial(dt.Rows[0]["Departamento"].ToString());
                                                lbUbicacionFisicaAsignar.Text = db.formatoMayusculaInicial(dt.Rows[0]["Ubicacion_fisica"].ToString());
                                                pnlDatoSolicitante.Visible = true;

                                            }
                                            else
                                            {
                                                verModal("Alerta", "La nómina no se encontro");
                                            }

                                             break;

                                         case "Existe":

                                             break;


                                         default:
                                             verModal("Alerta",mensaje);
                                             break;
                                     }
                                }
                            }
                            break;

                    }
                   
                }

            
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
            















            //query = "sp_muestra_informacion_nomina '" + txtAsignarNomina.Text + "'";
            //dt = db.getQuery(conexionBecarios, query);
            //if(dt.Rows.Count>0)
            //{
            //    //Mostramos la informacion del solicitante
            //    pnlDatoSolicitante.Visible = true;

            //    lblNominaAsignar.Text = dt.Rows[0]["Nomina"].ToString();
            //    lblNombreSolicitanteAsignar.Text = dt.Rows[0]["NombreCompleto"].ToString();
            //    lblPuestoAsignar.Text = dt.Rows[0]["Puesto"].ToString();
            //    lblDepartamentoAsignar.Text = dt.Rows[0]["Departamento"].ToString();
            //    lbUbicacionFisicaAsignar.Text = dt.Rows[0]["Ubicacion_fisica"].ToString();
            //}
            //else
            //{
            //    dt = db.infoEmpleados(txtAsignarNomina.Text);
            //    query = "sp_inserta_nomina_directa '" + dt.Rows[0]["Nomina"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Nombres"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Apaterno"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Amaterno"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Correo"].ToString().Trim() + "','" + dt.Rows[0]["Divicion"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["UFisica"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Puesto"].ToString().Trim() + "','" + dt.Rows[0]["Campus"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Extencion"].ToString().Trim().ToUpper() + "','" + dt.Rows[0]["Estatus"].ToString().Trim() + "','" + dt.Rows[0]["Departamento"].ToString().Trim() + "'," + dt.Rows[0]["Grupo"].ToString().Trim() + "," + dt.Rows[0]["Area"].ToString().Trim() + "";
            //    dt = db.getQuery(conexionBecarios, query);
            //    query = "sp_muestra_informacion_nomina '" + txtAsignarNomina.Text + "'";
            //    dt = db.getQuery(conexionBecarios, query);
            //    if (dt.Rows.Count > 0)
            //    {
            //        //Mostramos la informacion del solicitante
            //        pnlDatoSolicitante.Visible = true;

            //        lblNominaAsignar.Text = dt.Rows[0]["Nomina"].ToString();
            //        lblNombreSolicitanteAsignar.Text = dt.Rows[0]["NombreCompleto"].ToString();
            //        lblPuestoAsignar.Text = dt.Rows[0]["Puesto"].ToString();
            //        lblDepartamentoAsignar.Text = dt.Rows[0]["Departamento"].ToString();
            //        lbUbicacionFisicaAsignar.Text = dt.Rows[0]["Ubicacion_fisica"].ToString();
            //    }

            //}

        }

        public void limpiarDatos()
        {
            lblNominaAsignar.Text = "";
            lblPuestoAsignar.Text = "";
            lblDepartamentoAsignar.Text = "";
            lbUbicacionFisicaAsignar.Text = "";
        }


        public void limpiarproyects()
        {
            txtAsignarNomina.Text = "";
            txtProyecto.Text = "";

        }


        public void limpiarDatosPrimeros()
        {
            txtMatricula.Text = "";
            lblMatricula.Text = "";
        }

        protected void chkProyecto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProyecto.Checked)
            {
                pnlProyecto.Visible = true;
            }
            else
            {
                pnlProyecto.Visible = false;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                regresoDefault();
                txtMatricula.Text = "";
                lblMatricula.Text = "";
                btnverAsignacion.Visible = false;
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void btnCan_Click(object sender, EventArgs e)
        {
            try
            {
                regresoDefault();
                txtMatricula.Text = "";
                lblMatricula.Text = "";
                btnverAsignacion.Visible = false;
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

    }
}