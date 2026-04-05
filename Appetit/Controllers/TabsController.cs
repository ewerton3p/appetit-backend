using Appetit.Application.DTOs.Tab;
using Appetit.Application.Interfaces;
using Appetit.Domain.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appetit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TabsController : ControllerBase
    {
        private readonly ITabService _tabService;

        public TabsController(ITabService tabService)
        {
            _tabService = tabService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<List<TabViewDTO>>>> GetTabs(int page = 1, string search = "")
            => Ok(await _tabService.GetTabs(page, search));

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<TabViewDTO>>> GetTabById(int id)
            => Ok(await _tabService.GetTabById(id));

        [HttpPost("Tab")]
        public async Task<ActionResult<Response>> CreateTab(TabCreateDTO newTab)
            => Created(nameof(CreateTab), await _tabService.CreateTab(newTab));

        [HttpPut("Tab/{id}")]
        public async Task<ActionResult<Response>> UpdateTab(TabCreateDTO updatedTab, int id)
            => Ok(await _tabService.UpdateTab(id, updatedTab));

        [HttpDelete("Tab/{id}")]
        public async Task<ActionResult<Response>> DeleteTab(int id)
            => Ok(await _tabService.DeleteTab(id));
    }
}
