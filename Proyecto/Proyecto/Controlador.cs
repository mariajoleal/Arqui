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
        public int numeroHilos;
        public int reloj;

        public Controlador(int q) {
            quantum = q;
            sync = new Barrier(4); // Barrier con 4 participantes(los 3 núcleos y el hilo controlador)
            procesador0 = new Procesador(0, q, sync);
            procesador1 = new Procesador(1, q, sync);
            numeroHilos = 4;
            reloj = 0;
          /*procesador0 = new Procesador(0,sync);
            procesador1 = new Procesador(1,sync); */ // Aquí 
        }

        public int[] cargarTxt(int numProc)
        {
            string path0 = "C:\\Users\\Mariajo Leal\\Downloads\\4  HILILLOS SIN LW NI SW-v2\\hilillos0";
            string path1 = "C:\\Users\\Mariajo Leal\\Downloads\\4  HILILLOS SIN LW NI SW-v2\\hilillos1";

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

                /*Console.WriteLine("Instrucciones p0");
                for (int i = 0; i < 300; ++i)
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

                /*Console.WriteLine("Instrucciones p1");
                for (int i = 0; i < 150; ++i)
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

        public void iniciarPrograma()
        {
            int[] temp1 = cargarTxt(0);
            int[] temp2 = cargarTxt(1);
            procesador0.crearColaContextos(temp1);
            procesador1.crearColaContextos(temp2);
            crearHilos();
           
        }

        public void crearHilos()
        {
            Thread nucleo0 = new Thread(delegate ()
            {
                procesador0.ejecutarInstruccion(procesador0.cacheInstN0,0);
            });

            Thread nucleo1 = new Thread(delegate ()
            {
                procesador0.ejecutarInstruccion(procesador0.cacheInstN1,1);
            });

            Thread nucleo2 = new Thread(delegate ()
            {
                procesador1.ejecutarInstruccion(procesador1.cacheInstN0,2);
            });

            nucleo0.Start();
            nucleo1.Start();
            nucleo2.Start();
            

            controlarReloj(nucleo0, nucleo1, nucleo2);

        }

        // Incrementa el reloj del sistema mientras los hilos (procesadores) esten corriendo
        public void controlarReloj(Thread n0, Thread n1, Thread n2)
        {
            bool n0Vivo = true;
            bool n1Vivo = true;
            bool n2Vivo = true;

            Console.WriteLine("\nAvance del reloj: " + reloj);

            while (sync.ParticipantCount > 1)
            {
                if (!n0.IsAlive && n0Vivo)
                {
                    n0Vivo = false;
                    sync.RemoveParticipant();
                }
                if (!n1.IsAlive && n1Vivo)
                {
                    n1Vivo = false;
                    sync.RemoveParticipant();
                }
                if (!n2.IsAlive && n2Vivo)
                {
                    n2Vivo = false;
                    sync.RemoveParticipant();
                }

                // Incrementa el reloj y lo asigna a los 3 procesadores
                ++reloj;
                ++this.procesador0.reloj;
                ++this.procesador1.reloj;
                sync.SignalAndWait();   // Envia una señal y espera a que los 3 procesadores ejecuten envien señal

                //Console.WriteLine(reloj);
            }
        }



    }//clase controlador
}//namespace proyecto
