using System.ComponentModel.DataAnnotations;

namespace ZavrsniTestDjordjeVojcanin.Models
{
    public class Proizvodjac
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120, ErrorMessage = "Maksimalan broj karaktera za naziv je 120")]
        public string Naziv { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Broj karaktera za drzavu porekla je izmedju 2 i 60")]
        public string DrzavaPorekla { get; set; }
    }
}
