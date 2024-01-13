using System.Collections.Generic;
using System.Linq;
using ZavrsniTestDjordjeVojcanin.Interface;
using ZavrsniTestDjordjeVojcanin.Models;
using ZavrsniTestDjordjeVojcanin.Models.DTO;

namespace ZavrsniTestDjordjeVojcanin.Repository
{
    public class ProizvodjacRepository : IProizvodjacRepository
    {

        private readonly AppDbContext _context;

        public ProizvodjacRepository(AppDbContext context)
        {
            this._context = context;
        }

        public IQueryable<Proizvodjac> GetAll()
        {
            return _context.Proizvodjaci.OrderBy(z => z.Naziv);
        }

        public Proizvodjac GetById(int id)
        {
            return _context.Proizvodjaci.FirstOrDefault(p => p.Id == id);
        }



        //Preuzimanje svih proizvodjaca sa njihovim nazivom, cenom najjeftinijeg telefona tog proizvodjaca i prosecnom cenom svih telefona tog proizvodjaca.
        //Pri tome prosecna cena telefona za proizvodjaca je manja od unete vrednosti (granice) i sortirano je prema nazivu proizvodjaca, rastuce.

        public List<ProizvodjaciBrModelaProsecnaCenaDTO> ProizvodjaciProsek(int granica)
        {
            return _context.Telefoni.GroupBy(s => s.ProizvodjacId).Select(grupa =>
                new ProizvodjaciBrModelaProsecnaCenaDTO()
                {
                    Proizvodjac = _context.Proizvodjaci.Where(p => p.Id == grupa.Key).Select(s => s.Naziv).Single(),
                    ProsecnaCena = (double)grupa.Average(f => f.Cena),
                    NajjeftinijiTelefon = grupa.Min(t => t.Cena),

                }
                ).Where(dto => dto.ProsecnaCena < granica).OrderBy(dto => dto.Proizvodjac).ToList();
        }


       //Preuzimanje svih proizvodjaca, sa sledecim podacima:
       // naziv, tacan broj razlicitih modela i ukupna dostupna kolicina po modelu
       // sortirano po ukupnoj dostupnoj kolicini modela opadajuce

        public List<BrojTelefonaPoProizDTO> GetBrojModela()
        {
            return _context.Proizvodjaci
       .Select(proizvodjac => new BrojTelefonaPoProizDTO
            {
           Proizvodjac = proizvodjac.Naziv,
           BrModela = _context.Telefoni.Count(s => s.ProizvodjacId == proizvodjac.Id),
           DostupnaKolicina = _context.Telefoni.Where(t => t.ProizvodjacId == proizvodjac.Id)
           .Sum(t => t.DostupnaKolicina)

             })
             .OrderByDescending(z => z.DostupnaKolicina)
             .ToList();
        }

        // Pretrazi po imenu, ciji je naziv JEDNAK prosledjenoj vrednosti ime
        //sortirati po drzavi porekla rastuce, a ako su im iste drzave porekla, onda po nazivu opadajuce.

        public List<Proizvodjac> PretraziProizvodjacaPoImenu(string ime)
        {
            return _context.Proizvodjaci.Where(z => z.Naziv.Equals(ime)).OrderBy(z => z.DrzavaPorekla)
                .ThenByDescending(z => z.Naziv).ToList();
        }

        public void Delete(Proizvodjac proizvodjac)
        {
            _context.Proizvodjaci.Remove(proizvodjac);
            _context.SaveChanges();
        }

    }
}
