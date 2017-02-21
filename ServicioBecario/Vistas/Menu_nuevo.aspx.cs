using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using ServicioBecario.Codigo;
using System.Text;

namespace ServicioBecario.Vistas
{
    public partial class Menu_nuevo : System.Web.UI.Page
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
                    llenarDropList();
                    MostrarFiltro();
                }
                
            }
            catch { }
            
            
        }
        
        
        public void MostrarFiltro()
        {
            try
            {
                
                    llenarDropListFiltro();
                    pnlmenus.Visible = true;
                    mostrarListaMenu();
                  
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
            if (db.NumeroLetraConEspacio(txtFiltrarMenu.Text))
            {
                if (db.NumeroLetraConEspacio(txtfiltarPantalla.Text))
                {
                    if (txtListaPadrefiltro.SelectedValue == "")
                    {
                        query = "exec sp_mostrar_menus_nuevo '" + txtFiltrarMenu.Text.Trim() + "','" + txtfiltarPantalla.Text.Trim() + "', -1";
                    }
                    else
                    {
                        query = "exec sp_mostrar_menus_nuevo '" + txtFiltrarMenu.Text.Trim() + "','" + txtfiltarPantalla.Text.Trim() + "'," + txtListaPadrefiltro.SelectedValue.Trim() + "";
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
                        Gvmenu.DataSource = null;
                        Gvmenu.DataBind();
                    }
                }
                else
                {
                    verModal("Alerta", "El campo pantalla no tiene el formato correcto");
                }
            }
            else
            {
                verModal("Alerta","El campo menú no tiene el formato correcto");
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

            if (db.NumeroLetraConEspacio(txtmenu.Text))
            {
                if (db.NumeroLetraConEspacio(txtlink.Text))
                {
                    //Agrega como padre
                    if (txtListaPadre.SelectedValue == "0")
                    {

                        query = "sp_agrera_como_padre '" + txtmenu.Text.Trim() + "',NULL";
                        dt = db.getQuery(conexionBecarios, query);
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                            {
                                verModal("Éxito", "Se creo el menu como principal");
                                llenarDropList();

                            }
                            else
                            {
                                verModal("Alerta", "Ya existe un menu padre con el mismo nombre");
                            }
                        }
                    }
                    else //significa que no es un menu padre
                    {
                        query = "sp_agrega_como_hijos '" + txtmenu.Text.Trim() + "','" + txtlink.Text.Trim() + "'," + txtListaPadre.SelectedValue + "";
                        dt = db.getQuery(conexionBecarios, query);
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                            {
                                verModal("Exito", "El menu se agrego correctamente como hijo");
                            }
                            else
                            {
                                verModal("Alerta", "Ya existe un menu para ese mismo menu padre");
                            }
                        }
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
        public void limpiarComponentes()
        {
            txtlink.Text = "";
            txtmenu.Text = "";
            //   txtPadre.Text = "";
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
            query = "sp_eliminar_menus_nuevo " + id;
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                switch(dt.Rows[0]["Mensaje"].ToString())
                {
                    case "Ok":
                        verModal("Éxito","Se quito correctamente el registro");
                        break;
                    case "Padre":
                        verModal("Alerta", "No se pude quitar este menu hasta que se quiten los menus hijos");    
                        break;

                }
                mostrarListaMenu();
            }
        }

        protected void Gvmenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClientScript.RegisterStartupScript(GetType(), "Banderas", "Bandera();", true);
                hdfid_permiso.Value = Gvmenu.SelectedDataKey.Value.ToString();
                txtmenu.Text = HttpUtility.HtmlDecode(Gvmenu.SelectedRow.Cells[1].Text);
                if (Gvmenu.SelectedRow.Cells[2].Text == "N/A")
                {
                    txtlink.Text = "";
                    txtListaPadre.Enabled = false;                    
                }
                else
                {
                    txtlink.Text = Gvmenu.SelectedRow.Cells[2].Text;
                    //  System.Text.Encoding.UTF8.


                    //convierte en acentos
                    string convertido = Gvmenu.SelectedRow.Cells[3].Text;
                    convertido = HttpUtility.HtmlDecode(convertido);


                    txtListaPadre.SelectedValue = txtListaPadre.Items.FindByText(convertido).Value;
                    txtListaPadre.Enabled = true;
                }
                
                pnlActualizar.Visible = true;
                pnlAgregar.Visible = false;
            }catch(Exception es){
                verModal("Error",es.Message.ToString());
            }
           
            
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                actulizarInformacion();
                mostrarListaMenu();
                txtListaPadre.Enabled = true;
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\n", "").Replace("\r", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
            txtlink.Text = "";
            txtmenu.Text = "";
            pnlActualizar.Visible = false;
            pnlAgregar.Visible = true;
            txtListaPadre.SelectedValue = "0";
            txtListaPadre.Enabled = true;
        }
        public void actulizarInformacion()
        {
            if (txtlink.Text == "" && txtListaPadre.Enabled==false)
           {
               query = "sp_actuliza_menus_nuevos " + hdfid_permiso.Value + ",'" + txtmenu.Text + "',NULL,0";
           }
           else
           {
               query = "sp_actuliza_menus_nuevos " + hdfid_permiso.Value + ",'" + txtmenu.Text + "','" + txtlink.Text + "'," + txtListaPadre.SelectedValue + "";
           }
            
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
            {
                verModal("Éxito", "El menú se actualizó satisfactoriamente");
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
        public void llenarDropList()
        {
            query = @"SELECT id_menu,Nombre FROM cat_menus WHERE Padre='0'
                    ORDER BY id_menu DESC";                                 
            dt = db.getQuery(conexionBecarios, query);

            txtListaPadre.DataSource = dt;
            txtListaPadre.DataValueField = "id_menu";
            txtListaPadre.DataTextField = "Nombre";
            txtListaPadre.DataBind();
        }
        public void llenarDropListFiltro()
        {
            query = "SELECT id_menu,Nombre FROM cat_menus WHERE Padre=0";
            
            dt = db.getQuery(conexionBecarios, query);

            txtListaPadrefiltro.DataSource = dt;
            txtListaPadrefiltro.DataValueField = "id_menu";
            txtListaPadrefiltro.DataTextField = "Nombre";
            txtListaPadrefiltro.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtlink.Text = "";
            txtmenu.Text = "";
            pnlActualizar.Visible = false;
            pnlAgregar.Visible = true;
            txtListaPadre.SelectedValue = "0";
            txtListaPadre.Enabled = true;
           
        }

        protected void txtListaPadre_DataBound(object sender, EventArgs e)
        {
            txtListaPadre.Items.Insert(0, new ListItem("-- Seleccione --", ""));
            txtListaPadre.Items.Insert(1, new ListItem("!!Crear menu principal!!" ,"0"));
        }

        protected void txtListaPadrefiltro_DataBound(object sender, EventArgs e)
        {
            txtListaPadrefiltro.Items.Insert(0, new ListItem("-- Seleccione --", ""));

        }

    }
}