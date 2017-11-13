using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Proyecto
{
    class Controlador
    {
        public Procesador procesador0;
        public Procesador procesador1;
        public Barrier sync;
        public int quantum;
        int numeroHilos;

        public Controlador(int q) {
            quantum = q;
            sync = new Barrier(4); // Barrier con 4 participantes(los 3 núcleos y el hilo controlador)
            procesador0 = new Procesador(0);
            procesador1 = new Procesador(1);
            numeroHilos = 4;
            // sync = new Barrier(); 
          /*procesador0 = new Procesador(0,sync);
            procesador1 = new Procesador(1,sync); */ // Aquí 
        }

        public int[] cargarTxt(int numProc)
        {
            string path0 = "C:\\Users\\b37709\\Desktop\\Arqui\\Proyecto\\hilillos\\hilillos0";
            string path1 = "C:\\Users\\b37709\\Desktop\\Arqui\\Proyecto\\hilillos\\hilillos1";

            int indiceMem = 0;  // Para movernos por el array de la memoria principal
            int indiceMem1 = 0;
            int indiceMem2 = 0;
            int[] inicioHilillo;
            int indiceInicioHilillo = 0;
            if (numProc == 0)
            {
                string[] nombreArchivos = System.IO.Directory.GetFiles(path0, "*.txt");
                inicioHilillo = new int[nombreArchivos.Length];
                for (int i = 0; i < nombreArchivos.Length; ++i)
                {
                    inicioHilillo[indiceInicioHilillo] = indiceMem;
                    ++indiceInicioHilillo;

                    string[] instrucciones = System.IO.File.ReadAllLines(nombreArchivos[i]);
                    for (int j = 0; j < instrucciones.Length; ++j, ++indiceMem)  // Se itera por el array de instrucciones
                    {
                        string numero = "";     // El numero que se va a parsear
                        for (int k = 0; k < instrucciones[j].Length; ++k) // Se van a separar los numeros
                        {
                            if (instrucciones[j][k] != ' ')
                            {
                                numero += instrucciones[j][k];
                            }
                            else
                            {
                                procesador0.memInst[indiceMem] = Int32.Parse(numero);
                                numero = "";
                                ++indiceMem;
                            }
                        }
                        procesador0.memInst[indiceMem] = Int32.Parse(numero);
                        numero = "";
                    }
                }

               /* Console.WriteLine("Instrucciones p0");
                for (int i = 0; i < 50; ++i)
                {
                    Console.Write(procesador0.memInst[i] + "  ");
                }
                Console.Write("\n");
                Console.WriteLine("\n inicioHilillos p 0");
                for (int i = 0; i < inicioHilillo.Length; ++i)
                {
                    Console.Write(inicioHilillo[i] + "  ");
                }*/
            }
            else
            {
                string[] nombreArchivos = System.IO.Directory.GetFiles(path1, "*.txt");
                inicioHilillo = new int[nombreArchivos.Length];
                for (int i = 0; i < nombreArchivos.Length; ++i)
                {
                    inicioHilillo[indiceInicioHilillo] = indiceMem;
                    ++indiceInicioHilillo;
                    string[] instrucciones = System.IO.File.ReadAllLines(nombreArchivos[i]);
                    for (int j = 0; j < instrucciones.Length; ++j, ++indiceMem)  // Se itera por el array de instrucciones
                    {
                        string numero = "";     // El numero que se va a parsear
                        for (int k = 0; k < instrucciones[j].Length; ++k) // Se van a separar los numeros
                        {
                            if (instrucciones[j][k] != ' ')
                            {
                                numero += instrucciones[j][k];
                            }
                            else
                            {
                                procesador1.memInst[indiceMem] = Int32.Parse(numero);
                                numero = "";
                                ++indiceMem;
                            }
                        }
                        procesador1.memInst[indiceMem] = Int32.Parse(numero);
                        numero = "";
                    }
                }

               /* Console.WriteLine("Instrucciones p1");
                for (int i = 0; i < 50; ++i)
                {
                    Console.Write(procesador1.memInst[i] + "  ");
                }


                Console.WriteLine("\n inicioHilillos p 1");
                for (int i = 0; i < inicioHilillo.Length; ++i)
                {
                    Console.Write(inicioHilillo[i] + "  ");
                }*/
            }
        
           
            

            return inicioHilillo;
        } 
   }
}
