using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using ServicioBecario.Codigo;
using System.Collections;
using System.Net.Mail;

namespace ServicioBecario.Vistas
{
    public partial class CalificacionBecario : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, mensaje;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        Correo msj = new Correo();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    llenarperiodo(ddlFiltrarPeriodo);
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }

        }

        public void llenarperiodo(DropDownList lista)
        {
            query = "select Periodo,Descripcion from cat_periodos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                lista.DataValueField = "Periodo";
                lista.DataTextField = "Descripcion";
                lista.DataSource = dt;
                lista.DataBind();
            }
        }



        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();

        }
        public void llenarInformacionGrid()
        {
            if (txtfiltraMatricula.Text != "" && ddlFiltrarPeriodo.SelectedValue == "-1")//1
            {
                query = "sp_buscar_calificacion_becario '" + txtfiltraMatricula.Text.Trim() + "'," + ddlFiltrarPeriodo.SelectedValue + "";
            }
            if (txtfiltraMatricula.Text == "" && ddlFiltrarPeriodo.SelectedValue != "-1")
            {
                query = "sp_buscar_calificacion_becario null," + ddlFiltrarPeriodo.SelectedValue + "";
            }
            if (txtfiltraMatricula.Text != "" && ddlFiltrarPeriodo.SelectedValue != "-1")
            {
                query = "sp_buscar_calificacion_becario '" + txtfiltraMatricula.Text.Trim() + "'," + ddlFiltrarPeriodo.SelectedValue + "";
            }
            if (txtfiltraMatricula.Text == "" && ddlFiltrarPeriodo.SelectedValue == "-1")
            {
                query = "sp_buscar_calificacion_becario null," + ddlFiltrarPeriodo.SelectedValue + "";
            }
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                gvInformacion.DataSource = dt;
                gvInformacion.DataBind();
                UpdatePanel1.Update();
            }
            else
            {
                verModal("Alerta", "No se encontrar los datos del filtro");
            }
        }
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                llenarInformacionGrid();

            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }

        }

        protected void ddlFiltrarPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarPeriodo.Items.Insert(0, new ListItem("--Seleccione --", "-1"));
        }

        protected void gvInformacion_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {

                GridViewRow row = gvInformacion.SelectedRow;

                llanarCalificacion();
                var colnov = gvInformacion.DataKeys[row.RowIndex].Values;
                foreach (DictionaryEntry ds in colnov)
                {
                    switch (ds.Key.ToString())
                    {
                        case "id_consecutivo":
                            hdfid.Value = ds.Value.ToString();
                            break;
                        case "Correo_alumno":
                            hdfCorreo.Value = ds.Value.ToString();
                            break;
                    }
                }

                string matricula = gvInformacion.SelectedRow.Cells[2].Text;
               
                Pnlmuestra.Visible = true;
                lblNombreBecario.Text = gvInformacion.SelectedRow.Cells[3].Text;
                lblMatricula.Text = matricula;
                string calificacion = gvInformacion.SelectedRow.Cells[4].Text;
                if (calificacion != "PENDIENTE" && calificacion != "")
                {
                    ddlCalificacion.SelectedValue = ddlCalificacion.Items.FindByText(gvInformacion.SelectedRow.Cells[4].Text).Value;
                }

                hdfperiodo.Value = gvInformacion.SelectedRow.Cells[5].Text;

                UpdatePanel2.Update();

            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }


        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            if (hdfValidacion.Value == "true")
            {
                query = "sp_cambia_calificacion_becario  " + hdfid.Value + " ,'" + ddlCalificacion.SelectedItem.Text + "','" + Session["Usuario"].ToString() + "','" + lblMatricula.Text + "','" + hdfperiodo.Value + "'";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                    {
                        //sacar los cuerpos del correo y el titulo
                        query = "sp_toma_cuerpo_correo_Cambio_calificacion '" + lblMatricula.Text + "','" + ddlCalificacion.SelectedItem.Text + "','" + hdfperiodo.Value + "'";
                        dt = db.getQuery(conexionBecarios, query);
                        if (dt.Rows.Count > 0)
                        {
                            if (mandarCorreo(dt.Rows[0]["cuerpo"].ToString(), dt.Rows[0]["asunto"].ToString(), hdfCorreo.Value))
                            {
                                verModal("Exito", "Se cambió la calificación a la Matrícula: " + lblMatricula.Text);
                                llenarInformacionGrid();
                            }
                        }
                    }
                    else
                    {
                        verModal("Alerta","No se puede cambiar la calificación porque aun no se desplega el primer resultado"); 
                    }
                }
                Pnlmuestra.Visible = false;
            }
            else
            {
                //Con este metodo lo que ago es mostrar un un alerta pero lo quite al ultuimo.
                ScriptManager.RegisterStartupScript(this.UpdatePanel2, GetType(), "miFuncion", "mostrarGlobo();", true);
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

        public void llanarCalificacion()
        {
            query = "select id_calificacion,valor_calificacion from cat_calificacion ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCalificacion.DataValueField = "id_calificacion";
                ddlCalificacion.DataTextField = "valor_calificacion";
                ddlCalificacion.DataSource = dt;
                ddlCalificacion.DataBind();
            }
        }

        protected void ddlCalificacion_DataBound(object sender, EventArgs e)
        {
            ddlCalificacion.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void gvInformacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvInformacion.PageIndex = e.NewPageIndex;
                llenarInformacionGrid();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }

        }

    }
}