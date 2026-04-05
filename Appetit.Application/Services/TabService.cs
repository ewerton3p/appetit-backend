using Appetit.Application.DTOs.Tab;
using Appetit.Application.Interfaces;
using Appetit.Domain.Common.Exceptions;
using Appetit.Domain.Common.Responses;
using Appetit.Domain.Models;
using Appetit.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Appetit.Application.Services
{
    public class TabService : ITabService
    {
        private readonly IUserService _userService;
        private readonly ITabRepository _tabRepository;

        public TabService(IUserService userService, ITabRepository tabRepository)
        {
            _userService = userService;
            _tabRepository = tabRepository;
        }

        public async Task<ResponseData<List<TabViewDTO>>> GetTabs(int page, string search)
        {
            ResponseData<List<TabViewDTO>> response = new();

            Expression<Func<Tab, bool>> filter = t => string.IsNullOrEmpty(search) || t.Identifier.Contains(search);
            Expression<Func<Tab, object>> order = t => t.Identifier;

            var result = await _tabRepository.GetWithPagination(page, filter, order);
            response.Data = result.Items?.Select(t => t.ToTabViewDTO()).ToList();
            response.Pagination = new Pagination(page, result.TotalCount);

            if (response.Data == null || response.Data.Count == 0)
                response.Message = "Nenhuma comanda/mesa encontrada.";

            return response;
        }

        public async Task<ResponseData<TabViewDTO>> GetTabById(int id)
        {
            ResponseData<TabViewDTO> response = new();

            Tab? tab = await _tabRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Comanda/Mesa com o código: {id} não foi localizada.");

            response.Data = tab.ToTabViewDTO();

            return response;
        }

        public async Task<Response> CreateTab(TabCreateDTO newTab)
        {
            Response response = new();

            var tab = newTab.ToTab();
            tab.CreatedById = await _userService.GetLoggedUserId();

            await _tabRepository.AddAsync(tab);

            response.Message = "Comanda/Mesa criada com sucesso.";

            return response;
        }

        public async Task<Response> UpdateTab(int id, TabCreateDTO updatedTab)
        {
            Response response = new();

            Tab? tab = await _tabRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Comanda/Mesa com o código: {id} não foi localizada.");

            updatedTab.ToTab(tab);
            tab.UpdatedById = await _userService.GetLoggedUserId();

            await _tabRepository.Update(tab);

            response.Message = "Comanda/Mesa atualizada com sucesso.";

            return response;
        }

        public async Task<Response> DeleteTab(int id)
        {
            Response response = new();

            try
            {
                Tab? tab = await _tabRepository.GetByIdAsync(id)
                    ?? throw new NotFoundException("Comanda/Mesa não localizada.");

                await _tabRepository.DeleteAsync(tab);

                response.Message = "Comanda/Mesa excluída com sucesso.";
            }
            catch (DbUpdateException ex)
            {
                response.Success = false;
                response.Message = ex.InnerException?.Message.Contains("REFERENCE", StringComparison.OrdinalIgnoreCase) == true
                    ? "Não é possível excluir esta comanda/mesa porque ela está associada a outros registros."
                    : "Ocorreu um erro ao tentar excluir a comanda/mesa.";
            }

            return response;
        }
    }
}
