using System.Collections.Generic;
using System.Linq;
using ZavrsniTestDjordjeVojcanin.Models;
using ZavrsniTestDjordjeVojcanin.Models.DTO;

namespace ZavrsniTestDjordjeVojcanin.Interface
{
    public interface IProizvodjacRepository
    {

        public IQueryable<Proizvodjac> GetAll();
        public Proizvodjac GetById(int id);
        public List<ProizvodjaciBrModelaProsecnaCenaDTO> ProizvodjaciProsek(int granica);
        public List<BrojTelefonaPoProizDTO> GetBrojModela();
        public List<Proizvodjac> PretraziProizvodjacaPoImenu(string ime);
        public void Delete(Proizvodjac proizvodjac);
    }
}
