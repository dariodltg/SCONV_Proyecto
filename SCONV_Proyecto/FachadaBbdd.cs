using Microsoft.EntityFrameworkCore;
using SCONV_Proyecto.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SCONV_Proyecto
{
    public class BbddConnection: DbContext
    {
        internal DbSet<Establecimiento> Establecimientos { get; set; }
        internal DbSet<Plato> Platos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=establecimientos.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Establecimiento>();
            modelBuilder.Entity<Plato>();
            base.OnModelCreating(modelBuilder);
        }
    }

    public class FachadaBbdd
    {
        private static FachadaBbdd singleton;
        private readonly BbddConnection bbddConnection;

        private FachadaBbdd()
        {
            bbddConnection = new BbddConnection();
            if (bbddConnection.Database.EnsureCreated())
            {
                InicializarDatosBbdd();
            }
        }

        public void Dispose()
        {
            if (bbddConnection != null)
                bbddConnection.Dispose();
        }

        public static FachadaBbdd GetSingleton()
        {
            if (singleton == null)
            {
                singleton = new FachadaBbdd();
            }
            return singleton;
        }

        public List<Establecimiento> GetEstablecimientos()
        {
            return bbddConnection.Establecimientos.ToList();
        }

        public List<Plato> GetPlatos()
        {
            return bbddConnection.Platos.ToList();
        }

        /// <summary>
        /// Devuelve un establecimiento a partir de su nombre
        /// </summary>
        /// <param name="nombre">Nombre del establecimiento</param>
        /// <returns>El establecimiento</returns>
        public Establecimiento GetEstablecimientoByNombre(string nombre)
        {
            return bbddConnection.Establecimientos.First(x => x.Nombre == nombre);
        }

        /// <summary>
        /// Deuelve la lista de platos que pertenecen a un establecimiento dado
        /// </summary>
        /// <param name="establecimiento">El establecimiento</param>
        /// <returns>La lista de platos</returns>
        internal List<Plato> GetPlatosDeEstablecimiento(Establecimiento establecimiento)
        {
            return bbddConnection.Platos.Where(x => x.Establecimiento == establecimiento).ToList();
        }

        /// <summary>
        /// Método para insertar datos de prueba de establecimientos y platos en la base de datos
        /// </summary>
        private void InicializarDatosBbdd()
        {
            Establecimiento telepizza = new Establecimiento() { Nombre = "Telepizza" };
            bbddConnection.Establecimientos.Add(telepizza);

            Establecimiento dominospizza = new Establecimiento() { Nombre = "Domino's Pizza" };
            bbddConnection.Establecimientos.Add(dominospizza);

            Establecimiento burgerking = new Establecimiento() { Nombre = "Burger King" };
            bbddConnection.Establecimientos.Add(burgerking);

            Establecimiento kfc = new Establecimiento() { Nombre="KFC"};
            bbddConnection.Establecimientos.Add(kfc);

            Plato barbacoaTelepizza = new Plato() { Nombre="Barbacoa", Establecimiento=telepizza, Precio=9.75 };
            bbddConnection.Platos.Add(barbacoaTelepizza);
            Plato baconcrispyTelepizza = new Plato() { Nombre = "Bacon Crispy", Establecimiento = telepizza, Precio = 9.50 };
            bbddConnection.Platos.Add(baconcrispyTelepizza);
            Plato carbonaraTelepizza = new Plato() { Nombre = "Carbonara", Establecimiento = telepizza, Precio = 9.25 };
            bbddConnection.Platos.Add(carbonaraTelepizza);

            Plato pulledporkDominos = new Plato() { Nombre = "Pulled Pork", Establecimiento = dominospizza, Precio = 10.75 };
            bbddConnection.Platos.Add(pulledporkDominos);
            Plato buffaloChicken = new Plato() { Nombre = "Buffalo Chicken", Establecimiento = dominospizza, Precio = 10.50 };
            bbddConnection.Platos.Add(buffaloChicken);

            Plato whopperBK = new Plato() { Nombre = "Whooper", Establecimiento = burgerking, Precio = 4.50 };
            bbddConnection.Platos.Add(whopperBK);
            Plato kingchickenBK = new Plato() { Nombre = "King Chicken", Establecimiento = burgerking, Precio = 6.50 };
            bbddConnection.Platos.Add(kingchickenBK);

            Plato bucket12TirasKFC = new Plato() { Nombre = "Bucket 12 Tiras de pollo", Establecimiento = kfc, Precio = 8.75 };
            bbddConnection.Platos.Add(bucket12TirasKFC);
            Plato chickshare6piezasKFC = new Plato() { Nombre = "Chick&Share 6 piezas", Establecimiento = kfc, Precio = 7.50 };
            bbddConnection.Platos.Add(chickshare6piezasKFC);

            bbddConnection.SaveChanges();
        }

        
    }
}
