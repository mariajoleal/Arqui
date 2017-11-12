using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Proyecto
{
    class Nucleo
    {
        public int[] registros;
        public Barrier sync;
        public int[,] cacheDatos;
        public int[,] cacheInst;
        int pc;

        public Nucleo()
        {
            registros = new int[32];
            cacheDatos = new int[4, 6];
            cacheInst = new int[4, 17];

            //pone en -1 el todos los bloques de la caché
            for (int i = 0; i < 4; ++i)
            {
                cacheDatos[i, 4] = -1;
                cacheDatos[i, 5] = -1;
            }
            Console.WriteLine("cache de datos procesador\n");
            for(int i = 0; i < 4; ++i)
            {
                for(int j = 0; j < 6; ++j)
                {
                    Console.Write(cacheDatos[i, j] + " ");
                }
                Console.Write("\n");
            }
            //pone en -1 los bloques de caché de instrucciones
            for (int i = 0; i < 4; ++i)
            {
                cacheInst[i, 16] = -1;
            }
        }

    }

    
}
