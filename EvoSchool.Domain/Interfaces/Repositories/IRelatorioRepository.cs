using EvoSchool.Service.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoSchool.Domain.Interfaces.Repositories
{
    public interface IRelatorioRepository
    {
        Task<List<AlunosPorTurmaResponse>> ListStudentsPerClassAsync();
    }
}
