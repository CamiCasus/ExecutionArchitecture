using System;
using ExecutionSolution.Core;

namespace ExecutionSolution.Peticiones
{
    public class EjecucionPlantillaPeticion : IPeticion
    {
        public DateTime Fecha { get; set; }
        public PlantillaQueued PlantillaQueued { get; set; }

        public void ProcesarNotificacion()
        {
            EjecucionesStorage.RegistrarEjecucionPlantilla(PlantillaQueued);
        }
    }
}