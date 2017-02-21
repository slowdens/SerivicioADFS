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
using System.Web.UI.HtmlControls;
namespace ServicioBecario.Vistas
{
    public partial class EnvioCorreoEvaluacion : System.Web.UI.Page
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
                vermisosdeCampus();
                llenarPeriodo();
            }
        }

        public void llenarPeriodo()
        {
            query = "select Descripcion,Periodo from cat_periodos where Activo=1";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                ddlperiodo.DataValueField = "Periodo";
                ddlperiodo.DataTextField = "Descripcion";
                ddlperiodo.DataSource = dt;
                ddlperiodo.DataBind();
            }
        }

        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus order by nombre asc";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }

        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
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
        protected void ddlperiodo_DataBound(object sender, EventArgs e)
        {
            ddlperiodo.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }
        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void btnMandar_Click(object sender, EventArgs e)
        {
            try
            {
                int i =0;
                if(hdfActivarRol.Value=="1")
                {
                    hdfidCampus.Value = ddlCampus.SelectedValue;
                }
              //Validamos contra la fecha
                query = "sp_sacamos_fechas_de_evaluacion_por_campus " + ddlperiodo.SelectedValue + "," + hdfidCampus.Value + "";
                dt = db.getQuery(conexionBecarios,query);
                if(dt.Rows.Count>0)
                {
                    //Validamos que este dentro de la fechas de evaluacion
                    if(dt.Rows[0]["Valor"].ToString()=="Si")
                    {
                        
                        //Esta informacion es mandada al solicitante
                        query = "sp_Corroe_Recordatorio_evaluacion_solicitantes "+ddlperiodo.SelectedValue+","+hdfidCampus.Value+"";
                        dt = db.getQuery(conexionBecarios,query);
                        if (dt.Rows.Count > 0)
                        {
                            foreach(DataRow r in dt.Rows)
                            {
                                if(mandarCorreo(dt.Rows[i]["Cuerpo"].ToString(),dt.Rows[i]["Asunto"].ToString(),dt.Rows[i]["Correo"].ToString()))
                                {
                                    //Marcamos los correo que ya fueron enviados
                                    query = "sp_marcar_correo_evaluacion_solicitante " + dt.Rows[i]["id_MiSolicitud"].ToString() + "";
                                    db.getQuery(conexionBecarios,query);
                                }
                                i++;
                            }
                        }


                        //Esta informacion es donde se envia al alumno becario
                        i = 0;
                        query = "sp_Correo_recordatorio_evalacion_becario "+ddlperiodo.SelectedValue+","+hdfidCampus.Value+"";
                        dt = db.getQuery(conexionBecarios,query);                       
                        if(dt.Rows.Count>0)
                        {
                            double porcetaje;
                            int total = int.Parse( dt.Rows[0]["Contador"].ToString());
                            foreach(DataRow r in dt.Rows)
                            {
                                if(mandarCorreo(dt.Rows[i]["Cuerpo"].ToString(),dt.Rows[i]["Asunto"].ToString(),dt.Rows[i]["Correo"].ToString()))
                                {
                                    query = "sp_marcar_correo_evaluacion_al_becario " + dt.Rows[i]["id_consecutivo"].ToString() + " ";
                                    db.getQuery(conexionBecarios,query);
                                    i = i + 1;
                                    porcetaje = (double) (i * 100) / total;
                                    i = i - 1;
                                    //Este metodo actualiza los datos en el progressbar 
                                    //SetTheProgress(bar1, Math.Round(porcetaje) + "%");

                                    ClientScript.RegisterStartupScript(this.GetType(), "prog" + i, "progreso(" + Math.Round(porcetaje) + ");", true);
                                   
                                }
                                i++;
                            }
                        }
                        pnlprogres.Visible = true;
                        
                    }
                    else
                    {
                        verModal("Alerta", "No se encuentra dentro de la fecha de evaluación para mandar el correo");
                    }
                }
                
            }catch(Exception es){
                verModal("Error", es.Message.ToString());
            }
        }

        public void SetTheProgress(HtmlGenericControl bar, string value)
        {
            bar.Attributes.Add("style", string.Format("width:{0};", value));
            lblMensaje.Text = "Envio de correo " + value;
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



    }
}