using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExecutionSolution.AccionesEstados;
using ExecutionSolution.Core;
using ExecutionSolution.Notificador;
using ExecutionSolution.Parametros;
using ExecutionSolution.Peticiones;
using ExecutionSolution.Test.AccionesNotificacionFake;

namespace ExecutionSolution.Test.Fakes
{
    public class FakeNotificador : INotificador
    {
        private readonly ConcurrentQueue<INotificacion> _notificaciones;
        private readonly ConcurrentQueue<IPeticion> _peticiones;

        private readonly Dictionary<TipoNotificacion, INotificacionAccion> _notificacionAccions;

        public FakeNotificador()
        {
            _notificaciones = new ConcurrentQueue<INotificacion>();
            _peticiones = new ConcurrentQueue<IPeticion>();

            _notificacionAccions = new Dictionary<TipoNotificacion, INotificacionAccion>
            {
                {TipoNotificacion.EsperaParametro, new EsperaParametroNotificacionAccion()}
            };

            var peticionEjecucionPlantilla = new EjecucionPlantillaPeticion
            {
                PlantillaQueued = GetPlantillaParametrosProceso(this)
            };

            _peticiones.Enqueue(peticionEjecucionPlantilla);
        }

        public void EnviarNotificacion(INotificacion notificacion)
        {
            _notificaciones.Enqueue(notificacion);
        }

        public void EnviarPeticion(IPeticion peticion)
        {
            _peticiones.Enqueue(peticion);
        }

        public void RecibirPeticion()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    IPeticion notificacionActual;
                    _peticiones.TryDequeue(out notificacionActual);

                    if (notificacionActual == null)
                        Thread.Sleep(200);
                    else
                        notificacionActual.ProcesarPeticion();
                }
            });
        }

        public void RecibirNotificacion()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    INotificacion notificacionActual;
                    _notificaciones.TryDequeue(out notificacionActual);

                    if (notificacionActual == null)
                        Thread.Sleep(200);
                    else
                    {
                        _notificacionAccions[notificacionActual.TipoNotificacion].ProcesarNotificacion(notificacionActual, this);
                    }
                }
            });
        }

        public PlantillaQueued GetPlantillaDependenciaEntreProcesos(INotificador notificador)
        {
            var plantilla = new PlantillaQueued(notificador);
            var procesoNodo5 = new ProcesoQueued { ProcesoId = 5, Descripcion = "Nodo5", PlantillaQueued = plantilla };

            plantilla.ProcesosQueued = new List<ProcesoQueued>
            {
                new ProcesoQueued
                {
                    ProcesoId = 1,
                    Descripcion = "Nodo1",
                    PlantillaQueued = plantilla,
                    ProcesosQueued = new List<ProcesoQueued>
                    {
                        new ProcesoQueued
                        {
                            ProcesoId = 3,
                            Descripcion = "Nodo3",
                            PlantillaQueued = plantilla,
                            ProcesosQueued = new List<ProcesoQueued> {procesoNodo5}

                        },
                        new ProcesoQueued {ProcesoId = 4, Descripcion = "Nodo4", PlantillaQueued = plantilla},
                    }
                },
                new ProcesoQueued
                {
                    ProcesoId = 2,
                    Descripcion = "Nodo2",
                    PlantillaQueued = plantilla,
                    ProcesosQueued = new List<ProcesoQueued> {procesoNodo5}
                }
            };

            return plantilla;
        }

        public PlantillaQueued GetPlantillaParametrosProceso(INotificador notificador)
        {
            var plantilla = new PlantillaQueued(notificador) { ProcesosQueued = new List<ProcesoQueued>(), PlantillaId = 1};
            var proceso = new CuboProcesoQueued
            {
                ProcesoId = 1,
                Descripcion = "CuboProceso",
                PlantillaQueued = plantilla,
                ParametrosQueued = new List<ParametroQueued>()
            };

            var parametro = new ParametroEjecucionUsuario
            {
                NombreParametro = "Parametro1",
                ParametroId = 1,
                ProcesoQueued = proceso
            };

            proceso.ParametrosQueued.Add(parametro);
            plantilla.ProcesosQueued.Add(proceso);

            return plantilla;
        }
    }
}