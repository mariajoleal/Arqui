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

        public Nucleo()
        {

        }

    }

    
}
