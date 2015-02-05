using System;
using System.Collections.Generic;
using System.Threading;
using ExecutionSolution.Extensions;

namespace ExecutionSolution.Core
{
    public class ProcesoQueued : IObservable<ProcesoQueued>
    {
        public int ProcesoId { get; set; }
        public string Descripcion { get; set; }
        public EstadoProceso Estado { get; set; }
        public PlantillaQueued PlantillaQueued { get; set; }
        public List<ProcesoQueued> ProcesosQueued { get; set; }
        public List<ParametroQueued> ParametrosQueued { get; set; }

        private readonly List<IObserver<ProcesoQueued>> _observers;
        private readonly ManualResetEventSlim _pauseEvent = new ManualResetEventSlim(true);
        protected CancellationTokenSource TokenSource;

        public ParametroManager ParametroManager { get; set; }

        public ProcesoQueued()
        {
            Estado = EstadoProceso.SinProcesar;
            ParametroManager = new ParametroManager(this);
            _observers = new List<IObserver<ProcesoQueued>>();
        }

        public void Ejecutar(CancellationTokenSource tokenSource)
        {
            TokenSource = tokenSource;

            if (tokenSource.Token.IsCancellationRequested)
            {
                tokenSource.Token.ThrowIfCancellationRequested();
            }

            if (!EsPosibleEjecutarProceso()) return;

           GestionarEjecucion();
        }

        protected virtual bool EjecutarProceso()
        {
            var aleatorio = new Random();

            var duracionEjecucion = ProcesoId == 2 ? 3000 : aleatorio.Next(5000, 8000);

            Thread.Sleep(duracionEjecucion);

            Estado = EstadoProceso.Exito;
            Notify();

            return true;
        }

        public IDisposable Subscribe(IObserver<ProcesoQueued> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber<ProcesoQueued>(_observers, observer);
        }

        private void GestionarEjecucion()
        {
            Estado = EstadoProceso.Procesando;
            Notify();

            ParametroManager.GestionarParametrosProceso();

            if (EjecutarProceso())
            {
                ProcesosQueued.GestionarProcesos(TokenSource);
            }
        }

        private bool EsPosibleEjecutarProceso()
        {
            return Estado == EstadoProceso.SinProcesar;
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(this);
            }
        }

        public void Pausar()
        {
            _pauseEvent.Reset();
            _pauseEvent.Wait(TokenSource.Token);
        }

        public void EsperarRespuestaParametros()
        {
            Estado = EstadoProceso.EsperandoParametro;
            Notify();

            Pausar();
        }

        public void Reanudar()
        {
            _pauseEvent.Set();
        }
    }
}