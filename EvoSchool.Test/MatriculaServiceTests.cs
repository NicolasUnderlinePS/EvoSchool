using System.Data;
using System.Threading.Tasks;
using EvoSchool.Domain.Entities;
using EvoSchool.Domain.Exceptions;
using EvoSchool.Domain.Interfaces.Repositories;
using EvoSchool.Service.DTOs.Requests;
using EvoSchool.Service.Services;
using Moq;
using Xunit;

namespace EvoSchool.Tests.Services
{
    public class MatriculaServiceTests
    {
        private readonly Mock<IMatriculaRepository> _matriculaRepoMock;
        private readonly Mock<IAlunoRepository> _alunoRepoMock;
        private readonly Mock<ITurmaRepository> _turmaRepoMock;
        private readonly MatriculaService _service;

        public MatriculaServiceTests()
        {
            _matriculaRepoMock = new Mock<IMatriculaRepository>();
            _alunoRepoMock = new Mock<IAlunoRepository>();
            _turmaRepoMock = new Mock<ITurmaRepository>();

            const string fakeConnectionString = "Server=fake;Database=fake;Trusted_Connection=True;";

            _service = new MatriculaService(
                _matriculaRepoMock.Object,
                _alunoRepoMock.Object,
                _turmaRepoMock.Object,
                fakeConnectionString
            );
        }

        [Fact]
        public async Task AddAsync_DeveLancarNotFoundException_QuandoAlunoNaoExiste()
        {
            // Arrange
            var request = new InsertMatriculaRequest { AlunoId = 1, TurmaId = 10 };
            _alunoRepoMock.Setup(r => r.GetByIdAsync(request.AlunoId))
                          .ReturnsAsync((Aluno)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.AddAsync(request));
            Assert.Contains("Aluno com Id 1 não foi encontrado", ex.Message);
        }

        [Fact]
        public async Task AddAsync_DeveLancarBusinessRuleException_QuandoAlunoEstiverInativo()
        {
            // Arrange
            var request = new InsertMatriculaRequest { AlunoId = 1, TurmaId = 10 };
            var alunoInativo = new Aluno { Id = 1, Nome = "Lucas", Ativo = false };

            _alunoRepoMock.Setup(r => r.GetByIdAsync(request.AlunoId))
                          .ReturnsAsync(alunoInativo);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _service.AddAsync(request));
            Assert.Equal("Aluno inativo não pode realizar matrícula.", ex.Message);

        }

        [Fact]
        public async Task AddAsync_DeveLancarBusinessRuleException_QuandoTurmaNaoPossuirVagas()
        {
            // Arrange
            var request = new InsertMatriculaRequest { AlunoId = 1, TurmaId = 10 };
            var alunoAtivo = new Aluno { Id = 1, Nome = "Lucas", Ativo = true };
            var turmaSemVaga = new Turma { Id = 10, Nome = "3º Ano B", VagasDisponiveis = 0 };

            _alunoRepoMock.Setup(r => r.GetByIdAsync(request.AlunoId)).ReturnsAsync(alunoAtivo);
            _turmaRepoMock.Setup(r => r.GetByIdAsync(request.TurmaId)).ReturnsAsync(turmaSemVaga);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BusinessRuleException>(() => _service.AddAsync(request));
            Assert.Equal("Não há vagas disponíveis para a turma informada.", ex.Message);
        }
    }
}