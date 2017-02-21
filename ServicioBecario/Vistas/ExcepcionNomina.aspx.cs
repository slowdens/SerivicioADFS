using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using ServicioBecario.Codigo;
using System.Collections;
using System.Text.RegularExpressions;
namespace ServicioBecario.Vistas
{
    public partial class ExcepcionNomina : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, cadena;
        DataTable dt,rd;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    mostrarPeriodo();
                    llenarFiltroPeriodo();
                    try
                    {

                        PnlExcepcionNominas.Visible = true;
                        //llenargridNominaEspecifico();

                    }
                    catch (Exception es)
                    {
                        // db.sacarPop("Alerta", es.Message.ToString(), this);
                        cadena = es.Message.ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("'", "").Replace(",", "");
                        verModal("Error", cadena);
                    }
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }

        }
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }
        public void mostrarPeriodo()
        {
            query = "select Periodo,Descripcion from cat_periodos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }

        }
        public void guarNominaMasBecarios()
        {
            if (db.nominaSinEspacio(txtNomina.Text))
            {
                if (db.numeroSinEspacio(txtNumeroBecarios.Text))
                {
                    if (db.justificacionSinEspacio(txtJustificacion.Text))
                    {
                        if (db.fechaSinEspacio(txtFechaInicio.Text))
                        {
                            if (db.fechaSinEspacio(txtFechaFin.Text))
                            {
                                query = "sp_excepcion_de_nominas_mas_becarios '" + txtNomina.Text + "'," + txtNumeroBecarios.Text + ",'" + txtJustificacion.Text + "','" + db.convertirFecha(txtFechaInicio.Text) + "','" + db.convertirFecha(txtFechaFin.Text) + "'," + ddlPeriodo.SelectedValue + ",'" + Session["Usuario"].ToString() + "'";
                                dt = db.getQuery(conexionBecarios, query);
                                if (dt.Rows.Count > 0)
                                {
                                    if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                                    {
                                        verModal("Éxito", "Guardado correctamente");
                                    }
                                    else
                                    {
                                        verModal("Alerta", "Ya existe la nómina en el periodo registrado");
                                    }
                                }
                            }
                            else
                            {
                                verModal("Error","El campo fecha inicio no tiene el formato correcto");
                            }
                            
                        }
                        else
                        {
                            verModal("Error","El campo fecha inicio no tiene el formato correcto");
                        }
                        
                    }
                    else
                    {
                        verModal("Error","El campo justificación no contiene el formato correcto ");
                    }
                    
                }
                else
                {
                    verModal("Error","El campo cantidad de becarios no tiene el formato correcto");
                }
                
            }
            else
            {
                verModal("Error","El campo nómina no tiene el formato correcto");
            }
            
        }

        protected void txtNomina_TextChanged(object sender, EventArgs e)
        {

            string cadena = txtNomina.Text.ToLower().Trim();
            
            try
            {
                if (!string.IsNullOrEmpty(cadena))
                {
                    if (cadena.Contains("l") || cadena.Contains("L"))
                    {
                        txtNomina.Text = txtNomina.Text.ToUpper();
                    }
                    else
                    {
                        txtNomina.Text = "L" + txtNomina.Text;
                    }
                    sacarNombre(txtNomina.Text);
                }
                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void sacarNombre(string nomina)
        {
             string ambiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
             DataTable dtws;
             int campus = 0;
            query = "sp_saca_informacion_del_empleado '" + nomina + "'";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                if(dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    lblNombre.Text = dt.Rows[0]["NombreEmpleado"].ToString();
                }
                else//saves que no existe y vamos a buscarlo en el ws la nomina
                {
                    switch(ambiente)//Mostramos el ambien del ws
                    {
                        case "pprd":

                            dtws=db.infoEmpleados(nomina);//Leemos del ws de pruebas
                            if(dtws.Rows[0]["Nomina"].ToString()!="Nada")
                            {
                                campus = int.Parse(dtws.Rows[0]["Campus"].ToString().Trim());
                                query = "sp_inserta_nomina_directa_nuevo '" + db.formatoEscritura(dtws.Rows[0]["Nomina"].ToString()) + "','" + db.formatoEscritura(dtws.Rows[0]["Nombres"].ToString()) + "','" + db.formatoEscritura(dtws.Rows[0]["Apaterno"].ToString()) + "','" + db.formatoEscritura(dtws.Rows[0]["Amaterno"].ToString()) + "','" + dtws.Rows[0]["Correo"].ToString().Trim() + "','" + db.formatoEscritura(dtws.Rows[0]["Divicion"].ToString()) + "','" + dtws.Rows[0]["UFisica"].ToString() + "','" + db.formatoEscritura(dtws.Rows[0]["Puesto"].ToString()) + "','" + campus + "','" + dtws.Rows[0]["Extencion"].ToString() + "','" + dtws.Rows[0]["Estatus"].ToString().Trim() + "','" + db.formatoEscritura(dtws.Rows[0]["Departamento"].ToString()) + "'," + dtws.Rows[0]["Grupo"].ToString().Trim() + "," + dtws.Rows[0]["Area"].ToString().Trim() + "";
                                dt = db.getQuery(conexionBecarios, query);
                                if(dt.Rows.Count>0)
                                {
                                    switch(dt.Rows[0]["Mensaje"].ToString())
                                    {
                                        case "Ok":
                                            break;
                                        case "Existe":
                                            break;
                                        default:
                                            verModal("Error", dt.Rows[0]["Mensaje"].ToString());
                                            break;

                                    }
                                }
                            }
                            else
                            {
                                verModal("Alerta", "La nómina no se encuentra");
                            }
                            
                            
                            break;
                        case "prod":
                            dtws=db.informacionEmpleadosProduccion(nomina);//Leemos del ws de producción
                            if(dtws.Rows[0]["Nomina"].ToString()!="Nada")
                            {
                                campus = int.Parse(dtws.Rows[0]["Campus"].ToString().Trim());
                                query = "sp_inserta_nomina_directa_nuevo '" + db.formatoEscritura(dtws.Rows[0]["Nomina"].ToString()) + "','" + db.formatoEscritura(dtws.Rows[0]["Nombres"].ToString()) + "','" + db.formatoEscritura(dtws.Rows[0]["Apaterno"].ToString()) + "','" + db.formatoEscritura(dtws.Rows[0]["Amaterno"].ToString()) + "','" + dtws.Rows[0]["Correo"].ToString().Trim() + "','" + db.formatoEscritura(dtws.Rows[0]["Divicion"].ToString()) + "','" + dtws.Rows[0]["UFisica"].ToString() + "','" + db.formatoEscritura(dtws.Rows[0]["Puesto"].ToString()) + "','" + campus + "','" + dtws.Rows[0]["Extencion"].ToString() + "','" + dtws.Rows[0]["Estatus"].ToString().Trim() + "','" + db.formatoEscritura(dtws.Rows[0]["Departamento"].ToString()) + "'," + dtws.Rows[0]["Grupo"].ToString().Trim() + "," + dtws.Rows[0]["Area"].ToString().Trim() + "";
                                dt = db.getQuery(conexionBecarios, query);
                                if(dt.Rows.Count>0)
                                {
                                    switch(dt.Rows[0]["Mensaje"].ToString())
                                    {
                                        case "Ok":
                                            break;
                                        case "Existe":
                                            break;
                                        default:
                                            verModal("Error", dt.Rows[0]["Mensaje"].ToString());
                                            break;

                                    }
                                }
                            }
                            else
                            {
                                verModal("Alerta", "La nómina no se encuentra");
                            }


                            break;
                    }
                }
            }

            dt = db.infoEmpleados(nomina);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Nomina"].ToString() != "Nada")
                {
                    lblNombre.Text = dt.Rows[0]["NombreCompleto"].ToString();
                }
                else
                {

                    verModal("Alerta", "No existe información disponible para la nómina " + txtNomina.Text);
                    txtNomina.Text = "";
                    lblNombre.Text = "";
                }

            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if(Convert.ToDateTime(txtFechaInicio.Text)<=Convert.ToDateTime(txtFechaFin.Text))
                {
                    guarNominaMasBecarios();
                    limpiarComponentes();
                    llenargridNominaEspecifico();
                }
                else
                {
                    verModal("Error","La fecha inicio es menor a la fecha fin");
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void limpiarComponentes()
        {
            txtNomina.Text = "";
            lblNombre.Text = "";
            txtNumeroBecarios.Text = "";
            txtJustificacion.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            ddlPeriodo.SelectedValue = "";
        }

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        public void llenargridNominaEspecifico()
        {

            bool bandera1 = false, bandera2 = false;
            Match match;
            bool vacio=false;

            if (db.nominaconEspacio(txtfiltrarNomina.Text))
            {
                if (db.numeroConEspacio(txtfiltrarCantidaddeBecarios.Text))
                {
                    bandera2 = true;
                }
                else
                {
                    verModal("Error", "El campo cantidad de becarios no tiene el formato correcto");
                    bandera2 = false;
                }
            }
            else
            {
                verModal("Error", "El campo nómina no tiene el formato correcto");
                bandera2 = false;
            }

            if(!string.IsNullOrEmpty(txtfiltraFechaInicio.Text))
            {
               
                match = Regex.Match(txtfiltraFechaInicio.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                if(match.Success)
                {
                    bandera1 = true;
                    vacio = true;
                }
                else
                {
                    verModal("Error","La fecha inicio no tiene el formato de fecha dd/mm/aaaa");
                }
            }
            else
            {
                bandera1 = true;
            }

            if(!string.IsNullOrEmpty(txtfiltrarFechaFin.Text))
            {
                match = Regex.Match(txtfiltrarFechaFin.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                if(match.Success)
                {
                    if(vacio)
                    {
                        if (Convert.ToDateTime(txtfiltraFechaInicio.Text) <= Convert.ToDateTime(txtfiltrarFechaFin.Text))
                        {
                            bandera2 = true;
                        }else{
                            verModal("Error", "La fecha inicio no puede ser mayor a la fecha fin");
                            bandera2 = false;
                        } 
                    }else{
                        bandera2 = true;
                    }
                    
                }
                else
                {
                    verModal("Error","La fecha fin no tiene el formato de fecha dd/mm/aaaa");
                }
            }
            else
            {
                bandera2 = true;
            }


            

            if (bandera1 && bandera2)
            {


                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue == "-1")//1
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1 ,null,null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue == "-1")//2
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + " ,null,null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue == "-1")//3
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1 ,'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "',null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue == "-1")//4
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text + "',-1 ,null,'" + db.convertirFecha(txtfiltrarFechaFin.Text) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue != "-1")//5
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1 ,null,null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue == "-1")//6
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + " ,null,null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue == "-1")//7
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1 ,'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "',null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue == "-1")//8
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1 ,null,'" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue.Trim() != "-1")//9
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1 ,null,null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue == "-1")//10
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "',null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue == "-1")//11
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",null,'" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue == "-1")//12
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",null,null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue == "-1")//13
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1,'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "','" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue != "-1")//14
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1,'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "',null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue != "-1")//15
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1,null,'" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue == "-1")//16
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "',null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue == "-1")//17
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",null,'" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue != "-1")//18
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",null,null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue == "-1")//19
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "','" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue != "-1")//20
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "',null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue != "-1")//21
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1,'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "','" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue != "-1")//22
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",null,'" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue != "-1")//23
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1,null,'" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue == "-1")//24
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "','" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue != "-1")//25
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "',null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue.Trim() != "-1")//26
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1,'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "','" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue.Trim() != "-1")//27
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "','" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue != "-1")//28
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",null,'" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() != "" && txtfiltrarCantidaddeBecarios.Text.Trim() != "" && txtfiltraFechaInicio.Text.Trim() != "" && txtfiltrarFechaFin.Text.Trim() != "" && ddlFiltrarPeriodo.SelectedValue != "-1")//29
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "'," + txtfiltrarCantidaddeBecarios.Text.Trim() + ",'" + db.convertirFecha(txtfiltraFechaInicio.Text.Trim()) + "','" + db.convertirFecha(txtfiltrarFechaFin.Text.Trim()) + "'," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                if (txtfiltrarNomina.Text.Trim() == "" && txtfiltrarCantidaddeBecarios.Text.Trim() == "" && txtfiltraFechaInicio.Text.Trim() == "" && txtfiltrarFechaFin.Text.Trim() == "" && ddlFiltrarPeriodo.SelectedValue == "-1")//30
                {
                    query = "exec sp_muestra_nomina_especifico '" + txtfiltrarNomina.Text.Trim() + "',-1,null,null," + ddlFiltrarPeriodo.SelectedValue + "";
                }
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    GVExcepcionNominas.DataSource = dt;
                    GVExcepcionNominas.DataBind();
                }
                else
                {
                    verModal("Alerta", "No existen registros");
                    GVExcepcionNominas.DataSource = null;
                    GVExcepcionNominas.DataBind();
                }

            }
            
            
        }

        

        protected void GVExcepcionNominas_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GVExcepcionNominas.SelectedRow;
            string id_periodo = "";
            hdfid.Value = GVExcepcionNominas.SelectedDataKey.Value.ToString();
            txtNomina.Text = GVExcepcionNominas.SelectedRow.Cells[1].Text;
            txtNumeroBecarios.Text = GVExcepcionNominas.SelectedRow.Cells[2].Text;
            txtFechaInicio.Text = GVExcepcionNominas.SelectedRow.Cells[3].Text;
            txtFechaFin.Text = GVExcepcionNominas.SelectedRow.Cells[4].Text;
            txtJustificacion.Text = GVExcepcionNominas.SelectedRow.Cells[5].Text;
            string fofo = GVExcepcionNominas.SelectedRow.Cells[9].Text;
            var colnov = GVExcepcionNominas.DataKeys[row.RowIndex].Values;
            string per = GVExcepcionNominas.SelectedRow.Cells[8].Text;
            ddlPeriodo.SelectedValue = ddlPeriodo.Items.FindByText(per).Value;
            ClientScript.RegisterStartupScript(GetType(), "BanderaEXN", "Bandera();", true);
            //foreach (DictionaryEntry ds in colnov)
            //{
            //    id_periodo = ds.Value.ToString();
            //}
            //ddlPeriodo.SelectedValue = id_periodo;
            btnGuardar.Visible = false;
            btnUpdate.Visible = true;
            query = "select Nombre+' ' +Apellido_paterno+' '+Apellido_materno as Nombre from tbl_empleados where nomina='" + txtNomina.Text + "'";
            rd = db.getQuery(conexionBecarios,query);
            lblNombre.Text = rd.Rows[0]["Nombre"].ToString();
            btncancelar.Visible = true;
            txtNomina.Enabled = false;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //Con este método actualizamos los datos en la bd
                actualizarDatos(hdfid.Value);
                //Muestroinformación en el Grid
                llenargridNominaEspecifico();
                btnUpdate.Visible = false;
                btnGuardar.Visible = true;
                btncancelar.Visible = false;
                txtNomina.Enabled = true;

            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void actualizarDatos(string id)
        {
            query = "sp_actualizar_nomina_mas_becario '" + txtNomina.Text + "','" + txtNumeroBecarios.Text + "','" + txtJustificacion.Text + "','" + db.convertirFecha(txtFechaInicio.Text) + "','" + db.convertirFecha(txtFechaFin.Text) + "','" + ddlPeriodo.SelectedValue + "'," + id + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Éxito", "La actualización se realizó de datos se realizó correctamente");
                }
                else
                {
                    verModal("Alerta", "No se realizo la actualización de datos");
                }
            }
        }

        protected void GVExcepcionNominas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //hdfDesion.Value = "";
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    foreach (Image button in e.Row.Cells[10].Controls.OfType<Image>())
            //    {
            //        button.Attributes["onclick"] = "confirmar('¿Está seguro de eliminar la nómina de excepciones?');";
            //    }
            //}
        }

        protected void GVExcepcionNominas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = (int)GVExcepcionNominas.DataKeys[e.RowIndex].Value;
                if (hdfDesion.Value == "true")
                {
                    eliminarNominaEspecifico(id);
                    llenargridNominaEspecifico();
                }
                hdfDesion.Value = "false";
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void eliminarNominaEspecifico(int id)
        {
            query = "sp_eliminar_nominas_mas_becarios " + id;
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
            {
                verModal("Éxito", "Se eliminó con éxito el registro");
            }
        }

        protected void GVExcepcionNominas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVExcepcionNominas.PageIndex = e.NewPageIndex;
                llenargridNominaEspecifico();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void llenarFiltroPeriodo()
        {
            query = "select Periodo,Descripcion from cat_periodos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarPeriodo.DataValueField = "Periodo";
                ddlFiltrarPeriodo.DataTextField = "Descripcion";
                ddlFiltrarPeriodo.DataSource = dt;
                ddlFiltrarPeriodo.DataBind();
            }
        }

        protected void ddlFiltrarPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarPeriodo.Items.Insert(0, new ListItem("--Seleccione --", "-1"));
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                llenargridNominaEspecifico();
            }
            catch (Exception es)
            {
                cadena = es.Message.ToString().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("'", "").Replace(",", "");
                verModal("Error", cadena);
            }
        }

        protected void txtfiltrarNomina_TextChanged(object sender, EventArgs e)
        {
            string cadena = txtfiltrarNomina.Text.ToLower().Trim();
            if(!string.IsNullOrEmpty(cadena))
            {
                if (cadena.Contains("l") || cadena.Contains("L"))
                {
                    txtfiltrarNomina.Text = txtfiltrarNomina.Text.ToUpper();
                }
                else
                {
                    txtfiltrarNomina.Text = "L" + txtfiltrarNomina.Text;
                }
            }
            
        }


        protected void imbEliminar_click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse((sender as ImageButton).CommandArgument);
                eliminarNominaEspecifico(id);
                llenargridNominaEspecifico();
                limpiarComponentes();
                txtNomina.Enabled = true;
                btncancelar.Visible = false;
                btnUpdate.Visible = false;
                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btncancelar_Click(object sender, EventArgs e)
        {
            try
            {
                btncancelar.Visible = false;
                btnUpdate.Visible = false;
                btnGuardar.Visible = true;
                limpiarComponentes();
                limpiarcomponentesFiltros();
                GVExcepcionNominas.SelectedIndex = -1;
                txtNomina.Enabled = true;
            }catch(Exception es){
                verModal("Error",es.Message.ToString());
            }
        }
        public void limpiarcomponentesFiltros()
        {
            txtfiltrarNomina.Text = "";
            txtfiltrarFechaFin.Text = "";
            txtfiltraFechaInicio.Text = "";
            txtfiltrarCantidaddeBecarios.Text = "";
            ddlFiltrarPeriodo.SelectedValue = "-1";
        }
       
    }
}