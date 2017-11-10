using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Proyecto
{

    class Program
    {
        private static int quantum;
        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese valor del quantum: ");   // Valor del quantum
            quantum = int.Parse(Console.ReadLine());
        }
    }
}
