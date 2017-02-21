using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using ServicioBecario.Codigo;
using System.Data;
using System.Configuration;
using System.Text;


namespace ServicioBecario.Service_References
{
    /// <summary>
    /// Descripción breve de WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]

    public class WebService2 : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string ObtenerLenguajes()
        {
            
            StringBuilder alumnos = new StringBuilder();
            string nombrecompleto = null;
            string id_periodo="0";
            if(Session["Periodo"] !=null)
            {
                id_periodo= Session["Periodo"].ToString();
            }

             


            // Declaracion de objetos de bd
            SqlConnection connection = new SqlConnection();
            //ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();//
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();// @"Data Source=(local); Initial Catalog=ServicioBecario; Integrated Security=False; User Id=sa;Password=96512; MultipleActiveResultSets=True";

            SqlCommand command = new SqlCommand();
            command.Connection = connection;

            SqlDataReader dr;

            connection.Open(); // Abertura de la conexion

            // Recuperación del campus del solicitante
            string campusID = "0";
            string nominaEmpleado = Session["Usuario"].ToString();

            command = new SqlCommand(
                "SELECT "
                    + "* "
                + "FROM tbl_empleados "
                + "WHERE Nomina = '" + nominaEmpleado + "'"
                , connection);

            dr = command.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    //campusID = Convert.ToInt32(dr["Codigo_campus"].ToString());
                    campusID =dr["Codigo_campus"].ToString();
                }
            }
            connection.Close();
            try
            {
                connection.Open();
                // Busqueda de los alumnos que pertenescan al campus del solicitante
                command = new SqlCommand(                    
                string.Format(
//@"
//                                    select  Matricula,
//                                    Nombre,
//                                    Apellido_paterno,
//                                    Apellido_materno
//                                    from tbl_alumnos 
//                                    where Tipo_apoyo in (select s.Tipo_apoyo from tbl_apoyo_financiero s where Periodo=@id_periodo and Codigo_campus=@campusid) and Codigo_campus=@campusid and Periodo=@id_periodo"

                                    @"select  Matricula,
                                    Nombre,
                                    Apellido_paterno,
                                    Apellido_materno
                                    from tbl_alumnos 
                                    where Clave_apoyo in (select s.Clave_apoyo from tbl_apoyo_financiero s where Periodo=@id_periodo and Codigo_campus=@campusid) and Codigo_campus=@campusid and Periodo=@id_periodo"

                    ), connection);

                command.Parameters.Clear();
               
                if (Session["CampusAlterno"] != null)
                {
                    string se = Session["CampusAlterno"].ToString();
                    command.Parameters.AddWithValue("@campusid", Session["CampusAlterno"].ToString());
                    command.Parameters.AddWithValue("@id_periodo", id_periodo);
                }
                
                else
                {
                    command.Parameters.AddWithValue("@campusid", campusID);
                    command.Parameters.AddWithValue("@id_periodo", id_periodo);
                }

                dr = command.ExecuteReader();

                // agregamos todos los lenguajes a un StringBuilder y les concatenamos ":" para separarlos
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        nombrecompleto = dr["Matricula"].ToString();
                        nombrecompleto += "-- ";
                        nombrecompleto += dr["Nombre"].ToString();
                        nombrecompleto += " ";
                        nombrecompleto += dr["Apellido_paterno"].ToString();
                        nombrecompleto += " ";
                        nombrecompleto += dr["Apellido_materno"].ToString();
                        alumnos.Append(nombrecompleto + ":");
                        //alumnos.Append(dr[nombrecompleto].ToString() + ":");
                    }
                }
                else
                {
                    alumnos.Append("000000000 Sin alumnos:");
                    alumnos.Append("123456789 Sin alumnos:");
                }
            }
            catch (Exception es)
            {
                string errorws = es.ToString();
            }
            connection.Close();

            // eliminamos el último ":"
            return alumnos.ToString().Substring(0, alumnos.Length - 2);
        }
    }
}