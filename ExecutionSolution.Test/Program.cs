using System;
using ExecutionSolution.Test.Fakes;

namespace ExecutionSolution.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Demo");

            var notificador = new FakeNotificador();
            notificador.RecibirPeticion();

            Console.ReadLine();
        }
    }
}
