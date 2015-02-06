using System;

namespace ExecutionSolution.Notificador
{
    public interface INotificacion
    {
        TipoNotificacion TipoNotificacion { get; set; }
        DateTime Fecha { get; set; }
    }
}