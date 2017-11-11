using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Proyecto
{
    class Controlador
    {
        Procesador procesador0;
        Procesador procesador1;
        public Barrier sync;
        public int quantum;

        public Controlador(int q) {
            quantum = q;
            sync = new Barrier(4); // Barrier con 4 participantes(los 3 núcleos y el hilo controlador)
            procesador0 = new Procesador(0);
            procesador1 = new Procesador(1);
            // sync = new Barrier(); 
          /*procesador0 = new Procesador(0,sync);
            procesador1 = new Procesador(1,sync); */ // Aquí 
        }
    }
}
