using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioBecario.Codigo
{
    public class Trabajadores
    {
        private string nomina;
        private string correo;
        private string departamento;
        private string campus;
        private string divicion;
        private string extencion;
        private string nombre;
        private string materno;
        private string paterno;
        private string puesto;
        private string ubicacionFisica;
        private string grupo;
        private string area;
        public string  Nomina
        {
            get 
            {
                return this.nomina;
            }
            set
            {
                this.nomina = value;
            }
        }


        public string Correo
        {
            get
            {
                return this.correo;
            }

            set
            {
                this.correo = value;
            }
        }

        public string Departamentamento
        {
            set
            {
                this.departamento = value;
            }
            get
            {
                return this.departamento;
            }
        }
        public string Campus
        {
            set
            {
                this.campus = value;
            }
            get
            {
               return  this.campus;
            }
        }

        public string Divicion
        {
            set
            {
                this.divicion = value;
            }
            get
            {
                return this.divicion;
            }
        }
        public string Extencion
        {
            set {
                this.extencion = value;
            }
            get {
                return this.extencion;
            }
        }


        public string Nombre
        {
            set {
                this.nombre = value;
            }
            get {
                return this.nombre;
            }
        }

        public string Paterno
        {
            set {
                this.paterno = value;
            }
            get {
                return this.paterno;
            }
        }

        public string Materno
        {
            set
            {
                this.materno = value;
            }
            get
            {
                return this.materno;
            }
        }
        public string Puesto
        {
            set
            {
                this.puesto = value;
            }
            get
            {
                return this.puesto;
            }
        }
        public string UbicacionFisica
        {
            set
            {
                this.ubicacionFisica = value;
            }
            get
            {
                return this.ubicacionFisica;
            }
        }

        public string Grupo
        {
            set
            {
                this.grupo = value;
            }
            get
            {
                return this.grupo;
            }
        }

        public string Area
        {
            set
            {
                this.area = value;
            }
            get
            {
                return this.area;
            }
        }
    }
}