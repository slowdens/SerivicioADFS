using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using ServicioBecario.Codigo;
using System.Net.Mail;
namespace ServicioBecario.Vistas
{
    public partial class SeguimientoSolicitudes : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        static string staticconexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        Correo msj = new Correo();

        public static DataTable getQuery(string conexion, string query)
        {
            //Se crea el datatable
            DataTable dt = new DataTable();
            //Creamos la conexion
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            //Llenanos nuestro  data table
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();
            //Retorno mi data table
            return dt;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //Llenamos la informacion necesaria.
                    llenarPeriodo();
                    vermisosdeCampus();
                    llenarTipoSolicitud();
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
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
                    ddlCampus.Visible = true;
                    llenarCampus();
                    lblCampus.Visible = false;
                }
                else
                {
                    lblCampus.Text = dt.Rows[0]["Campus"].ToString();
                    hdfMostrarId.Value = dt.Rows[0]["Codigo_campus"].ToString();

                    hdfActivarRol.Value = "0";
                    ddlCampus.Visible = false;
                }
            }
            else
            {
                verModal("Alerta", "El usuario no se encontró registrado");
            }
        }

        public void mostarDatos_solicitud()
        {
      

            query = @"  select s.id_MiSolicitud, e.Nomina,   p.Descripcion as Periodo,c.Nombre as Campus, ts.Nombre as Solicitud,e.Nomina,e.Nombre + ' ' + e.Apellido_paterno +' ' + e.Apellido_materno as [Nombre Solicitante]
                          ,e.Ubicacion_fisica as [Ubicacion fisica] ,s.Ubicacion_alterna as [Ubicacion alterna],
                          case  when  y.Nombre is null then
                            'N/A'
                          else y.Nombre
                          end as Proyecto,      
                          ss.Solicitud_estatus as [Solicitud estatus],
                          e.Correo_electronico as [Correo electronico],
                          e.Extencion_telefonica as [Extencion telefonica],
                          e.Departamento ,
                          e.Puesto
                          ,case when ss.Solicitud_estatus= 'Aprobada'  then 'True'
                          else 'False'
                          end as Decide
                          from tbl_solicitudes s 
                          inner join tbl_empleados e on e.Nomina=s.Nomina
                          inner join cat_periodos p on p.Periodo=s.Periodo
                          left join tbl_proyectos y on y.id_proyecto=s.id_proyecto
                          inner join cat_tipo_solicitudes ts on ts.id_tipo_solicitud = s.id_tipo_solicitud
                          inner join Cat_solicitud_estatus ss on ss.id_solicitud_estatus =s.id_solicitud_estatus
                          inner join cat_campus c on  c.Codigo_campus = e.Codigo_campus
                          where p.Activo=1 ";


            if (ddlPeriodo.SelectedValue != "-1") { query += " and p.Periodo = " + "'" + ddlPeriodo.SelectedValue + "'"; }
            if(hdfActivarRol.Value=="1")
            {
                if (ddlCampus.SelectedValue != "") { query += "And  c.Codigo_campus = '" + ddlCampus.SelectedValue + "' "; }
            }
            else
            {
                query += "And  c.Codigo_campus = '" + hdfMostrarId.Value + "' ";
            }
            
            if (txtNomina.Text != "") { query += "and e.Nomina= '" + txtNomina.Text.Trim() + "'"; }
            if (chkAprovado.Checked == true) { query += "And ss.id_solicitud_estatus = 2 --Aprobadas "; }
            if (ddlTipoSolicitud.SelectedValue != "-1") { query += " And ts.id_tipo_solicitud = " + ddlTipoSolicitud.SelectedValue + " "; }





            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvMostrarDatos.DataSource = dt;
                GvMostrarDatos.DataBind();
            }
            else
            {
                GvMostrarDatos.DataSource = null;
                GvMostrarDatos.DataBind();
                verModal("Alerta","No se encontraron registros");
            }


            ViewState["dt"] = dt;
        }


        public void llenarPeriodo()
        {
            query = @"select Periodo,Descripcion  from cat_periodos where Activo=1";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay periodos activos");
            }
        }
        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus where Codigo_campus!='PRT' order by Nombre";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
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

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione --", "-1"));
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        public void llenarTipoSolicitud()
        {
            query = "select id_tipo_solicitud, Nombre from cat_tipo_solicitudes where id_tipo_solicitud in(1,2,3)";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlTipoSolicitud.DataValueField = "id_tipo_solicitud";
                ddlTipoSolicitud.DataTextField = "Nombre";
                ddlTipoSolicitud.DataSource = dt;
                ddlTipoSolicitud.DataBind();
            }

        }

        protected void ddlTipoSolicitud_DataBound(object sender, EventArgs e)
        {
            ddlTipoSolicitud.Items.Insert(0, new ListItem("--Seleccione --", "-1"));
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                mostarDatos_solicitud();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void GvMostrarDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                mostarDatos_solicitud();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void txtNomina_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string cadena = txtNomina.Text.ToLower().Trim();
                if (!String.IsNullOrEmpty(cadena))
                {
                    if (cadena.Contains("l") || cadena.Contains("L"))
                    {
                        txtNomina.Text = txtNomina.Text.ToUpper();
                    }
                    else
                    {
                        txtNomina.Text = "L" + txtNomina.Text;
                    }
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string GetDynamicContent(string contextKey)
        {
            DataTable dts;
            string html = "";
            string query = "select Correo_electronico,Departamento,Puesto,Extencion_telefonica from tbl_empleados where Nomina= '" + contextKey + "'";
            dts = getQuery(staticconexionBecarios, query);
            if (dts.Rows.Count > 0)
            {

                html = @"   <table  class='table table-hover'>
                                <tr><td> <label>Correo: </labe> </td> <td> " + dts.Rows[0]["Correo_electronico"].ToString() + @"   </td> </tr>
                                <tr><td><label>Departamento: </label> </td> <td> " + dts.Rows[0]["Departamento"].ToString() + @" </td>  <tr>
                                <tr> <td> <label>Puesto:  </label> </td> <td>" + dts.Rows[0]["Puesto"].ToString() + @" </td>  </tr>
                                <tr> <td> <label> Extención:</label> </td> <td> " + dts.Rows[0]["Extencion_telefonica"].ToString() + @" </td>   </tr>
                          </table>
                                                    
                        ";

            }
            return html;
        }

        protected void GvMostrarDatos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                AjaxControlToolkit.PopupControlExtender pce = e.Row.FindControl("PopupDetalles") as AjaxControlToolkit.PopupControlExtender;
                string behaviorID = String.Concat("pce", e.Row.RowIndex);
                pce.BehaviorID = behaviorID;
                Image i = (Image)e.Row.Cells[5].FindControl("ImageButtonDetalles");
                string OnMouseOverScript = String.Format("$find('{0}').showPopup();", behaviorID);
                string OnMouseOutScript = String.Format("$find('{0}').hidePopup();", behaviorID);
                i.Attributes.Add("onmouseover", OnMouseOverScript);
                i.Attributes.Add("onmouseout", OnMouseOutScript);
            }
        }

        protected void IbtnExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                dt = (DataTable)ViewState["dt"];
                descargarInfo(dt);
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


        public void descargarInfo(DataTable ds)
        {
            if (dt != null)
            {
                string attachment = "attachment; filename=Tablero.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                string tab = "";
                foreach (DataColumn dc in ds.Columns)
                {
                    if (dc.ColumnName != "id_empleado" && dc.ColumnName != "id_MiSolicitud" && dc.ColumnName != "id_MiSolicitud")
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }

                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in ds.Rows)
                {
                    tab = "";
                    for (i = 0; i < ds.Columns.Count; i++)
                    {
                        if (i != 0)
                        {
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }

                    }
                    Response.Write("\n");
                }
                Response.End();

            }
            else
            {
                verModal("Alerta", "No hay informacion disponible para descargar");
            }

        }
        protected void chkAprovar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                string decide = hdfDecide.Value;
                CheckBox chk = (CheckBox)sender;
                if(decide == "true")
                {
                    string dato = chk.ToolTip;
                    string periodo;
                    string idSolicitud;
                    string solicitud;
                    idSolicitud = dato.Substring(0, dato.IndexOf("!"));
                    dato = dato.Remove(0, dato.IndexOf("!") + 1);
                    periodo = dato.Substring(0, dato.IndexOf("!"));
                    dato = dato.Remove(0, dato.IndexOf("!") + 1);
                    solicitud = dato;
                    //mandamos los datos store
                    query = "sp_corre_cancelacion_solicitud " + idSolicitud + ",'" + solicitud + "','" + periodo + "' ";
                    dt = db.getQuery(conexionBecarios, query);
                    chk.Enabled = false;
                    if (dt.Rows.Count > 0)
                    {
                        if (mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(), dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["Correo"].ToString()))
                        {
                            query = "sp_elimina_solicitud_cambiando_estatus " + idSolicitud + "";
                            dt = db.getQuery(conexionBecarios, query);
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                                {                                    
                                    query = "sp_guardar_historial_asignacion '" + Session["Usuario"].ToString() + "'," + idSolicitud + ",'Se elimino la solicutud' ";
                                    dt = db.getQuery(conexionBecarios, query);
                                    if(dt.Rows.Count>0)
                                    {
                                        if(dt.Rows[0]["Mensaje"].ToString()=="Ok")
                                        {
                                            verModal("Exito", "Se canceló la solicitud y se mandó un correo al destinatario");
                                            mostarDatos_solicitud();
                                        }
                                    }
                                    
                                }
                            }

                        }
                        else
                        {
                            verModal("Alerta", "el Correo no se pudo enviar");
                        }

                    }
                    else
                    {
                        verModal("Alerta", "No existe informacion para este tipo de correo");
                    }
                }
                else
                {
                    chk.Checked = true;
                }
                
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
            
        }

        public bool mandarCorreo2(string cuerpo, string asunto ,string correo )
        {
            bool bandera = false;
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(correo, ""));
            msg.From = new MailAddress("servicio.becario@itesm.mx", "Servicio Becario");
            msg.Subject = asunto;
            
            msg.Body = cuerpo + db.noEnvio() ;
            msg.IsBodyHtml = true;
            try
            {
                
                msj.MandarCorreo(msg);
                bandera = true;
            }catch(Exception es )
            {
                verModal("Error",es.Message.ToString());
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






        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                Correo Cr = new Correo();
                MailMessage mnsj = new MailMessage();
                mnsj.Subject = "Hola Mundo";
                mnsj.To.Add(new MailAddress("israelneftali@gmail.com"));
                mnsj.To.Add(new MailAddress("nefta_2_7@hotmail.com"));
                mnsj.From = new MailAddress("neftaliTorres@itesm.mx", "Israel Neftali");
                /* Si deseamos Adjuntar algún archivo*/
                //mnsj.Attachments.Add(new Attachment("C:\\archivo.pdf"));
                mnsj.Body = "  Mensaje de Prueba \n\n Enviado desde C#\n\n *VER EL ARCHIVO ADJUNTO*";
                mnsj.IsBodyHtml = true;
                /* Enviar */
                Cr.MandarCorreo(mnsj);

            }catch(SmtpException es){
                verModal("Error",es.Message.ToString());
            }
        }

        protected void btnLlevarRegistro_Click(object sender, EventArgs e)
        {
            try
            {
                 Button btn = (Button)sender;
                 string id = btn.CommandName.Substring(0, btn.CommandName.IndexOf("!"));
                 btn.CommandName = btn.CommandName.Remove(0,id.Length); 
                 string solicitud = btn.CommandName;
                 solicitud = solicitud.Remove(0, 1);   
                 Response.Redirect("Mostrar.aspx?ds=" + id + "&soli=" + solicitud);
            }catch(Exception es )
            {
                verModal("Error", es.Message.ToString());
            }
        }       
    }
}