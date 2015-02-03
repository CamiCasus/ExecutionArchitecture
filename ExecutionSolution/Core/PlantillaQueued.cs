﻿using System;
using System.Collections.Generic;
using System.Threading;
using ExecutionSolution.Extensions;

namespace ExecutionSolution.Core
{
    public class PlantillaQueued
    {
        public string EjecucionId { get; set; }
        public int PlantillaId { get; set; }

        public List<ProcesoQueued> ProcesosQueued { get; set; }

        private readonly CancellationTokenSource _cancellationToken;
        private ProcesoNotificador _procesoNotificador;

        public PlantillaQueued()
        {
            _cancellationToken = new CancellationTokenSource();
        }
     
        public void IniciarEjecucion()
        {
            _procesoNotificador = new ProcesoNotificador();
            _procesoNotificador.SuscribeProcesos(ProcesosQueued);

            ProcesosQueued.GestionarProcesos(_cancellationToken);
            Console.WriteLine("Se termino la ejecucion de la plantilla");
        }

        public void Cancelar()
        {
            _cancellationToken.Cancel();
        }
    }
}