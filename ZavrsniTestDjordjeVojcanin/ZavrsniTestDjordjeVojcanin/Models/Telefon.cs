using System.ComponentModel.DataAnnotations;

namespace ZavrsniTestDjordjeVojcanin.Models
{
    public class Telefon
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 3, ErrorMessage = "Broj karaktera za model izmedju 3 i 120")]
        public string Model { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Broj karaktera za OS je izmedju 2 i 30")]
        public string OperativniSistem { get; set; }
        [Required]
        [Range(0, 1000, ErrorMessage = "Dostupna kolicina mora biti u rasponu od 0 do 1000")]
        public int DostupnaKolicina { get; set; }
        [Required]
        [Range(1.0, 250000.0, ErrorMessage = "Cena mora biti u rasponu od 1.0 do 250000.0")]
        public decimal Cena { get; set; }

        public int ProizvodjacId { get; set; }
        public Proizvodjac Proizvodjac { get; set; }

    }
}
