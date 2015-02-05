using System;
using System.Linq;
using ExecutionSolution.Core;

namespace ExecutionSolution.Test.Fakes
{
    public class CuboProcesoQueued : ProcesoQueued
    {
        protected override bool EjecutarProceso()
        {
            var parametro = ParametrosQueued.First();
            Console.WriteLine(Math.Pow(int.Parse(parametro.ValorParametro), 3));

            return true;
        }
    }
}