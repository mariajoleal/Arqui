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
<<<<<<< Updated upstream
        public int hilillosTerminados;
=======
        public List<int[]> hilillosTerminados;
      

        public int[] registrosN0;
        public int[] registrosN1;
        public struct cacheDatos
        {
            public int idCache;
            public int[,] cache;

            public cacheDatos(int idC)
            {
                idCache = idC;
                cache = new int[4, 6];
            }
        }
        public cacheDatos cacheDatosN0;
        public cacheDatos cacheDatosN1;
        public int[,] cacheInstN0;
        public int[,] cacheInstN1;
        public int pc;
        int quantum;
        public int reloj;

        public Barrier sync;
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
            Console.WriteLine("colaContexto.Count = " + colaContexto.Count);
            for (int i = 0; i < colaContexto.Count; ++i)
=======
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
            Console.WriteLine("Registros hilillo " + registros[33]);
            for(int i = 0; i < 32; ++i)
            {
                Console.Write(registros[i] + " ");
                //Console.Write("\n");
                //Console.Write("R"+ i +": " + registros[i] + "\n");
            }
            Console.WriteLine(" ");
        }

        public void ejecutarInstruccion(ref int[,] cache, ref cacheDatos cachePropia, int numNuc, ref cacheDatos cache1, ref cacheDatos cache2, ref int[,] directorioP0, ref int[,] directorioP1, ref int[] memDatosP0, ref int[] memDatosP1)
        {
            

            while(colaContexto.Count != 0)
            {
                int[] registros = sacarContexto();
                int pc = registros[32] ;
                int quantumLocal = quantum;
                while (quantumLocal != 0 && pc != -1)
                {
                    int[] instruccion = recuperarInstruccion(pc, ref cache, numNuc);
                    pc += 4;
                    int codOp = instruccion[0];

                    //Console.WriteLine("Corriendo hilillo " + registros[33] + " en el núcleo " + numNuc);
                    switch (codOp)
                    {
                        case 2:     // JR
                            pc = registros[instruccion[1]];
                            break;
                        case 3:     // JAL
                            registros[31] = pc;
                            pc += instruccion[3];
                            break;
                        case 4:     // BEQZ
                            if (registros[instruccion[1]] == 0)
                            {
                                pc += instruccion[3] * 4;
                            }
                            break;
                        case 5:     // BENZ
                            if (registros[instruccion[1]] != 0)
                            {
                                pc += instruccion[3] * 4;
                            }
                            break;
                        case 8:     // DADDI
                            registros[instruccion[2]] = registros[instruccion[1]] + instruccion[3];
                            break;
                        case 12:    // DMUL
                            registros[instruccion[3]] = registros[instruccion[1]] * registros[instruccion[2]];
                            break;
                        case 14:    // DDIV
                            registros[instruccion[3]] = registros[instruccion[1]] / registros[instruccion[2]];
                            break;
                        case 32:    // DADD
                            registros[instruccion[3]] = registros[instruccion[1]] + registros[instruccion[2]];
                            break;
                        case 34:    // DSUB
                            registros[instruccion[3]] = registros[instruccion[1]] - registros[instruccion[2]];
                            break;
                        case 35:    // LW
                            int direccion = instruccion[1] + instruccion[3];
                            int[] resultadoLW = ejecutarLW(ref cachePropia, ref cache1, ref cache2, ref directorioP0, ref directorioP1, direccion, ref memDatosP0, ref memDatosP1, numNuc);
                            if (resultadoLW[1] == 1)
                            {
                                registros[instruccion[2]] = resultadoLW[0];
                            }
                            else
                            {
                                pc -= 4;
                            }
                                    
                            break;
                        case 43:    // SW
                                    // Ejecutar STORE
                            break;
                        case 63:    // Termino el hilillo
                            
                            imprimirRegistros(registros);
                            //Console.WriteLine("termine" + " " + numeroProcesador);
                            //Console.ReadKey();
                            pc = -1;    // "Stamp" para indicar que el hilillo ya se ejecutó en su totalidad
                            break;
                    }
                    quantumLocal--;
                   sync.SignalAndWait();
                    
                }
                if (pc == -1)
                {
                    hilillosTerminados.Add(registros);
                }
                else
                {
                    registros[32] = pc;
                    while(!(Monitor.TryEnter(colaContexto)))
                    {

                    }
                    colaContexto.Enqueue(registros);
                    Monitor.Exit(colaContexto);
                }
                 
                
            }

            /*for (int i = 0; i < hilillosTerminados.Count; ++i)
            {
                imprimirRegistros(hilillosTerminados[i]);
            }*/
          
        }

        public int[] ejecutarLW(ref cacheDatos cachePropia, ref cacheDatos cache1, ref cacheDatos cache2, ref int[,] directorioP0, ref int[,] directorioP1, int direccion, ref int[] memDatosP0, ref int[] memDatosP1, int numNuc)
        {
            int numBloque = direccion / 16;
            int numPalabra = (direccion % 16) / 4;
            int posCache = numBloque % 4;
            int[] registro = new int[2];
            registro[1] = 0;

           
            if (Monitor.TryEnter(cachePropia.cache))
            {
                try
                {
                    if(cachePropia.cache[posCache, 4] == numBloque && cachePropia.cache[posCache, 5] != 0)//0 = invalido, 1 = compartido, 2 = modificado
                    {
                        registro[0] = cachePropia.cache[posCache, numPalabra];
                        registro[1] = 1;//el valor es valido
                           
                    }
                    else
                    {
                        if (cachePropia.cache[posCache, 5] == 2)//bloque victima modificado
                        {
                            int bloqueVictima = cachePropia.cache[posCache, 4];//numero de bloque victima
                            if(bloqueVictima < 16)
                            {
                                if (Monitor.TryEnter(memDatosP0))
                                {
                                    try
                                    {
                                        int indiceMem = bloqueVictima * 16;                                        
                                        for (int i = 0; i < 4; ++i)//baja bloque victima a memoria
                                        {
                                            memDatosP0[indiceMem] = cache1.cache[posCache, i];
                                            indiceMem++;
                                        }

                                        if (numNuc == 0 || numNuc == 1)
                                        {
                                            for (int i = 0; i < 16; ++i)
                                            {
                                                sync.SignalAndWait();
                                            }
                                        }
                                        else
                                        {
                                            for (int i = 0; i < 40; ++i)
                                            {
                                                sync.SignalAndWait();
                                            }
                                        }
                                    }
                                    finally
                                    {
                                        Monitor.Exit(memDatosP0);
                                    }
                                }
                                else
                                {
                                    //Monitor.Exit(cachePropia.cache);
                                    registro[1] = 0;//valor invalido
                                    //return registro;

                                }
                            }
                            else
                            {
                                if (Monitor.TryEnter(memDatosP1))
                                {
                                    try
                                    {
                                        int indiceMem = bloqueVictima * 16 - 256;
                                        for (int i = 0; i < 4; ++i)//sube el bloque a la cache propia
                                        {
                                            memDatosP1[indiceMem] = cache1.cache[posCache, i];
                                            indiceMem++;
                                        }

                                        if (numNuc == 0 || numNuc == 1)
                                        {
                                            for (int i = 0; i < 40; ++i)
                                            {
                                                sync.SignalAndWait();
                                            }
                                        }
                                        else
                                        {
                                            for (int i = 0; i < 16; ++i)
                                            {
                                                sync.SignalAndWait();
                                            }
                                        }
                                    }
                                    finally
                                    {
                                        Monitor.Exit(memDatosP1);
                                    }
                                }
                                else
                                {
                                    //Monitor.Exit(cachePropia.cache);
                                    registro[1] = 0;//valor invalido 
                                   // return registro;
                                }
                            }
                            
                        }

                        if (numBloque < 16)// el bloque esta en el procesador 0
                        {
                            if (Monitor.TryEnter(directorioP0))
                            {

                                try
                                {
                                    if(numNuc == 0 || numNuc == 1)
                                    {
                                        sync.SignalAndWait();
                                    }
                                    else
                                    {
                                        for(int i = 0; i < 5; ++i)
                                        {
                                            sync.SignalAndWait();
                                        }
                                    }
                                    
                                    if (directorioP0[numBloque, 1] == 2)//esta modificado en otra cache
                                    {
                                        int cacheModificado = -1;
                                        for (int i = 2; i < 5; ++i)//busca en que cache esta modificado
                                        {
                                            if (directorioP0[numBloque, i] == 1)
                                            {
                                                cacheModificado = i;
                                            }

                                        }
                                       
                                        if (cache1.idCache == cacheModificado)
                                        {
                                            if (Monitor.TryEnter(cache1.cache))
                                            {
                                                try
                                                {
                                                    // bajarAMemoria(cache1, numBloque);
                                                    if (Monitor.TryEnter(memDatosP0))
                                                    {
                                                        try
                                                        {
                                                            int indiceMem = direccion;
                                                            for (int i = 0; i < 4; ++i)//sube el bloque a la cache propia
                                                            {
                                                                memDatosP0[indiceMem] = cache1.cache[posCache, i];
                                                                cachePropia.cache[posCache, i] = cache1.cache[posCache, i];
                                                                indiceMem++;
                                                            }
                                                            if (numNuc == 0 || numNuc == 1)
                                                            {
                                                                for (int i = 0; i < 16; ++i)
                                                                {
                                                                    sync.SignalAndWait();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                for (int i = 0; i < 40; ++i)
                                                                {
                                                                    sync.SignalAndWait();
                                                                }
                                                            }
                                                        }
                                                        finally
                                                        {
                                                            Monitor.Exit(memDatosP0);
                                                        }
                                                        cachePropia.cache[posCache, 4] = cache1.cache[posCache, 4];
                                                        cachePropia.cache[posCache, 5] = cache1.cache[posCache, 5];
                                                        registro[0] = cachePropia.cache[posCache, numPalabra];
                                                        registro[1] = 1;
                                                        directorioP0[numBloque, cache1.idCache + 2] = 1;//pone el bloque compartido en la cache que lo tenia modificado
                                                        directorioP0[numBloque, cachePropia.idCache + 2] = 1;//pone el bloque compartido en la cache propia
                                                    }
                                                    else
                                                    {
                                                        //Monitor.Exit(cachePropia.cache);
                                                        Monitor.Exit(directorioP0);
                                                        Monitor.Exit(cache1.cache);
                                                        registro[1] = 0;//valor invalido
                                                        //return registro;

                                                    }      
                                                }
                                                finally
                                                {
                                                    Monitor.Exit(cache1.cache);
                                                }
                                            }
                                            else
                                            {
                                               // Monitor.Exit(cachePropia.cache);
                                                Monitor.Exit(directorioP0);
                                                registro[1] = 0;//valor invalido
                                                //return registro;

                                            }

                                        }
                                        else if (cache2.idCache == cacheModificado)
                                        {
                                            if(Monitor.TryEnter(cache2.cache))
                                            {
                                                try
                                                {
                                                    //bajarAMemoria(cache2, numBloque);

                                                    if(Monitor.TryEnter(memDatosP0))
                                                    {
                                                        try
                                                        {
                                                            int indiceMem = direccion * 16;
                                                            for (int i = 0; i < 4; ++i)//sube el bloque a la cache propia
                                                            {
                                                                memDatosP0[indiceMem] = cache1.cache[posCache, i];
                                                                cachePropia.cache[posCache, i] = cache1.cache[posCache, i];
                                                                indiceMem++;
                                                            }
                                                            if (numNuc == 0 || numNuc == 1)
                                                            {
                                                                for (int i = 0; i < 16; ++i)
                                                                {
                                                                    sync.SignalAndWait();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                for (int i = 0; i < 40; ++i)
                                                                {
                                                                    sync.SignalAndWait();
                                                                }
                                                            }
                                                        }
                                                        finally
                                                        {
                                                            Monitor.Exit(memDatosP0);
                                                        }
                                                        cachePropia.cache[posCache, 4] = cache2.cache[posCache, 4];
                                                        cachePropia.cache[posCache, 5] = cache2.cache[posCache, 5];
                                                        registro[0] = cachePropia.cache[posCache, numPalabra];
                                                        registro[1] = 1;
                                                        directorioP0[numBloque, cache2.idCache + 2] = 1;//pone el bloque compartido en la cache que lo tenia modificado
                                                        directorioP0[numBloque, cachePropia.idCache + 2] = 1;//pone el bloque compartido en la cache propia
                                                    }
                                                    else
                                                    {
                                                       // Monitor.Exit(cachePropia.cache);
                                                        Monitor.Exit(directorioP0);
                                                        Monitor.Exit(cache2.cache);
                                                        registro[1] = 0;//valor invalido
                                                      //  return registro;

                                                    }
                                                    
                                                }
                                                finally
                                                {
                                                    Monitor.Exit(cache2.cache);
                                                }
                                            }
                                            else
                                            {
                                                //Monitor.Exit(cachePropia.cache);
                                                Monitor.Exit(directorioP0);
                                                registro[1] = 0;//valor invalido  
                                                //return registro;
                                            }
                                        }

                                    }
                                    else//bloque compartido o uncached
                                    {
                                        int indiceMem = direccion;

                                        if(Monitor.TryEnter(memDatosP0))
                                        {
                                            try
                                            {
                                                if (numNuc == 0 || numNuc == 1)
                                                {
                                                    for (int i = 0; i < 16; ++i)
                                                    {
                                                        sync.SignalAndWait();
                                                    }
                                                }
                                                else
                                                {
                                                    for (int i = 0; i < 40; ++i)
                                                    {
                                                        sync.SignalAndWait();
                                                    }
                                                }
                                                for (int i = 0; i < 4; ++i)//sube el bloque a la cache propia
                                                {
                                                    cachePropia.cache[posCache, i] = memDatosP0[indiceMem];
                                                    indiceMem++;
                                                }
                                            }
                                            finally
                                            {
                                                Monitor.Exit(memDatosP0);
                                            }
                                        }
                                        
                                        cachePropia.cache[posCache, 4] = numBloque;
                                        cachePropia.cache[posCache, 5] = 1;//compartido
                                        registro[0] = cachePropia.cache[posCache, numPalabra];
                                        registro[1] = 1;
                                        directorioP0[numBloque, cachePropia.idCache + 2] = 1;//pone el bloque compartido en la cache propia
                                        
                                    }
                                }
                                finally
                                {
                                    Monitor.Exit(directorioP0);
                                }
                            }
                            else
                            {
                               // Monitor.Exit(cachePropia.cache);
                                registro[1] = 0;//valor invalido
                               // return registro;

                            }
                        }
                        else
                        {
                            if (Monitor.TryEnter(directorioP1))
                            {
                                try
                                {
                                    if (numNuc == 0 || numNuc == 1)
                                    {
                                        for (int i = 0; i < 5; ++i)
                                        {
                                            sync.SignalAndWait();
                                        }
                                        
                                    }
                                    else
                                    {
                                        sync.SignalAndWait();
                                    }
                                    if (directorioP1[numBloque, 1] == 2)//esta modificado en otra cache
                                    {
                                        int cacheModificado = -1;
                                        for (int i = 2; i < 5; ++i)//busca en que cache esta modificado
                                        {
                                            if (directorioP1[numBloque, i] == 1)
                                            {
                                                cacheModificado = i;
                                            }

                                        }

                                        if (cache1.idCache == cacheModificado)
                                        {
                                            //bajarAMemoria(cache1, numBloque);
                                            if (Monitor.TryEnter(memDatosP1))
                                            {
                                                try
                                                {
                                                    int indiceMem = direccion - 256;
                                                    for (int i = 0; i < 4; ++i)//sube el bloque a la cache propia
                                                    {
                                                        memDatosP1[indiceMem] = cache1.cache[posCache, i];
                                                        cachePropia.cache[posCache, i] = cache1.cache[posCache, i];
                                                        indiceMem++;
                                                    }

                                                    if (numNuc == 0 || numNuc == 1)
                                                    {
                                                        for (int i = 0; i < 40; ++i)
                                                        {
                                                            sync.SignalAndWait();
                                                        }

                                                    }
                                                    else
                                                    {
                                                        for (int i = 0; i < 16; ++i)
                                                        {
                                                            sync.SignalAndWait();
                                                        }
                                                    }
                                                }
                                                finally
                                                {
                                                    Monitor.Exit(memDatosP1);
                                                }
                                                cachePropia.cache[posCache, 4] = cache1.cache[posCache, 4];
                                                cachePropia.cache[posCache, 5] = cache1.cache[posCache, 5];
                                                registro[0] = cachePropia.cache[posCache, numPalabra];
                                                registro[1] = 1;
                                                directorioP1[numBloque, cache1.idCache + 2] = 1;//pone el bloque compartido en la cache que lo tenia modificado
                                                directorioP1[numBloque, cachePropia.idCache + 2] = 1;//pone el bloque compartido en la cache propia
                                            }
                                            else
                                            {
                                                //Monitor.Exit(cachePropia.cache);
                                                Monitor.Exit(directorioP1);
                                                registro[1] = 0;//valor invalido 
                                                //return registro;
                                            }
                                            
                                            
                                        }
                                        else if (cache2.idCache == cacheModificado)
                                        {
                                            //bajarAMemoria(cache2, numBloque);
                                            if (Monitor.TryEnter(memDatosP1))
                                            {
                                                try
                                                {
                                                    int indiceMem = direccion - 256;
                                                    for (int i = 0; i < 4; ++i)//sube el bloque a la cache propia
                                                    {
                                                        memDatosP1[indiceMem] = cache2.cache[posCache, i];
                                                        cachePropia.cache[posCache, i] = cache2.cache[posCache, i];
                                                        indiceMem++;
                                                    }
                                                    if (numNuc == 0 || numNuc == 1)
                                                    {
                                                        for (int i = 0; i < 40; ++i)
                                                        {
                                                            sync.SignalAndWait();
                                                        }

                                                    }
                                                    else
                                                    {
                                                        for (int i = 0; i < 16; ++i)
                                                        {
                                                            sync.SignalAndWait();
                                                        }
                                                    }
                                                }
                                                finally
                                                {
                                                    Monitor.Exit(memDatosP1);
                                                }
                                                cachePropia.cache[posCache, 4] = cache2.cache[posCache, 4];
                                                cachePropia.cache[posCache, 5] = cache2.cache[posCache, 5];
                                                registro[0] = cachePropia.cache[posCache, numPalabra];
                                                registro[1] = 1;
                                                directorioP1[numBloque, cache2.idCache + 2] = 1;//pone el bloque compartido en la cache que lo tenia modificado
                                                directorioP1[numBloque, cachePropia.idCache + 2] = 1;//pone el bloque compartido en la cache propia
                                            }
                                            else
                                            {
                                                //Monitor.Exit(cachePropia.cache);
                                                Monitor.Exit(cache2.cache);
                                                Monitor.Exit(directorioP1);
                                                registro[1] = 0;//valor invalido
                                                //return registro;   
                                            }
                                        }

                                    }
                                    else//bloque compartido o uncached
                                    {
                                        int indiceMem = direccion - 256;
                                        if(Monitor.TryEnter(memDatosP1))
                                        {
                                            try
                                            {
                                                for (int i = 0; i < 4; ++i)//sube el bloque a la cache propia
                                                {
                                                    cachePropia.cache[posCache, i] = memDatosP1[indiceMem];
                                                    ++indiceMem;
                                                }
                                                if (numNuc == 0 || numNuc == 1)
                                                {
                                                    for (int i = 0; i < 40; ++i)
                                                    {
                                                        sync.SignalAndWait();
                                                    }

                                                }
                                                else
                                                {
                                                    for (int i = 0; i < 16; ++i)
                                                    {
                                                        sync.SignalAndWait();
                                                    }
                                                }
                                            }
                                            finally
                                            {
                                                Monitor.Exit(memDatosP1);
                                            }
                                        }
                                        
                                        cachePropia.cache[posCache, 4] = numBloque;
                                        cachePropia.cache[posCache, 5] = 1;//compartido
                                        registro[0] = cachePropia.cache[posCache, numPalabra];
                                        registro[1] = 1;
                                        directorioP1[numBloque, cachePropia.idCache + 2] = 1;//pone el bloque compartido en la cache propia  
                                    }
                                }
                                finally
                                {
                                    Monitor.Exit(directorioP1);
                                }
                            }
                            else
                            {
                              //  Monitor.Exit(cachePropia.cache);
                                registro[1] = 0;//valor invalido
                                //return registro;
                            }
                        }                        
                    }
                } //try
                finally
                {
                    Monitor.Exit(cachePropia.cache);
                }
           }//tryEnter(cachePropia)
            else
>>>>>>> Stashed changes
            {
                int[] temp = colaContexto.Dequeue();
                Console.WriteLine(temp[32]);
            }
        } 
    }
    
}
