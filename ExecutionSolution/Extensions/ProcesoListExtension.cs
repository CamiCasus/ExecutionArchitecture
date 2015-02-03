using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExecutionSolution.Core;

namespace ExecutionSolution.Extensions
{
    public static class ProcesoListExtension
    {
        public static void GestionarProcesos(this IEnumerable<ProcesoQueued> procesos, CancellationTokenSource tokenSource)
        {
            var processBag = new ConcurrentBag<Task>();

            if (procesos == null)
                return;

            foreach (
                var taskProceso in
                    procesos.Select(
                        queued => Task.Factory.StartNew(() => queued.Ejecutar(tokenSource), tokenSource.Token)))
            {
                processBag.Add(taskProceso);
            }

            WaitToFinish(processBag);
        }

        private static void WaitToFinish(IProducerConsumerCollection<Task> concurrentBag)
        {
            try
            {
                Task.WaitAll(concurrentBag.ToArray());
            }
            catch (AggregateException e)
            {
                Console.WriteLine("\nAggregateException thrown with the following inner exceptions:");
                foreach (var v in e.InnerExceptions)
                {
                    var exception = v as TaskCanceledException;

                    if (exception != null)
                        Console.WriteLine("   TaskCanceledException: Task {0}", exception.Task.Id);
                    else
                        Console.WriteLine("   Exception: {0}", v.GetType().Name);
                }

                Console.WriteLine();
            }
        }
    }
}