namespace ExecutionSolution.Notificador
{
    public interface INotificacionAccion
    {
        void ProcesarNotificacion(INotificacion notificacion, INotificador notificador);
    }
}