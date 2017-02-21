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
    public partial class CambioBecarioDdA : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();//global
        Correo msj = new Correo();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    llenarPeriodo();
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void llenarPeriodo()
        {
            query = "select Descripcion,Periodo from cat_periodos where Activo=1 ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
        }

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void btnAsignacion_Click(object sender, EventArgs e)
        {
            try
            {
                query = "sp_muestra_datos_asignacion_periodo  " + ddlPeriodo.SelectedValue + ",'" + txtMatricula.Text.Trim() + "'  ";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    lblNomina.Text = dt.Rows[0]["Nomina"].ToString();
                    lblNombreSolicitante.Text = dt.Rows[0]["Nombres"].ToString();
                    lblPuesto.Text = dt.Rows[0]["Puesto"].ToString();
                    lblCampus.Text = dt.Rows[0]["Campus"].ToString();
                    lblUbicacion.Text = dt.Rows[0]["Ubicacion_fisica"].ToString();
                    lblExtencion.Text = dt.Rows[0]["Extencion_telefonica"].ToString();
                    hdfId_miSolicitud.Value = dt.Rows[0]["id_Misolicitud"].ToString();
                    pnlVerAsignacion.Visible = true;
                    pnlMotovos.Visible= true;
                }
                else
                {
                    verModal("Alerta","La matrícula no esta asignada");
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void txtMatricula_TextChanged(object sender, EventArgs e)
        {
            try
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
                    sacarNombreAlumno(txtMatricula.Text);
                    btnAsignacion.Visible = true;
                }

            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }

        }

        public void sacarNombreAlumno(string matricula)
        {
            if(ddlPeriodo.SelectedValue!="")
            {
                lblNombreBecario.Text = "";
                query = "select Nombre +' ' + Apellido_paterno +' '+ Apellido_materno As Dato from tbl_alumnos where Periodo='" + ddlPeriodo.SelectedValue + "' and Matricula='" + matricula + "'";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    lblNombreBecario.Text = dt.Rows[0]["Dato"].ToString();
                }
                else
                {
                    verModal("Alerta", "Lo sentimos pero no se encontró el alumno en el periodo " + ddlPeriodo.SelectedItem.Text + "");
                }
            }
            else
            {
                verModal("Alerta","Seleccione un periodo");

            }
            
        }

        protected void btnDesasingar_Click(object sender, EventArgs e)
        {
            try
            {
                string consecutivo;
                //Des asignamos al becario
                //query = "sp_des_asigna_para_becario " + ddlPeriodo.SelectedValue + ",'" + txtMatricula.Text + "','" + Session["Usuario"].ToString()+"','"+txtJustificacion.Text+"'";
                query = "sp_guarda_des_asignacion '" + txtMatricula.Text.Trim() + "','" + ddlPeriodo.SelectedItem.Text + "','" + txtJustificacion.Text + "','" + Session["Usuario"].ToString() + "','" + lblNomina.Text + "'";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                    {
                        //Necesitamos enviar correos de desasociacion pero al solicitante
                        query = "sp_toma_aviso_designacion '" + txtMatricula.Text.Trim() + "','" + lblNomina.Text + "','" + dt.Rows[0]["id_consecutivo"].ToString() + "','" + ddlPeriodo.SelectedItem.Text + "'";
                        consecutivo = dt.Rows[0]["id_consecutivo"].ToString();
                        dt = db.getQuery(conexionBecarios, query);
                        if (dt.Rows.Count > 0)//validamos que tenga informacion el correo
                        {
                            if (mandarCorreo2(dt.Rows[0]["Cuerpo"].ToString(), dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["CorreoSolicitante"].ToString()))
                            {
                                //mandamos un correo al alumo becario
                                query = "sp_guarda_des_asignacion_al_becario '" + txtMatricula.Text.Trim() + "','" + lblNomina.Text + "'," + consecutivo + ",'" + ddlPeriodo.SelectedItem.Text + "'";
                                dt = db.getQuery(conexionBecarios, query);
                                if (dt.Rows.Count > 0)
                                {
                                    if (mandarCorreo2(dt.Rows[0]["Cuerpo"].ToString(), dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["CorreoAlumno"].ToString()))
                                    {

                                        verModal("Exito", "La desasignación se realizo correctamente");
                                        //Mostrar los datos necesarios para ver lo becarios no asignados
                                        poblacionDisponible();
                                        pnlBecariosNoAsignados.Visible = true;
                                        txtJustificacion.Text = "";
                                    }
                                }
                            }
                        }


                    }
                    else
                    {
                        verModal("Alerta", "Sucesido un error en la base de de datos");
                    }

                    //limpiaComponentes();
                    pnlVerAsignacion.Visible = true;
                    pnlMotovos.Visible = false;
                    
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void poblacionDisponible()
        {
            query = "sp_muestra_becarios_disponibles '" + lblNomina.Text + "'," + ddlPeriodo.SelectedValue + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                gvDatos.DataSource = dt;
                gvDatos.DataBind();
            }
        }

        public void limpiaComponentes()
        {
            lblNomina.Text = "";
            lblNombreSolicitante.Text = "";
            lblPuesto.Text = "";
            lblCampus.Text = "";
            lblUbicacion.Text = "";
            lblExtencion.Text = "";
        }

        public bool mandarCorreo(string cuerpo, string asunto, string correo)
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


        public bool mandarCorreo2(string cuerpo, string asunto, string correo)
        {
            
                 
            //bool bandera = false;
            //System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            //msg.To.Add(correo);//Destinatario
            //msg.From = new MailAddress("servicio.becario@itesm.mx", "Servicio becario", System.Text.Encoding.UTF8);//Emisor y nombre de usuario
            //msg.Subject = asunto;//Asunto del mensaje
            //msg.SubjectEncoding = System.Text.Encoding.UTF8;
            //msg.Body = cuerpo;//Mensaje a ser enviado
            //msg.BodyEncoding = System.Text.Encoding.UTF8;
            //msg.IsBodyHtml = true;

            //SmtpClient client = new SmtpClient();
            //client.Credentials = new System.Net.NetworkCredential(DatosCorreo.CorreoServicio, DatosCorreo.PasswordServicio);
            ////hotmail
            //client.Port = 587; // ùerto de envio tanto de Hotmail como para Gmail
            //client.Host = "smtp.office365.com";// Protocolo Simple de Transferencia de Correo de (Hotmail)
            //// 
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;            

            //client.EnableSsl = true;
            //try
            //{
            //    client.Send(msg);//Enviamos el mensaje
            //    //                MessageBox.Show("Mensaje Enviado Correctamente", "Correo C#", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    //              sw = false;
            //    bandera = true;
            //}
            //catch (System.Net.Mail.SmtpException e)
            //{
            //    string es = e.Source.ToString();
            //    // MessageBox.Show("Error");
            //}

            //return bandera;

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
            }


            return bandera;




        }

        protected void chkselecioname_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string decide = hdfDecide.Value;
                CheckBox chk = (CheckBox)sender;
                string id_alumno = chk.ToolTip;
                
                if (decide == "true")
                {
                    query = "sp_guarda_asignacion_cambio_becario " + id_alumno + "," + hdfId_miSolicitud.Value + "," + ddlPeriodo.SelectedValue + "";
                    dt = db.getQuery(conexionBecarios, query);
                    if (dt.Rows.Count > 0)
                    {
                        string resultado = dt.Rows[0]["Mensaje"].ToString();
                        if (resultado == "Ok")
                        {
                           
                            //Envio de asingacion al solicitante
                            query = "sp_mostrar_cuerpo_asignacion_directa_solicitante  '" + lblNomina.Text + "','" + ddlPeriodo.SelectedItem.Text + "','" + id_alumno + "'";

                            dt = db.getQuery(conexionBecarios,query);
                            if(dt.Rows.Count>0)
                            {
                                //mandamos el correo de asignación al solicigtnate
                                if (mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(), dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["Correo"].ToString()))
                                {
                                    //Mandamos el cuerp de asignacion al alumno
                                    query = "sp_mostrar_cuerpo_asignacion_directa_becario '" + lblNomina.Text + "','" + ddlPeriodo.SelectedItem.Text + "','" + id_alumno + "'";
                                    dt = db.getQuery(conexionBecarios,query);
                                    if(dt.Rows.Count>0)
                                    {
                                        if (mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(), dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["Correo"].ToString()))
                                        {

                                        }
                                    }
                                }
                            }
                            //ennvio de correo de asignacion al alumno
                            verModal("Éxito", "El becario fue asignadó");
                            gvDatos.DataSource = null;
                            gvDatos.DataBind();
                            limpiaComponentes();
                            pnlVerAsignacion.Visible = false;
                            txtMatricula.Text = "";
                            lblNombreBecario.Text = "";
                            
                        }
                        else
                        {
                            verModal("Alerta", "El becario ya se encuentra con otra asignación");
                        }
                    }
                }
                else
                {
                    chk.Checked = false;
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btnCance_Click(object sender, EventArgs e)
        {
            try
            {
                regresarInicio();
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        public void regresarInicio()
        {
            pnlMotovos.Visible = false;
            pnlVerAsignacion.Visible = false;
            txtMatricula.Text = "";
            lblNombreBecario.Text = "";
            btnAsignacion.Visible = false;
            lblNomina.Text = "";
            lblNombreSolicitante.Text = "";
            lblPuesto.Text = "";
            lblCampus.Text = "";
            lblUbicacion.Text = "";
            lblExtencion.Text = "";
            txtJustificacion.Text = "";

        }
    }


    

}