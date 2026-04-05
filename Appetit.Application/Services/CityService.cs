using Appetit.Application.DTOs.City;
using Appetit.Application.Interfaces;
using Appetit.Domain.Common.Exceptions;
using Appetit.Domain.Common.Responses;
using Appetit.Domain.Models;
using Appetit.Infrastructure.Data.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Appetit.Application.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<ResponseData<List<CityViewDTO>>> GetCities(int page, string search)
        {
            ResponseData<List<CityViewDTO>> response = new();

            Expression<Func<City, bool>> filter = c => string.IsNullOrEmpty(search) || c.Name.Contains(search) || c.Uf.Contains(search);
            Expression<Func<City, object>> order = c => c.Name;

            var result = await _cityRepository.GetWithPagination(page, filter, order);
            response.Data = result.Items?.Select(c => c.ToCityViewDTO()).ToList();
            response.Pagination = new Pagination(page, result.TotalCount);

            if (response.Data == null || response.Data.Count == 0)
                response.Message = "Nenhuma cidade encontrada.";

            return response;
        }

        public async Task<ResponseData<CityViewDTO>> GetCityById(int id)
        {
            ResponseData<CityViewDTO> response = new();

            City? city = await _cityRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Cidade com o código: {id} não foi localizada.");

            response.Data = city.ToCityViewDTO();

            return response;
        }
    }
}
