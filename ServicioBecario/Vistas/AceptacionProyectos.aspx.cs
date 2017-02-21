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
    public partial class AceptacionProyectos : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                verificarRolPersona();
                //llananos el periodo
                llenarPeriodo();
            }
        }
        //Verificamos que tipo de perfil tiene  la persona que se ingresa ala panatalla
        public void verificarRolPersona()
        {
            query = "EXEC sp_cheka_campus_vs_perfil_nuevo '" + Session["Usuario"].ToString() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)//Verifica que tenga datos
            {
                //guardamos en nuestra el rol
                hdfRol.Value = dt.Rows[0]["idRol"].ToString();

                //Verifica que el rol sea Administrador multicampus
                if (dt.Rows[0]["idRol"].ToString() == "4")
                {
                    //llanamos la etiqueta que muestra todos los campus
                    llenarCampusddl();
                    //Mostramos nuestro Canpus
                    pnlDrodownlis.Visible = true;
                }
                else
                {
                    //Cuado tiene otro rol solamente puede accsar  a ver la información de su  campus
                    lblCampus.Text = dt.Rows[0]["campus"].ToString();
                    hdf_id_campus.Value = dt.Rows[0]["Codigo_campus"].ToString();
                    pnllabel.Visible = true;
                }
            }
        }
        public void llenarCampusddl()
        {
            query = "select Codigo_campus , Nombre as Campus from cat_campus ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataTextField = "Campus";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
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



        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));

        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                llenarDatosGrid();
               
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


        public void llenarDatosGrid()
        {
            string id_campus;
            //Rol administrador Multicampus
            if (hdfRol.Value == "4")
            {
                id_campus = ddlCampus.SelectedValue;
            }
            else
            {
                id_campus = hdf_id_campus.Value;
            }

            if(ddlPeriodo.SelectedValue!="-1")
            { 
            query = "sp_consulta_proyecto '" + id_campus + "','" + ddlPeriodo.SelectedValue + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                gviDatos.DataSource = dt;
                gviDatos.DataBind();
            }
            else
            {
                verModal("Error","No existen proyectos para  el periodo " + ddlPeriodo.SelectedItem.Text);
            }
            }
            else
            {
                verModal("Nota", "Selecciona un periodo!!!");
            }

        }

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();

        }

        protected void chkaprovar_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            string id = chk.ToolTip;
            if(hdfDesion.Value=="true")
            {                
                 //Autiriza pro
                query = "sp_autoriza_proyecto_normal "+id+", 1";
                dt = db.getQuery(conexionBecarios,query);
                if(dt.Rows.Count>0)
                {
                    if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                    {                               
                        verModal("Éxito","El proyecto se autorizó con éxito");
                        llenarDatosGrid();
                    }
                }
                else
                {
                    verModal("Alerta","Sucedió un error en la aplicación");
                }

            }
            else
            {
                //cuando no es aceptado el proyecto se envia un mensaje por correo.
                query = "sp_autoriza_proyecto_normal " + id + ", 0";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                    {
                        //faltsa crear un query.
                        query = "sp_manda_correo_por_proyecto_cancelado "+id+"";
                        dt = db.getQuery(conexionBecarios,query);
                        if(dt.Rows.Count>0)
                        {
                            if(mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(),dt.Rows[0]["Asunto"].ToString(),dt.Rows[0]["Correo"].ToString()))
                            {
                                verModal("Alerta", "No se autorizó el proyecto y se mando un correo a la persona que lo creo");
                            }
                        }
                        
                    }
                }
                else
                {
                    verModal("Alerta", "Sucedió un error en la aplicación");
                }


            }
            
        }



        public bool mandarCorreo2(string cuerpo,string asunto, string correo)
        {
            bool bandera = false;
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(correo,""));
            msg.From = new MailAddress("servicio.becario@itesm.mx", "Servicio Becario");
            msg.Subject = asunto;
            msg.Body = cuerpo+ db.noEnvio();
            msg.IsBodyHtml = true;
            try
            {
                Correo msj= new Correo();
                msj.MandarCorreo(msg);
                bandera=true;
            }catch(Exception es )
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



        protected void gviDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gviDatos.PageIndex = e.NewPageIndex;
                llenarDatosGrid();
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

     


    }
}