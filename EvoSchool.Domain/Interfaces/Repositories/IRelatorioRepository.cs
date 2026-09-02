using EvoSchool.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoSchool.Domain.Interfaces.Repositories
{
    public interface IRelatorioRepository
    {
        Task<List<AlunosTurmaModel>> ListStudentsPerClassAsync();
    }
}
