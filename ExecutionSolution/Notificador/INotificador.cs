namespace ExecutionSolution.Notificador
{
    public interface INotificador
    {
        void EnviarNotificacion(INotificacion notificacion);
        void RecibirNotificacion();
        void RecibirPeticion();
    }
}