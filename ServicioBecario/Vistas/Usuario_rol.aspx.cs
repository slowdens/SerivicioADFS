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
    public partial class Usuario_rol : System.Web.UI.Page
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
                    selectRoles();
                    llenarRoles();
                    llenarCampus();
                }
                catch (Exception es)
                {
                    verModal("Errór", es.Message.ToString());
                }
            }
        }
        public void llenarCampus()
        {
            query = "select id_campus,Nombre as Campus  from cat_campus where id_campus!=3";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataValueField = "id_campus";
                ddlCampus.DataTextField = "Campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }

        public void llenarRoles()
        {
            query = "select id_rol,Nombre as Rol from cat_roles where Activio=1";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlfiltrarRol.DataValueField = "id_rol";
                ddlfiltrarRol.DataTextField = "Rol";
                ddlfiltrarRol.DataSource = dt;
                ddlfiltrarRol.DataBind();
            }
        }

        protected void txtNomina_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Con el método establece_nombre busca en la bd la nómina y pone nombre
                string cadena = txtNomina.Text.ToLower().Trim();
                if (!String.IsNullOrEmpty(cadena))
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

                establecerNombre();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
        }
        public void establecerNombre()
        {
            //agreganominaDirecta();
            query = "exec dbo.sp_nombre_empleado '" + txtNomina.Text.ToUpper().Trim() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                lblNombre.Text = dt.Rows[0]["Completo"].ToString();
            }
            else
            {
                lblNombre.Text = "";
                txtNomina.Text = "";
                verModal("Alerta", "No se encontró la nomina :" + txtNomina.Text.ToUpper().ToString() + "");
            }
        }
        public DataTable agreganominaDirecta(string nomina)
        {
            DataTable res = db.infoEmpleados(nomina);

            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    if (res.Rows[0]["Nomina"].ToString() != "Nada")
                    {
                        query = "sp_inserta_nomina_directa '" + res.Rows[0]["Nomina"].ToString().Trim().ToUpper() + "','" + res.Rows[0]["Nombres"].ToString().Trim().ToUpper() + "','" + res.Rows[0]["Apaterno"].ToString().Trim().ToUpper() + "','" + res.Rows[0]["Amaterno"].ToString().Trim().ToUpper() + "','" + res.Rows[0]["Correo"].ToString().Trim() + "','" + res.Rows[0]["Divicion"].ToString().Trim().ToUpper() + "','" + res.Rows[0]["UFisica"].ToString().Trim().ToUpper() + "','" + res.Rows[0]["Puesto"].ToString().Trim() + "','" + res.Rows[0]["Campus"].ToString().Trim().ToUpper() + "','" + res.Rows[0]["Extencion"].ToString().Trim().ToUpper() + "','" + res.Rows[0]["Estatus"].ToString().Trim() + "','" + res.Rows[0]["Departamento"].ToString().Trim() + "'," + res.Rows[0]["Grupo"].ToString().Trim() + "," + res.Rows[0]["Area"].ToString().Trim() + "";
                        dt = db.getQuery(conexionBecarios, query);
                        lblNombre.Text = res.Rows[0]["Nombres"].ToString() + " " + res.Rows[0]["Apaterno"].ToString() + " " + res.Rows[0]["Amaterno"].ToString();
                    }
                    else
                    {
                        verModal("Alerta", "No hay información disponible  de la nómina");
                    }

                }
                else
                {
                    //Validamos si no esta nomina
                    verModal("Error", "La nomina no existe");
                }
            }
            return dt;
        }
        public void selectRoles()
        {
            string query = "select id_rol,Nombre from cat_roles where Activio=1";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                DdlListaRoles.DataSource = dt;
                DdlListaRoles.DataTextField = "Nombre";
                DdlListaRoles.DataValueField = "id_rol";
                DdlListaRoles.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay roles disponibles");

            }
        }

        protected void btnVerRolUsuario_Click(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Con este evento agregamos en la tabla el usuario y perfil que decidamos darle
            query = "sp_crea_Rol_usuario '" + txtNomina.Text.Trim() + "'," + DdlListaRoles.SelectedValue.ToString().Trim();
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["salida"].ToString() == "Ok")
                {
                    //db.mensajeAlerta("Se asignó correctamente la el rol a la nomina: " + txtNomina.Text, this);
                    verModal("Exito", "Se asignó correctamente la el rol a la nomina: " + txtNomina.Text);
                    //Actualizo el gridview para mostrar los usuarios ligados a su rol
                    roles_usuarios();
                }
                else
                {
                    verModal("Alerta", "No se permite tener más de un rol en una nomina");
                }
            }
            limpiaComponentes();
        }

        public void roles_usuarios()
        {
            //Con este método sacamos todos los usuarios ligados al rol            
            query = "exec sp_usuario_rol_filtrado " + ddlfiltrarRol.SelectedValue + " ,'" + txtFiltraNomimina.Text + "'," + ddlCampus.SelectedValue + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvRolesUsuarios.DataSource = dt;
                GvRolesUsuarios.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay roles ligados a usuarios");
            }
        }

        protected void GvRolesUsuarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = (int)GvRolesUsuarios.DataKeys[e.RowIndex].Value;
                //Checo las repuestas si desea eliminar o no 
                if (hdfDesion.Value == "true")
                {
                    //Con este método elimino  elimino al usuario del rol
                    eliminar_usuario_rol(id.ToString());
                    //Actualizo el gridview para mostrar los usuarios ligados a su rol
                    roles_usuarios();
                }
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }

        }
        public void eliminar_usuario_rol(string id)
        {
            query = "exec sp_elimina_usuario_rol " + id;
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Salida"].ToString() == "Ok")
                {
                    verModal("Exito", "Se eliminó correctamente el rol del usuario");
                }
                else
                {
                    verModal("Alerta", "No se puede pudo eliminar el rol de usuario");
                }
            }
        }
        protected void GvRolesUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            hdfDesion.Value = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (Image button in e.Row.Cells[6].Controls.OfType<Image>())
                {
                    button.Attributes["onclick"] = "confirmar('¿Estás seguro  en eliminar el usuario del rol?');";
                }
            }
        }

        protected void DdlListaRoles_DataBound(object sender, EventArgs e)
        {
            //Con este evento lo que hago es insertar un dato antes que se ligue el componente a la base de datos
            DdlListaRoles.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        protected void GvRolesUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GvRolesUsuarios.PageIndex = e.NewPageIndex;
                //Actualizo el gridview para mostrar los usuarios ligados a su rol
                roles_usuarios();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }

        }
        public void limpiaComponentes()
        {
            txtNomina.Text = "";
            DdlListaRoles.SelectedValue = "";
            lblNombre.Text = "";
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void btnFiltar_Click(object sender, EventArgs e)
        {
            try
            {
                roles_usuarios();
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("\n", "").Replace("\t", "").Replace("\r", "");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }
        }

        protected void ddlfiltrarRol_DataBound(object sender, EventArgs e)
        {
            ddlfiltrarRol.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void txtFiltraNomimina_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Con el método establece_nombre busca en la bd la nómina y pone nombre
                string cadena = txtFiltraNomimina.Text.ToLower().Trim();
                if (!String.IsNullOrEmpty(cadena))
                {
                    if (cadena.Contains("l") || cadena.Contains("L"))
                    {
                        txtFiltraNomimina.Text = txtFiltraNomimina.Text.ToUpper();
                    }
                    else
                    {
                        txtFiltraNomimina.Text = "L" + txtFiltraNomimina.Text;
                    }
                }

            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("\n", "").Replace("\t", "").Replace("\r", "");
                caracter = caracter.Replace("'", " ");
                //db.mensajeAlerta(caracter, this);
                verModal("Error", caracter);
            }
        }

        protected void chkver_CheckedChanged(object sender, EventArgs e)
        {

            try
            {
                if (chkver.Checked)
                {
                    pnlRolesUsuarios.Visible = true;
                    roles_usuarios();
                    limpiaComponentes();
                }
                else
                {
                    pnlRolesUsuarios.Visible = false;
                }
            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ");
                caracter = caracter.Replace("'", " ");
                verModal("Error", caracter);
            }

        }
        protected void imbEliminar_click(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse((sender as ImageButton).CommandArgument);
                //Con este método elimino  elimino al usuario del rol
                eliminar_usuario_rol(i.ToString());
                //Actualizo el gridview para mostrar los usuarios ligados a su rol
                roles_usuarios();
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

        protected void btnAgregarEmpleado_Click(object sender, EventArgs e)
        {
            try
            {

                dt = agreganominaDirecta(txtAgragerNomina.Text.ToString());
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        switch (dt.Rows[0]["Mensaje"].ToString())
                        {
                            case "Ok":
                                pnlAgregarNuevo.Visible = false;
                                chkAgregar.Checked = false;
                                txtNomina.Text = txtAgragerNomina.Text;
                                txtAgragerNomina.Text = "";
                                verModal("Existo", "La nómina se agregó correctamente al sistema");
                                break;
                            case "Existe":
                                txtNomina.Text = txtAgragerNomina.Text;
                                txtAgragerNomina.Text = "";
                                pnlAgregarNuevo.Visible = false;
                                chkAgregar.Checked = false;
                                verModal("Alerta", "La nómina: " + txtAgragerNomina.Text + " ya estaba registrada en el sistema");
                                break;
                            case "Error":
                                verModal("Error", "Error interno");
                                break;
                        }

                    }
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        protected void chkAgregar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAgregar.Checked)
            {
                pnlAgregarNuevo.Visible = true;
            }
            else
            {
                pnlAgregarNuevo.Visible = false;
            }
        }

        protected void GvRolesUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DdlListaRoles.SelectedValue = DdlListaRoles.Items.FindByText(GvRolesUsuarios.SelectedRow.Cells[1].Text).Value;
                txtNomina.Text=  GvRolesUsuarios.SelectedRow.Cells[2].Text;
                lblNombre.Text = GvRolesUsuarios.SelectedRow.Cells[3].Text;
                btnGuardar.Visible = false;
                btnEditar.Visible = true;

            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                query = "sp_editar_rol_usuario '"+txtNomina.Text.Trim()+"',"+DdlListaRoles.SelectedValue+" ";
                dt = db.getQuery(conexionBecarios,query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                    {
                        verModal("Exito","Se edito correctamente el rol a la nómina");
                        btnEditar.Visible = false;
                        btnGuardar.Visible = true;
                        roles_usuarios();
                    }
                    else
                    {
                        verModal("Error", "Sucedio un error dentro del sistema");
                    }
                }
                
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

    }
}