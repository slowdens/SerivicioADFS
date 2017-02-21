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
    public partial class Rol_nuevo : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, mensaje;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    llenarCatRoles();
                    pnlMostrarGrid.Visible = true;
                    mostrarDatosGvroles();
                }
            }
            catch (Exception es)
            {

                mensaje = es.Message.ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("'", "").Replace("(", "").Replace(")", "");
                verModal("Error", mensaje);
            }
        }

        public void llenarCatRoles()
        {
            query = "select id_rol,Nombre as Rol from cat_roles_n where Activo=1";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarRol.DataValueField = "id_rol";
                ddlFiltrarRol.DataTextField = "Rol";
                ddlFiltrarRol.DataSource = dt;
                ddlFiltrarRol.DataBind();

            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Guardo el nombre de mis roles
            if (db.NumeroLetrainiciosinEspacio(txtNombre.Text))
            {
                if (db.NumeroLetrainiciosinEspacio(txtdescripcion.Text))
                {
                    query = @"sp_crear_roles_nuevo '" + txtNombre.Text + "' , '" + txtdescripcion.Text + "'";
                    dt = db.getQuery(conexionBecarios, query);
                    try
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Salida"].ToString() == "Ok")
                            {
                                verModal("Exito", "La información se guarda exitosamente");
                                txtNombre.Text = "";
                                txtdescripcion.Text = "";
                                mostrarDatosGvroles();
                            }
                            else
                            {
                                verModal("Alerta", "Ya existe un registro con los mismos datos");
                            }
                        }
                    }
                    catch (Exception es)
                    {
                        verModal("Error", es.Message.ToString());
                    }
                    //Limpio los componentes
                    limpiarComponentes();

                }
                else
                {
                    verModal("Error","El campo descripción no tiene el formato correcto");
                }
                
            }
            else
            {
                verModal("Error","El campo nombre no tiene el formato correcto");
            }
            
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            //Con este evento lo que hago es mostrar el gridView e incrusto información con el

        }

        protected void Gvroles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //Completo el auto paginado en el GridView
                Gvroles.PageIndex = e.NewPageIndex;
                //Con este método refresco el Gridview
                mostrarDatosGvroles();
            }
            catch (Exception es)
            {
                mensaje = es.Message.ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("'", "").Replace("(", "").Replace(")", "");
                verModal("Error", mensaje);
            }
        }

        protected void Gvroles_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Este evento se activa cuando damos clic al botón actualizar
            //Sacamos los datos del GridView           
            hdfid_rol.Value = Gvroles.SelectedDataKey.Value.ToString();
            txtNombre.Text = Gvroles.SelectedRow.Cells[1].Text;
            txtdescripcion.Text = Gvroles.SelectedRow.Cells[2].Text;

            //Mostramos los paneles de donde viene el botón de actualizar
            pnlactualizar.Visible = true;
            pnlguardar.Visible = false;
            ClientScript.RegisterStartupScript(GetType(), "BanderaRol", "Bandera();", true);
        }
        public void actualizarInformacionRoles(string p_nombre, string p_descripcion, string p_id)
        {
            //Función para actualizar los roles dentro de la base de datos
            query = "EXEC sp_actualiza_roles_nuevo " + p_id + ",'" + p_nombre + "','" + p_descripcion + "'";
            try
            {
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Salida"].ToString() == "Ok")
                    {
                        verModal("Exito", "La información se  modifico exitosamente");
                    }
                    else
                    {
                        verModal("Alerta", dt.Rows[0]["Salida"].ToString());
                    }
                }
            }
            catch (Exception es)
            {
                mensaje = es.Message.ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("'", "").Replace("(", "").Replace(")", "");
                verModal("Error", mensaje);
            }

        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                //Mando a llamar la función para actualizar los roles en tabla
                if (hdfDesion.Value == "true")
                {
                    actualizarInformacionRoles(txtNombre.Text, txtdescripcion.Text, hdfid_rol.Value);
                    //Muestra los roles en la tabla
                    mostrarDatosGvroles();
                }
                else
                {
                    verModal("Alerta", "El rol seguirá con la misma información!");
                }
            }
            catch (Exception es)
            {
                mensaje = es.Message.ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("'", "").Replace("(", "").Replace(")", "");
                verModal("Error", mensaje);
            }

            txtdescripcion.Text = "";
            txtNombre.Text = "";
            pnlguardar.Visible = true;
            pnlactualizar.Visible = false;
        }
        public void llenar()
        {
            if (txtfiltarFecha.Text != "")
            {
                query = "sp_mostrar_roles_nuevo " + ddlFiltrarRol.SelectedValue + ",'" + txtfiltrarDescripcion.Text + "','" + db.convertirFecha(txtfiltarFecha.Text) + "'";
            }
            else
            {
                query = "sp_mostrar_roles_nuevo " + ddlFiltrarRol.SelectedValue + ",'" + txtfiltrarDescripcion.Text + "',Null ";
            }
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                Gvroles.DataSource = dt;
                Gvroles.DataBind();
            }
            else
            {
                verModal("Alerta", "No se encontro información en la busqueda");
                Gvroles.DataSource = null;
                Gvroles.DataBind();
            }
        }
        public void mostrarDatosGvroles()
        {
            //query = "select id_rol , Nombre,Descripcion as Descripción ,Fecha_creacion as Fecha_creación  from cat_roles where Activio= 1";
            if (db.NumeroLetraConEspacio(txtfiltrarDescripcion.Text))
            {
                if (!string.IsNullOrEmpty(txtfiltarFecha.Text))
                {
                    Match match = Regex.Match(txtfiltarFecha.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                    if (match.Success)
                    {
                        llenar();
                    }
                    else
                    {
                        verModal("Error", "La fecha no tiene el formato dd/mm/aaaa");
                    }
                }
                else
                {
                    llenar();
                }
            }
            else
            {
                verModal("Error","El campo descripción no tiene el formato correcto");
            }

            
           
        }
        protected void Gvroles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ////Tomamos el id del Gridview para pasarlo como parámetro
            //int id = (int)Gvroles.DataKeys[e.RowIndex].Value;
            //if (hdfDesion.Value == "true")
            //{
            //    try
            //    {
            //        //Con este método desactivo una bandera para simulación de eliminar un registro
            //        eliminarRegistro(id.ToString());
            //        //Con este método refresco el Gridview
            //        mostrarDatosGvroles();
            //    }
            //    catch (Exception es)
            //    {
            //        mensaje = es.Message.ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("'", "").Replace("(", "").Replace(")", "");
            //        db.mensajeAlerta(mensaje, this);
            //    }
            //}
        }
        public void eliminarRegistro(string id)
        {
            query = "sp_desactiva_rol_nuevo " + id;
            try
            {
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Informacion"].ToString() == "Ok")
                    {
                        verModal("Exito", "El registro se elimino exitosamente");
                    }
                    else
                    {
                        verModal("Alerta", "El registro no se elimino");
                    }
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }

        }

        protected void Gvroles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ////Con este evento agrego la función del confirm para decidir si elimina y se ejecuta la decesion en el evento Gvroles_RowDeleting
            //hdfDesion.Value = "";
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    foreach (Image button in e.Row.Cells[5].Controls.OfType<Image>())
            //    {
            //        button.Attributes["onclick"] = "confirmar('¿Estás seguro  en eliminar el rol?');";
            //    }
            //}
        }


        public void limpiarComponentes()
        {
            txtdescripcion.Text = "";
            txtNombre.Text = "";
        }

        protected void ddlFiltrarRol_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarRol.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void btnFiltar_Click(object sender, EventArgs e)
        {
            try
            {
                mostrarDatosGvroles();
            }
            catch (Exception es)
            {
                mensaje = es.Message.ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("'", "").Replace("(", "").Replace(")", "");
                verModal("Error", mensaje);

            }
        }


        protected void imbEliminar_click(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse((sender as ImageButton).CommandArgument);
                //Con este método desactivo una bandera para simulación de eliminar un registro
                eliminarRegistro(i.ToString());
                //Con este método refresco el Gridview
                mostrarDatosGvroles();
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

        public void limpircompoentes()
        {
            txtdescripcion.Text = "";
            txtNombre.Text = "";
        }
        public void limpiaFiltros()
        {
            ddlFiltrarRol.SelectedValue = "-1";
            txtdescripcion.Text = "";
            txtfiltarFecha.Text = "";
        }
        protected void btnmodificar_Click(object sender, EventArgs e)
        {
            try
            {
                limpircompoentes();
                pnlactualizar.Visible = false;
                pnlguardar.Visible = true;
                Gvroles.SelectedIndex = -1;
                limpiaFiltros();

            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
            
        }

       
    }
}