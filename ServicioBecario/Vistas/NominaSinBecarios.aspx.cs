using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServicioBecario.Codigo;
using System.Text.RegularExpressions;

namespace ServicioBecario.Vistas
{
    public partial class NominaSinBecarios : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, caracter;
        DataTable dt,dr;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                try
                {
                    pnlGridview.Visible = true;
                    mostrarRegistros();
                }
                catch (Exception es)
                {
                    verModal("Error", es.Message.ToString());
                }
            }
        }
        protected void imbEliminar_click(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse((sender as ImageButton).CommandArgument);
                //Con este método elimino  elimino al usuario del rol
                query = "delete from tbl_nomina_sin_becarios where id_nomina_sin_becario="+ i ;
                db.getQuery(conexionBecarios,query);
                mostrarRegistros();
                limpiarComponentes();
                btnCancelar.Visible = false;
                btnModificar.Visible = false;
                txtNomina.Enabled = true;

            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
    
        protected void txtNomina_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Con el método establece_nombre busca en la bd la nómina y pone nombre
                string cadena = txtNomina.Text.ToLower().Trim();
                lblcuerpo.Text = "";
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
                    agreganominaDirecta(txtNomina.Text.ToString());
                }                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


        //Metodo con el que guarda nomina si no encuentra en la base de datos la encuentra en el ws
        public void agreganominaDirecta(string nomina)
        {
            DataTable res;
            string ambiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
            int campus = 0;
            query = "sp_saca_informacion_del_empleado '" + nomina + "'";
            res = db.getQuery(conexionBecarios, query);
            if (res.Rows.Count > 0)
            {
                if (res.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    lblNombreSolicitante.Text = db.formatoEscritura( res.Rows[0]["NombreEmpleado"].ToString());
                    btnGuardar.Visible = true;
                }
                else
                {

                    switch(ambiente)
                    {
                        case "pprd":

                            string hola = "";
                            res = db.infoEmpleados(nomina);
                            if (res.Rows[0]["Nombres"].ToString() != "Nada")
                            {
                                campus= int.Parse(res.Rows[0]["Campus"].ToString().Trim());
                                query = "sp_inserta_nomina_directa_nuevo '" + db.formatoEscritura( res.Rows[0]["Nomina"].ToString()) + "','" + db.formatoEscritura(res.Rows[0]["Nombres"].ToString()) + "','" + db.formatoEscritura(res.Rows[0]["Apaterno"].ToString()) + "','" + db.formatoEscritura(res.Rows[0]["Amaterno"].ToString()) + "','" + res.Rows[0]["Correo"].ToString().Trim() + "','" + db.formatoEscritura(res.Rows[0]["Divicion"].ToString()) + "','" + res.Rows[0]["UFisica"].ToString() + "','" + db.formatoEscritura(res.Rows[0]["Puesto"].ToString()) + "','" + campus + "','" + res.Rows[0]["Extencion"].ToString() + "','" + res.Rows[0]["Estatus"].ToString().Trim() + "','" + db.formatoEscritura(res.Rows[0]["Departamento"].ToString()) + "'," + res.Rows[0]["Grupo"].ToString().Trim() + "," + res.Rows[0]["Area"].ToString().Trim() + "";
                                dt = db.getQuery(conexionBecarios, query);
                                if(dt.Rows.Count>0)
                                {
                                    switch(dt.Rows[0]["Mensaje"].ToString())
                                    {
                                        case "Ok":
                                            lblNombreSolicitante.Text = db.formatoEscritura(res.Rows[0]["NombreCompleto"].ToString());
                                            btnGuardar.Visible = true;
                                            break;
                                        case "Existe":
                                            break;
                                        default:
                                            verModal("Alerta",dt.Rows[0]["Mensaje"].ToString());
                                            break;

                                    }

                            
                                }
                        
                            }
                            else
                            {
                                verModal("Alerta", "La nómina no existe o no esta registrada");
                                lblNombreSolicitante.Text = "";
                                btnGuardar.Visible = false;
                            }

                            break;
                        case "prod":
                            res = db.informacionEmpleadosProduccion(nomina);
                            if (res.Rows[0]["Nombres"].ToString() != "Nada")
                            {
                                campus= int.Parse(res.Rows[0]["Campus"].ToString().Trim());
                                query = "sp_inserta_nomina_directa_nuevo '" + db.formatoEscritura( res.Rows[0]["Nomina"].ToString()) + "','" + db.formatoEscritura(res.Rows[0]["Nombres"].ToString()) + "','" + db.formatoEscritura(res.Rows[0]["Apaterno"].ToString()) + "','" + db.formatoEscritura(res.Rows[0]["Amaterno"].ToString()) + "','" + res.Rows[0]["Correo"].ToString().Trim() + "','" + db.formatoEscritura(res.Rows[0]["Divicion"].ToString()) + "','" + res.Rows[0]["UFisica"].ToString() + "','" + db.formatoEscritura(res.Rows[0]["Puesto"].ToString()) + "','" + campus + "','" + res.Rows[0]["Extencion"].ToString() + "','" + res.Rows[0]["Estatus"].ToString().Trim() + "','" + db.formatoEscritura(res.Rows[0]["Departamento"].ToString()) + "'," + res.Rows[0]["Grupo"].ToString().Trim() + "," + res.Rows[0]["Area"].ToString().Trim() + "";
                                dt = db.getQuery(conexionBecarios, query);
                                if(dt.Rows.Count>0)
                                {
                                    switch(dt.Rows[0]["Mensaje"].ToString())
                                    {
                                        case "Ok":
                                            lblNombreSolicitante.Text = db.formatoEscritura(res.Rows[0]["NombreCompleto"].ToString());
                                            btnGuardar.Visible = true;
                                            break;
                                        case "Existe":
                                            break;
                                        default:
                                            verModal("Alerta",dt.Rows[0]["Mensaje"].ToString());
                                            break;

                                    }

                            
                                }
                        
                            }
                            else
                            {
                                verModal("Alerta", "La nómina no existe o no esta registrada");
                                lblNombreSolicitante.Text = "";
                                btnGuardar.Visible = false;
                            }
                            break;
                    }

                    
                }
            }


        }

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (db.nominaSinEspacio(txtNomina.Text))
                {
                    if (Convert.ToDateTime(txtFechaInicio.Text) <= Convert.ToDateTime(txtFechaFin.Text))
                    {
                        query = "sp_guardar_NominasinBecarios '" + txtNomina.Text.Trim() + "','" + db.convertirFecha(txtFechaInicio.Text) + "','" + db.convertirFecha(txtFechaFin.Text) + "','" + txtJustificacion.Text.Trim() + "','" + Session["usuario"].ToString() + "'";
                        dt = db.getQuery(conexionBecarios, query);
                        if (dt.Rows.Count > 0)
                        {
                            switch (dt.Rows[0]["Mensaje"].ToString())
                            {
                                case "Ok":
                                    verModal("Éxito", "Se guardó correctamente la información");
                                    limpiarComponentes();
                                    mostrarRegistros();
                                    break;
                                case "Existe":
                                    verModal("Alerta", "Ya hay un registro del usuario en la tabla");
                                    /*Seria bueno que habra directamente el registro aqui**/
                                    limpiarComponentes();
                                    break;
                            }

                        }
                    }
                    else
                    {
                        verModal("Error", "La fecha inicio es menor a la fecha fin");
                    }

                }
                else
                {
                    verModal("Error","El campo nómina no contiene el formato correcto");
                }
            }catch(Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void limpiarComponentes()
        {
            txtNomina.Text = "";
            lblNombreSolicitante.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            txtJustificacion.Text = "";

        }

      

        public void mostrarRegistros()
        {

            //if(string.IsNullOrEmpty(txtNomina.Text))
            //{
            //    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios NULL";
            //}
            //else
            //{
            //    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios '" + txtNomina.Text + "'";
            //}
            bool bandera1 = false, bandera2 = false;
            bool vacio = false;
            Match match;
            if (db.nominaconEspacio(txtFiltrarNomina.Text))
            {
                if (db.justificacionConEspacio(txtFiltrarJustificacion.Text))
                {
                    bandera2 = true;
                }
                else
                {
                    verModal("Error", "El campo justificación no tiene el formato correcto");
                    bandera2 = false;
                }
            }
            else
            {
                verModal("Error", "El campo nómina no tiene el formato correcto");
                bandera2 = false;
            }

            if(!string.IsNullOrEmpty(txtfiltrarFechainicio.Text))
            {
                match = Regex.Match(txtfiltrarFechainicio.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                if(match.Success)
                {
                    bandera1 = true;
                    vacio = true;
                }
                else
                {
                    verModal("Error", "Fecha inicio no tiene el formato dd/mm/aaaa");
                }

            }
            else
            {
                bandera1 = true;
            }

            if(!string.IsNullOrEmpty(txtfiltrarFechafin.Text))
            {
                match = Regex.Match(txtfiltrarFechafin.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                if(match.Success)
                {
                    if(vacio)
                    {
                        if (Convert.ToDateTime(txtfiltrarFechainicio.Text) <= Convert.ToDateTime(txtfiltrarFechafin.Text))
                        {
                            bandera2 = true;
                        }
                        else
                        {
                            verModal("Error", "La fecha inicio no puede ser mayor a la fecha fin");
                            bandera2 = false;
                        }

                    }
                    else
                    {
                        bandera2 = true;
                    }
                   
                }
                else
                {
                    verModal("Error","La fecha fin no tiene el formato dd/mm/aaaa");
                }

            }
            else
            {
                bandera2 = true;
            }

            

            if(bandera1 && bandera2)
            {
                if (txtFiltrarNomina.Text != "" && txtfiltrarFechainicio.Text == "" && txtfiltrarFechafin.Text == "" && txtFiltrarJustificacion.Text == "")//1
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios '" + txtFiltrarNomina.Text + "',NULL,NULL,NULL ";
                }
                if (txtFiltrarNomina.Text == "" && txtfiltrarFechainicio.Text != "" && txtfiltrarFechafin.Text == "" && txtFiltrarJustificacion.Text == "")//2
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios NULL,'" + db.convertirFecha(txtfiltrarFechainicio.Text) + "',NULL,NULL ";
                }
                if (txtFiltrarNomina.Text == "" && txtfiltrarFechainicio.Text == "" && txtfiltrarFechafin.Text != "" && txtFiltrarJustificacion.Text == "")//3
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios NULL,NULL,'" + db.convertirFecha(txtfiltrarFechafin.Text) + "',NULL ";
                }
                if (txtFiltrarNomina.Text == "" && txtfiltrarFechainicio.Text == "" && txtfiltrarFechafin.Text == "" && txtFiltrarJustificacion.Text != "")//4
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios NULL,NULL,NULL,'" + txtFiltrarJustificacion.Text + "' ";
                }
                if (txtFiltrarNomina.Text != "" && txtfiltrarFechainicio.Text != "" && txtfiltrarFechafin.Text == "" && txtFiltrarJustificacion.Text == "")//5
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios '" + txtFiltrarNomina.Text + "','" + db.convertirFecha(txtfiltrarFechainicio.Text) + "',NULL,NULL ";
                }
                if (txtFiltrarNomina.Text != "" && txtfiltrarFechainicio.Text == "" && txtfiltrarFechafin.Text != "" && txtFiltrarJustificacion.Text == "")//6
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios '" + txtFiltrarNomina.Text + "',NULL,'" + db.convertirFecha(txtfiltrarFechafin.Text) + "',NULL ";
                }
                if (txtFiltrarNomina.Text != "" && txtfiltrarFechainicio.Text == "" && txtfiltrarFechafin.Text == "" && txtFiltrarJustificacion.Text != "")//7
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios '" + txtFiltrarNomina.Text + "',NULL,NULL,'" + txtFiltrarJustificacion.Text + "' ";
                }
                if (txtFiltrarNomina.Text == "" && txtfiltrarFechainicio.Text != "" && txtfiltrarFechafin.Text != "" && txtFiltrarJustificacion.Text == "")//8
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios NULL,'" + db.convertirFecha(txtfiltrarFechainicio.Text) + "','" + db.convertirFecha(txtfiltrarFechafin.Text) + "',NULL";
                }
                if (txtFiltrarNomina.Text == "" && txtfiltrarFechainicio.Text != "" && txtfiltrarFechafin.Text == "" && txtFiltrarJustificacion.Text != "")//9
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios NULL,'" + db.convertirFecha(txtfiltrarFechainicio.Text) + "',NULL,'" + txtFiltrarJustificacion.Text + "'";
                }
                if (txtFiltrarNomina.Text == "" && txtfiltrarFechainicio.Text == "" && txtfiltrarFechafin.Text != "" && txtFiltrarJustificacion.Text != "")//10
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios NULL,NULL,'" + db.convertirFecha(txtfiltrarFechafin.Text) + "','" + txtFiltrarJustificacion.Text + "'";
                }
                if (txtFiltrarNomina.Text != "" && txtfiltrarFechainicio.Text != "" && txtfiltrarFechafin.Text != "" && txtFiltrarJustificacion.Text == "")//11
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios '" + txtFiltrarNomina.Text + "','" + db.convertirFecha(txtfiltrarFechainicio.Text) + "','" + db.convertirFecha(txtfiltrarFechafin.Text) + "',NULL";
                }
                if (txtFiltrarNomina.Text != "" && txtfiltrarFechainicio.Text == "" && txtfiltrarFechafin.Text != "" && txtFiltrarJustificacion.Text != "")//12
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios '" + txtFiltrarNomina.Text + "',NULL,'" + db.convertirFecha(txtfiltrarFechafin.Text) + "','" + txtFiltrarJustificacion.Text + "'";
                }
                if (txtFiltrarNomina.Text == "" && txtfiltrarFechainicio.Text != "" && txtfiltrarFechafin.Text != "" && txtFiltrarJustificacion.Text != "")//13
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios NULL,'" + db.convertirFecha(txtfiltrarFechainicio.Text) + "','" + db.convertirFecha(txtfiltrarFechafin.Text) + "','" + txtFiltrarJustificacion.Text + "'";
                }
                if (txtFiltrarNomina.Text != "" && txtfiltrarFechainicio.Text != "" && txtfiltrarFechafin.Text == "" && txtFiltrarJustificacion.Text != "")//14
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios '" + txtFiltrarNomina.Text + "','" + db.convertirFecha(txtfiltrarFechainicio.Text) + "',NULL,'" + txtFiltrarJustificacion.Text + "'";
                }
                if (txtFiltrarNomina.Text != "" && txtfiltrarFechainicio.Text != "" && txtfiltrarFechafin.Text != "" && txtFiltrarJustificacion.Text != "")//15
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios '" + txtFiltrarNomina.Text + "','" + db.convertirFecha(txtfiltrarFechainicio.Text) + "','" + db.convertirFecha(txtfiltrarFechafin.Text) + "','" + txtFiltrarJustificacion.Text + "'";
                }
                if (txtFiltrarNomina.Text == "" && txtfiltrarFechainicio.Text == "" && txtfiltrarFechafin.Text == "" && txtFiltrarJustificacion.Text == "")//16
                {
                    query = "sp_muestra_Nominas_quenoPuedenTenerBecarios NULL,NULL,NULL,NULL";
                }
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    gvRegistros.DataSource = dt;
                    gvRegistros.DataBind();
                }
                else
                {
                    verModal("Alerta", "No hay registros");
                    gvRegistros.DataSource = null;
                    gvRegistros.DataBind();
                }
            }

            

        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                query = "sp_modificar_nomina_sinbecario " + hdid.Value + ",'" + txtNomina.Text + "','" + txtJustificacion.Text + "','" + db.convertirFecha(txtFechaInicio.Text) + "','" + db.convertirFecha(txtFechaFin.Text) +"' ";
                dt = db.getQuery(conexionBecarios,query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                    {
                        verModal("Éxito", "Se modificó correctamente la información");
                    }
                }
                
                btnCancelar.Visible = false;
                btnModificar.Visible = false;
                btnGuardar.Visible = true;
                limpiarComponentes();
                mostrarRegistros();
                txtNomina.Enabled = true;
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void gvRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hdid.Value = gvRegistros.SelectedDataKey.Value.ToString();
                txtNomina.Text = gvRegistros.SelectedRow.Cells[1].Text;
                txtFechaInicio.Text = gvRegistros.SelectedRow.Cells[2].Text;
                txtFechaFin.Text = gvRegistros.SelectedRow.Cells[3].Text;
                txtJustificacion.Text = gvRegistros.SelectedRow.Cells[4].Text;
                ClientScript.RegisterStartupScript(GetType(), "BanderaNSB", "Bandera();", true);
                btnGuardar.Visible = false;
                btnModificar.Visible = true;
                txtNomina.Enabled = false;
                //txtNombre.Text = gvRegistros.SelectedRow.Cells[1].Text;
                //txtdescripcion.Text = gvRegistros.SelectedRow.Cells[2].Text;
                query = "select  Nombre+' '+' '+Apellido_paterno+' ' +Apellido_materno as Nombre from tbl_empleados where Nomina='"+txtNomina.Text+"'";
                dr = db.getQuery(conexionBecarios,query);
                if (dr.Rows.Count > 0)
                {
                    lblNombreSolicitante.Text = dr.Rows[0]["Nombre"].ToString();
                }
                btnCancelar.Visible = true;
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                limpiarComponentes();
                btnCancelar.Visible = false;
                btnModificar.Visible = false;
                btnGuardar.Visible = true;
                gvRegistros.SelectedIndex = -1;
                txtNomina.Enabled = true;

            }catch(Exception es){
                verModal("Error",es.Message.ToString());
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                mostrarRegistros();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void txtFiltrarNomina_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //Con el método establece_nombre busca en la bd la nómina y pone nombre
                string cadena = txtFiltrarNomina.Text.ToLower().Trim();
                
                if (!String.IsNullOrEmpty(cadena))
                {
                    if (cadena.Contains("l") || cadena.Contains("L"))
                    {
                        txtFiltrarNomina.Text = txtFiltrarNomina.Text.ToUpper();
                    }
                    else
                    {
                        txtFiltrarNomina.Text = "L" + txtFiltrarNomina.Text;
                    }
                    
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }


        }
    }
}