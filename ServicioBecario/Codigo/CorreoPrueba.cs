using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ServicioBecario.Codigo
{
    public class CorreoPrueba
    {
        SmtpClient client = new SmtpClient();

        public CorreoPrueba()
        {

            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("nefta_2_7@hotmail.com","coolmiyo321-");
            client.Port = 25; // You can use Port 25 if 587 is blocked (mine is!)
            client.Host = "smtp.live.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
        }

        public void MandarCorreo(MailMessage mensaje)
        {
            client.Send(mensaje);
        }
       
    }
}