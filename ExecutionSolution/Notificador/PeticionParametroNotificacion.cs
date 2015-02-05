using System;
using System.Collections.Generic;
using ExecutionSolution.Messages;

namespace ExecutionSolution.Notificador
{
    public class PeticionParametroNotificacion : INotificacion
    {
        //public int PlantillaId { get; set; }
        public DateTime Fecha { get; set; }
        public int ProcesoId { get; set; }
        public List<ParametroProcesoMessage> ParametroProcesoMessages { get; set; }
    }
}