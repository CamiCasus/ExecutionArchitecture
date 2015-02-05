using ExecutionSolution.Core;

namespace ExecutionSolution.Parametros
{
    public class ParametroEjecucionUsuario : ParametroQueued
    {
        public override void CargarParametro()
        {
            ProcesoQueued.ParametroManager.RegistrarPeticionParametro(this);
        }
    }
}