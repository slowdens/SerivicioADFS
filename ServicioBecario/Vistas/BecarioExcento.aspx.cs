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
    public partial class BecarioExcento : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, cadena;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        int con = 1;
        bool vacio = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    llenarPeriodo();
                    llenarPeriodoFiltrado();
                    pnlMostrar.Visible = true;
                  
                }

            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void txtMatricula_TextChanged(object sender, EventArgs e)
        {
            string matricula = txtMatricula.Text.ToUpper();
            if (matricula!="")
            {
                if (!matricula.Contains("A"))
                {
                    matricula = "A" + matricula;
                    txtMatricula.Text = matricula;
                }
                else
                {
                    txtMatricula.Text = matricula.ToUpper();
                }
                sacarNombreAlumno(txtMatricula.Text);
            }
            


        }

        public void sacarNombreAlumno(string matricula)
        {
            if (db.matriculaSinEspacio(matricula))
            {
                lblNombre.Text = "";
                query = "select Nombre +' ' + Apellido_paterno +' '+ Apellido_materno As Dato from tbl_alumnos where Matricula='" + matricula + "'";
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    lblNombre.Text = dt.Rows[0]["Dato"].ToString();
                }
                else
                {
                    verModal("Alerta", "Lo sentimos pero no se encontró el alumno");
                }
            }
            else
            {
                verModal("Error","El campo matrícula no tiene el formato correcto");
            }
            
        }

        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }
        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        public void llenarPeriodo()
        {
            query = "select Periodo,Descripcion  from cat_periodos where Activo=1";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay  periodos activos");
            }
        }
        public void llenarPeriodoFiltrado()
        {
            query = "select Periodo,Descripcion  from cat_periodos where Activo=1";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarPeriodo.DataValueField = "Periodo";
                ddlFiltrarPeriodo.DataTextField = "Descripcion";
                ddlFiltrarPeriodo.DataSource = dt;
                ddlFiltrarPeriodo.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay  periodos activos");
            }
        }

        protected void ddlFiltrarPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarPeriodo.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

       

        public void llenarGrid()
        {
            bool bandera1=false,bandera2 = false;
            Match match;
            if (db.matriculaConEspacio(txtFiltrarMatricula.Text))
            {
                bandera1 = true;
            }
            else
            {
                verModal("Error", "El campo matrícula no tiene el formato correcto");
                bandera1 = false;
            }
            if(!string.IsNullOrEmpty(txtFiltarFechaIncio.Text))
            {
                match = Regex.Match(txtFiltarFechaIncio.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                if(match.Success)
                {
                    bandera1 = true;
                    vacio = true;
                }
                else
                {
                    verModal("Error","La fecha inicio no tiene el formato dd/mm/aaaa");
                }
            }
            else
            {
                bandera1 = true;
            }


            if (!string.IsNullOrEmpty(txtFiltrarFechaFin.Text))
            {
                match = Regex.Match(txtFiltrarFechaFin.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                if (match.Success)
                {
                    bandera2 = true;
                    if(vacio)
                    {
                        if (Convert.ToDateTime(txtFiltarFechaIncio.Text) <= Convert.ToDateTime(txtFiltrarFechaFin.Text))
                        {
                            bandera2 = true;
                        }
                        else
                        {
                            verModal("Error", "La fecha inicio no puede ser mayor a la fecha fin");
                            bandera2 = false;
                        }

                    }
                }
                else
                {
                    verModal("Alerta","La fecha inicio no tiene el formato dd/mm/aaaa");
                }
            }
            else
            {
                bandera2 = true;
            }

            


            if(bandera1 && bandera2)
            {
                if (txtFiltrarMatricula.Text != "" && ddlFiltrarPeriodo.SelectedValue == "-1" && txtFiltarFechaIncio.Text == "" && txtFiltrarFechaFin.Text == "")//1
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",null,null";
                }
                if (txtFiltrarMatricula.Text == "" && ddlFiltrarPeriodo.SelectedValue != "-1" && txtFiltarFechaIncio.Text == "" && txtFiltrarFechaFin.Text == "")//2
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",null,null ";
                }
                if (txtFiltrarMatricula.Text == "" && ddlFiltrarPeriodo.SelectedValue == "-1" && txtFiltarFechaIncio.Text != "" && txtFiltrarFechaFin.Text == "")//3
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",'" + db.convertirFecha(txtFiltarFechaIncio.Text) + "', null";
                }
                if (txtFiltrarMatricula.Text == "" && ddlFiltrarPeriodo.SelectedValue == "-1" && txtFiltarFechaIncio.Text == "" && txtFiltrarFechaFin.Text != "")//4
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",null ,'" + db.convertirFecha(txtFiltrarFechaFin.Text) + "'";
                }
                if (txtFiltrarMatricula.Text != "" && ddlFiltrarPeriodo.SelectedValue != "-1" && txtFiltarFechaIncio.Text == "" && txtFiltrarFechaFin.Text == "")//5
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",null ,null";
                }
                if (txtFiltrarMatricula.Text != "" && ddlFiltrarPeriodo.SelectedValue == "-1" && txtFiltarFechaIncio.Text != "" && txtFiltrarFechaFin.Text == "")//6
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",'" + db.convertirFecha(txtFiltarFechaIncio.Text) + "' ,null";
                }
                if (txtFiltrarMatricula.Text != "" && ddlFiltrarPeriodo.SelectedValue == "-1" && txtFiltarFechaIncio.Text == "" && txtFiltrarFechaFin.Text != "")//7
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",null ,'" + db.convertirFecha(txtFiltrarFechaFin.Text) + "'";
                }
                if (txtFiltrarMatricula.Text == "" && ddlFiltrarPeriodo.SelectedValue != "-1" && txtFiltarFechaIncio.Text != "" && txtFiltrarFechaFin.Text == "")//8
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",'" + db.convertirFecha(txtFiltarFechaIncio.Text) + "' ,null";
                }
                if (txtFiltrarMatricula.Text == "" && ddlFiltrarPeriodo.SelectedValue != "-1" && txtFiltarFechaIncio.Text == "" && txtFiltrarFechaFin.Text != "")//9
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",null ,'" + db.convertirFecha(txtFiltrarFechaFin.Text) + "'";
                }
                if (txtFiltrarMatricula.Text == "" && ddlFiltrarPeriodo.SelectedValue == "-1" && txtFiltarFechaIncio.Text != "" && txtFiltrarFechaFin.Text != "")//10
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",'" + db.convertirFecha(txtFiltarFechaIncio.Text) + "' ,'" + db.convertirFecha(txtFiltrarFechaFin.Text) + "'";
                }
                if (txtFiltrarMatricula.Text != "" && ddlFiltrarPeriodo.SelectedValue != "-1" && txtFiltarFechaIncio.Text != "" && txtFiltrarFechaFin.Text == "")//11
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",'" + db.convertirFecha(txtFiltarFechaIncio.Text) + "' , null";
                }
                if (txtFiltrarMatricula.Text != "" && ddlFiltrarPeriodo.SelectedValue != "-1" && txtFiltarFechaIncio.Text == "" && txtFiltrarFechaFin.Text != "")//12
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",null, '" + db.convertirFecha(txtFiltrarFechaFin.Text) + "'";
                }
                if (txtFiltrarMatricula.Text == "" && ddlFiltrarPeriodo.SelectedValue != "-1" && txtFiltarFechaIncio.Text != "" && txtFiltrarFechaFin.Text != "")//13
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",'" + db.convertirFecha(txtFiltarFechaIncio.Text) + "', '" + db.convertirFecha(txtFiltrarFechaFin.Text) + "'";
                }
                if (txtFiltrarMatricula.Text != "" && ddlFiltrarPeriodo.SelectedValue == "-1" && txtFiltarFechaIncio.Text != "" && txtFiltrarFechaFin.Text != "")//14
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",'" + db.convertirFecha(txtFiltarFechaIncio.Text) + "', '" + db.convertirFecha(txtFiltrarFechaFin.Text) + "'";
                }
                if (txtFiltrarMatricula.Text != "" && ddlFiltrarPeriodo.SelectedValue != "-1" && txtFiltarFechaIncio.Text != "" && txtFiltrarFechaFin.Text != "")//15
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",'" + db.convertirFecha(txtFiltarFechaIncio.Text) + "', '" + db.convertirFecha(txtFiltrarFechaFin.Text) + "'";
                }
                if (txtFiltrarMatricula.Text == "" && ddlFiltrarPeriodo.SelectedValue == "-1" && txtFiltarFechaIncio.Text == "" && txtFiltrarFechaFin.Text == "")//15
                {
                    query = "sp_muestra_alumnos_exentos '" + txtFiltrarMatricula.Text + "'," + ddlFiltrarPeriodo.SelectedValue + ",null,null ";
                }

                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    GVMostrar.DataSource = dt;
                    GVMostrar.DataBind();
                }
                else
                {
                    verModal("Alerta", "No hay información existente");
                    GVMostrar.DataSource = null;
                    GVMostrar.DataBind();

                }
            }
            
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {

                //if (db.matriculaConEspacio(txtFiltrarMatricula.Text))
                //{
                //    if (!string.IsNullOrEmpty(txtFiltarFechaIncio.Text))
                //    {
                //        if (!string.IsNullOrEmpty(txtFiltrarFechaFin.Text))
                //        {
                //            Match match = Regex.Match(txtFiltarFechaIncio.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                //            if (match.Success)
                //            {
                //                match = Regex.Match(txtFiltrarFechaFin.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                //                if (match.Success)
                //                {
                //                    if (Convert.ToDateTime(txtFiltarFechaIncio.Text) <= Convert.ToDateTime(txtFiltrarFechaFin.Text))
                //                    {
                //                        llenarGrid();
                //                    }
                //                    else
                //                    {
                //                        verModal("Error", "La fecha inicio es mayor que la fecha fin");
                //                    }
                //                }
                //                else
                //                {
                //                    verModal("Error", "La fecha fin no tiene el formato correcto dd/mm/aaaa");
                //                }
                //            }
                //            else
                //            {
                //                verModal("Error", "La fecha fin no tiene el formato correcto dd/mm/aaaa");
                //                txtFiltarFechaIncio.Focus();
                //            }

                //        }
                //        else
                //        {
                //            Match match = Regex.Match(txtFiltrarFechaFin.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                //            if (match.Success)
                //            {
                //            llenarGrid();
                //            }
                //            else
                //            {
                //                verModal("Error", "La fecha fin no tiene el formato correcto dd/mm/aaaa");
                //            }
                //        }
                //    }
                //    else if (!string.IsNullOrEmpty(txtFiltrarFechaFin.Text))
                //    {
                //        Match match = Regex.Match(txtFiltrarFechaFin.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                //        if (match.Success)
                //        {
                //            llenarGrid();
                //        }
                //        else
                //        {
                //            verModal("Error", "La fecha fin no tiene el formato correcto dd/mm/aaaa");
                //        }
                //    }
                //    else
                //    {
                //        llenarGrid();
                //    }
                //}
                //else
                //{
                //    verModal("Error", "El campo matrícula no tiene el formato correcto");
                //}

                llenarGrid();





                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void GVMostrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hd_id.Value = GVMostrar.SelectedDataKey.Value.ToString();
                txtMatricula.Text = GVMostrar.SelectedRow.Cells[1].Text;
                ddlPeriodo.SelectedValue = ddlPeriodo.Items.FindByText(GVMostrar.SelectedRow.Cells[2].Text.Trim()).Value;
                txtFechaInico.Text = GVMostrar.SelectedRow.Cells[3].Text;
                txtFechaFin.Text = GVMostrar.SelectedRow.Cells[4].Text;
                txtJustificacion.Text = GVMostrar.SelectedRow.Cells[5].Text;
                btnActualizar.Visible = true;
                btnGuardar.Visible = false;
                sacarNombreAlumno(txtMatricula.Text);
                txtMatricula.Enabled = false;
                btnCancelar.Visible = true;
                ClientScript.RegisterStartupScript(GetType(), "BanderaNSB", "Bandera();", true);
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
                if (db.matriculaSinEspacio(txtMatricula.Text))
                {
                    if (lblNombre.Text != "")
                    {
                        Match match = Regex.Match(txtFechaInico.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                        if (match.Success)
                        {
                            match = Regex.Match(txtFechaFin.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                            if (match.Success)
                            {
                                if (Convert.ToDateTime(txtFechaInico.Text) <= Convert.ToDateTime(txtFechaFin.Text))
                                {
                                    guardarInformacion();
                                    llenarGrid();
                                    limpiarControles();
                                }
                                else
                                {
                                    verModal("Error", "La fecha inicio es mayor que la fecha fin");
                                }
                            }
                            else
                            {
                                verModal("Error", "La fecha fin no tiene el formato correcto dd/mm/aaaa");
                            }
                        }
                        else
                        {
                            verModal("Error", "La fecha fin no tiene el formato correcto dd/mm/aaaa");
                            txtFechaInico.Focus();
                        }



                    }
                    else
                    {
                        verModal("Alerta", "Lo sentimos pero no se puede guardar la información, ya que no existe la matrícula");
                    }

                }
                else
                {
                    verModal("Error","El campo matrícula no tiene el formato correcto");
                }
                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

       
        public void guardarInformacion()
        {
            query = "Exec sp_insertar_becario_excento '" + txtMatricula.Text.Trim() + "'," + ddlPeriodo.SelectedValue + ",'" + db.convertirFecha(txtFechaInico.Text.Trim()) + "','" + db.convertirFecha(txtFechaFin.Text.Trim()) + "','" + txtJustificacion.Text.Trim() + "','" + Session["Usuario"].ToString().Trim() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Exito", "Se guardó correctamente el registro");
                }
                else
                {
                    verModal("Alerta", "El registro ya existe");
                    limpiarControles();
                }
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblNombre.Text != "")
                {
                    actualizarInformacion();
                    llenarGrid();
                    txtMatricula.Enabled = true;
                    limpiar_componentes();
                    btnGuardar.Visible = true;
                    btnActualizar.Visible = false;
                    btnCancelar.Visible = false;
                }
                else
                {
                    verModal("Error", "Lo sentimos pero si la matrícula no se encuentra no se podrá actualizar la información");
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

         public void limpiar_componentes()
        {
            txtMatricula.Text = "";
            lblNombre.Text = "";
            ddlPeriodo.SelectedValue = "";
            txtFechaFin.Text = "";
            txtFechaInico.Text = "";
            txtJustificacion.Text = "";
        }


        public void actualizarInformacion()
        {
            query = "Sp_actuliza_becarios_excentos " + hd_id.Value + ",'" + txtMatricula.Text + "'," + ddlPeriodo.SelectedValue + ",'" + db.convertirFecha(txtFechaInico.Text) + "','" + db.convertirFecha(txtFechaFin.Text) + "','" + txtJustificacion.Text + "','" + Session["Usuario"].ToString() + "' ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Exito", "El registro se actualizó correctamente");
                }                
                else
                {
                    verModal("Error", "No se puede actualizar el registro");
                }
            }
        }

        protected void GVMostrar_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //try
            //{
            //    int id = (int)GVMostrar.DataKeys[e.RowIndex].Value;
            //    if (hdfDesion.Value == "true")
            //    {
            //        eliminarRegistro(id);
            //        llenarGrid();
            //    }

            //}catch(Exception es )
            //{
            //    verModal("Error",es.Message.ToString());
            //}
        }

        //ESTA FUNCION ES PARA ELIMINAR REGISTROS
        public void eliminarRegistro(int id)
        {
            query = "Sp_eliminar_becarios_excetos " + id + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Exito", "El registro se borró con éxito");
                }
                else
                {
                    verModal("Alerta", "No se borró el registro");
                }
            }
        }
        protected void GVMostrar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ////Con este evento agrego la función del confirm para decidir si elimina y se ejecuta la decesion en el evento Gvroles_RowDeleting
            //hdfDesion.Value = "";
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    foreach (Image button in e.Row.Cells[9].Controls.OfType<Image>())
            //    {
            //        button.Attributes["onclick"] = "confirmar('¿Estás seguro  en eliminar la matricula de excentos?');";
            //    }
            //}

        }

        protected void imbEliminar_click(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse((sender as ImageButton).CommandArgument);
                eliminarRegistro(i);
                llenarGrid();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }

        }

        public void limpiarControles()
        {
            txtMatricula.Text = "";
            lblNombre.Text = "";
            ddlPeriodo.SelectedValue = "";
            txtFechaInico.Text = "";
            txtFechaFin.Text = "";
            txtJustificacion.Text = "";
        }

        public void limpiarfiltros()
        {
            txtFiltrarMatricula.Text = "";
            ddlFiltrarPeriodo.SelectedValue = "-1";
            txtFiltarFechaIncio.Text = "";
            txtFechaFin.Text = "";
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                btnCancelar.Visible = false;
                btnActualizar.Visible = false;
                btnGuardar.Visible = false;
                limpiarControles();      
                limpiarfiltros();
                btnGuardar.Visible = true;
                txtMatricula.Enabled = true;
                GVMostrar.SelectedIndex = -1;
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }
    }
}