using System.Linq;
using System;
using ZavrsniTestDjordjeVojcanin.Interface;
using ZavrsniTestDjordjeVojcanin.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ZavrsniTestDjordjeVojcanin.Repository
{
    public class TelefonRepository : ITelefonRepository
    {
        private readonly AppDbContext _context;

        public TelefonRepository(AppDbContext context)
        {
            this._context = context;
        }

        public IQueryable<Telefon> GetAll()
        {
            return _context.Telefoni.OrderBy(m => m.Model).Include(z => z.Proizvodjac);

        }

        public Telefon GetById(int id)
        {
            return _context.Telefoni.Include(z => z.Proizvodjac).FirstOrDefault(p => p.Id == id);
        }

        public List<Telefon> PretraziTelefonePoUpitu(string upit)
        {

            return _context.Telefoni.Include(t => t.Proizvodjac)
                .Where(t => t.Model.Contains(upit) || t.Proizvodjac.Naziv.Contains(upit))
                .OrderByDescending(t => t.Cena)
                .ToList();

        }

        public void Add(Telefon telefon)
        {
            _context.Telefoni.Add(telefon);
            _context.SaveChanges();
        }
        public void Update(Telefon telefon)
        {
            _context.Entry(telefon).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public void Delete(Telefon telefon)
        {
            _context.Telefoni.Remove(telefon);
            _context.SaveChanges();
        }

        public IQueryable<Telefon> GetAllByParameters(decimal najmanje, decimal najvise)
        {
            return _context.Telefoni.Where(c => c.Cena >= najmanje && c.Cena <= najvise).OrderByDescending(s => s.Cena);
        }
    }
}
