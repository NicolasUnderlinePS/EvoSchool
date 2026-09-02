using EvoSchool.Service.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoSchool.Service.Interfaces.Services
{
    public interface IRelatorioService
    {
        Task<List<AlunosTurmaResponse>> ListStudentsPerClassAsync();
    }
}
