using EvoSchool.Domain.Commons;
using EvoSchool.Service.DTOs.Requests;
using EvoSchool.Service.DTOs.Responses;
using System.Threading.Tasks;

namespace EvoSchool.Service.Interfaces.Services
{
    public interface IAlunoService
    {
        Task<PaginationModel<AlunoResponse>> GetListAsync(string nome, int pagina, int tamanhoPagina);
        Task<AlunoResponse> GetByIdAsync(int id);
        Task<AlunoResponse> AddAsync(InsertAlunoRequest request);
        Task UpdateAsync(int id, UpdateAlunoRequest request);
        Task DeleteAsync(int id);
    }
}
