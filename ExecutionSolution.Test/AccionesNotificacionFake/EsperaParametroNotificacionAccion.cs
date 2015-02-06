using System;
using System.Collections.Generic;
using System.Linq;
using ExecutionSolution.Messages;
using ExecutionSolution.Notificador;
using ExecutionSolution.Peticiones;

namespace ExecutionSolution.Test.AccionesNotificacionFake
{
    public class EsperaParametroNotificacionAccion : INotificacionAccion
    {
        public void ProcesarNotificacion(INotificacion notificacion, INotificador notificador)
        {
            var notificacionEsperaParametro = notificacion as PeticionParametroNotificacion;
            var parametro = notificacionEsperaParametro.ParametroProcesoMessages.First();

            Console.WriteLine("El parametro {0} requiere un valor", parametro.NombreParametro);
            var valorParametro = Console.ReadLine();


            var actualizarValorParametroPeticion = new ActualizarParametroProcesoPeticion
            {
                PlantillaId = 1,
                ProcesoId = 1,
                ValorParametros = new List<ParametroProcesoRespuestaMessage>
                {
                    new ParametroProcesoRespuestaMessage
                    {
                        NombreParametro = parametro.NombreParametro,
                        ParametroId = parametro.ParametroId,
                        Valor = valorParametro
                    }
                }
            };

            notificador.EnviarPeticion(actualizarValorParametroPeticion);
        } 
    }
}