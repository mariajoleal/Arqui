﻿using System;
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
        public int pc;

        public Nucleo()
        {
            registros = new int[32];
            cacheDatos = new int[4, 6];
            cacheInst = new int[4, 17];

            //pone en -1 el numero de bloque de todos los bloques de la caché
            //se pone en -1 el estado del bloque
            for (int i = 0; i < 4; ++i)
            {
                cacheDatos[i, 4] = -1;
                cacheDatos[i, 5] = -1;
            }
            
            //pone en -1 los bloques de caché de instrucciones
            for (int i = 0; i < 4; ++i)
            {
                cacheInst[i, 16] = -1;
            }
        }

    }

    
}