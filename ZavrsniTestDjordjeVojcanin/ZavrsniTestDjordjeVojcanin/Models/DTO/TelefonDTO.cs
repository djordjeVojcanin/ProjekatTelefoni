namespace ZavrsniTestDjordjeVojcanin.Models.DTO
{
    public class TelefonDTO
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string OperativniSistem { get; set; }
        public int DostupnaKolicina { get; set; }
        public decimal Cena { get; set; }
        public int ProizvodjacId { get; set; }
        public string ProizvodjacNaziv { get; set; }
        public string ProizvodjacDrzavaPorekla { get; set; }
    }
}
