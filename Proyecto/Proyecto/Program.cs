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
            int[] temp1 = cont.cargarTxt(0);
            int[] temp2 = cont.cargarTxt(1);
            Console.WriteLine("PCs proc 0: ");
            cont.procesador0.crearColaContextos(temp1);
            Console.WriteLine("PCs proc 1: ");
            cont.procesador1.crearColaContextos(temp2);

            cont.procesador0.calcularBloque(0);
            cont.procesador0.calcularBloque(1);
            cont.procesador1.calcularBloque(0);


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
