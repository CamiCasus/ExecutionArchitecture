using System.Collections.Concurrent;
using System.Linq;
using ExecutionSolution.Messages;
using System;
using ExecutionSolution.Notificador;

namespace ExecutionSolution.Core
{
    public class ParametroManager
    {
        public ConcurrentBag<ParametroQueued> ParametrosPeticionUsuario { get; set; }
        public ProcesoQueued ProcesoQueued { get; set; }

        public ParametroManager(ProcesoQueued procesoQueued)
        {
            ProcesoQueued = procesoQueued;
            ParametrosPeticionUsuario = new ConcurrentBag<ParametroQueued>();
        }

        public void GestionarParametrosProceso()
        {
            if (ProcesoQueued.ParametrosQueued == null || !ProcesoQueued.ParametrosQueued.Any()) return;

            foreach (ParametroQueued parametroQueued in ProcesoQueued.ParametrosQueued)
            {
                parametroQueued.CargarParametro();
            }

            if (ParametrosPeticionUsuario.Count > 0)
                ProcesoQueued.EsperarRespuestaParametros();
        }

        public void RegistrarPeticionParametro(ParametroQueued parametroQueued)
        {
            ParametrosPeticionUsuario.Add(parametroQueued);
        }

        public PeticionParametroNotificacion GenerarMensajePeticionParametros()
        {
            var nuevoMensajePeticionParametro = new PeticionParametroNotificacion
            {
                Fecha = DateTime.Now,
                //PlantillaId = ProcesoQueued.PlantillaQueued.PlantillaId,
                ProcesoId = ProcesoQueued.ProcesoId,
                ParametroProcesoMessages = ParametrosPeticionUsuario
                    .Select(p => new ParametroProcesoMessage
                    {
                        ParametroId = p.ParametroId,
                        NombreParametro = p.NombreParametro
                    }).ToList()
            };

            return nuevoMensajePeticionParametro;
        }
    }
}