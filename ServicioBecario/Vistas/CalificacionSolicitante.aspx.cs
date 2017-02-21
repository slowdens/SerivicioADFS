using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using ServicioBecario.Codigo;
namespace ServicioBecario.Vistas
{
    public partial class CalificacionSolicitante : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                llenarDatosPeriodo();
            }
        }

        public void llenarDatosPeriodo()
        {
            query = "select Periodo,Descripcion from cat_periodos";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
        }

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione --", "-1"));
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                llenarDatosGrid();
                
            }catch(Exception es){
                verModal("Error",es.Message.ToString());
            }
        }

        public void llenarDatosGrid()
        {
            
            if(hdfvalidar.Value=="true")
            {
                if (txtNomina.Text != "" && txtcalificacion.Text == "" && ddlPeriodo.SelectedValue == "-1")//1
                {
                    query = "sp_muestra_calificacion_solicitantes '" + txtNomina.Text.Trim() + "',null," + ddlPeriodo.SelectedValue + "";
                }
                if (txtNomina.Text == "" && txtcalificacion.Text != "" && ddlPeriodo.SelectedValue == "-1")//2
                {
                    query = "sp_muestra_calificacion_solicitantes null, " + txtcalificacion.Text.Trim() + "," + ddlPeriodo.SelectedValue + " ";
                }
                if (txtNomina.Text == "" && txtcalificacion.Text == "" && ddlPeriodo.SelectedValue != "-1")//3
                {
                    query = "sp_muestra_calificacion_solicitantes null , null , " + ddlPeriodo.SelectedValue + "";
                }
                if (txtNomina.Text != "" && txtcalificacion.Text != "" && ddlPeriodo.SelectedValue == "-1")//4
                {
                    query = "sp_muestra_calificacion_solicitantes '" + txtNomina.Text.Trim() + "'," + txtcalificacion.Text.Trim() + "," + ddlPeriodo.SelectedValue + " ";
                }
                if (txtNomina.Text != "" && txtcalificacion.Text == "" && ddlPeriodo.SelectedValue != "-1")//5
                {
                    query = "sp_muestra_calificacion_solicitantes '" + txtNomina.Text.Trim() + "',null," + ddlPeriodo.SelectedValue + "";
                }
                if (txtNomina.Text == "" && txtcalificacion.Text != "" && ddlPeriodo.SelectedValue != "-1")//6
                {
                    query = "sp_muestra_calificacion_solicitantes null," + txtcalificacion.Text.Trim() + "," + ddlPeriodo.SelectedValue + " ";
                }
                if (txtNomina.Text != "" && txtcalificacion.Text != "" && ddlPeriodo.SelectedValue != "-1")//7
                {
                    query = "sp_muestra_calificacion_solicitantes '" + txtNomina.Text.Trim() + "'," + txtcalificacion.Text.Trim() + "," + ddlPeriodo.SelectedValue + "";
                }
                if (txtNomina.Text == "" && txtcalificacion.Text == "" && ddlPeriodo.SelectedValue == "-1")//8
                {
                    query = "sp_muestra_calificacion_solicitantes null,null,-1";
                }
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    GvDatos.DataSource = dt;
                    GvDatos.DataBind();
                }      
            }                                                
        }



        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();

        }

        protected void GvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //Mostramos los datos dentro del grid pero con una serier de botones.
                GvDatos.PageIndex = e.NewPageIndex;
                llenarDatosGrid();
            }catch(Exception es){
                verModal("Erro",es.Message.ToString());
            }
        }

        protected void txtNomina_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string cadena = txtNomina.Text.ToLower().Trim();
                if(cadena!="")
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
                
            }catch(Exception es ){
                verModal("Error",es.Message.ToString());
            }
        }



    }
}