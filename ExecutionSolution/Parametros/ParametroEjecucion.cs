using ExecutionSolution.Core;

namespace ExecutionSolution.Parametros
{
    public class ParametroEjecucion : ParametroQueued
    {
        public ParametroQueued ParametroPadre { get; set; }

        public override void CargarParametro()
        {
            if (ParametroPadre != null)
                ValorParametro = ParametroPadre.ValorParametro;
        }
    }
}