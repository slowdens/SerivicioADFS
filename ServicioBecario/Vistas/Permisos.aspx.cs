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
    public partial class Permisos : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, mensaje;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                llenarFiltroPermiso();
            }
        }
        public void llenarFiltroPermiso()
        {
            query = "select id_permiso,Permiso from tbl_permisos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarPermisos.DataValueField = "id_permiso";
                ddlFiltrarPermisos.DataTextField = "Permiso";
                ddlFiltrarPermisos.DataSource = dt;
                ddlFiltrarPermisos.DataBind();
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                guardaPermiso();
                //Limpio los componentes de entrada
                limpiarCompoentes();
                muestraPermisos();
            }
            catch (Exception es)
            {
                string caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);

            }
        }
        public void guardaPermiso()
        {
            //Con este método guardamos los registros del permiso en la base de datos
            query = "sp_crear_permisos '" + txtNombre.Text.Trim() + "','" + txtDescripcion.Text.Trim() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Salida"].ToString() == "Ok")
                {
                    //db.mensajeAlerta("El permiso se creó con éxito ya lo puedes cazar con un  rol y menús", this);
                    verModal("Exito", "El permiso se creó con éxito ya lo puedes cazar con un  rol y menús");
                }
                else
                {                    
                    verModal("Alerta", "Ya existe un permiso con el mismo nombre");
                }
            }
        }
        public void limpiarCompoentes()
        {
            //Con este método limpiamos los componentes de entrada
            txtDescripcion.Text = "";
            txtNombre.Text = "";
        }

        public void muestraPermisos()
        {
            //Con este método lo que hago es mostrar todos los permisos
            query = "exec dbo.sp_ver_permisos " + ddlFiltrarPermisos.SelectedValue + ",'" + txtfiltrarDescripcion.Text.Trim() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvPermisos.DataSource = dt;
                GvPermisos.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay permisos registrados");
            }
        }

        protected void btnVerpermisos_Click(object sender, EventArgs e)
        {
            try
            {
                pnlPermisos.Visible = true;
                muestraPermisos();
            }
            catch (Exception es)
            {
                string caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
        }

        protected void GvPermisos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Este evento se activa cuando damos clic al botón actualizar
            txtNombre.Text = GvPermisos.SelectedRow.Cells[1].Text;
            txtDescripcion.Text = GvPermisos.SelectedRow.Cells[2].Text;
            //id del permiso
            hdfid_permiso.Value = GvPermisos.SelectedDataKey.Value.ToString();
            //Activamos el botón de actualizar
            pnlupdate.Visible = true;
            PnlGuardar.Visible = false;

        }

        protected void GvPermisos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GvPermisos.PageIndex = e.NewPageIndex;
                pnlPermisos.Visible = true;
                muestraPermisos();
            }
            catch (Exception es)
            {
                string caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);

            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                actualizarPermisos();
                limpiarCompoentes();
                //Muestra en la grilla los permisos
                muestraPermisos();
                PnlGuardar.Visible = true;
                pnlupdate.Visible = false;
            }
            catch (Exception es)
            {
                string caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);

            }
        }
        public void actualizarPermisos()
        {
            query = @"sp_actualiza_permisos " + hdfid_permiso.Value + ",'" + txtNombre.Text.Trim() + "','" + txtDescripcion.Text.Trim() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                verModal("Exito", "La actualización se realizó exitosamente");
            }
            else
            {
                verModal("Alerta", "La actualización del permiso no se realizo");
            }
        }

        protected void GvPermisos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            hdfDesion.Value = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (Image button in e.Row.Cells[3].Controls.OfType<Image>())
                {
                    button.Attributes["onclick"] = "confirmar('¿Estás seguro  en eliminar el usuario del rol?');";
                }
            }
        }

        protected void GvPermisos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //Esto es para eliminar el permiso
                int id = (int)GvPermisos.DataKeys[e.RowIndex].Value;
                if (hdfDesion.Value == "true")
                {
                    eliminarPermiso(id);
                    //Muestra de nuevo los permisos
                    muestraPermisos();
                }

            }
            catch (Exception es)
            {
                string caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\n", "").Replace("\r", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }

        }

        public void eliminarPermiso(int permiso)
        {
            query = "sp_eliminar_permiso " + permiso;
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Exito", "El permiso se eliminó correctamente");
                }
                else
                {
                    verModal("Alerta", "No se eliminó el permiso");
                }
            }

        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                muestraPermisos();
            }
            catch (Exception es)
            {
                mensaje = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("\'", "").Replace("'", "");
                verModal("Error", mensaje);
            }

        }

        protected void ddlFiltrarPermisos_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarPermisos.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void chkverPermisos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkverPermisos.Checked)
                {
                    pnlPermisos.Visible = true;
                    muestraPermisos();
                }
                else
                {
                    pnlPermisos.Visible = false;
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void imbEliminar_click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse((sender as ImageButton).CommandArgument);
                eliminarPermiso(id);
                //Muestra de nuevo los permisos
                muestraPermisos();
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