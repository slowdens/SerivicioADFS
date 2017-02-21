using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using ServicioBecario.Codigo;


namespace ServicioBecario.Vistas
{
    public partial class Menus : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, caracter;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void chkVer_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkVer.Checked)
                {
                    pnlmenus.Visible = true;
                    mostrarListaMenu();
                }
                else
                {
                    pnlmenus.Visible = false;
                }
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\n", "").Replace("\r", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
        }
        public void mostrarListaMenu()
        {
            if (txtfiltrarPadre.Text == "")
            {
                query = "exec sp_mostrar_menus '" + txtFiltrarMenu.Text.Trim() + "','" + txtfiltarPantalla.Text.Trim() + "', -1";
            }
            else
            {
                query = "exec sp_mostrar_menus '" + txtFiltrarMenu.Text.Trim() + "','" + txtfiltarPantalla.Text.Trim() + "'," + txtfiltrarPadre.Text.Trim() + "";
            }

            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                Gvmenu.DataSource = dt;
                Gvmenu.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay menús para mostrar");
            }
        }
        protected void Gvmenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //Completo el auto paginado en el GridView
                Gvmenu.PageIndex = e.NewPageIndex;
                mostrarListaMenu();
                //Con este método refresco el Gridview
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\n", "").Replace("\r", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            //Con este evento capturo el menú
            try
            {
                agregarMenu();
                mostrarListaMenu();
                limpiarComponentes();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\n", "").Replace("\r", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
        }
        public void agregarMenu()
        {

            if (String.IsNullOrEmpty(txtlink.Text))
            {
                query = "sp_Agregar_menus '" + txtmenu.Text + "',NULL," + txtPadre.Text + "";
            }
            else
            {
                query = "sp_Agregar_menus '" + txtmenu.Text + "','" + txtlink.Text + "'," + txtPadre.Text + "";
            }
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Exito", "El menú quedo guardado correctamente");
                }
                else
                {
                    verModal("Alerta", "El menú no se pudo guardar");
                }
            }
            else
            {
                verModal("Alerta", "No existen menús disponibles");
            }
        }
        public void limpiarComponentes()
        {
            txtlink.Text = "";
            txtmenu.Text = "";
            txtPadre.Text = "";
        }

        protected void Gvmenu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Con este evento agrego la función del confirm para decidir si elimina y se ejecuta la decesion en el evento Gvroles_RowDeleting
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (Image button in e.Row.Cells[4].Controls.OfType<Image>())
                {
                    button.Attributes["onclick"] = "confirmar('¿Estás seguro  en eliminar el menu ?');";
                }
            }
        }

        protected void Gvmenu_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (hdfDesion.Value == "true")
                {
                    int id = (int)Gvmenu.DataKeys[e.RowIndex].Value;
                    eliminarMenu(id);
                    mostrarListaMenu();
                }
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\n", "").Replace("\r", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);


            }
        }

        public void eliminarMenu(int id)
        {
            query = "sp_eliminar_menus " + id;
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Exito", "Menú eliminado satisfactoriamente");
                }
                else
                {
                    verModal("Alerta", "No se puede eliminar el menú");
                }
            }
        }

        protected void Gvmenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Con este evento saco los datos de la grilla
            hdfid_permiso.Value = Gvmenu.SelectedDataKey.Value.ToString();
            txtmenu.Text = Gvmenu.SelectedRow.Cells[1].Text;
            txtlink.Text = Gvmenu.SelectedRow.Cells[2].Text;
            txtPadre.Text = Gvmenu.SelectedRow.Cells[3].Text;
            pnlActualizar.Visible = true;
            pnlAgregar.Visible = false;
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                actulizarInformacion();
                mostrarListaMenu();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\n", "").Replace("\r", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
        }
        public void actulizarInformacion()
        {
            query = "sp_actuliza_menus " + hdfid_permiso.Value + ",'" + txtmenu.Text + "','" + txtlink.Text + "'," + txtPadre.Text + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
            {
                verModal("Exito", "El menú se actualizo satisfactoriamente");
            }
            else
            {
                verModal("Alerta", "No se pudo actualizar el menú");
            }
        }

        protected void btnFiltar_Click(object sender, EventArgs e)
        {
            try
            {
                mostrarListaMenu();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\n", "").Replace("\r", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
        }


        protected void imbEliminar_click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse((sender as ImageButton).CommandArgument);
                eliminarMenu(id);
                mostrarListaMenu();
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
    }
}