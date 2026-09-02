using EvoSchool.Service.DTOs.Requests;
using EvoSchool.Service.DTOs.Responses;
using System.Threading.Tasks;

namespace EvoSchool.Service.Interfaces.Services
{
    public interface IMatriculaService
    {
        Task<MatriculaResponse> AddAsync(InsertMatriculaRequest request);
    }
}
