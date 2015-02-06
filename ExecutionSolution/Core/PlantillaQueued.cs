using System;
using System.Collections.Generic;
using System.Threading;
using ExecutionSolution.Extensions;
using ExecutionSolution.Notificador;
using ExecutionSolution.Messages;

namespace ExecutionSolution.Core
{
    public class PlantillaQueued
    {
        public string EjecucionId { get; set; }
        public int PlantillaId { get; set; }
        public List<ProcesoQueued> ProcesosQueued { get; set; }
        public event EventHandler<EventArgs> Finish;

        private readonly CancellationTokenSource _cancellationToken;
        private ProcesoNotificador _procesoNotificador;
        private readonly INotificador _notificador;

        public PlantillaQueued(INotificador notificador)
        {
            _cancellationToken = new CancellationTokenSource();
            _notificador = notificador;
        }

        public void IniciarEjecucion()
        {
            _procesoNotificador = new ProcesoNotificador(_notificador);
            _procesoNotificador.SuscribeProcesos(ProcesosQueued);

            ProcesosQueued.GestionarProcesos(_cancellationToken);

            OnFinish();
        }

        public void Cancelar()
        {
            _cancellationToken.Cancel();
        }

        private void OnFinish()
        {
            if (Finish != null)
                Finish(this, new EventArgs());
        }

        public void ActualizarParametroProceso(int procesoId, List<ParametroProcesoRespuestaMessage> valoresParametrosUsuario)
        {
            var procesoActual = ProcesosQueued.Find(p => p.ProcesoId == procesoId);

            procesoActual.ParametroManager.ActualizarParametroProceso(valoresParametrosUsuario);
            procesoActual.Reanudar();
        }
    }
}