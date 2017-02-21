using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServicioBecario.Codigo;
using System.Data;
using System.Configuration;

namespace ServicioBecario.Vistas
{
    public partial class Menus_permisos : System.Web.UI.Page
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
                    llenarPermisos();
                    llenarPadre();
                    llenarPadre2();
                    llenarPermisosFiltros();
                    mostrarMenus();
                }
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\t", "").Replace("\r", "").Replace("\n", "");
               
                verModal("Error", caracter);
            }
        }

        public void llenarPermisosFiltros()
        {
            query = "select id_permiso, Permiso from tbl_permisos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarPermisos.DataTextField = "Permiso";
                ddlFiltrarPermisos.DataValueField = "id_permiso";
                ddlFiltrarPermisos.DataSource = dt;
                ddlFiltrarPermisos.DataBind();
            }
        }

        public void guardarMenuPermiso(string id)
        {
            query = "exec sp_guardarMenu_permiso " + id + "," + ddlpermiso.SelectedValue;
            db.getQuery(conexionBecarios, query);

        }

        public void llenarPadre2()
        {
            query = "select id_menu,Nombre from tbl_menus where Padre=0";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarpadresegundo.DataTextField = "Nombre";
                ddlFiltrarpadresegundo.DataValueField = "id_menu";
                ddlFiltrarpadresegundo.DataSource = dt;
                ddlFiltrarpadresegundo.DataBind();
            }
        }
        public void llenarPadre()
        {
            query = "select id_menu,Nombre from tbl_menus where Padre=0";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarPadre.DataTextField = "Nombre";
                ddlFiltrarPadre.DataValueField = "id_menu";
                ddlFiltrarPadre.DataSource = dt;
                ddlFiltrarPadre.DataBind();
            }
        }
        public void llenarPermisos()
        {
            query = "select id_permiso, Permiso from tbl_permisos ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlpermiso.DataTextField = "Permiso";
                ddlpermiso.DataValueField = "id_permiso";
                ddlpermiso.DataSource = dt;
                ddlpermiso.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay permisos por mostrar");
            }
        }

        protected void ddlpermiso_DataBound(object sender, EventArgs e)
        {
            ddlpermiso.Items.Insert(0, new ListItem("-- Seleccione un permiso --", ""));
        }
        public void mostrarMenus()
        {
            //query = "exec dbo.sp_mostrar_menus" ;
            //            query = @"	select id_menu, Nombre,
            //			case 
            //			 when Link  Is null then 'N/A'
            //			 ELSE Link
            //			 end  as Link
            //			,Padre from tbl_menus";

            query = "exec ps_muestra_menus_mpp '" + txtFiltraMenus.Text.Trim() + "' ,'" + txtpantala.Text.Trim() + "', " + ddlFiltrarPadre.SelectedValue + " ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvPermisoMenu.DataSource = dt;
                GvPermisoMenu.DataBind();
            }
            else
            {
                verModal("Error", "No hay menús que mostrar");
            }
        }

        protected void btnFiltrarMenus_Click(object sender, EventArgs e)
        {
            try
            {
                mostrarMenus();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\t", "").Replace("\r", "").Replace("\n", "");
                //db.mensajeAlerta(caracter, this);
                verModal("Error", caracter);
            }
        }

        protected void chkMostrarFoltros_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMostrarFoltros.Checked)
            {
                pnlFiltro1.Visible = true;

            }
            else
            {
                pnlFiltro1.Visible = false;
            }
        }

        protected void chkVerLigadosPermisosMenus_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVerLigadosPermisosMenus.Checked)
            {
                pnlLigarRolesPermisos.Visible = true;
                mostarMenusPermisos();
                chkMostrarFiltro2.Visible = true;
            }
            else
            {
                pnlLigarRolesPermisos.Visible = false;
                chkMostrarFiltro2.Visible = false;
            }
        }

        public void mostarMenusPermisos()
        {
            query = "sp_mostrar_menus_permisos " + ddlFiltrarPermisos.SelectedValue + ", '" + txtfiltrarMenusegundo.Text + "','" + txtfiltrarPantalla.Text + "'," + ddlFiltrarpadresegundo.SelectedValue + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvLigarolesPermisos.DataSource = dt;
                GvLigarolesPermisos.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay permisos o menús disponibles en la lista");
            }
        }

        protected void GvLigarolesPermisos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (Image button in e.Row.Cells[5].Controls.OfType<Image>())
                {
                    button.Attributes["onclick"] = "confirmar('¿Estás seguro  en eliminar el menu del permiso?');";
                }
            }
        }

        protected void GvLigarolesPermisos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //Completo el auto paginado en el GridView
                GvLigarolesPermisos.PageIndex = e.NewPageIndex;
                //Con este método refresco el Gridview
                mostarMenusPermisos();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\t", "").Replace("\r", "").Replace("\n", "");
                verModal("Error", caracter);
            }
        }

        protected void GvLigarolesPermisos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (hdfDesion.Value == "true")
                {
                    int id = (int)GvLigarolesPermisos.DataKeys[e.RowIndex].Value;
                    eleminarPermisos_roles(id);
                    //Actualizamos la lista
                    mostarMenusPermisos();

                }
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\t", "").Replace("\r", "").Replace("\n", "");
                verModal("Error", caracter);
            }
        }
        public void eleminarPermisos_roles(int id)
        {
            query = "sp_eliminar_permisos_menus " + id;
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Exito", "Se eliminó el menú del permiso satisfactoriamente");
                }
                else
                {
                    verModal("Alerta", "No se puede eliminar el menú del permiso");
                }
            }
        }

        protected void chkMostrarFiltro2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMostrarFiltro2.Checked)
            {
                pnlFiltro2.Visible = true;


            }
            else
            {
                pnlFiltro2.Visible = false;
            }
        }

        protected void ddlFiltrarPermisos_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarPermisos.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void ddlFiltrarpadresegundo_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarpadresegundo.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void GvPermisoMenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //Completo el auto paginado en el GridView
                GvPermisoMenu.PageIndex = e.NewPageIndex;
                //Con este método refresco el Gridview
                mostrarMenus();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\t", "").Replace("\r", "").Replace("\n", "");
                verModal("Error", caracter);
            }
        }

        protected void ddlFiltrarPadre_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarPadre.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void btnFiltrarDos_Click(object sender, EventArgs e)
        {
            try
            {
                mostarMenusPermisos();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\t", "").Replace("\r", "").Replace("\n", "");
                verModal("Error", caracter);
            }
        }

        protected void imbEliminar_click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse((sender as ImageButton).CommandArgument);
                eleminarPermisos_roles(id);
                //Actualizamos la lista
                mostarMenusPermisos();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }

        }
        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void btnGuarMenus_Click(object sender, EventArgs e)
        {
            int id;
            bool bandera = false;
            try
            {
                foreach (GridViewRow row in GvPermisoMenu.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chseleccion = (row.Cells[4].FindControl("chkselecioname") as CheckBox);
                        if (chseleccion.Checked)
                        {
                            id = (int)GvPermisoMenu.DataKeys[row.RowIndex].Value;
                            bandera = true;
                            guardarMenuPermiso(id.ToString());
                        }
                    }
                }
                if (bandera)
                {
                    verModal("Exito", "La información se almaceno correctamente!!");
                }
                else
                {
                    verModal("Alerta", "Usted no selecciono ningún menú");
                }
                //Actualizamos la lista
                mostarMenusPermisos();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\t", "").Replace("\r", "").Replace("\n", "");
               // db.mensajeAlerta(caracter, this);
            }
        }

    }
}