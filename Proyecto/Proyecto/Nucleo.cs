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
        }

    }

    
}
