using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using ServicioBecario.Codigo;
using Microsoft.Web.Services3.Security.Tokens;
using System.Text.RegularExpressions;

namespace ServicioBecario.Vistas
{
    public partial class Permiso : System.Web.UI.Page
    {
        static string conexionBecariosestatico = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        BasedeDatos bd = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            AgregarNomina.Attributes["onBlur"] = "VerificarUser()";
        }
       
        protected void Filtrar_Click(object sender, EventArgs e)
        {
            filtrar();    
        }
        protected void DatosGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DatosGrid.PageIndex = e.NewPageIndex;
                filtrar();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        protected void DatosGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //int index = e.RowIndex;

                
                
                //    e.Cancel = true;
                //    DatosGrid.DeleteRow(index);
                //    DatosGrid.DataBind();
                


            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        

        public void filtrar()
        {
            try
            {
                string queryes = @"select es.id_especiales, m.Nombre as Menu, m.Link Pantalla, convert(varchar(90),Fecha_inicio,103)as  inicio, CONVERT(varchar(90), Fecha_fin, 103) as fin,
                                  e.Nomina , e.Nombre+' ' + e.Apellido_paterno+' ' + e.Apellido_materno as NombreCompleto
                                  from cat_menus m inner join tbl_especiales es on m.id_menu=es.id_menu
                                  inner join tbl_empleados e on e.Nomina=es.Nomina where e.Nomina!='' ";
                if (NominaFiltro.Text != "") { queryes += " AND e.Nomina='" + NominaFiltro.Text + "'"; }
                Match inicio;
                Match fin;
                if (FechaInicioFiltro.Text != "")
                {
                    inicio = Regex.Match(FechaInicioFiltro.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                    if (inicio.Success)
                    {
                        queryes += " AND CONVERT(date,  es.Fecha_inicio,101) > CONVERT(date,'" + FechaInicioFiltro.Text.Replace("/","") + "',101)";
                    }
                    else
                    {
                        inicio=null;
                        verModal("Error","El formato de fecha inicial es incorrecto");
                    }
                }
                else
                {
                    inicio = null;
                }
                if (FechaFinFiltro.Text != "")
                {
                    fin = Regex.Match(FechaFinFiltro.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                    if (fin.Success)
                    {
                        queryes += " AND CONVERT(date,  es.Fecha_fin,101) < CONVERT(date,'" + FechaFinFiltro.Text.Replace("/", "") + "',101)";
                    }
                    else
                    {
                        fin=null;
                        verModal("Error", "El formato de fecha final es incorrecto");
                    }
                }
                else
                {
                    fin = null;
                }
                if(inicio!=null && fin!=null)
                {
                    if (Convert.ToDateTime(FechaInicioFiltro.Text) > Convert.ToDateTime(FechaFinFiltro.Text))
                    {
                        verModal("Error","La fecha inicial no puede ser mayor a la final");
                    }
                }

                DataTable dt = bd.getQuery(conexionBecariosestatico, queryes);
                if(dt.Rows.Count>0)
                {
                    DatosGrid.Visible = true;
                    DatosGrid.DataSource = dt;
                    DatosGrid.DataBind();
                }
                else
                {
                    DatosGrid.Visible = false;
                    DatosGrid.DataSource = null;
                    DatosGrid.DataBind();
                    verModal("Error", "No hay resultados de la búsqueda");
                }
                
            }
            catch (Exception ex)
            {
                verModal("Error", ex.Message.ToString());
            }
        }

        public void AgregarEspecial()
        {
           /* try
            {
                //string query = "exec sp_guarda_permisos_especiales '" + nomina + "'," + id + ",'" + inicio + "','" + fin + "' ";
                string query = "exec sp_guarda_permisos_especiales_nuevo_is '" + AgregarNomina.Text + "'," + id + ",'" + inicio + "','" + fin + "' ";
                DataTable dt = bd.getQuery(conexionBecariosestatico, query); ;
                if (dt.Rows.Count > 0)
                {
                    retorno = dt.Rows[0]["Mensaje"].ToString();
                }
            }
            catch (Exception es)
            {
                retorno = es.Message.ToString();
            }
            return retorno;*/
        }
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }
        protected void imbEliminar_Click(object sender, ImageClickEventArgs e)
        {
            string id = (sender as ImageButton).CommandArgument;
            eliminarMenu(Convert.ToInt32(id));
            filtrar();
        }
        public void eliminarMenu(int i)
        {
            
            try
            {
            string query = "DELETE FROM tbl_especiales WHERE id_especiales='"+i+"'";
            bd.setQuery(conexionBecariosestatico, query);
            verModal("Éxito", "Se eliminó correctamente la información");
                    
                }
                catch(Exception ex)
                {
                    verModal("Error", ex.Message);
                }
            }
        
    [WebMethod]
        public static string ElementosAMostrar(string nomina)
        {
            int i = 0;
            string query;
            DataTable dt, dtw, dt1;
            BasedeDatos bd = new BasedeDatos();
            string json = "";
            string ambiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
            string Mensaje = "";
            query = "sp_saca_informacion_del_empleado '" + nomina + "'";
            dt = bd.getQuery(conexionBecariosestatico, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")//El empleado esta
                {
                    Mensaje = "Ok";
                    json = Empleadosformarcion(nomina, Mensaje);
                }
                else //El empleado no esta en la base de datos lo agregamos desde el ws
                {
                    if (ambiente == "pprd")//Ambiente de pruebas
                    {
                        dtw = informacionEmpleado_pruebas(nomina);
                        if (dtw.Rows[0]["Nomina"].ToString() != "Nada")
                        {
                            //Guardamos el empleado en la base de datos
                            int campus = int.Parse(dtw.Rows[0]["Campus"].ToString().Trim());
                            query = "sp_inserta_nomina_directa_nuevo '" + formatoEscritura(dtw.Rows[0]["Nomina"].ToString()) + "','" + formatoEscritura(dtw.Rows[0]["Nombres"].ToString()) + "','" + formatoEscritura(dtw.Rows[0]["Apaterno"].ToString()) + "','" + formatoEscritura(dtw.Rows[0]["Amaterno"].ToString()) + "','" + dtw.Rows[0]["Correo"].ToString().Trim() + "','" + formatoEscritura(dtw.Rows[0]["Divicion"].ToString()) + "','" + dtw.Rows[0]["UFisica"].ToString() + "','" + formatoEscritura(dtw.Rows[0]["Puesto"].ToString()) + "','" + campus + "','" + dtw.Rows[0]["Extencion"].ToString() + "','" + dtw.Rows[0]["Estatus"].ToString().Trim() + "','" + formatoEscritura(dtw.Rows[0]["Departamento"].ToString()) + "'," + dtw.Rows[0]["Grupo"].ToString().Trim() + "," + dtw.Rows[0]["Area"].ToString().Trim() + "";
                            dt1 = bd.getQuery(conexionBecariosestatico, query);
                            if (dt1.Rows.Count > 0)
                            {
                                switch (dt1.Rows[0]["Mensaje"].ToString())
                                {
                                    case "Ok":
                                        //insertamos el emplado en con el rol solicitante
                                        query = "sp_agregar_rol_nomina_solicitante " + nomina + "";
                                        dt = bd.getQuery(conexionBecariosestatico, query);
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                                            {
                                                Mensaje = "Ok";
                                                json = Empleadosformarcion(nomina, Mensaje);
                                            }
                                        }
                                        break;
                                    case "Existe":
                                        break;
                                    default:
                                        Mensaje = dt1.Rows[0]["Mensaje"].ToString();
                                        json = Empleadosformarcion("", Mensaje);//Solamente mandamos el mensaje error 
                                        break;
                                }
                            }
                        }

                    }
                    if (ambiente == "prod")//Ambiente de produccion
                    {
                        string aquivama = "aqui tiene que ir istaladao el ws de produccion";
                        dtw = informaciónEmpleado_produccion(nomina);
                        if (dtw.Rows[0]["Nomina"].ToString() != "Nada")
                        {
                            //Guardamos el empleado en la base de datos
                            int campus = int.Parse(dtw.Rows[0]["Campus"].ToString().Trim());
                            query = "sp_inserta_nomina_directa_nuevo '" + formatoEscritura(dtw.Rows[0]["Nomina"].ToString()) + "','" + formatoEscritura(dtw.Rows[0]["Nombres"].ToString()) + "','" + formatoEscritura(dtw.Rows[0]["Apaterno"].ToString()) + "','" + formatoEscritura(dtw.Rows[0]["Amaterno"].ToString()) + "','" + dtw.Rows[0]["Correo"].ToString().Trim() + "','" + formatoEscritura(dtw.Rows[0]["Divicion"].ToString()) + "','" + dtw.Rows[0]["UFisica"].ToString() + "','" + formatoEscritura(dtw.Rows[0]["Puesto"].ToString()) + "','" + campus + "','" + dtw.Rows[0]["Extencion"].ToString() + "','" + dtw.Rows[0]["Estatus"].ToString().Trim() + "','" + formatoEscritura(dtw.Rows[0]["Departamento"].ToString()) + "'," + dtw.Rows[0]["Grupo"].ToString().Trim() + "," + dtw.Rows[0]["Area"].ToString().Trim() + "";
                            dt1 = bd.getQuery(conexionBecariosestatico, query);
                            if (dt1.Rows.Count > 0)
                            {
                                switch (dt1.Rows[0]["Mensaje"].ToString())
                                {
                                    case "Ok":
                                        //insertamos el emplado en con el rol solicitante
                                        query = "sp_agregar_rol_nomina_solicitante " + nomina + "";
                                        dt = bd.getQuery(conexionBecariosestatico, query);
                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                                            {
                                                Mensaje = "Ok";
                                                json = Empleadosformarcion(nomina, Mensaje);
                                            }
                                        }
                                        break;
                                    case "Existe":
                                        break;
                                    default:
                                        Mensaje = dt1.Rows[0]["Mensaje"].ToString();
                                        json = Empleadosformarcion("", Mensaje);//Solamente mandamos el mensaje error 
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            return json;
        }


    public static string Empleadosformarcion(string nomina, string mensajes)
    {
        string json = "";
        string query = "";
        DataTable dt;
        int i = 0;
        BasedeDatos bd = new BasedeDatos();
        json = "[";



        if (!string.IsNullOrEmpty(nomina))
        {
            query = "exec sp_nombre_nomina_rol  '" + nomina + "'";
            dt = bd.getQuery(conexionBecariosestatico, query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (i == dt.Rows.Count - 1)
                    {

                        json += "{\"NombreComleto\":\"" + dt.Rows[i]["NombreCompleto"].ToString() + "\",\"Rol\":\"" + dt.Rows[i]["Rol"].ToString() + "\",\"Mensaje\":\"" + mensajes + "\"}";
                    }
                    else
                    {
                        //json += "[\"" + dt.Rows[i]["NombreCompleto"].ToString() + "\",\"" + dt.Rows[i]["Rol"].ToString() + "\"],";
                        json += "{\"NombreComleto\":\"" + dt.Rows[i]["NombreCompleto"].ToString() + "\",\"Rol\":\"" + dt.Rows[i]["Rol"].ToString() + "\",\"Mensaje\":\"" + mensajes + "\"},";
                    }
                    i++;
                }
            }
            else
            {
                json += "{\"Mensaje\":\"No\"}";
            }

        }
        else
        {
            json += "{\"Mensaje\":\"" + mensajes + "\"}";

        }
        json += "]";

        return json;
    }
    public static string formatoEscritura(string valu)
    {
        if (!string.IsNullOrEmpty(valu))
        {
            string dato = valu.ToLower();
            string[] arr1 = dato.Split(" ".ToCharArray());
            valu = "";
            foreach (string av in arr1)
            {
                string ades;
                dato = av.Substring(0, 1);
                ades = av.Remove(0, 1);
                dato = dato.ToUpper();
                ades = dato + ades;
                valu += ades + " ";

            }
            valu = valu.Trim();
        }

        return valu;
    }

    public static DataTable informaciónEmpleado_produccion(string nomina)
    {
        DataTable dt = new DataTable();
        prodInformacionLaboral.InformacionLaboralHttpService tester = new prodInformacionLaboral.InformacionLaboralHttpService();
        prodInformacionLaboral.consultaInformacionLaboralResponse respuesta = null;

        UsernameToken token = new UsernameToken("sSISTEMA-DEVGDL", "EdaU5Mq#", PasswordOption.SendPlainText);
        tester.RequestSoapContext.Security.Tokens.Add(token);
        prodInformacionLaboral.consultaInformacionLaboral objdatos = new prodInformacionLaboral.consultaInformacionLaboral();
        objdatos.idPersona = nomina;
        objdatos.cveSociedad = "0010";
        respuesta = tester.consultaInformacionLaboral(objdatos);


        //Creo las columnas del datatable
        dt.Columns.Add("Nomina");
        dt.Columns.Add("Nombres");
        dt.Columns.Add("Apaterno");
        dt.Columns.Add("Amaterno");
        dt.Columns.Add("Extencion");
        dt.Columns.Add("Correo");
        dt.Columns.Add("Divicion");
        dt.Columns.Add("UFisica");
        dt.Columns.Add("Puesto");
        dt.Columns.Add("Campus");
        dt.Columns.Add("Estatus");
        dt.Columns.Add("Departamento");
        dt.Columns.Add("NombreCompleto");
        dt.Columns.Add("Grupo");
        dt.Columns.Add("Area");
        var infor = respuesta.responseInformacionLaboral.InformacionLaboral;
        if (infor != null)
        {
            foreach (var info in respuesta.responseInformacionLaboral.InformacionLaboral)
            {
                DataRow row = dt.NewRow();
                row["Nomina"] = info.Nomina;
                row["Nombres"] = info.Nombre;
                row["Apaterno"] = info.apellidoPaterno;
                row["Amaterno"] = info.apellidoMaterno;
                row["Correo"] = info.correoElectronico;
                row["Divicion"] = info.descDivision;
                row["UFisica"] = info.descUbicacion;
                row["Puesto"] = info.descFuncion;
                //row["Campus"] = info.descSubDivision;
                row["Campus"] = info.SubDivision;
                row["Extencion"] = info.Extension;
                row["Estatus"] = info.descEstatusEmpleado;
                row["Departamento"] = info.descUnidadOrganizacional;
                row["NombreCompleto"] = info.NombreCompleto;
                row["Grupo"] = info.GrupoPersonal;
                row["Area"] = info.AreaPersonal;
                dt.Rows.Add(row);

            }
        }
        else
        {
            DataRow row = dt.NewRow();
            row["Nomina"] = "Nada";
            row["Nombres"] = "Nada";
            row["Apaterno"] = "Nada";
            row["Amaterno"] = "Nada";
            row["Correo"] = "Nada";
            row["Divicion"] = "Nada";
            row["UFisica"] = "Nada";
            row["Puesto"] = "Nada";
            row["Campus"] = "Nada";
            row["Extencion"] = "Nada";
            row["Estatus"] = "Nada";
            row["NombreCompleto"] = "Nada";
            row["Grupo"] = "Nada";
            row["Area"] = "Nada";
            dt.Rows.Add(row);
        }


        return dt;
    }


    public static DataTable informacionEmpleado_pruebas(string nomina)
    {
        DataTable dt = new DataTable();
        Empleados.InformacionLaboralHttpService tester = new Empleados.InformacionLaboralHttpService();
        Empleados.consultaInformacionLaboralResponse respuesta = null;


        UsernameToken token = new UsernameToken("sSISTEMA-DEVGDL", "EdaU5Mq#", PasswordOption.SendPlainText);
        tester.RequestSoapContext.Security.Tokens.Add(token);

        Empleados.consultaInformacionLaboral objdatos = new Empleados.consultaInformacionLaboral();
        objdatos.idPersona = nomina;
        objdatos.cveSociedad = "0010";
        respuesta = tester.consultaInformacionLaboral(objdatos);


        //Creo las columnas del datatable
        dt.Columns.Add("Nomina");
        dt.Columns.Add("Nombres");
        dt.Columns.Add("Apaterno");
        dt.Columns.Add("Amaterno");
        dt.Columns.Add("Extencion");
        dt.Columns.Add("Correo");
        dt.Columns.Add("Divicion");
        dt.Columns.Add("UFisica");
        dt.Columns.Add("Puesto");
        dt.Columns.Add("Campus");
        dt.Columns.Add("Estatus");
        dt.Columns.Add("Departamento");
        dt.Columns.Add("NombreCompleto");
        dt.Columns.Add("Grupo");
        dt.Columns.Add("Area");
        var infor = respuesta.responseInformacionLaboral.InformacionLaboral;
        if (infor != null)
        {
            foreach (var info in respuesta.responseInformacionLaboral.InformacionLaboral)
            {
                DataRow row = dt.NewRow();
                row["Nomina"] = info.Nomina;
                row["Nombres"] = info.Nombre;
                row["Apaterno"] = info.apellidoPaterno;
                row["Amaterno"] = info.apellidoMaterno;
                row["Correo"] = info.correoElectronico;
                row["Divicion"] = info.descDivision;
                row["UFisica"] = info.descUbicacion;
                row["Puesto"] = info.descFuncion;
                //row["Campus"] = info.descSubDivision;
                row["Campus"] = info.SubDivision;
                row["Extencion"] = info.Extension;
                row["Estatus"] = info.descEstatusEmpleado;
                row["Departamento"] = info.descUnidadOrganizacional;
                row["NombreCompleto"] = info.NombreCompleto;
                row["Grupo"] = info.GrupoPersonal;
                row["Area"] = info.AreaPersonal;
                dt.Rows.Add(row);

            }
        }
        else
        {
            DataRow row = dt.NewRow();
            row["Nomina"] = "Nada";
            row["Nombres"] = "Nada";
            row["Apaterno"] = "Nada";
            row["Amaterno"] = "Nada";
            row["Correo"] = "Nada";
            row["Divicion"] = "Nada";
            row["UFisica"] = "Nada";
            row["Puesto"] = "Nada";
            row["Campus"] = "Nada";
            row["Extencion"] = "Nada";
            row["Estatus"] = "Nada";
            row["NombreCompleto"] = "Nada";
            row["Grupo"] = "Nada";
            row["Area"] = "Nada";
            dt.Rows.Add(row);
        }


        return dt;


    }
    [WebMethod]
    public static string agregarElementos(string nomina)
    {
        //Este metodo construye el html  de los checkbox que se implementan dentro del div  para seleccionar solamente a los permisos 
        //el cual no tiene.
        int i = 0;
        BasedeDatos bd = new BasedeDatos();
        int columna = 0;
        string cadena = "";
        string componentes = "";
        string strColumnas = "";
        string strRows = "";
        //string querystatic = "exec sp_muestra_menus_de_nomina '" + nomina + "'";
        string querystatic = "exec sp_muestra_menus_de_nomina_nuevo '" + nomina + "'";
        DataTable dt = bd.getQuery(conexionBecariosestatico, querystatic);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (columna == 0)
                {
                    strRows = "<div class='Row' >";
                }
                else
                {
                    strRows = "";
                }
                if (dt.Rows[i]["rol"].ToString() != "N/A")
                {
                    strColumnas = "<div class='col-md-3'>";
                    if (columna > 0)
                    {
                        componentes = strRows + strColumnas + componentes + "<input id='che" + dt.Rows[i]["id_menu"] + "' name='radios'  checked  class='checkbox' type='checkbox' value='" + dt.Rows[i]["id_menu"] + "' /> " + dt.Rows[i]["Nombre"] + "</div>";
                    }
                    else
                    {
                        componentes = strRows + strColumnas + componentes + "<input id='che" + dt.Rows[i]["id_menu"] + "' name='radios'  checked class='checkbox'  type='checkbox' value='" + dt.Rows[i]["id_menu"] + "' /> " + dt.Rows[i]["Nombre"] + "</div>";
                    }
                }
                else
                {
                    strColumnas = "<div class='col-md-3'style='height:50px' >";
                    if (columna > 0)
                    {
                        componentes = strRows + strColumnas + componentes + " <input id='che" + dt.Rows[i]["id_menu"] + "' name='radios' class='css-checkbox'   type='checkbox' value='" + dt.Rows[i]["id_menu"] + "' />  <label for='che" + dt.Rows[i]["id_menu"] + "' name='che" + dt.Rows[i]["id_menu"] + "' class='css-label'>" + dt.Rows[i]["Nombre"] + "</label> </div>  ";
                    }
                    else
                    {
                        componentes = strRows + strColumnas + componentes + " <input id='che" + dt.Rows[i]["id_menu"] + "' name='radios' class='css-checkbox'   type='checkbox' value='" + dt.Rows[i]["id_menu"] + "' />  <label for='che" + dt.Rows[i]["id_menu"] + "' name='che" + dt.Rows[i]["id_menu"] + "' class='css-label'>" + dt.Rows[i]["Nombre"] + "</label> </div> ";
                    }
                }
                if (columna < 3)
                {
                    columna++;
                }
                else
                {
                    componentes = componentes + "</div>";
                    columna = 0;
                }
                i++;
                cadena = cadena + componentes;
                componentes = "";
            }
        }
        return cadena;
    }
    [WebMethod]
    public static string guardar(string p_encrii, string usiario, string inicio, string fin)
    {
        //Manda todos una serie de permisos casados a una nomina
        string[] ar = p_encrii.Split('!');
        string retorno = "";
        foreach (string dato in ar)
        {
            if (dato != "")
            {
                retorno = insertarPermisos_especial(dato, usiario, convertirFecha(inicio), convertirFecha(fin));

            }
        }

        return retorno;
    }
    public static string convertirFecha(string fecha)
    {
        //Me entregan la fecha en formato Dia/Mes/Año            
        string mes, dia, anio;
        dia = fecha.Substring(0, 2);
        mes = fecha.Substring(3, 2);
        anio = fecha.Substring(6, 4);
        //Entrega la fecha en formato Mes/Dia/Año
        return (mes + "/" + dia + "/" + anio);
    }
    public static string insertarPermisos_especial(string id, string nomina, string inicio, string fin)
    {
        //Inserta un permiso por nomina dentro de la base de datos
        BasedeDatos bd = new BasedeDatos();
        string retorno = "";
        try
        {
            //string query = "exec sp_guarda_permisos_especiales '" + nomina + "'," + id + ",'" + inicio + "','" + fin + "' ";
            string query = "exec sp_guarda_permisos_especiales_nuevo_is '" + nomina + "'," + id + ",'" + inicio + "','" + fin + "' ";
            DataTable dt = bd.getQuery(conexionBecariosestatico, query); ;
            if (dt.Rows.Count > 0)
            {
                retorno = dt.Rows[0]["Mensaje"].ToString();
            }
        }
        catch (Exception es)
        {
            retorno = es.Message.ToString();
        }
        return retorno;

    }
    }
}