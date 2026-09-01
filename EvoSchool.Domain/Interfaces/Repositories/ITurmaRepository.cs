using EvoSchool.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvoSchool.Domain.Interfaces.Repositories
{
    public interface ITurmaRepository
    {
        Task<List<Turma>> ListOpenClassVacanciesAsync();
        Task<Turma> GetByIdAsync(int id);

    }
}
