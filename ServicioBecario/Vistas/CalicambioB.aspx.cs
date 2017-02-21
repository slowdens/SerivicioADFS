
using ServicioBecario.Codigo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServicioBecario.Vistas
{
    public partial class CalicambioB : System.Web.UI.Page
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
            query = @"select a.Correo_alumno, a.Matricula ,p.Descripcion as Periodo ,  sb.id_consecutivo, a.Matricula,a.Nombre +' ' + a.Apellido_paterno  +' '+ a.Apellido_materno as Becario,  sb.Becario_calificacion    from tbl_solicitudes s inner join tbl_solicitudes_becarios sb on sb.id_Misolicitud=s.id_Misolicitud
		inner join tbl_alumnos a on a.Matricula=sb.Matricula
		inner join cat_periodos p on p.Periodo=s.Periodo
		inner join cat_estatus_asignacion cea on cea.id_estatus_asignacion=sb.id_estatus_asignacion where p.Periodo!='' ";

            if (txtfiltraMatricula.Text != "") { query += " AND a.Matricula='" + txtfiltraMatricula.Text + "'"; }
            if (ddlFiltrarPeriodo.SelectedValue != "-1") { query += " AND p.Periodo='" + ddlFiltrarPeriodo.SelectedValue + "'"; }


            /*
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
            }*/
            query = query.Replace("\r\n\t\t", " ");
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                gvInformacion.DataSource = dt;
                gvInformacion.DataBind();
                
            }
            else
            {
                verModal("Alerta", "No se encontrar los datos del filtro");
                gvInformacion.DataSource = null;
                gvInformacion.DataBind();
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
                                verModal("Éxito", "Se cambio la calificación a la matrícula: " + lblMatricula.Text);
                                llenarInformacionGrid();
                            }
                        }
                    }
                    else
                    {
                        verModal("Alerta", "No se puede cambiar la calificación porque aún no se desplega el primer resultado");
                    }
                }
                Pnlmuestra.Visible = false;
            }
            else
            {
                //Con este metodo lo que ago es mostrar un un alerta pero lo quite al ultuimo.
                
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

        public void limpiarcom()
        {
            lblMatricula.Text = "";
            lblNombreBecario.Text = "";
            ddlCalificacion.SelectedValue = "";
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                limpiarcom();
                Pnlmuestra.Visible = false;
                gvInformacion.SelectedIndex = -1;
                //gvInformacion
               // gvInformacion. = -1;

            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }


    }
}