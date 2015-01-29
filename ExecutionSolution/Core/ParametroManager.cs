using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using ExecutionSolution.Messages;
using ExecutionSolution.Notificador;

namespace ExecutionSolution.Core
{
    public static class ParametroManager
    {
        public static ConcurrentDictionary<int, ConcurrentBag<ParametroQueued>> ParametrosPeticionUsuarioPorProceso { get; set; }

        public static void GestionarParametrosProceso(ProcesoQueued procesoQueued)
        {
            if (procesoQueued.ParametrosQueued == null || !procesoQueued.ParametrosQueued.Any()) return;

            foreach (ParametroQueued parametroQueued in procesoQueued.ParametrosQueued)
            {
                parametroQueued.CargarParametro();
            }

            GenerarMensajePeticionParametros(procesoQueued);
        }

        public static void RegistrarPeticionParametro(ProcesoQueued procesoQueued, ParametroQueued parametroQueued)
        {
            ConcurrentBag<ParametroQueued> listaParametrosDelProceso;
            ParametrosPeticionUsuarioPorProceso.TryGetValue(procesoQueued.ProcesoId, out listaParametrosDelProceso);

            if (listaParametrosDelProceso == null)
            {
                listaParametrosDelProceso = new ConcurrentBag<ParametroQueued> {parametroQueued};
                ParametrosPeticionUsuarioPorProceso.TryAdd(procesoQueued.ProcesoId, listaParametrosDelProceso);
            }
            else
            {
                listaParametrosDelProceso.Add(parametroQueued);
            }
        }

        public static void GenerarMensajePeticionParametros(ProcesoQueued procesoQueued)
        {
            ConcurrentBag<ParametroQueued> parametroQueueds;

            if (ParametrosPeticionUsuarioPorProceso.ContainsKey(procesoQueued.ProcesoId))
                parametroQueueds = ParametrosPeticionUsuarioPorProceso[procesoQueued.ProcesoId];
            else return;

            var nuevoMensajePeticionParametro = new PeticionParametroNotificacion
            {
                Fecha = DateTime.Now,
                PlantillaId = procesoQueued.PlantillaQueued.PlantillaId,
                ProcesoId = procesoQueued.ProcesoId,
                ParametroProcesoMessages = parametroQueueds
                    .Select(p => new ParametroProcesoMessage
                    {
                        ParametroId = p.ParametroId,
                        NombreParametro = p.NombreParametro
                    }).ToList()
            };

            NotificadorExterno.EnviarNotificacion(nuevoMensajePeticionParametro);

            //detener hilo
        }
    }
}