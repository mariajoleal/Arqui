using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Proyecto
{
    class Controlador
    {
        Procesador procesador0;
        Procesador procesador1;
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

        public void cargarTxt()
        {

            Procesador p = new Procesador();

            int indiceMem = 0;  // Para movernos por el array de la memoria principal
            int indiceMem1 = 0;
            int indiceMem2 = 0;

            string path = "C:\\Users\\jpvar\\Desktop\\Arqui\\Proyecto\\hilillos";
            string[] nombreArchivos = System.IO.Directory.GetFiles(path, "*.txt");
            // Carga los TXT a la memoria de cada procesador
            for (int i = 1; i <= numeroHilos; ++i)
            {
                if (i % 2 == 0)
                {
                    p = procesador0;
                    indiceMem = indiceMem1;

                }
                else if (i % 2 == 1)
                {
                    p = procesador1;
                    indiceMem = indiceMem2;
                }
                

                if (indiceMem != 0)
                {
                    indiceMem += 4;
                }
                p.setDireccionHilillo(indiceMem);

                string[] instrucciones = System.IO.File.ReadAllLines(nombreArchivos[i - 1]);
                // Cada string de instrucciones se descompone y pasa a int
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
                            p.memInst[indiceMem] = Int32.Parse(numero);
                            numero = "";
                            ++indiceMem;
                        }
                    }
                    p.memInst[indiceMem] = Int32.Parse(numero);
                    numero = "";
                }

                if (i % 2 == 0)
                {
                    indiceMem1 = indiceMem;
                }
                else if (i % 2 == 1)
                {
                    indiceMem2 = indiceMem;
                }
                
            }
            Console.WriteLine("Instrucciones p0");
            for (int i = 0; i < 50; ++i)
            {
                Console.Write(procesador0.memInst[i] + "  ");
            }
            Console.Write("\n");
            Console.WriteLine("Instrucciones p1");
            for (int i = 0; i < 50; ++i)
            {
                Console.Write(procesador1.memInst[i] + "  ");
            }
            Console.WriteLine("\n inicioHilillos p 0");
            for (int i = 0; i < 3; ++i)
            {
                Console.Write(procesador0.inicioHilillo[i] + "  ");
            }

            Console.WriteLine("\n inicioHilillos p 1");
            for (int i = 0; i < 3; ++i)
            {
                Console.Write(procesador1.inicioHilillo[i] + "  ");
            }
        } 
   }
}
