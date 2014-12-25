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

            var procesoNodo5 = new ProcesoQueued {ProcesoId = 5, Descripcion = "Nodo5"};

            var plantilla = new PlantillaQueued
            {
                ProcesosQueued = new List<ProcesoQueued>
                {
                    new ProcesoQueued
                    {
                        ProcesoId = 1,
                        Descripcion = "Nodo1",
                        ProcesosQueued = new List<ProcesoQueued>
                        {
                            new ProcesoQueued
                            {
                                ProcesoId = 3,
                                Descripcion = "Nodo3",
                                ProcesosQueued = new List<ProcesoQueued> {procesoNodo5}

                            },
                            new ProcesoQueued {ProcesoId = 4, Descripcion = "Nodo4"},
                        }
                    },
                    new ProcesoQueued
                    {
                        ProcesoId = 2,
                        Descripcion = "Nodo2",
                        ProcesosQueued = new List<ProcesoQueued> {procesoNodo5}
                    }
                }
            };

            Task.Run(() => plantilla.IniciarEjecucion());
            var cancelar = Console.ReadLine();

            if (cancelar == "exit")
                plantilla.Cancelar();

            Console.ReadLine();
        }
    }
}
