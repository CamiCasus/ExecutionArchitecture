using ExecutionSolution.Peticiones;

namespace ExecutionSolution.Notificador
{
    public interface INotificador
    {
        void EnviarNotificacion(INotificacion notificacion);
        void EnviarPeticion(IPeticion peticion);
        void RecibirNotificacion();
        void RecibirPeticion();
    }
}