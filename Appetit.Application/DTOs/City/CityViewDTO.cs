namespace Appetit.Application.DTOs.City
{
    public class CityViewDTO
    {
        public int Id { get; set; }
        public int Ibge { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
    }
}
