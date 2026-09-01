using System.Data;
using System.Threading.Tasks;

namespace EvoSchool.Domain.Interfaces.Repositories
{
    public interface IMatriculaRepository
    {
        Task<bool> AlreadyRegisteredAsync(int alunoId, int turmaId, IDbConnection connection, IDbTransaction transaction);
        Task<int> AddAsync(int alunoId, int turmaId, IDbConnection connection, IDbTransaction transaction);
        Task<bool> DecrementVacancyAsync(int turmaId, IDbConnection connection, IDbTransaction transaction);
    }
}
