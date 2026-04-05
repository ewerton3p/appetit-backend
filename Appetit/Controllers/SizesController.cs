using Appetit.Application.DTOs.Size;
using Appetit.Application.Interfaces;
using Appetit.Domain.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appetit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SizesController : ControllerBase
    {
        private readonly ISizeService _sizeService;

        public SizesController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<List<SizeViewDTO>>>> GetSizes(int page = 1, string search = "")
            => Ok(await _sizeService.GetSizes(page, search));

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<SizeViewDTO>>> GetSizeById(int id)
            => Ok(await _sizeService.GetSizeById(id));

        [HttpPost("Size")]
        public async Task<ActionResult<Response>> CreateSize(SizeCreateDTO newSize)
            => Created(nameof(CreateSize), await _sizeService.CreateSize(newSize));

        [HttpPut("Size/{id}")]
        public async Task<ActionResult<Response>> UpdateSize(SizeCreateDTO updatedSize, int id)
            => Ok(await _sizeService.UpdateSize(id, updatedSize));

        [HttpDelete("Size/{id}")]
        public async Task<ActionResult<Response>> DeleteSize(int id)
            => Ok(await _sizeService.DeleteSize(id));
    }
}
