using Appetit.Application.DTOs.City;
using Appetit.Domain.Common.Responses;

namespace Appetit.Application.Interfaces
{
    public interface ICityService
    {
        Task<ResponseData<List<CityViewDTO>>> GetCities(int page, string search);
        Task<ResponseData<CityViewDTO>> GetCityById(int id);
    }
}
