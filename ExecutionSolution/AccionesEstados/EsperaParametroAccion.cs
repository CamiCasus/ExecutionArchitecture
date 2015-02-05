using ExecutionSolution.Core;
using ExecutionSolution.Notificador;

namespace ExecutionSolution.AccionesEstados
{
    public class EsperaParametroAccion : IAccion
    {
        public INotificador Notificador { get; set; }
        
        public EsperaParametroAccion(INotificador notificador)
        {
            Notificador = notificador;
        }

        public void EjecutarAccion(ProcesoQueued procesoQueued)
        {
            var notificacion = procesoQueued.ParametroManager.GenerarMensajePeticionParametros();
            Notificador.EnviarNotificacion(notificacion);
        }
    }
}