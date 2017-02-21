using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Web.Services3.Security.Tokens;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ServicioBecario.Codigo
{
    public class BasedeDatos
    {
        private DataTable dt;

        public DataTable getQuery(string conexion, string query)
        {
            dt = new DataTable();
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandTimeout = 100000;
            conn.Open();
            //Llenanos nuestro  data table
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();
            return dt;
        }

        public void setQuery(string conexion, string query)
        {
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand my = new SqlCommand(query, conn);
            my.CommandType = CommandType.Text;
            conn.Open();
            my.ExecuteNonQuery();
            my.Connection.Close();
            conn.Close();
        }
        
        public string convertirFecha(string fecha)
        {
            //Me entregan la fecha en formato Dia/Mes/Año            
            string mes, dia, anio;
            dia = fecha.Substring(0, 2);
            mes = fecha.Substring(3, 2);
            anio = fecha.Substring(6, 4);
            //Entrega la fecha en formato Mes/Dia/Año
            return (mes + "/" + dia + "/" + anio);
        }

        public DataTable informacionEmpleadosProduccion(string nomina)
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
        
        public DataTable infoEmpleados(string nomina)
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
        public string noEnvio()
        {
            return @"</br>
                    </br>
                        <p style='font-style: normal; font-weight: normal; font-size: 10px; color: #A09A9A;'>Favor de no responder a este correo.&nbsp;<br>
                            Este correo ha sido enviado desde un servidor que no acepta  correos entrantes.&nbsp;</p>";
        }


        public DataTable AgregarNomina(string nomina,string conexionBecarios)
        {
            string query;
            DataTable dt = new DataTable();
            query = "sp_saca_informacion_del_empleado_nuevo '" + nomina + "'";
            dt = this.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0) 
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "No esta")//Es por que no hubo registro en la base de datos
                {
                    dt = this.infoEmpleados(nomina);
                    if (dt.Rows[0]["Nombres"].ToString() != "Nada")
                    {
                        int campus = int.Parse(dt.Rows[0]["Campus"].ToString().Trim());
                       

                        query = "sp_inserta_nomina_directa_nuevo_primero '" + formatoEscritura( dt.Rows[0]["Nomina"].ToString() )+ "','" + formatoEscritura( dt.Rows[0]["Nombres"].ToString()) + "','" + formatoEscritura(dt.Rows[0]["Apaterno"].ToString()) + "','" + formatoEscritura( dt.Rows[0]["Amaterno"].ToString()) + "','" + dt.Rows[0]["Correo"].ToString().Trim() + "','" + formatoEscritura(dt.Rows[0]["Divicion"].ToString()) + "','" + formatoEscritura( dt.Rows[0]["UFisica"].ToString()) + "','" +  formatoEscritura( dt.Rows[0]["Puesto"].ToString()) + "','" + campus + "','" + dt.Rows[0]["Extencion"].ToString().Trim() + "','" + dt.Rows[0]["Estatus"].ToString().Trim() + "','" + formatoMayusculaInicial( dt.Rows[0]["Departamento"].ToString()) + "'," + dt.Rows[0]["Grupo"].ToString().Trim() + "," + dt.Rows[0]["Area"].ToString().Trim() + "";
                        dt = this.getQuery(conexionBecarios, query);

                    }
                }
                
            }
            return dt;
        }

        public string formatoEscritura(string valu)
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
        public string formatoMayusculaInicial(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string inicial = value.ToLower();

                inicial = inicial.Substring(0, 1);
                value = value.ToLower();
                value = value.Remove(0, 1);
                inicial = inicial.ToUpper();
                value = inicial + value;
            }
            
            return value;

        } 



        public bool letraacento (string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+$");
        }

        public bool numeLetAceSinEspacio(string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-Z0.ñÑáéíóúÁÉÍÓÚ1-9]+$");
        }

        public bool NumeroLetraConEspacio(string valor)
        {
            return Regex.IsMatch(valor, @"^[\s\wñÑáéíóúÁÉÍÓÚ]*$");
        }

        public bool validaNumeros(string valor )
        {
            return Regex.IsMatch("", @"^[0-9]*$");
        }
        public bool validaNumeroSinEspacio(string valor)
        {
            return Regex.IsMatch(valor, @"^[0-9]+$");
        }

        public bool NumeroLetrainiciosinEspacio(string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-Z0.ñÑáéíóúÁÉÍÓÚ1-9\s]+$");
        }
        
        public bool validaNominaEspacio(string valor)
        {
            if(valor=="")
            {
                return true;
            }
            else
            {
                return Regex.IsMatch(valor, @"^L{0,1}[0-9]{8}$");
            }
            
        }


        public bool validaNominaSinEspacio(string valor)
        {
            return Regex.IsMatch(valor, @"^[L{1}[\d]{9}]*$");
        }
        public bool matriculaSinEspacio(string valor)
        {
            return Regex.IsMatch(valor, @"^A{0,1}[0-9]{8}$");
        }

        public bool matriculaConEspacio(string valor)
        {
            if(valor=="")
            {
                return true;
            }
            else
            {
                return Regex.IsMatch(valor, @"^A{0,1}[0-9]{8}$");
            }
            
        }

        public bool nominaconEspacio(string valor)
        {
            if (valor == "")
            {
                return true;
            }
            else
            {
                return Regex.IsMatch(valor, @"^L{0,1}[0-9]{8}$");
            }

        }


        public bool nominaSinEspacio(string valor)
        {
           
                return Regex.IsMatch(valor, @"^L{0,1}[0-9]{8}$");
           
        }
        public bool fechaConEspacio(string valor)
        {
            if (valor == "")
            {
                return true;
            }
            else
            {
                return Regex.IsMatch(valor, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
            }
        }
        public bool fechaSinEspacio(string valor)
        {
            if (valor == "")
            {
                return true;
            }
            else
            {
                return Regex.IsMatch(valor, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
            }
        }
        public bool numeroSinEspacio(string valor)
        {

            return Regex.IsMatch(valor, @"^[0-9]+$");
        }
        public bool numeroConEspacio(string valor)
        {
            if (valor == "")
            {
                return true;
            }
            else
            {
                return Regex.IsMatch(valor, @"^[0-9]+$");
            }
        }



        public bool justificacionConEspacio(string valor)
        {
            if (valor == "")
            {
                return true;
            }
            else
            {
                return Regex.IsMatch(valor, @"^[a-zA-Z0.ñÑáéíóúÁÉÍÓÚ1-9\s]+$");
            }
        }
        public bool justificacionSinEspacio(string valor)
        {
            
                return Regex.IsMatch(valor, @"^[a-zA-Z0.ñÑáéíóúÁÉÍÓÚ1-9\s]+$");
            
        }
        
    }

    

}