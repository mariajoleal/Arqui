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
        public int[] inicioHilillo; //inicioHilillo[i] indica donde empieza el hilillo que esta en la posicion i 
        public int indiceInicioHilillo;//indice que se mueve sobre el array inicioHilillo

        public Procesador(int np = 0)
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
               
                //pone en el directorio todos los bloques u. U = 0, M = 1, C =2
              
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
            inicioHilillo = new int[3];
            indiceInicioHilillo = 0;
        }

        public void setDireccionHilillo(int direccion)
        {
            inicioHilillo[indiceInicioHilillo] = direccion;
            ++indiceInicioHilillo;
        }
    }
    
}
