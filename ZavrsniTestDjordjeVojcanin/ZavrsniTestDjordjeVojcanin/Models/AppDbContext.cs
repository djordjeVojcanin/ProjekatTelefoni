using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ZavrsniTestDjordjeVojcanin.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {


        public DbSet<Proizvodjac> Proizvodjaci { get; set; }
        public DbSet<Telefon> Telefoni { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Proizvodjac>().HasData(
                new Proizvodjac() { Id = 1, Naziv = "Xiaomi", DrzavaPorekla = "Kina" },
                new Proizvodjac() { Id = 2, Naziv = "Apple", DrzavaPorekla = "SAD" },
                new Proizvodjac() { Id = 3, Naziv = "Huawei", DrzavaPorekla = "Kina" }
            );

            modelBuilder.Entity<Telefon>().HasData(
                new Telefon()
                {
                    Id = 1,
                    Model = "A94",
                    OperativniSistem = "Android",
                    DostupnaKolicina = 12,
                    Cena = 31125.42m,
                    ProizvodjacId = 3,

                },
               new Telefon()
               {
                   Id = 2,
                   Model = "13T Pro",
                   OperativniSistem = "Android",
                   DostupnaKolicina = 7,
                   Cena = 104999.99m,
                   ProizvodjacId = 1,

               },
                new Telefon()
                {
                    Id = 3,
                    Model = "11",
                    OperativniSistem = "iOS",
                    DostupnaKolicina = 17,
                    Cena = 71290.35m,
                    ProizvodjacId = 2,

                },
                new Telefon()
                {
                    Id = 4,
                    Model = "Reno10 Pro",
                    OperativniSistem = "Android",
                    DostupnaKolicina = 4,
                    Cena = 68264.74m,
                    ProizvodjacId = 3,

                },
                new Telefon()
                {
                    Id = 5,
                    Model = "12 Lite",
                    OperativniSistem = "Android",
                    DostupnaKolicina = 5,
                    Cena = 44999.56m,
                    ProizvodjacId = 1,

                }

            ) ;

            base.OnModelCreating(modelBuilder);
        }

    }
}
