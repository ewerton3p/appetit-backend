using Appetit.Application.DTOs.Size;
using Appetit.Domain.Common.Responses;

namespace Appetit.Application.Interfaces
{
    public interface ISizeService
    {
        Task<ResponseData<List<SizeViewDTO>>> GetSizes(int page, string search);
        Task<ResponseData<SizeViewDTO>> GetSizeById(int id);
        Task<Response> CreateSize(SizeCreateDTO newSize);
        Task<Response> UpdateSize(int id, SizeCreateDTO updatedSize);
        Task<Response> DeleteSize(int id);
    }
}
