using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;    

namespace ServicioBecario.Vistas
{
    public partial class Reportes : System.Web.UI.Page
    {
        string urlSharepoint = "SPHostUrl=https%3A%2F%2Fcolaboratest.itesm.mx%2Fsites%2FDevPortal**SPLanguage=es-ES**SPClientTag=6**SPProductNumber=15.0.4675.1000";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBecarios_Click(object sender, EventArgs e)
        {
            urlSharepoint = urlSharepoint.Replace("**", "&");
            Response.Redirect("ReporteBecarioGenerales.aspx?" + urlSharepoint);
        }

        protected void btnSNovaluado_Click(object sender, EventArgs e)
        {
            urlSharepoint = urlSharepoint.Replace("**", "&");
            Response.Redirect("SbNoEvaluados.aspx?"+urlSharepoint);
        }

        protected void btnReasignacion_Click(object sender, EventArgs e)
        {
            urlSharepoint = urlSharepoint.Replace("**", "&");
            Response.Redirect("ReporteBecariosReasingados.aspx?"+urlSharepoint);
        }

        protected void btnproyectos_Click(object sender, EventArgs e)
        {
            urlSharepoint = urlSharepoint.Replace("**", "&");
            Response.Redirect("ReporteProyectos.aspx" + urlSharepoint);
        }
    }
}