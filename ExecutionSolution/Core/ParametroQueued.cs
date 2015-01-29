namespace ExecutionSolution.Core
{
    public class ParametroQueued
    {
        public string NombreParametro { get; set; }
        public string ValorParametro { get; set; }
        public int ParametroId { get; set; }
        public ProcesoQueued ProcesoQueued { get; set; }

        public virtual void CargarParametro()
        {
            
        }
    }
}