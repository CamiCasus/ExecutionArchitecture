﻿using System;
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
        public IEnumerable<ProcesoQueued> ProcesosQueued { get; set; }

        private readonly List<IObserver<ProcesoQueued>> _observers;

        protected CancellationTokenSource TokenSource;

        public ProcesoQueued()
        {
            Estado = EstadoProceso.SinProcesar;
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

        private void GestionarEjecucion()
        {
            Estado = EstadoProceso.Procesando;
            Notify();

            if (EjecutarProceso())
            {
                ProcesosQueued.GestionarProcesos(TokenSource);
            }
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

        private bool EsPosibleEjecutarProceso()
        {
            return Estado == EstadoProceso.SinProcesar;
        }

        public IDisposable Subscribe(IObserver<ProcesoQueued> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
                //observer.OnNext(this);
            }

            return new Unsubscriber<ProcesoQueued>(_observers, observer);
        }

        private void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(this);
            }
        }
    }
}