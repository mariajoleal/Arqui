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

            Controlador cont = new Controlador(quantum);
            cont.cargarTxt();
            /*Thread hiloPrincipal = new Thread(new ThreadStart(cont.iniciarPrograma));    // Se crea un nuevo hilo controlador
            hiloPrincipal.Start();  // Se inicia el hilo controlador

            while (hiloPrincipal.IsAlive)   // El programa principal espera a que el hilo principal haya terminado
            {

            }*/

            Console.WriteLine("\nPresione cualquier tecla para salir del programa");
            Console.ReadKey();
        }
    }
}
