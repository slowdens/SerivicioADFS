using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
namespace ServicioBecario.Codigo
{
    public class Correo
    {
        /*
         * Cliente SMTP
         * Gmail:  smtp.gmail.com  puerto:587
         * Hotmail: smtp.live.com  puerto:25
         */

        SmtpClient client = new SmtpClient();
        public Correo()
        {


            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("servicio.becario@itesm.mx", "DroQ.7342");
            client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
            client.Host = "smtp.office365.com";
            //client.Host = "10.40.42.49";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            //client.EnableSsl = false;



            //client.UseDefaultCredentials = true;
            //client.Host = "10.97.83.72";
            //client.EnableSsl = false;
            
        }
        public void MandarCorreo(MailMessage mensaje)
        {
           client.Send(mensaje);
        }
       
    }
}