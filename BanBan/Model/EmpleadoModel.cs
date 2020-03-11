using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanBan.Model
{
    public class EmpleadoModel
    {
        public EmpleadoModel()
        {
            planillasHorario = new List<PlanillaEmpleadoModel>();
            horarioExtra = new List<HorarioExtra>();
        }
        public EmpleadoModel(empleado emp)
        {
            idEmpleado = emp.idEmpleado;
            nombre = emp.nombre;
            apellido = emp.apellido;
            estado = emp.estado;
            sueldo = emp.sueldo;
            planillasHorario = new List<PlanillaEmpleadoModel>();
            horarioExtra = new List<HorarioExtra>();
        }
        public int idEmpleado { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public bool estado { get; set; }
        public decimal? sueldo { get; set; }
        public List<PlanillaEmpleadoModel> planillasHorario { get; set; }
        public List<HorarioExtra> horarioExtra { get; set; }
        public decimal desucentoAFP { get; set; }
        public decimal atencionCargo { get; set; }

    }
    public class PlanillaEmpleadoModel
    {
        public PlanillaEmpleadoModel()
        {
        }
        public PlanillaEmpleadoModel(int idPlanilla, planillahorario pnh)
        {
            comentario = pnh.comentario;
            entrada = pnh.entrada;
            idEmpleado = pnh.idEmpleado;
            salida = pnh.salida;
            this.idPlanilla = idPlanilla;
        }
        public DateTime entrada { get; set; }
        public DateTime salida { get; set; }
        public string comentario { get; set; }
        public int idEmpleado { get; set; }
        public int idPlanilla { get; set; }
    }

    public class HorarioExtra
    {
        public HorarioExtra()
        {
        }
        public HorarioExtra(int planilla,horarioextra he) { }
        public int id { get; set; }
        public double? horas { get; set; }
        public DateTime? fecha { get; set; }
        public string comentarios { get; set; }
        public int idTipoHora { get; set; }
        public int idEmpleado { get; set; }
        public int idPlanilla { get; set; }
    }
}
