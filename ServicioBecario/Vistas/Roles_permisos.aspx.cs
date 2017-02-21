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
    public partial class Roles_permisos : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, caracter;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                try
                {
                    mostrarRoles();
                    mostarPermisos();
                    filtrarRoles();
                    filtrarPermisos();
                }
                catch (Exception es)
                {
                    caracter = es.Message.ToString().Replace("\r", "").Replace("\n", "").Replace("'", "");
                    caracter = caracter.Replace("'", " ");
                    verModal("Error", caracter);
                }
            }
        }

        public void filtrarPermisos()
        {
            query = @"select id_permiso,Permiso from tbl_permisos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlfiltrarPermisos.DataValueField = "id_permiso";
                ddlfiltrarPermisos.DataTextField = "Permiso";
                ddlfiltrarPermisos.DataSource = dt;
                ddlfiltrarPermisos.DataBind();
            }
            else
            {
                verModal("Alerta", "Lo sentimos pero no hay roles disponibles");
            }
        }

        public void filtrarRoles()
        {
            query = @"select id_rol, Nombre from cat_roles where Activio =1";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlfiltrarRoles.DataValueField = "id_rol";
                ddlfiltrarRoles.DataTextField = "Nombre";
                ddlfiltrarRoles.DataSource = dt;
                ddlfiltrarRoles.DataBind();
            }
            else
            {
                verModal("Alerta", "Lo sentimos pero no hay roles disponibles");
            }
        }
        public void mostrarRoles()
        {
            query = @"select id_rol, Nombre from cat_roles where Activio =1";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                DdlRoles.DataValueField = "id_rol";
                DdlRoles.DataTextField = "Nombre";
                DdlRoles.DataSource = dt;
                DdlRoles.DataBind();
            }
            else
            {
                verModal("Alerta", "Lo sentimos pero no hay roles disponibles");
            }
        }

        protected void DdlRoles_DataBound(object sender, EventArgs e)
        {
            //Con este evento lo que hago es insertar un dato antes que se ligue el componente a la base de datos
            DdlRoles.Items.Insert(0, new ListItem("-- Seleccione rol --", ""));
        }
        public void mostarPermisos()
        {
            //Con este método mostramos todos los permisos disponibles
            dt.Clear();//Limpio el DataTable
            query = "select id_permiso,Permiso from tbl_permisos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                DdlPermisos.DataValueField = "id_permiso";
                DdlPermisos.DataTextField = "Permiso";
                DdlPermisos.DataSource = dt;
                DdlPermisos.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay roles disponibles");
            }
        }

        protected void DdlPermisos_DataBound(object sender, EventArgs e)
        {
            //Con este evento lo que hago es insertar un dato antes que se ligue el componente a la base de datos
            DdlPermisos.Items.Insert(0, new ListItem("-- Seleccione permiso --", ""));
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                gurardarRol_perfil();
                limpiarDrowdowlis();
                mostrar_roles_permisos();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("\r", "").Replace("\n", "").Replace("'", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }

        }
        public void gurardarRol_perfil()
        {
            //Con este método insertamos nuestra información en la tabla


            query = "exec sp_Guardar_Roles_Permisos  " + DdlRoles.SelectedValue.ToString() + "," + DdlPermisos.SelectedValue.ToString() + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Exito", "Se asignó  el rol con el permiso con éxito!!");
                }
                else
                {
                    verModal("Alerta", "Ya existe el rol con el permiso");
                }
            }
        }
        public void limpiarDrowdowlis()
        {
            DdlPermisos.SelectedValue = "";
            DdlRoles.SelectedValue = "";

        }
        protected void btnVerpermisos_Click(object sender, EventArgs e)
        {
            try
            {
                pnlPermisos_roles.Visible = true;
                mostrar_roles_permisos();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("\r", "").Replace("\n", "").Replace("'", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
        }

        public void mostrar_roles_permisos()
        {
            //Con este método muestro en la grilla los datos del permiso y el permiso
            query = "sp_mostrar_roles_permisos " + ddlfiltrarRoles.SelectedValue + " , " + ddlfiltrarPermisos.SelectedValue + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvPermisos_roles.DataSource = dt;
                GvPermisos_roles.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay roles con sus permisos");
            }
        }

        protected void GvPermisos_roles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Controlamos nuestro paginado con este evento
            try
            {
                GvPermisos_roles.PageIndex = e.NewPageIndex;
                mostrar_roles_permisos();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("\r", "").Replace("\n", "").Replace("'", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
        }

        protected void GvPermisos_roles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (Image button in e.Row.Cells[3].Controls.OfType<Image>())
                {
                    button.Attributes["onclick"] = "confirmar('¿Estás seguro  en eliminar el usuario del rol?');";
                }
            }
        }

        protected void GvPermisos_roles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Con este evento lo utilizamos para eliminar los permisos de los roles
            try
            {
                if (hdfDesion.Value == "true")
                {
                    int id = (int)GvPermisos_roles.DataKeys[e.RowIndex].Value;
                    elimina_rol_permiso(id);
                    mostrar_roles_permisos();
                }
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("\r", "").Replace("\n", "").Replace("'", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
            hdfDesion.Value = "";
        }

        public void elimina_rol_permiso(int id)
        {
            //Con este método elimino el rol con el permiso
            query = "sp_elimina_rol_permiso " + id;
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Resultado"].ToString() == "Ok")
                {
                    verModal("Exito", "Se eliminó correctamente el rol de permiso");
                }
                else
                {
                    verModal("Alerta", "No se puede eliminar el rol del permiso");
                }
            }

        }

        protected void ddlfiltrarPermisos_DataBound(object sender, EventArgs e)
        {
            ddlfiltrarPermisos.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void ddlfiltrarRoles_DataBound(object sender, EventArgs e)
        {
            ddlfiltrarRoles.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                mostrar_roles_permisos();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("\r", "").Replace("\n", "").Replace("'", "");
                verModal("Error", caracter);
            }
        }

        protected void chkVerpermisos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkVerpermisos.Checked)
                {
                    pnlPermisos_roles.Visible = true;
                    mostrar_roles_permisos();
                }
                else
                {
                    pnlPermisos_roles.Visible = false;
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
                elimina_rol_permiso(id);
                mostrar_roles_permisos();
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