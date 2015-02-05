using System;
using System.Collections.Generic;
using ExecutionSolution.AccionesEstados;
using ExecutionSolution.Notificador;

namespace ExecutionSolution.Core
{
    public class ProcesoNotificador : IObserver<ProcesoQueued>
    {
        private readonly Dictionary<EstadoProceso, IAccion> _accionesEstadoProceso;
        private INotificador _notificador;

        public ProcesoNotificador(INotificador notificador)
        {
            _notificador = notificador;

            _accionesEstadoProceso = new Dictionary<EstadoProceso, IAccion>
            {
                {EstadoProceso.EsperandoParametro, new EsperaParametroAccion(_notificador)}
            };
        }

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
            if(_accionesEstadoProceso.ContainsKey(value.Estado))
                _accionesEstadoProceso[value.Estado].EjecutarAccion(value);
           
            Console.WriteLine("El proceso {0} se encuentra en el estado {1}", value.ProcesoId, value.Estado);
        }
    }
}