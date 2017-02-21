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
    public partial class BecariosNoAsistieron : System.Web.UI.Page
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
                vermisosdeCampus();
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
                    llenarCampus();
                    pnlDdlis.Visible = true;
                    hdfRol.Value = "4";
                }
                else
                {
                    lblcampus.Text = dt.Rows[0]["Campus"].ToString();
                    hdfId_campus.Value = dt.Rows[0]["Codigo_campus"].ToString();
                    pnlconjunto.Visible = true;
                    hdfRol.Value = "1";
                }
            }
            else
            {
                verModal("Alerta", "El usuario no se encontró registrado");
            }
        }

        public void llenarPeriodo()
        {
            query = "select Descripcion,Periodo from cat_periodos ";
            dt = db.getQuery(conexionBecarios,query);
            if (dt.Rows.Count > 0)
            {
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
        }

        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus";
            dt = db.getQuery(conexionBecarios,query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
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

        

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void txtNomina_TextChanged(object sender, EventArgs e)
        {
            string cadena = txtNomina.Text.ToLower().Trim();
            try
            {
                if(!string.IsNullOrEmpty(cadena))
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

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                buscarDatos();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void buscarDatos()
        {
          

            if(hdfRol.Value=="4")
            {
                 query = @" select sb.id_consecutivo, e.Nomina, e.Nombre + ' ' + e.Apellido_paterno+ ' '+e.Apellido_materno as NombreSolicitante,a.Matricula , a.Nombre+' ' +a.Apellido_paterno+' ' +a.Apellido_materno as NombreBecario,p.Descripcion as Nombre
    from tbl_solicitudes s inner join tbl_solicitudes_becarios sb on sb.id_Misolicitud=s.id_MiSolicitud
    inner join tbl_alumnos a on sb.Matricula=a.Matricula
    inner join tbl_empleados e on e.Nomina = s.Nomina
    inner join cat_periodos p on p.Periodo=s.Periodo
    where sb.Asistencia =0 and sb.Asistencia_fecha is null ";
                if (ddlPeriodo.SelectedValue != "-1"){ query +=" AND p.Periodo = '"+ddlPeriodo.SelectedValue+"'"; }
                if (ddlCampus.SelectedValue != "-1"){ query +=" AND e.Codigo_campus = '"+ddlCampus.SelectedValue+"'"; }
                if (txtNomina.Text != ""){ query +=" AND e.Nomina = '"+txtNomina.Text+"'"; }
                
                
                /*
                if (ddlPeriodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && txtNomina.Text == "")//1
                {
                    query = "sp_muestra_becarios_sin_asistencia '" + ddlPeriodo.SelectedValue + "' ,'-1',null";
                }
                if (ddlPeriodo.SelectedValue == "-1" && ddlCampus.SelectedValue != "-1" && txtNomina.Text == "")//2
                {
                    query = "sp_muestra_becarios_sin_asistencia  -1,'" + ddlCampus.SelectedValue + "', null ";
                }
                if (ddlPeriodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && txtNomina.Text != "")//3
                {
                    query = "sp_muestra_becarios_sin_asistencia -1,-1,'" + txtNomina.Text.Trim() + "'";
                }
                if (ddlPeriodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && txtNomina.Text == "")//4
                {
                    query = "sp_muestra_becarios_sin_asistencia " + ddlPeriodo.SelectedValue + "," + ddlCampus.SelectedValue + ",null";
                }
                if (ddlPeriodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && txtNomina.Text != "")//5
                {
                    query = "sp_muestra_becarios_sin_asistencia  " + ddlPeriodo.SelectedValue + ",-1,'" + txtNomina.Text + "' ";
                }
                if (ddlPeriodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && txtNomina.Text != "")//6
                {
                    query = "sp_muestra_becarios_sin_asistencia " + ddlPeriodo.SelectedValue + "," + ddlCampus.SelectedValue + " ,'" + txtNomina.Text.Trim() + "' ";
                }
                if (ddlPeriodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && txtNomina.Text == "")//7
                {
                    query = "sp_muestra_becarios_sin_asistencia -1,-1,null";
                }*/
            }
            else
            {
                query = @" select sb.id_consecutivo, e.Nomina, e.Nombre + ' ' + e.Apellido_paterno+ ' '+e.Apellido_materno as NombreSolicitante,a.Matricula , a.Nombre+' ' +a.Apellido_paterno+' ' +a.Apellido_materno as NombreBecario,p.Descripcion as Nombre
    from tbl_solicitudes s inner join tbl_solicitudes_becarios sb on sb.id_Misolicitud=s.id_MiSolicitud
    inner join tbl_alumnos a on sb.Matricula=a.Matricula
    inner join tbl_empleados e on e.Nomina = s.Nomina
    inner join cat_periodos p on p.Periodo=s.Periodo
    where sb.Asistencia =0 and sb.Asistencia_fecha is null ";
                if (ddlPeriodo.SelectedValue != "-1") { query += " AND p.Periodo = '" + ddlPeriodo.SelectedValue + "'"; }
                query += " AND e.Codigo_campus = '" + hdfId_campus.Value + "'";
                if (txtNomina.Text != "") { query += " AND e.Nomina = '" + txtNomina.Text + "'"; }
               /* if(ddlPeriodo.SelectedValue!="-1"&& txtNomina.Text =="")
                {
                    query = "sp_muestra_becarios_sin_asistencia "+ddlPeriodo.SelectedValue+","+hdfId_campus.Value+",null";
                }
                if(ddlPeriodo.SelectedValue=="-1"&& txtNomina.Text !="")
                {
                    query = "sp_muestra_becarios_sin_asistencia -1," + hdfId_campus.Value + ",'" + txtNomina.Text.Trim() + "'";
                }
                if (ddlPeriodo.SelectedValue != "-1" && txtNomina.Text != "")
                {
                    query = "sp_muestra_becarios_sin_asistencia " + ddlPeriodo.SelectedValue + "," + hdfId_campus.Value + ",'" + txtNomina.Text.Trim() + "' ";
                }
                if (ddlPeriodo.SelectedValue == "-1" && txtNomina.Text == "")
                {
                    query = "sp_muestra_becarios_sin_asistencia -1," + hdfId_campus.Value + ",null";
                }*/
            }

            query = query.Replace("\r\n ", " ");
            dt = db.getQuery(conexionBecarios,query);
            if (dt.Rows.Count > 0)
            {
                gvDatos.DataSource = dt;
                gvDatos.DataBind();
            }
            else
            {
                verModal("Alerta" ,"No se encontro información en la busqueda");
                gvDatos.DataSource = null;
                gvDatos.DataBind();
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDatos.PageIndex = e.NewPageIndex;
                buscarDatos();
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void chkselecioname_CheckedChanged(object sender, EventArgs e)
        {
            try{
                string matricula;
                string nomina;
                string decide = hdfDecide.Value;
                CheckBox chk = (CheckBox)sender;
                if(decide == "true")
                {

                    query = "sp_des_asigna_becario_por_iniasistencia " + chk.ToolTip + ",'" + Session["Usuario"].ToString() + "'";
                    dt = db.getQuery(conexionBecarios, query);
                    if(dt.Rows.Count>0)
                    {
                        if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                        {
                            matricula = dt.Rows[0]["Matricula"].ToString();
                            nomina = dt.Rows[0]["Nomina"].ToString();
                            query = "sp_toma_aviso_designacion '" + dt.Rows[0]["Matricula"].ToString() + "','" + dt.Rows[0]["Nomina"].ToString() + "'," + chk.ToolTip + ",'"+ddlPeriodo.SelectedItem.Text+"' ";
                            dt = db.getQuery(conexionBecarios,query);
                            if (dt.Rows.Count > 0)
                            {
                                if (mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(), dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["CorreoSolicitante"].ToString()))
                                {
                                    //mandamos un correo al alumo becario
                                    query = "sp_guarda_des_asignacion_al_becario '" + matricula + "','" + nomina + "'," + chk.ToolTip + ",'" + ddlPeriodo.SelectedItem.Text + "'";
                                    dt = db.getQuery(conexionBecarios, query);
                                    if (dt.Rows.Count > 0)
                                    {
                                        if (mandarCorreo(dt.Rows[0]["Cuerpo"].ToString(), dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["CorreoAlumno"].ToString()))
                                        {
                                            
                                            verModal("Exito", "La información se guardó con éxito y se mandará un correo de aviso");

                                        }
                                    }
                                    
                                }
                            }
                        }
                        else
                        {
                            verModal("Error","Anomalía dentro del procedimiento");
                        }
                        
                    }              
    
                }
                else
                {
                    chk.Checked = false;
                }
                
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
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


    }
}