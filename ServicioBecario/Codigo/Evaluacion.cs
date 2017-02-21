using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicioBecario.Vistas
{
    public partial class Evaluacion
    {
        public List<Pregunta> preguntas = new List<Pregunta>();
        public float puntaje;
    }

    [Serializable]
    public partial class Pregunta
    {
        public int ID;
        public string descripcion;
        public string tipo;
        public List<Respuesta> respuestas = new List<Respuesta>();
    }

    [Serializable]
    public partial class Respuesta
    {
        public int ID;
        public string descripcion;
        public float valor;
    }
}