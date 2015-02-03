using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExecutionSolution.Core;
using ExecutionSolution.Parametros;

namespace ExecutionSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Demo");

            var plantilla = GetPlantillaParametrosProceso();

            Task.Run(() => plantilla.IniciarEjecucion());
            var cancelar = Console.ReadLine();

            if (cancelar == "exit")
                plantilla.Cancelar();

            Console.ReadLine();
        }

        public static PlantillaQueued GetPlantillaDependenciaEntreProcesos()
        {
            var plantilla = new PlantillaQueued();
            var procesoNodo5 = new ProcesoQueued { ProcesoId = 5, Descripcion = "Nodo5", PlantillaQueued = plantilla };

            plantilla.ProcesosQueued = new List<ProcesoQueued>
            {
                new ProcesoQueued
                {
                    ProcesoId = 1,
                    Descripcion = "Nodo1",
                    PlantillaQueued = plantilla,
                    ProcesosQueued = new List<ProcesoQueued>
                    {
                        new ProcesoQueued
                        {
                            ProcesoId = 3,
                            Descripcion = "Nodo3",
                            PlantillaQueued = plantilla,
                            ProcesosQueued = new List<ProcesoQueued> {procesoNodo5}

                        },
                        new ProcesoQueued {ProcesoId = 4, Descripcion = "Nodo4", PlantillaQueued = plantilla},
                    }
                },
                new ProcesoQueued
                {
                    ProcesoId = 2,
                    Descripcion = "Nodo2",
                    PlantillaQueued = plantilla,
                    ProcesosQueued = new List<ProcesoQueued> {procesoNodo5}
                }
            };

            return plantilla;
        }

        public static PlantillaQueued GetPlantillaParametrosProceso()
        {
            var plantilla = new PlantillaQueued {ProcesosQueued = new List<ProcesoQueued>()};
            var proceso = new ProcesoQueued
            {
                ProcesoId = 1,
                Descripcion = "Nodo1",
                PlantillaQueued = plantilla,
                ParametrosQueued = new List<ParametroQueued>()
            };

            var parametro = new ParametroEjecucionUsuario
            {
                NombreParametro = "Parametro1",
                ParametroId = 1,
                ProcesoQueued = proceso
            };

            proceso.ParametrosQueued.Add(parametro);
            plantilla.ProcesosQueued.Add(proceso);

            return plantilla;
        }
    }
}
