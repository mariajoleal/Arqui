using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Proyecto
{
    class Procesador
    {
        public int numProc;
        Nucleo n0;
        Nucleo n1;
        public int[] memPric;
        public int[] memInst;
        public int[,] colaContext;
        public int[,] directorio;
        public int hilillosTerminados;

        public Procesador()
        {

        }
    }
}
