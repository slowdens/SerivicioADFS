using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using ServicioBecario.Codigo;
using System.Text.RegularExpressions;
namespace ServicioBecario.Vistas
{
    public partial class HistorialEvaluacion : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenarPeriodo();
            }
        }

        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();

        }
        public void llenarPeriodo()
        {
            query = "select Periodo,Descripcion from cat_periodos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
        }

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtfecha.Text))
                {
                     Match match = Regex.Match(txtfecha.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                    if (match.Success)
                    {
                        llenarGrid();
                    }
                    else
                    {
                        verModal("Error", "La fecha no tiene el formato dd/mm/aaaa");
                    }
                }
                else
                {
                llenarGrid();
                }
                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


        public void llenarGrid()
        {
            if(hdfDecide.Value=="true")
            {
                if (ddlPeriodo.SelectedItem.Text != "--Seleccione --" && txtNomina.Text == "" && txtmatricula.Text == "" && txtfecha.Text == "")//1
                {
                    query = "sp_muestra_historial_calificaciones '" + ddlPeriodo.SelectedItem.Text + "',null,null,null  ";
                }
                if (ddlPeriodo.SelectedItem.Text == "--Seleccione --" && txtNomina.Text != "" && txtmatricula.Text == "" && txtfecha.Text == "")//2
                {
                    query = "sp_muestra_historial_calificaciones '" + ddlPeriodo.SelectedItem.Text + "','" + txtmatricula.Text + "',null,null  ";
                }
                if (ddlPeriodo.SelectedItem.Text == "--Seleccione --" && txtNomina.Text == "" && txtmatricula.Text != "" && txtfecha.Text == "")//3
                {
                    query = "sp_muestra_historial_calificaciones  '" + ddlPeriodo.SelectedItem.Text + "',null,'" + txtmatricula.Text + "',null";
                }
                if (ddlPeriodo.SelectedItem.Text == "--Seleccione --" && txtNomina.Text == "" && txtmatricula.Text == "" && txtfecha.Text != "")//4
                {
                    query = "sp_muestra_historial_calificaciones  '" + ddlPeriodo.SelectedItem.Text + "',null,null,'" + db.convertirFecha(txtfecha.Text.Trim()) + "'";
                }
                if (ddlPeriodo.SelectedItem.Text != "--Seleccione --" && txtNomina.Text != "" && txtmatricula.Text == "" && txtfecha.Text == "")//5
                {
                    query = "sp_muestra_historial_calificaciones  '" + ddlPeriodo.SelectedItem.Text + "','" + txtNomina.Text.Trim() + "',null,null";
                }
                if (ddlPeriodo.SelectedItem.Text != "--Seleccione --" && txtNomina.Text == "" && txtmatricula.Text != "" && txtfecha.Text == "")//6
                {
                    query = "sp_muestra_historial_calificaciones '" + ddlPeriodo.SelectedItem.Text + "',null,'" + txtmatricula.Text.Trim() + "',null";
                }
                if (ddlPeriodo.SelectedItem.Text != "--Seleccione --" && txtNomina.Text == "" && txtmatricula.Text == "" && txtfecha.Text != "")//7
                {
                    query = "sp_muestra_historial_calificaciones '" + ddlPeriodo.SelectedItem.Text + "',null, null,'" + db.convertirFecha(txtfecha.Text.Trim()) + "'";
                }
                if (ddlPeriodo.SelectedItem.Text == "--Seleccione --" && txtNomina.Text != "" && txtmatricula.Text != "" && txtfecha.Text == "")//8
                {
                    query = "sp_muestra_historial_calificaciones '" + ddlPeriodo.SelectedItem.Text + "' '" + txtNomina.Text.Trim() + "','" + txtmatricula.Text + "',null";
                }
                if (ddlPeriodo.SelectedItem.Text == "--Seleccione --" && txtNomina.Text != "" && txtmatricula.Text == "" && txtfecha.Text != "")//9
                {
                    query = "sp_muestra_historial_calificaciones '" + ddlPeriodo.SelectedItem.Text + "','" + txtNomina.Text.Trim() + "',null,'" + db.convertirFecha(txtfecha.Text.Trim()) + "'";
                }
                if (ddlPeriodo.SelectedItem.Text == "--Seleccione --" && txtNomina.Text == "" && txtmatricula.Text != "" && txtfecha.Text != "")//10
                {
                    query = "sp_muestra_historial_calificaciones '" + ddlPeriodo.SelectedItem.Text + "',null,'" + txtmatricula.Text.Trim() + "','" + db.convertirFecha(txtfecha.Text.Trim()) + "'";
                }
                if (ddlPeriodo.SelectedItem.Text != "--Seleccione --" && txtNomina.Text != "" && txtmatricula.Text != "" && txtfecha.Text == "")//11
                {
                    query = "sp_muestra_historial_calificaciones '" + ddlPeriodo.SelectedItem.Text + "','" + txtNomina.Text.Trim() + "','" + txtmatricula.Text.Trim() + "',null";
                }
                if (ddlPeriodo.SelectedItem.Text != "--Seleccione --" && txtNomina.Text == "" && txtmatricula.Text != "" && txtfecha.Text != "")//12
                {
                    query = "sp_muestra_historial_calificaciones '" + ddlPeriodo.SelectedItem.Text + "',null,'" + txtmatricula.Text.Trim() + "','" + db.convertirFecha(txtfecha.Text.Trim()) + "'";
                }
                if (ddlPeriodo.SelectedItem.Text == "--Seleccione --" && txtNomina.Text != "" && txtmatricula.Text != "" && txtfecha.Text != "")//13
                {
                    query = "sp_muestra_historial_calificaciones '" + ddlPeriodo.SelectedItem.Text + "','" + txtNomina.Text.Trim() + "','" + txtmatricula.Text.Trim() + "','" + db.convertirFecha(txtfecha.Text.Trim()) + "'";
                }
                if (ddlPeriodo.SelectedItem.Text != "--Seleccione --" && txtNomina.Text != "" && txtmatricula.Text != "" && txtfecha.Text != "")//14
                {
                    query = "sp_muestra_historial_calificaciones '" + ddlPeriodo.SelectedItem.Text + "','" + txtNomina.Text.Trim() + "','" + txtmatricula.Text.Trim() + "','" + db.convertirFecha(txtfecha.Text.Trim()) + "'";
                }
                if (ddlPeriodo.SelectedItem.Text == "--Seleccione --" && txtNomina.Text == "" && txtmatricula.Text == "" && txtfecha.Text == "")//15
                {
                    query = "sp_muestra_historial_calificaciones '" + ddlPeriodo.SelectedItem.Text + "',null,null,null";
                }
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    gvdastos.DataSource = dt;
                    gvdastos.DataBind();
                }
                else
                {
                    gvdastos.DataSource = null;
                    gvdastos.DataBind();
                    verModal("Alerta", "No se encontró la información de búsqueda");
                }
            }
            
            
        }

        protected void txtNomina_TextChanged(object sender, EventArgs e)
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
        }

        protected void txtmatricula_TextChanged(object sender, EventArgs e)
        {
            string cadena = txtmatricula.Text.ToLower().Trim();
            if (cadena != "")
            {
                if (cadena.Contains("a") || cadena.Contains("A"))
                {
                    txtmatricula.Text = txtmatricula.Text.ToUpper();
                }
                else
                {
                    txtmatricula.Text = "A" + txtmatricula.Text;
                }
            }

        }

        protected void gvdastos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvdastos.PageIndex = e.NewPageIndex;
                llenarGrid();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

    }
}