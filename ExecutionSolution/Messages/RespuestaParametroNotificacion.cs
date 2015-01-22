namespace ExecutionSolution.Messages
{
    public class RespuestaParametroNotificacion : Notificacion
    {
        public int ProcesoId { get; set; }
        public int ParametroId { get; set; }
        public string ValorParametro { get; set; }
    }
}