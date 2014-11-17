using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExecutionSolution.Core;

namespace ExecutionSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Demo");

            var plantilla = new PlantillaQueued
            {
                Procesos = new List<ProcesoQueued>
                {
                    new ProcesoQueued {ProcesoId = 1, Descripcion = "Nodo1"},
                    new ProcesoQueued {ProcesoId = 2, Descripcion = "Nodo2"},
                    new ProcesoQueued {ProcesoId = 3, Descripcion = "Nodo3"},
                    new ProcesoQueued {ProcesoId = 4, Descripcion = "Nodo4"},
                    new ProcesoQueued {ProcesoId = 5, Descripcion = "Nodo5"}
                },
                DependenciaProcesos = new List<LinkProcesoQueued>
                {
                    new LinkProcesoQueued {From = 1, To = 3},
                    new LinkProcesoQueued {From = 1, To = 4},
                    new LinkProcesoQueued {From = 2, To = 5},
                    new LinkProcesoQueued {From = 3, To = 5}
                }
            };

            Task.Run(() => plantilla.IniciarEjecucion());

            var cancelar = Console.ReadLine();

            if(cancelar == "exit")
                plantilla.Cancelar();

            Console.ReadLine();
        }
    }
}
