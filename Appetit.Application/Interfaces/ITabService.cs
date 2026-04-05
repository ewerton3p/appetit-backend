using Appetit.Application.DTOs.Tab;
using Appetit.Domain.Common.Responses;

namespace Appetit.Application.Interfaces
{
    public interface ITabService
    {
        Task<ResponseData<List<TabViewDTO>>> GetTabs(int page, string search);
        Task<ResponseData<TabViewDTO>> GetTabById(int id);
        Task<Response> CreateTab(TabCreateDTO newTab);
        Task<Response> UpdateTab(int id, TabCreateDTO updatedTab);
        Task<Response> DeleteTab(int id);
    }
}
