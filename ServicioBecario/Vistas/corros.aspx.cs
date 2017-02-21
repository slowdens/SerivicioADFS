using ServicioBecario.Codigo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServicioBecario.Vistas
{
    public partial class corros : System.Web.UI.Page
    {
        BasedeDatos db = new BasedeDatos();
        string query;
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            mandarCorreo2("Hola", "Nada", "neftalitorres@itesm.mx");
        }

        public bool numeros(string            valor)
        {
            return Regex.IsMatch(valor, @"^[L{0,1}[0-9]{9}]*$");
        }
        public bool validar(string valor)
        {
            return Regex.IsMatch(valor, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d+$");
               // Regex.Match(valor, @"[\s]*|(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d");
        }
        

        public bool mandarCorreo(string cuerpo, string asunto, string correo)
        {
            bool bandera = false;
            System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
            email.To.Add(correo);//Destinatario
            email.From = new MailAddress(correo, "Nombre de Usuario", System.Text.Encoding.UTF8);//Emisor y nombre de usuario
            email.Subject = asunto;//Asunto del mensaje
            email.SubjectEncoding = System.Text.Encoding.UTF8;
            email.Body = cuerpo;//Mensaje a ser enviado
            email.BodyEncoding = System.Text.Encoding.UTF8;
            email.IsBodyHtml = false;

            SmtpClient objSmtp = new SmtpClient("10.2.27.144");
            //objSmtp.Credentials = new System.Net.NetworkCredential("servicio.becario@itesm.mx", "DroQ.7342");
            objSmtp.UseDefaultCredentials = true;
            //hotmail
            objSmtp.Port = 587; // ùerto de envio tanto de Hotmail como para Gmail
            //objSmtp.Host = "smtp.live.com";// Protocolo Simple de Transferencia de Correo de (Hotmail)
            //
            //objSmtp.EnableSsl = false;
            try
            {
                objSmtp.Send(email);//Enviamos el mensaje
//                MessageBox.Show("Mensaje Enviado Correctamente", "Correo C#", MessageBoxButtons.OK, MessageBoxIcon.Information);
  //              sw = false;
                bandera = true;
            }
            catch (System.Net.Mail.SmtpException e)
            {
                string es = e.Source.ToString();
                Response.Write(e);
            }

            return bandera;
        }

        public void mandarCorreo2(string cuerpo, string asunto, string correo)
        {
            string to = "neftalitorres@itesm.mx";
            string from = "servicio.becario@itesm.mx";
            string subject = "Using the new SMTP client.";
            string body = @"Using this new feature, you can send an e-mail message from an application very easily.";
            MailMessage message = new MailMessage( from,to, subject, body);
            SmtpClient client = new SmtpClient("10.2.27.144", 587);
            // Credentials are necessary if the server requires the client 
            // to authenticate before it will send e-mail on the client's behalf.
            //client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.UseDefaultCredentials = false;

            try
            {
                client.Send(message);//Enviamos el mensaje
                //                MessageBox.Show("Mensaje Enviado Correctamente", "Correo C#", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //              sw = false;
            
            }
            catch (System.Net.Mail.SmtpException e)
            {
                string es = e.Source.ToString();
                Response.Write(e);
            }

        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                query = @"select es.Nomina,p.Descripcion as Periodo,
                             p.Periodo as PeriodoID, 
                             sb.Nivel_academico as [Nivel academico] ,
                             cs.Nombre as  [Campus solicitante],
                             es.Nomina,es.Nombre +' '+ es.Apellido_paterno+' '+es.Apellido_materno  as [Nombre solicitante],
                             es.Ubicacion_fisica as [Ubicacion fisica],
                             case when s.Ubicacion_alterna is null then 'N/A'
                             else s.Ubicacion_alterna
                             end as [Ubicacion alterna] 
                             ,a.Matricula,sb.Asistencia,a.Nombre +' ' + a.Apellido_paterno+' '+a.Apellido_materno as [Nombre Becario] ,
                             case  when  po.Nombre is null then  'N/A'
                             else po.Nombre
                             end as Proyecto,sb.Becario_calificacion as [Becario calificacion],s.Empleado_puntuaje as [Solicitante Calificacion],
                             es.Correo_electronico as Correo,
                             es.Departamento,
                             es.Puesto,
                             es.Extencion_telefonica as [extencion telefonica],
                             sa.Estatus_asignacion as [Estatus asignacion]
                             from 
                             tbl_solicitudes s inner join cat_periodos p on s.Periodo=p.Periodo
                             inner join tbl_empleados es on es.Nomina=s.Nomina
                             inner join cat_tipo_solicitudes ts on ts.id_tipo_solicitud=s.id_tipo_solicitud
                             inner join Cat_solicitud_estatus se on se.id_solicitud_estatus=s.id_solicitud_estatus
                             inner join tbl_solicitudes_becarios sb on sb.id_Misolicitud=s.id_MiSolicitud
                             inner join tbl_alumnos a on a.Matricula=sb.Matricula
                             inner join cat_estatus_asignacion ea on ea.id_estatus_asignacion= sb.id_estatus_asignacion
                             inner join cat_campus as c on c.Codigo_campus=a.Codigo_campus
                             inner join cat_campus cs on cs.Codigo_campus=es.Codigo_campus
                             left join tbl_proyectos  po on po.id_proyecto=s.id_proyecto
                             inner join cat_estatus_asignacion sa on sa.id_estatus_asignacion=sb.id_estatus_asignacion
                             where es.Nomina!='' ";

                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }catch(Exception es)
            {
                Console.WriteLine(es.Message);
            }
            
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
        }
    }
}