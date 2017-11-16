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
        public int[] memPrinc;
        public int[] memInst;
        public Queue<int[]> colaContexto;
        public int[,] directorio;
        public int hilillosTerminados;

        public int[] registrosN0;
        public int[] registrosN1;
        public int[,] cacheDatosN0;
        public int[,] cacheDatosN1;
        public int[,] cacheInstN0;
        public int[,] cacheInstN1;
        public int pc;
        int quantum;
        //public int[] inicioHilillo; //inicioHilillo[i] indica donde empieza el hilillo que esta en la posicion i 
        //public int indiceInicioHilillo;//indice que se mueve sobre el array inicioHilillo
        //public int[] contexto;

        public Procesador(int np, int q)
        {
            quantum = q;
            if (np == 0)
            {
                numProc = 0;
                
                //inicializacion de la memoria
                memPrinc = new int[64];
                memInst = new int[384];
                directorio = new int[16, 5];
               // contexto = new int[33];
                colaContexto = new Queue<int[]>();
                registrosN0 = new int[32];
                registrosN1 = new int[32];
                cacheDatosN0 = new int[4, 6];
                cacheDatosN1 = new int[4, 6];
                cacheInstN0 = new int[4, 17];
                cacheInstN1 = new int[4, 17];
                pc = 0;


                for (int i = 0; i < 4; ++i)
                {
                    cacheDatosN0[i, 4] = -1;
                    cacheDatosN0[i, 5] = -1;

                    cacheDatosN1[i, 4] = -1;
                    cacheDatosN1[i, 5] = -1;
                }

                //pone en -1 los bloques de caché de instrucciones
                for (int i = 0; i < 4; ++i)
                {
                    cacheInstN0[i, 16] = -1;
                    cacheInstN1[i, 16] = -1;
                }

            }
            else
            {
                numProc = 1;
            
                //inicializacion de la memoria
                memPrinc = new int[32];
                memInst = new int[256];
                directorio = new int[8, 5];
                colaContexto = new Queue<int[]>();

                registrosN0 = new int[32];
               
                cacheDatosN0 = new int[4, 6];
                
                cacheInstN0 = new int[4, 17];
                
                pc = 0;

                for (int i = 0; i < 4; ++i)
                {
                    cacheDatosN0[i, 4] = -1;
                    cacheDatosN0[i, 5] = -1;
                }

                //pone en -1 los bloques de caché de instrucciones
                for (int i = 0; i < 4; ++i)
                {
                    cacheInstN0[i, 16] = -1;
                }

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

        public void traerInstruccion(int pc, int[,] cache)//sube de memoria el bloque donde esta la instruccion
        {
                   
            int numBloque = pc / 16;//calcula el numero de bloque donde esta la instruccion
            int[] bloque = new int[17];//array donde se va a guardar el bloque de instrucciones que se va a guardar en cache
            bloque[16] = numBloque;//pone en la ultima posicion del array el numero de bloque
            for (int i = 0; i < 16; ++i)
            {
                bloque[i] = memInst[pc];
                ++pc;
            }

            int posCache = numBloque % 4;
            for(int i = 0; i < 16; ++i)
            {
                cache[posCache, i] = bloque[i];
            }
            cache[posCache, 16] = numBloque;              
        }

        public int[] sacarContexto()
        {
            Monitor.Enter(this.colaContexto);

            int[] contexto = new int[33];//array donde se guarda el contexto que se saca de la cola de contextos
            contexto = colaContexto.Dequeue();
            Monitor.Exit(this.colaContexto);
            return contexto;
        }

        public int[] recuperarInstruccion(int pc, int[,] cache)//el parametro nucleo es para saber a cual cache se tiene que subir el bloque
        {

            int[] IR = new int[4];
            
            int numBloque = pc / 16;//calcula el numero de bloque donde esta la instruccion
            int posCache = numBloque % 4;
                    
            if(cache[posCache, 16] == numBloque)//pregunta si el bloque esta en cache
            {
                int pcLocal = pc % 16;
                for(int i = 0; i < 4; ++i)
                {
                    IR[i] = cache[posCache, pcLocal];
                    ++pcLocal;
                }
            }
            else
            {
                traerInstruccion(pc, cache);//trae la instruccion de memoria
                int pcLocal = pc % 16;
                for (int i = 0; i < 4; ++i)
                {
                    IR[i] = cache[posCache, pcLocal];
                    ++pcLocal;
                }
            }
                     
            return IR;
        }

        public void imprimirInstrucciones()
        {
            Console.WriteLine("memoria instrucciones procesador:");
            for (int i = 0; i < 50; ++i)
            {
                Console.Write(this.memInst[i] + "  ");
            }
        }

        public void imprimirCacheInst(int nucleo)
        {
            if(nucleo == 0)
            {
                Console.WriteLine("Cache instrucciones nucleo: " + nucleo);
                for (int i = 0; i < 4; ++i)
                {
                    for (int j = 0; j < 17; ++j)
                    {
                        Console.Write(this.cacheInstN0[i, j] + "  ");
                    }
                    Console.Write("\n");
                }
            }
            else
            {
                Console.WriteLine("Cache instrucciones nucleo: " + nucleo);
                for (int i = 0; i < 4; ++i)
                {
                    for (int j = 0; j < 17; ++j)
                    {
                        Console.Write(this.cacheInstN1[i, j] + "  ");
                    }
                    Console.Write("\n");
                }
            }
            
        }

        public void imprimirRegistros(int[] registros)
        {
            Console.WriteLine("Registros hilillo");
            for(int i = 0; i < 32; ++i)
            {
                Console.Write(registros[i] + "  ");
            }
            Console.WriteLine(" ");
        }

        public void ejecutarInstruccion(int[,] cache)
        {
            while(colaContexto.Count != 0)
            {
                int[] registros = sacarContexto();
                int pc = registros[32];
                int quantumLocal = quantum;
                while (quantumLocal != 0)
                {
                    int[] instruccion = recuperarInstruccion(pc, cache);
                    pc += 4;
                    int codOp = instruccion[0];
                    

                    switch (codOp)
                    {
                        case 2:     // JR
                            pc = registros[instruccion[1]];
                            quantumLocal--;
                            break;
                        case 3:     // JAL
                            registros[31] = pc;
                            pc += instruccion[3];
                            quantumLocal--;
                            break;
                        case 4:     // BEQZ
                            if (registros[instruccion[1]] == 0)
                            {
                                pc += instruccion[3] * 4;
                            }
                            quantumLocal--;
                            break;
                        case 5:     // BENZ
                            if (registros[instruccion[1]] != 0)
                            {
                                pc += instruccion[3] * 4;
                            }
                            quantumLocal--;
                            break;
                        case 8:     // DADDI
                            registros[instruccion[2]] = registros[instruccion[1]] + instruccion[3];
                            quantumLocal--;
                            break;
                        case 12:    // DMUL
                            registros[instruccion[3]] = registros[instruccion[1]] * registros[instruccion[2]];
                            quantumLocal--;
                            break;
                        case 14:    // DDIV
                            registros[instruccion[3]] = registros[instruccion[1]] / registros[instruccion[2]];
                            quantumLocal--;
                            break;
                        case 32:    // DADD
                            registros[instruccion[3]] = registros[instruccion[1]] + registros[instruccion[2]];
                            quantumLocal--;
                            break;
                        case 34:    // DSUB
                            registros[instruccion[3]] = registros[instruccion[1]] - registros[instruccion[2]];
                            quantumLocal--;
                            break;
                        case 35:    // LW
                                    // Ejecutar LOAD    
                            break;
                        case 43:    // SW
                                    // Ejecutar STORE
                            break;
                        case 63:    // Termino el hilillo
                            ++hilillosTerminados;
                            imprimirRegistros(registros);
                            //Console.WriteLine("termine" + " " + numeroProcesador);
                            //Console.ReadKey();
                            pc = -1;    // "Stamp" para indicar que el hilillo ya se ejecutó en su totalidad
                                        //guardarContexto();
                                        //cargarContexto();
                            break;
                    }
                }
                imprimirRegistros(registros);
            }
            

            
            
        }
    }//clase procesador
    
}//namespace Proyecto
