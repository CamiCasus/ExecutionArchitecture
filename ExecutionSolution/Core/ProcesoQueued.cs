using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExecutionSolution.Core
{
    public class ProcesoQueued
    {
        public int ProcesoId { get; set; }
        public string Descripcion { get; set; }

        public bool Ejecutar(CancellationTokenSource tokenSource)
        {
            var tareaProceso = Task.Factory.StartNew(() => EjecutarProceso(), tokenSource.Token);

            tareaProceso.Wait(tokenSource.Token);
            Console.WriteLine("El proceso {0} se ejecuto correctamente", ProcesoId);

            return true;
        }

        protected virtual bool EjecutarProceso()
        {
            var aleatorio = new Random();
            Thread.Sleep(ProcesoId == 2 ? 3000 : aleatorio.Next(10000, 20000));

            return true;
        }
    }
}