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
    public partial class Rol_menu_nuevo : System.Web.UI.Page
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
                    llenarMenuPadre();
                    llenarRolFiltrado();
                }
                catch (Exception es)
                {
                    caracter = es.Message.ToString().Replace("\r", "").Replace("\n", "").Replace("'", "");
                    caracter = caracter.Replace("'", " ");
                    verModal("Error", caracter);
                }
                try
                {
                    pnlvistaRolesMenu.Visible = true;
                    pnlgrid.Visible = false;//oculta el grid donde se asignan los datos 
                    sacarInformacionAsignado();

                }
                catch (Exception es)
                {
                    verModal("Error", es.Message.ToString());

                }
            }
        }

        public void llenarRolFiltrado()
        {
            query = "select id_rol,Nombre from cat_roles_n where Activo =1";
            dt = db.getQuery(conexionBecarios,query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarRol.DataValueField = "id_rol";
                ddlFiltrarRol.DataTextField = "Nombre";
                ddlFiltrarRol.DataSource = dt;
                ddlFiltrarRol.DataBind();
            }
        }

        public void llenarMenuPadre()
        {
            query = @"select id_menu,Nombre from cat_menus where Padre=0";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                DdlMenuPadre.DataValueField = "id_menu";
                DdlMenuPadre.DataTextField = "Nombre";
                DdlMenuPadre.DataSource = dt;
                DdlMenuPadre.DataBind();
            }
            else
            {
                verModal("Alerta", "Lo sentimos pero no hay roles disponibles");
            }
        }
     
        public void mostrarRoles()
        {
            query = @"select id_rol, Nombre from cat_roles_n ";
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
      

        protected void DdlPermisos_DataBound(object sender, EventArgs e)
        {
            //Con este evento lo que hago es insertar un dato antes que se ligue el componente a la base de datos
            DdlMenuPadre.Items.Insert(0, new ListItem("-- Seleccione permiso --", ""));
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int id;
                bool bandera = false;
                //casamos el padre con el rol
                agregarPadre_relacion();
                foreach (GridViewRow row in gvDatos.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chseleccion = (row.Cells[3].FindControl("chkselecioname") as CheckBox);
                        if (chseleccion.Checked)
                        {
                            id = (int)gvDatos.DataKeys[row.RowIndex].Value;
                            bandera = true;
                            agregerHijos_relacion(id.ToString());
                            
                        }
                    }
                }
                if (bandera)
                {
                    verModal("Exito", "La información se almaceno correctamente!!");
                    seleccionarmmenushijos();
                }
                else
                {
                    verModal("Alerta", "Usted no selecciono ningún menú");
                }
                //Actualizamos la lista
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("\r", "").Replace("\n", "").Replace("'", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }

        }

        public void agregerHijos_relacion(string id_menu_hijo)
        {
            query = "sp_agregar_hijo_menu "+DdlRoles.SelectedValue+","+id_menu_hijo+" ";
            dt = db.getQuery(conexionBecarios,query);
            
        }

        public void agregarPadre_relacion()
        {
            query = "sp_agregar_papa_menu " + DdlMenuPadre.SelectedValue + ", " + DdlRoles.SelectedValue + "";
            dt = db.getQuery(conexionBecarios,query);           

        }


        public void gurardarRol_perfil()
        {
            //Con este método insertamos nuestra información en la tabla


            query = "exec sp_Guardar_Roles_Permisos  " + DdlRoles.SelectedValue.ToString() + "," + DdlMenuPadre.SelectedValue.ToString() + "";
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
            DdlMenuPadre.SelectedValue = "";
            DdlRoles.SelectedValue = "";

        }
        protected void btnVerpermisos_Click(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("\r", "").Replace("\n", "").Replace("'", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
        }

        
        protected void GvPermisos_roles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Controlamos nuestro paginado con este evento
            try
            {
                gvistasRolesmenus.PageIndex = e.NewPageIndex;
                sacarInformacionAsignado();
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
                    //int id = (int)GvPermisos_roles.DataKeys[e.RowIndex].Value;
                   // elimina_rol_permiso(id);
                   
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

       
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("\r", "").Replace("\n", "").Replace("'", "");
                verModal("Error", caracter);
            }
        }

        

        
        public void sacarInformacionAsignado()
        {
            if (db.NumeroLetraConEspacio(txtfiltarMenu.Text))
            {
                if(db.NumeroLetraConEspacio(txtfiltraPantalla.Text))
                {
                    if (ddlFiltrarRol.SelectedValue != "-1" && txtfiltarMenu.Text == "" && txtfiltraPantalla.Text == "")//1
                    {
                        query = "sp_sacar_menus_asignador_con_roles " + ddlFiltrarRol.SelectedValue + ",NULL,NULL";
                    }
                    if (ddlFiltrarRol.SelectedValue == "-1" && txtfiltarMenu.Text != "" && txtfiltraPantalla.Text == "")//2
                    {
                        query = "sp_sacar_menus_asignador_con_roles " + ddlFiltrarRol.SelectedValue + ",'" + txtfiltarMenu.Text + "',NULL";
                    }
                    if (ddlFiltrarRol.SelectedValue == "-1" && txtfiltarMenu.Text == "" && txtfiltraPantalla.Text != "")//3
                    {
                        query = "sp_sacar_menus_asignador_con_roles " + ddlFiltrarRol.SelectedValue + ",NULL,'" + txtfiltraPantalla.Text + "'";
                    }
                    if (ddlFiltrarRol.SelectedValue != "-1" && txtfiltarMenu.Text != "" && txtfiltraPantalla.Text == "")//4
                    {
                        query = "sp_sacar_menus_asignador_con_roles " + ddlFiltrarRol.SelectedValue + ",'" + txtfiltarMenu.Text + "',NULL";
                    }
                    if (ddlFiltrarRol.SelectedValue != "-1" && txtfiltarMenu.Text == "" && txtfiltraPantalla.Text != "")//5
                    {
                        query = "sp_sacar_menus_asignador_con_roles " + ddlFiltrarRol.SelectedValue + ",NULL,'" + txtfiltraPantalla.Text + "'";
                    }
                    if (ddlFiltrarRol.SelectedValue == "-1" && txtfiltarMenu.Text != "" && txtfiltraPantalla.Text != "")//6
                    {
                        query = "sp_sacar_menus_asignador_con_roles " + ddlFiltrarRol.SelectedValue + ",'" + txtfiltarMenu.Text + "','" + txtfiltraPantalla.Text + "'";
                    }
                    if (ddlFiltrarRol.SelectedValue != "-1" && txtfiltarMenu.Text != "" && txtfiltraPantalla.Text != "")//7
                    {
                        query = "sp_sacar_menus_asignador_con_roles " + ddlFiltrarRol.SelectedValue + ",'" + txtfiltarMenu.Text + "','" + txtfiltraPantalla.Text + "'";
                    }
                    if (ddlFiltrarRol.SelectedValue == "-1" && txtfiltarMenu.Text == "" && txtfiltraPantalla.Text == "")//8
                    {
                        query = "sp_sacar_menus_asignador_con_roles " + ddlFiltrarRol.SelectedValue + ",NULL,NULL";
                    }
                    dt = db.getQuery(conexionBecarios, query);
                    if (dt.Rows.Count > 0)
                    {
                        gvistasRolesmenus.DataSource = dt;
                        gvistasRolesmenus.DataBind();
                    }
                    else
                    {
                        gvistasRolesmenus.DataSource = null;
                        gvistasRolesmenus.DataBind();
                        verModal("Alerta","No se encontro la busqueda");
                    }
                }
                else
                {
                    verModal("Alerta","El campo pantalla no tiene el formato correcto");
                }
                
            }
            else
            {
                verModal("Alerta","El campo menú no tiene el formato correcto");
            }

            
        }

        protected void imbEliminar_click(object sender, EventArgs e)
        {
            try
            {
                //Esto el lo bueno
               int id = int.Parse((sender as ImageButton).CommandArgument);
               if(verSiEsPadre(id))
               {
                   hdfidCancelacion.Value = id.ToString();
                   verConfirmacion("Alerta","Menú principal se quitaran tambien los menus inferiores");
               }
               else
               {
                   eminar_rol_menu(id);
                   sacarInformacionAsignado();
               }
                


                //eminar_rol_menu(id);
                //sacarInformacionAsignado();
                


            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public bool verSiEsPadre(int id)
        {
            dt = null;
            bool bandera = false;
            query = "sp_validaPadre " + id + "";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                bandera = true;
            }
            return bandera;
        }

        public void eminar_rol_menu(int id_menu_rol)
        {
           
            query = "sp_eliminar_menu_rol "+id_menu_rol+"";
            dt = db.getQuery(conexionBecarios,query);
            if (dt.Rows.Count > 0)
            {
                switch(dt.Rows[0]["Mensaje"].ToString())
                {
                    case "Ok":
                        verModal("Exito", "Se elimino correctamente el registro");
                        break;
                    case "Es padre":
                        verModal("Error","Para poder borrar el menu es necesario de eliminar sus hijos del rol");
                        break;
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


        //Esta funcion imprime el msj
        public void verConfirmacion(string header, string body)
        {
            lblCabeza2.Text = header;
            lblcuerpo2.Text = body;
            mp2.Show();
        }

        protected void DdlMenuPadre_DataBinding(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }


        public void seleccionarmmenushijos()
        {
            if (DdlMenuPadre.SelectedValue!="")
            {
                query = "sp_selecciona_hijos " + DdlMenuPadre.SelectedValue + "," + DdlRoles.SelectedValue + "";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    gvDatos.DataSource = dt;
                    gvDatos.DataBind();
                }
                else
                {
                    gvDatos.DataSource = null;
                    gvDatos.DataBind();
                    verModal("Alerta", "Ya se selecciono todo los registros");
                }
                pnlgrid.Visible = true;
            }
            else
            {
                verModal("Alerta", "Debes seleccionar un menu principal");
                gvDatos.DataSource = null;
                gvDatos.DataBind();
                    
            }
            
            //pnlvistaRolesMenu.Visible = false;
        }
        protected void DdlMenuPadre_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                seleccionarmmenushijos();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());

            }
        }

        protected void ddlFiltrarCampus_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlFiltrarRol.Items.Insert(0, new ListItem("-- Seleccione rol --", "-1"));
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btnfiltrar_Click1(object sender, EventArgs e)
        {
            try
            {
                //sacar la informacion.
                sacarInformacionAsignado(); 
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void btncalcenla_Click(object sender, EventArgs e)
        {
            
        }

        protected void btncanll_Click(object sender, EventArgs e)
        {
            string id = hdfidCancelacion.Value;
            query = "sp_elimina_hijos_padre "+id+"";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                if(dt.Rows[0]["Mensaje"].ToString()=="No")
                {
                    verModal("Alerta", "No se elimino");
                }
                else
                {
                    sacarInformacionAsignado();
                }
            }

        }
    }
}