using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace ServicioBecario
{
    public partial class HttpErrorPage : System.Web.UI.Page
    {
        void Application_Error(object sender, EventArgs e)
        {
            //get reference to the source of the exception chain
            Exception ex = Server.GetLastError().GetBaseException();

            //log the details of the exception and page state to the
            //Windows 2000 Event Log
            EventLog.WriteEntry("Test Web",
              "MESSAGE: " + ex.Message +
              "\nSOURCE: " + ex.Source +
              "\nFORM: " + Request.Form.ToString() +
              "\nQUERYSTRING: " + Request.QueryString.ToString() +
              "\nTARGETSITE: " + ex.TargetSite +
              "\nSTACKTRACE: " + ex.StackTrace,
              EventLogEntryType.Error);

            //Insert optional email notification here...
        }
    }
}