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

        public Procesador(int np)
        {
            //numProc = np;
            if (np == 0)
            {
                numProc = 0;
                n0 = new Nucleo();
                n1 = new Nucleo();
                //inicializacion de la memoria
                memPric = new int[64];
                memInst = new int[384];
                directorio = new int[16, 5];
            }
            else
            {
                numProc = 1;
                n0 = new Nucleo();
                //inicializacion de la memoria
                memPric = new int[32];
                memInst = new int[256];
                directorio = new int[8, 5];
            }
        }
     
            
    }
    
}
