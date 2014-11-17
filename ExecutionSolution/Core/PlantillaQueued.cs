using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExecutionSolution.Core
{
    public class PlantillaQueued
    {
        public string EjecucionId { get; set; }
        public int PlantillaId { get; set; }

        public List<ProcesoQueued> Procesos { get; set; }
        public List<LinkProcesoQueued> DependenciaProcesos { get; set; }

        private readonly CancellationTokenSource _cancellationToken;

        public PlantillaQueued()
        {
            _cancellationToken = new CancellationTokenSource();
        }

        public void Cancelar()
        {
            _cancellationToken.Cancel();
            Console.WriteLine("Se inicio peticion de cancelacion");
        }

        public void IniciarEjecucion()
        {
            try
            {
                IEnumerable<ProcesoQueued> procesosIniciales =
                        Procesos.Where(procesoInterno => DependenciaProcesos.All(p => p.To != procesoInterno.ProcesoId));

                ManageProcessList(procesosIniciales);
                Console.WriteLine("Se termino la ejecucion de la plantilla");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine("Se cancelo la plantilla");           
            }
        }

        private Task GenerarProcessTask(ProcesoQueued procesoQueued)
        {
            var taskActual = Task.Factory.StartNew(() =>
            {
                procesoQueued.Ejecutar(_cancellationToken);

                IEnumerable<ProcesoQueued> procesosHijos = BuscarProcesosHijos(procesoQueued.ProcesoId);
                ManageProcessList(procesosHijos);

            }, _cancellationToken.Token);

            
            return taskActual;
        }

        private void ManageProcessList(IEnumerable<ProcesoQueued> procesos)
        {
            var taskListParentProcess = new ConcurrentBag<Task>();

            foreach (var procesoQueued in procesos)
            {
                ProcesoQueued queued = procesoQueued;
                var taksActual = GenerarProcessTask(queued);

                taskListParentProcess.Add(taksActual);
            }

            Task.WaitAll(taskListParentProcess.ToArray(), _cancellationToken.Token);
        }

        private IEnumerable<ProcesoQueued> BuscarProcesosHijos(int procesoId)
        {
            IEnumerable<int> listaLinks = DependenciaProcesos.Where(p => p.From == procesoId).Select(p => p.To);
            return Procesos.Where(p => listaLinks.Contains(p.ProcesoId));
        }
    }
}