namespace ExecutionSolution.Messages
{
    public class PeticionParametroNotificacion : Notificacion
    {
        public int ProcesoId { get; set; }
        public int ParametroId { get; set; }
    }
}