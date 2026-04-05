using Appetit.Application.DTOs.City;
using Appetit.Application.Interfaces;
using Appetit.Domain.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appetit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<List<CityViewDTO>>>> GetCities(int page = 1, string search = "")
            => Ok(await _cityService.GetCities(page, search));

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<CityViewDTO>>> GetCityById(int id)
            => Ok(await _cityService.GetCityById(id));
    }
}
