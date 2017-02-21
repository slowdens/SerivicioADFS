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

namespace ReplicaAppServicioBecario.Vistas
{
    public partial class EnvioCorreoAsignacion : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        Correo msj = new Correo();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    vermisosdeCampus();
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
            query = "select Descripcion,Periodo from cat_periodos where Activo=1";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
        }

        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre as Campus from cat_campus order by nombre asc ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataTextField = "Campus";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                double porcentaje=0;
                pnlprogres.Visible = true;
                int cantidadCorrreos = 0;
                int cantidadDestinatarios = 0;
                int i = 0;
                //Verifico que el rol esta activado para que se asigne los datos al control de hidenfield
                if (hdfActivarRol.Value == "1")
                {
                    hdfidCampus.Value = ddlCampus.SelectedValue;
                }
                //Esto eso solamente para enviar informacion a todos los solicitantes
                //Fata que solo reconosca a los que aun no tiene el correo
                query = "sp_Correo_de_asignacion_sb_individual '" + ddlPeriodo.SelectedValue + "','" + hdfidCampus.Value + "' ";

                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    cantidadCorrreos +=int.Parse(dt.Rows[0]["CantidadCorros"].ToString());
                    cantidadDestinatarios += int.Parse(dt.Rows[0]["CantidadDestinatarios"].ToString());
                    if (cantidadCorrreos <= 10000 && cantidadDestinatarios<=500)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (mandarCorreo(dt.Rows[i]["Cuerpo"].ToString(), dt.Rows[i]["Asunto"].ToString(), dt.Rows[i]["Correo"].ToString()))
                            {
                                query = "sp_marcar_envio_asignacion_solicitante  " + dt.Rows[i]["id_MiSolicitud"].ToString() + "";
                                db.getQuery(conexionBecarios, query);
                            }
                            i++;
                        }
                    }
                    else
                    {
                        verModal("Alerta", "No se puede enviar los correos ya que sobre paso el limite de 10000 correos o 500 destinatarios");
                    }
                    
                }
                query = "sp_Correo_de_asignacion_sb_porProyecto_y_especial '" + ddlPeriodo.SelectedValue + "','" + hdfidCampus.Value + "' ";
                dt = db.getQuery(conexionBecarios, query);
                i = 0;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        if (mandarCorreo(dt.Rows[i]["Cuerpo"].ToString(), dt.Rows[i]["Asunto"].ToString(), dt.Rows[i]["Correo"].ToString()))
                        {
                            query = "sp_marcar_envio_asignacion_solicitante  " + dt.Rows[i]["id_MiSolicitud"].ToString() + "";
                            db.getQuery(conexionBecarios, query);
                        }
                        i++;
                    }
                }
                //Esta parte es para enviar información al alumno becario sobre su asignacion
                i = 0;
                query = "sp_Correo_de_asignacion_para_el_becario  '" + ddlPeriodo.SelectedValue + "','" + hdfidCampus.Value + "'";
                dt = db.getQuery(conexionBecarios, query);
               
                if (dt.Rows.Count > 0)
                {
                    int total = int.Parse(dt.Rows[i]["Total"].ToString());
                    
                    foreach (DataRow r in dt.Rows)
                    {
                        if (mandarCorreo(dt.Rows[i]["Cuerpo"].ToString(), dt.Rows[i]["Asunto"].ToString(), dt.Rows[i]["Correo"].ToString()))
                        {

                            query = "sp_marcar_envio_asignacion_becarios " + dt.Rows[i]["id_consecutivo"].ToString() + "";
                            db.getQuery(conexionBecarios, query);

                            i = i + 1;
                          
                            porcentaje = (double)(i * 100) / total;
                            
                            i = i - 1;
                            //Este metodo actualiza los datos en el progressbar 
                            //SetTheProgress(bar1, Math.Round(porcentaje) + "%");                                           
                            
                        }
                        i++;

                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "progs" + i, "progreso(" + Math.Round(porcentaje) + ");", true);
                    pnlprogres.Visible = true;
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
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

        public void vermisosdeCampus()
        {
            query = "Sp_muestra_perifil_campus '" + Session["Usuario"].ToString() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["id_rol"].ToString() == "4")//Este es administrador multicampus
                {
                    hdfActivarRol.Value = "1";
                    llenarCampus();
                }
                else
                {
                    lblCampus.Text = dt.Rows[0]["Campus"].ToString();
                    hdfidCampus.Value = dt.Rows[0]["Codigo_campus"].ToString();
                    ddlCampus.Visible = false;
                    lblCampus.Visible = true;
                    hdfActivarRol.Value = "0";
                }
            }
            else
            {
                verModal("Alerta", "El usuario no se encontró registrado");
            }
        }
        //public void SetTheProgress(HtmlGenericControl bar, string value)
        //{
        //    bar.Attributes.Add("style", string.Format("width:{0};", value));
        //    lblMensaje.Text = "Envio de correo " + value;
        //    bar.DataBind();
        //}



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


    }
}