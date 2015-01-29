using ExecutionSolution.Core;

namespace ExecutionSolution.Parametros
{
    public class ParametroEjecucionUsuario : ParametroQueued
    {
        public override void CargarParametro()
        {
            ParametroManager.RegistrarPeticionParametro(ProcesoQueued, this);
        }
    }
}