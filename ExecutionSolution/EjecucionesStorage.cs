using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ExecutionSolution.Core;

namespace ExecutionSolution
{
    public static class EjecucionesStorage
    {
        private static ConcurrentDictionary<int, PlantillaQueued> EjecucionesDePlantillas { get; set; }

        static EjecucionesStorage()
        {
            EjecucionesDePlantillas = new ConcurrentDictionary<int, PlantillaQueued>();
        }

        public static void RegistrarEjecucionPlantilla(PlantillaQueued plantillaQueued)
        {
            plantillaQueued.Finish += OnFinshPlantilla;

            EjecucionesDePlantillas.TryAdd(plantillaQueued.PlantillaId, plantillaQueued);
            Task.Run(() => plantillaQueued.IniciarEjecucion());
        }

        public static PlantillaQueued ObtenerPlantillaEnEjecucion(int plantillaId)
        {
            return EjecucionesDePlantillas.ContainsKey(plantillaId) ? EjecucionesDePlantillas[plantillaId] : default(PlantillaQueued);
        }

        private static void OnFinshPlantilla(object plantillaQueued, EventArgs argumentos)
        {
            var plantillaFinalizada = plantillaQueued as PlantillaQueued;
            if (plantillaFinalizada == null) return;

            EjecucionesDePlantillas.TryRemove(plantillaFinalizada.PlantillaId, out plantillaFinalizada);
            Console.WriteLine("La plantilla {0} finalizó correctamente", plantillaFinalizada.PlantillaId);
        }
    }
}