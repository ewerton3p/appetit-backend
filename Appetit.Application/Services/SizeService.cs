using Appetit.Application.DTOs.Size;
using Appetit.Application.Interfaces;
using Appetit.Domain.Common.Exceptions;
using Appetit.Domain.Common.Responses;
using Appetit.Domain.Models;
using Appetit.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Appetit.Application.Services
{
    public class SizeService : ISizeService
    {
        private readonly IUserService _userService;
        private readonly ISizeRepository _sizeRepository;

        public SizeService(IUserService userService, ISizeRepository sizeRepository)
        {
            _userService = userService;
            _sizeRepository = sizeRepository;
        }

        public async Task<ResponseData<List<SizeViewDTO>>> GetSizes(int page, string search)
        {
            ResponseData<List<SizeViewDTO>> response = new();

            Expression<Func<Size, bool>> filter = s => string.IsNullOrEmpty(search) || s.Description.Contains(search) || s.Abbreviation.Contains(search);
            Expression<Func<Size, object>> order = s => s.Description;

            var result = await _sizeRepository.GetWithPagination(page, filter, order);
            response.Data = result.Items?.Select(s => s.ToSizeViewDTO()).ToList();
            response.Pagination = new Pagination(page, result.TotalCount);

            if (response.Data == null || response.Data.Count == 0)
                response.Message = "Nenhum tamanho encontrado.";

            return response;
        }

        public async Task<ResponseData<SizeViewDTO>> GetSizeById(int id)
        {
            ResponseData<SizeViewDTO> response = new();

            Size? size = await _sizeRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Tamanho com o código: {id} não foi localizado.");

            response.Data = size.ToSizeViewDTO();

            return response;
        }

        public async Task<Response> CreateSize(SizeCreateDTO newSize)
        {
            Response response = new();

            var size = newSize.ToSize();
            size.CreatedById = await _userService.GetLoggedUserId();

            await _sizeRepository.AddAsync(size);

            response.Message = "Tamanho criado com sucesso.";

            return response;
        }

        public async Task<Response> UpdateSize(int id, SizeCreateDTO updatedSize)
        {
            Response response = new();

            Size? size = await _sizeRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Tamanho com o código: {id} não foi localizado.");

            updatedSize.ToSize(size);
            size.UpdatedById = await _userService.GetLoggedUserId();

            await _sizeRepository.Update(size);

            response.Message = "Tamanho atualizado com sucesso.";

            return response;
        }

        public async Task<Response> DeleteSize(int id)
        {
            Response response = new();

            try
            {
                Size? size = await _sizeRepository.GetByIdAsync(id)
                    ?? throw new NotFoundException("Tamanho não localizado.");

                await _sizeRepository.DeleteAsync(size);

                response.Message = "Tamanho excluído com sucesso.";
            }
            catch (DbUpdateException ex)
            {
                response.Success = false;
                response.Message = ex.InnerException?.Message.Contains("REFERENCE", StringComparison.OrdinalIgnoreCase) == true
                    ? "Não é possível excluir este tamanho porque ele está associado a outros registros."
                    : "Ocorreu um erro ao tentar excluir o tamanho.";
            }

            return response;
        }
    }
}
