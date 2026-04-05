namespace Appetit.Application.DTOs.City
{
    public static class CityExtension
    {
        public static CityViewDTO ToCityViewDTO(this Domain.Models.City city) => new()
        {
            Id = city.Id,
            Ibge = city.Ibge,
            Name = city.Name,
            Uf = city.Uf
        };
    }
}
