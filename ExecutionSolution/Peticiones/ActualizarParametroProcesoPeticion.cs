using ExecutionSolution.Messages;
using System.Collections.Generic;

namespace ExecutionSolution.Peticiones
{
    public class ActualizarParametroProcesoPeticion : IPeticion
    {
        public int PlantillaId { get; set; }
        public int ProcesoId { get; set; }
        public List<ParametroProcesoRespuestaMessage> ValorParametros { get; set; }

        public void ProcesarPeticion()
        {
            var plantillaEnEjecucion = EjecucionesStorage.ObtenerPlantillaEnEjecucion(PlantillaId);
            plantillaEnEjecucion.ActualizarParametroProceso(ProcesoId, ValorParametros);
        }
    }
}