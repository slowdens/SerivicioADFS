using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using ServicioBecario.Codigo;
using System.Net.Mail;
namespace ServicioBecario.Vistas
{
    public partial class CambioSolicitanteDdA : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();//global
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
            query = "select Descripcion,Periodo from cat_periodos ";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
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
                sacarNombreAlumno(txtMatricula.Text);
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
            }
            else
            {
                verModal("Alerta", "Lo sentimos pero no se encontró el alumno");
            }
        }

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void btnVerasignacion_Click(object sender, EventArgs e)
        {
            try
            {
                query = "sp_muestra_datos_asignacion_periodo  "+ddlPeriodo.SelectedValue+",'"+txtMatricula.Text.Trim()+"'  ";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    lblNomina.Text = dt.Rows[0]["Nomina"].ToString();
                    lblNombreSolicitante.Text = dt.Rows[0]["Nombres"].ToString();
                    lblPuesto.Text = dt.Rows[0]["Puesto"].ToString();
                    lblCampus.Text = dt.Rows[0]["Campus"].ToString();
                    lblubucacion.Text = dt.Rows[0]["Ubicacion_fisica"].ToString();
                    lblExtencion.Text = dt.Rows[0]["Extencion_telefonica"].ToString();
                    pnlAsignacion.Visible = true;
                }
                else
                {
                    verModal("Alerta","La matrícula: "+txtMatricula.Text+" no se encontro con asignación");
                }
            }catch(Exception es){
                verModal("Error",es.Message.ToString());
            }
            
        }

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void btnDesAsignacion_Click(object sender, EventArgs e)
        {
            try
            {
                string consecutivo;
                //Des asignamos al becario
                //query = "sp_des_asigna_para_becario " + ddlPeriodo.SelectedValue + ",'" + txtMatricula.Text + "','" + Session["Usuario"].ToString()+"','"+txtJustificacion.Text+"'";
                query = "sp_guarda_des_asignacion '" + txtMatricula.Text.Trim() + "','" + ddlPeriodo.SelectedItem.Text + "','" + txtJustificacion.Text + "','" + Session["Usuario"].ToString() + "','"+lblNomina.Text+"'";
                dt = db.getQuery(conexionBecarios,query);
                if(dt.Rows.Count>0)
                {
                    if(dt.Rows[0]["Mensaje"].ToString()=="Ok")
                    {
                         //Necesitamos enviar correos de desasociacion pero al solicitante
                        query = "sp_toma_aviso_designacion '" + txtMatricula.Text.Trim() + "','" + lblNomina.Text + "','" + dt.Rows[0]["id_consecutivo"].ToString() +"','"+ddlPeriodo.SelectedItem.Text+"'";
                        consecutivo = dt.Rows[0]["id_consecutivo"].ToString();
                        dt = db.getQuery(conexionBecarios, query);
                        if (dt.Rows.Count > 0)//validamos que tenga informacion el correo
                        {
                            if(mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(),dt.Rows[0]["Asunto"].ToString(),dt.Rows[0]["CorreoSolicitante"].ToString()))
                            {
                                //mandamos un correo al alumo becario
                                query = "sp_guarda_des_asignacion_al_becario '" + txtMatricula.Text.Trim() + "','" + lblNomina.Text + "'," + consecutivo + ",'" + ddlPeriodo.SelectedItem.Text + "'";
                                dt = db.getQuery(conexionBecarios, query);
                                if (dt.Rows.Count > 0)
                                {
                                    if (mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(), dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["CorreoAlumno"].ToString()))
                                    {

                                        verModal("Exito", "La desasignación se realizo correctamente");
                                        //poner slo
                                        muestraSolicitantesDisponibles();
                                    }
                                }
                            }
                        }
                      
                        
                    }
                    else
                    {
                        verModal("Alerta","Sucesidó un error en la base de de datos");
                    }

                    limpiaComponentes();
                    pnlAsignacion.Visible = false;
                }
            }catch(Exception es )
            {
                verModal("Error",es.Message.ToString());
            }
        }


        public void muestraSolicitantesDisponibles()
        {
            query = "sp_muestra_solitantes_disponibles " + ddlPeriodo.SelectedValue + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                gvDatos.DataSource = dt;
                gvDatos.DataBind();
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






        public void limpiaComponentes()
        {
            lblNomina.Text = "";
            lblNombreSolicitante.Text = "";
            lblPuesto.Text = "";
            lblCampus.Text = "";
            lblubucacion.Text = "";
            lblExtencion.Text = "";
        }
        protected void chkselecioname_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string decide = hdfDecide.Value;
                CheckBox chk = (CheckBox)sender;
                if(decide == "true")
                {
                    string id_miAsignacion = chk.ToolTip;
                    query = "sp_guarda_asingacion_para_solicitante_guardos " + id_miAsignacion + "," + txtMatricula.Text + "," + ddlPeriodo.SelectedValue + "";
                    dt = db.getQuery(conexionBecarios,query);
                    if (dt.Rows.Count > 0)
                    {
                        if(dt.Rows[0]["Mensaje"].ToString()=="Ok")
                        {
                            verModal("Exito","Se asignó correctamente");
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
                verModal("Error",es.Message.ToString());
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDatos.PageIndex = e.NewPageIndex;
                muestraSolicitantesDisponibles();
                
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }
    }
}