using System;
using System.Collections.Generic;
using System.Collections;
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
        public Queue<int[]> colaContexto;
        public int[,] directorio;
        public int hilillosTerminados;
        //public int[] inicioHilillo; //inicioHilillo[i] indica donde empieza el hilillo que esta en la posicion i 
        //public int indiceInicioHilillo;//indice que se mueve sobre el array inicioHilillo
        //public int[] contexto;

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
               // contexto = new int[33];
                colaContexto = new Queue<int[]>();
                //inicioHilillo = new int[4];
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
                colaContexto = new Queue<int[]>();
                //inicioHilillo = new int[2];
            }
            //inicioHilillo = new int[4];
            //indiceInicioHilillo = 0;
        }

        /*public void setDireccionHilillo(int direccion)
        {
            inicioHilillo[indiceInicioHilillo] = direccion;
            ++indiceInicioHilillo;
        }*/

        public void crearColaContextos(int[] inicioHilillo)
        {
            for(int i = 0; i < inicioHilillo.Length; ++i)
            {
                int[] contexto = new int[33];
                contexto[32] = inicioHilillo[i];
                colaContexto.Enqueue(contexto);
            }
            int count = colaContexto.Count;
           /* Console.WriteLine("colaContexto.Count = " + colaContexto.Count);
            for (int i = 0; i < count; ++i)
            {
                int[] temp = colaContexto.Dequeue();
                Console.WriteLine(temp[32]);
            }*/
        } 

        public void calcularBloque(int nucleo)//el parametro nucleo es para saber a cual cache se tiene que subir el bloque
        {
            if (Monitor.TryEnter(this.colaContexto))
            {
                try
                {
                    int[] contexto = new int[33];//array donde se guarda el contexto que se saca de la cola de contextos
                    contexto = colaContexto.Dequeue();
                    int pcContexto = contexto[32];//direccion de memoria de la instruccion
                    int numBloque = pcContexto / 16;//calcula el numero de bloque donde esta la instruccion
                    int[] bloque = new int[17];//array donde se va a guardar el bloque de instrucciones que se va a guardar en cache
                    bloque[16] = numBloque;//pone en la ultima posicion del array el numero de bloque
                    for (int i = 0; i < 16; ++i)
                    {
                        bloque[i] = memInst[pcContexto];
                        ++pcContexto;
                    }
                    if (nucleo == 0)
                    {
                        this.n0.subirInstruccionCache(bloque);//manda a subir el bloque a cache
                    }
                    else
                    {
                        this.n1.subirInstruccionCache(bloque);//manda a subir el bloque a cache
                    }
                }
                finally
                {
                    Monitor.Exit(this.colaContexto);
                }
            }
                
            
        }
    }//clase procesador
    
}//namespace Proyecto
