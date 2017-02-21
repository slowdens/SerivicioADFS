using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServicioBecario.Codigo;
using System.Data.SqlClient;


namespace ServicioBecario.Vistas
{
    public partial class Asignados : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        static string staticconexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, cadena;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    query = "sp_presenta_alumnos_asignados_al_sb '"+Session["usuario"].ToString()+"'";
                    dt = db.getQuery(conexionBecarios,query);
                    if (dt.Rows.Count > 0)
                    {
                        GvDatos.DataSource = dt;
                        GvDatos.DataBind();
                        pnlmostrarGrid.Visible = true;
                    }
                    else
                    {
                        verModal("Alerta","Usted no tiene asignación de becarios"); 
                    }
                }
            }catch(Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


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
        protected void GvDatos_RowCreated(object sender, GridViewRowEventArgs e)
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


        protected void btnLlevarRegistro_Click(object sender, EventArgs e)
        {


            Response.Redirect("AsistenciaBecario.aspx");
        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string GetDynamicContent(string contextKey)
        {
            DataTable dts;
            string html = "";
            string query = @"sp_muestra_ubicaciones " + contextKey;
            dts = getQuery(staticconexionBecarios, query);
            if (dts.Rows.Count > 0)
            {

                html = @"   <table  class='table table-hover'>
                                <tr><td> <label>Ubicación fisica: </labe> </td> <td> " + dts.Rows[0]["Ubicacion_fisica"].ToString() + @"   </td> </tr>
                                <tr><td><label>Ubicación alterna: </label> </td> <td> " + dts.Rows[0]["Ubicacion_alterna"].ToString() + @" </td>  <tr>                                
                            </table>
                                                    
                        ";
            }
            return html;
        }


        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }
    }
}