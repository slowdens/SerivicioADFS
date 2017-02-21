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
    public partial class BecarioPorPuesto : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, caracter;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    verificaElRol();
                    llenaPuesto();
                    llenarFiltroCampus();
                    llenarFiltroPuesto();
                    pnlMostrarGrid.Visible = true;//Muestra todo el grid con sus filtros
                    if (hdf_rol.Value == "4")//Esto para rol multicampus
                    {
                        pnlFitrarCampus.Visible = true;
                    }
                    mostrar_datos();
                }
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("'", "").Replace("\n", "").Replace("\t", "").Replace("\r", "");
                verModal("Error", caracter);
            }
        }
        public void llenarFiltroPuesto()
        {
            query = "select id_puesto_campus,Nombre_puesto from cat_puestos_campus ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFitrarPuesto.DataTextField = "Nombre_puesto";
                ddlFitrarPuesto.DataValueField = "id_puesto_campus";
                ddlFitrarPuesto.DataSource = dt;
                ddlFitrarPuesto.DataBind();
            }

        }
        public void llenarFiltroCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus order by nombre asc ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarCampus.DataTextField = "Nombre";
                ddlFiltrarCampus.DataValueField = "Codigo_campus";
                ddlFiltrarCampus.DataSource = dt;
                ddlFiltrarCampus.DataBind();
            }

        }

        public void llenaPuesto()
        {
            query = " select id_puesto_campus,Nombre_puesto from cat_puestos_campus";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlPuestos.DataTextField = "Nombre_puesto";
                ddlPuestos.DataValueField = "id_puesto_campus";
                ddlPuestos.DataSource = dt;
                ddlPuestos.DataBind();
            }
        }

        public void verificaElRol()
        {
            query = "EXEC sp_cheka_campus_vs_perfil_nuevo '" + Session["Usuario"].ToString() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)//Verifica que tenga datos
            {
                hdf_rol.Value = dt.Rows[0]["idRol"].ToString();
                if (dt.Rows[0]["idRol"].ToString() == "4")//Este es administrador multicampus
                {
                    //Avilitamos la selecccion de campus
                    pnlCampus.Visible = false;

                    llenarCampus();
                    ddlCampus.Visible = true;
                    lblCampus.Visible = false;
                }
                else
                {
                    pnlCampus.Visible = false;
                    lblCampus.Text = dt.Rows[0]["campus"].ToString();
                    hdf_idCampus.Value = dt.Rows[0]["Codigo_campus"].ToString();
                    ddlCampus.Visible = false;
                }
            }

        }

        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus  order by nombre asc";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }


        public void vericarCampus()
        {
            //query = "EXEC sp_cheka_campus_vs_perfil '"+Session["Usuario"].ToString()+"'";
            query = "EXEC sp_cheka_campus_vs_perfil '" + Session["Usuario"].ToString() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                lblCampus.Text = dt.Rows[0]["campus"].ToString();
            }
        }

        public void mostrar_datos()
        {
            if (hdf_rol.Value == "4")//Esto es pára que puede ver toda la informacion el administrador multicampus
            {
                if (ddlFitrarPuesto.SelectedValue != "-1" && txtFiltraCantidadBecarios.Text == "" && ddlFiltrarCampus.SelectedValue == "-1")//1
                {
                    query = "sp_CantidadBecarios_campus_completo " + ddlFitrarPuesto.SelectedValue + ",0," + ddlFiltrarCampus.SelectedValue + "";
                }
                if (ddlFitrarPuesto.SelectedValue == "-1" && txtFiltraCantidadBecarios.Text != "" && ddlFiltrarCampus.SelectedValue == "-1")//2
                {
                    query = "sp_CantidadBecarios_campus_completo " + ddlFitrarPuesto.SelectedValue + "," + txtFiltraCantidadBecarios.Text + "," + ddlFiltrarCampus.SelectedValue + "";
                }
                if (ddlFitrarPuesto.SelectedValue == "-1" && txtFiltraCantidadBecarios.Text == "" && ddlFiltrarCampus.SelectedValue != "-1")//3
                {
                    query = "sp_CantidadBecarios_campus_completo " + ddlFitrarPuesto.SelectedValue + ",0," + ddlFiltrarCampus.SelectedValue + "";
                }
                if (ddlFitrarPuesto.SelectedValue != "-1" && txtFiltraCantidadBecarios.Text != "" && ddlFiltrarCampus.SelectedValue == "-1")//4
                {
                    query = "sp_CantidadBecarios_campus_completo " + ddlFitrarPuesto.SelectedValue + "," + txtFiltraCantidadBecarios.Text + "," + ddlFiltrarCampus.SelectedValue + "";
                }
                if (ddlFitrarPuesto.SelectedValue != "-1" && txtFiltraCantidadBecarios.Text == "" && ddlFiltrarCampus.SelectedValue != "-1")//5
                {
                    query = "sp_CantidadBecarios_campus_completo " + ddlFitrarPuesto.SelectedValue + ",0," + ddlFiltrarCampus.SelectedValue + "";
                }
                if (ddlFitrarPuesto.SelectedValue == "-1" && txtFiltraCantidadBecarios.Text != "" && ddlFiltrarCampus.SelectedValue != "-1")//6
                {
                    query = "sp_CantidadBecarios_campus_completo " + ddlFitrarPuesto.SelectedValue + "," + txtFiltraCantidadBecarios.Text + "," + ddlFiltrarCampus.SelectedValue + "";
                }
                if (ddlFitrarPuesto.SelectedValue != "-1" && txtFiltraCantidadBecarios.Text != "" && ddlFiltrarCampus.SelectedValue != "-1")//7
                {
                    query = "sp_CantidadBecarios_campus_completo " + ddlFitrarPuesto.SelectedValue + "," + txtFiltraCantidadBecarios.Text + "," + ddlFiltrarCampus.SelectedValue + "";
                }
                if (ddlFitrarPuesto.SelectedValue == "-1" && txtFiltraCantidadBecarios.Text == "" && ddlFiltrarCampus.SelectedValue == "-1")//8
                {
                    query = "sp_CantidadBecarios_campus_completo " + ddlFitrarPuesto.SelectedValue + ",0," + ddlFiltrarCampus.SelectedValue + "";
                }

            }
            else
            {
                if (ddlFitrarPuesto.SelectedValue != "-1" && txtFiltraCantidadBecarios.Text == "")//1
                {
                    query = "sp_cantidadBecarioss_campus " + ddlFitrarPuesto.SelectedValue + ",0," + hdf_idCampus.Value + "";
                }
                if (ddlFitrarPuesto.SelectedValue == "-1" && txtFiltraCantidadBecarios.Text != "")//2
                {
                    query = "sp_cantidadBecarioss_campus " + ddlFitrarPuesto.SelectedValue + "," + txtFiltraCantidadBecarios.Text + "," + hdf_idCampus.Value + "";
                }
                if (ddlFitrarPuesto.SelectedValue != "-1" && txtFiltraCantidadBecarios.Text != "")//3
                {
                    query = "sp_cantidadBecarioss_campus " + ddlFitrarPuesto.SelectedValue + "," + txtFiltraCantidadBecarios.Text + "," + hdf_idCampus.Value + "";
                }
                if (ddlFitrarPuesto.SelectedValue == "-1" && txtFiltraCantidadBecarios.Text == "")//4
                {
                    query = "sp_cantidadBecarioss_campus " + ddlFitrarPuesto.SelectedValue + ",0," + hdf_idCampus.Value + "";
                }
            }

            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvMostrar.DataSource = dt;
                GvMostrar.DataBind();
            }
            else
            {
                verModal("Alerta", "No se encontró la información");
                GvMostrar.DataSource =null;
                GvMostrar.DataBind();
            }

        }



        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void ddlCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdf_idCampus.Value = ddlCampus.SelectedValue;
            lblCampus.Text = ddlCampus.SelectedItem.Text;

        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        protected void ddlPuestos_DataBound(object sender, EventArgs e)
        {
            ddlPuestos.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        

        protected void ddlFitrarPuesto_DataBound(object sender, EventArgs e)
        {
            ddlFitrarPuesto.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void ddlFiltrarCampus_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarCampus.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                mostrar_datos();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //guarda los dato 
                guardarBecarioCampus();
                mostrar_datos();
                limpiarcontroles();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void limpiarcontroles()
        {
            ddlPuestos.SelectedValue = "";
            txtCantidadBecarios.Text = "";
        }

        public void guardarBecarioCampus()
        {
            query = "sp_guardarCantidad_porPuesto " + hdf_idCampus.Value + "," + ddlPuestos.SelectedValue + "," + txtCantidadBecarios.Text + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Éxito", "Tu información se guardo");
                }
            }
            mostrar_datos();
        }

        protected void GvMostrar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GvMostrar.PageIndex = e.NewPageIndex;
                mostrar_datos();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void GvMostrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                ddlPuestos.SelectedValue = ddlPuestos.Items.FindByText(GvMostrar.SelectedRow.Cells[0].Text).Value;
                txtCantidadBecarios.Text = GvMostrar.SelectedRow.Cells[1].Text;
                if (hdf_rol.Value == "4")//rol multicaampus
                {
                    ddlCampus.SelectedValue = ddlCampus.Items.FindByText(HttpUtility.HtmlDecode(GvMostrar.SelectedRow.Cells[2].Text)).Value;
                    hdf_idCampus.Value = ddlCampus.Items.FindByText(HttpUtility.HtmlDecode(GvMostrar.SelectedRow.Cells[2].Text)).Value;
                    lblCampus.Visible = false;
                }
                ClientScript.RegisterStartupScript(GetType(), "BanderBPP", "Bandera();", true);
                lblCampus.Text = GvMostrar.SelectedRow.Cells[2].Text;

                btnGuardar.Visible = false;
                btnActualizar.Visible = true;
                btnCancelar.Visible = true;
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdfDesion.Value == "true")
                {
                    actualizarInformacion();
                    mostrar_datos();
                }
                btnActualizar.Visible = false;
                btnGuardar.Visible = true;
                btnCancelar.Visible = false;
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void actualizarInformacion()
        {
            query = "sp_actualiza_becarios_puestos " + hdf_idCampus.Value + "," + ddlPuestos.SelectedValue + "," + txtCantidadBecarios.Text + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                verModal("Éxito", "La información se actualizó correctamente");
            }
            else
            {
                verModal("Alerta", "Sucedió algo inesperado");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                btnCancelar.Visible = false;
                btnGuardar.Visible = true;
                btnActualizar.Visible = false;
                llimpiarcontroles();
                llimpiarFiltros();
                GvMostrar.SelectedIndex = -1;
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        public void llimpiarcontroles()
        {
            ddlPuestos.SelectedValue = "";
            txtCantidadBecarios.Text = "";
        }
        public void llimpiarFiltros()
        {
            ddlFitrarPuesto.SelectedValue = "-1";
            txtFiltraCantidadBecarios.Text = "";
             if (hdf_rol.Value == "4")//rol multicaampus
             {
                 ddlFiltrarCampus.SelectedValue = "-1";
             }
        }

    }
}