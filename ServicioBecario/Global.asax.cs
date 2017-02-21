using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Data;
using ServicioBecario.Codigo;
using System.Configuration;

namespace ServicioBecario
{
    public class Global : System.Web.HttpApplication
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        protected void Application_Start(object sender, EventArgs e)
        {
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            Session["Usuario"] = "";
        }
        void Application_End(object sender, EventArgs e)
        {
        
            
        }
    }
}