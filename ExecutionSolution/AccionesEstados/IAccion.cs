using ExecutionSolution.Core;
using ExecutionSolution.Notificador;

namespace ExecutionSolution.AccionesEstados
{
    public interface IAccion
    {
        INotificador Notificador { get; set; }
        void EjecutarAccion(ProcesoQueued procesoQueued);
    }
}