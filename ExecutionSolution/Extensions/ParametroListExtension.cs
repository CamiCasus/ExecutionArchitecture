using System.Collections.Generic;
using ExecutionSolution.Core;

namespace ExecutionSolution.Extensions
{
    public static class ParametroListExtension
    {
        public static void GestionarParametros(this IEnumerable<ParametroQueued> parametros, ProcesoQueued procesoQueued)
        {
            if (parametros == null)
                return;

            foreach (ParametroQueued parametroQueued in parametros)
            {
                parametroQueued.CargarParametro();
            }
        }
    }
}