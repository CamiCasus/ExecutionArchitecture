using System;
using System.Collections.Generic;

namespace ExecutionSolution.Core
{
    public class ProcesoNotificador : IObserver<ProcesoQueued>
    {
        private void Suscribe(IEnumerable<ProcesoQueued> procesosQueued)
        {
            if (procesosQueued == null) return;

            foreach (ProcesoQueued procesoQueued in procesosQueued)
            {
                procesoQueued.Subscribe(this);
                Suscribe(procesoQueued.ProcesosQueued);
            }
        }

        public void SuscribeProcesos(IEnumerable<ProcesoQueued> procesosQueued)
        {
            Suscribe(procesosQueued);
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(ProcesoQueued value)
        {
            Console.WriteLine("El proceso {0} se encuentra en el estado {1}", value.ProcesoId, value.Estado);
        }
    }
}