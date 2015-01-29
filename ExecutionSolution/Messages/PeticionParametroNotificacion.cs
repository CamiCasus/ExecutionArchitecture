using System.Collections.Generic;

namespace ExecutionSolution.Messages
{
    public class PeticionParametroNotificacion : Notificacion
    {
        public int ProcesoId { get; set; }
        public List<ParametroProcesoMessage> ParametroProcesoMessages { get; set; }
    }
}