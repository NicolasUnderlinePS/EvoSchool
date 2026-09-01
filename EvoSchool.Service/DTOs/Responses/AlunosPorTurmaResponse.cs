namespace EvoSchool.Service.DTOs.Responses
{
    public class AlunosPorTurmaResponse
    {
        public int TurmaId { get; set; }
        public string NomeTurma { get; set; }
        public int TotalMatriculados { get; set; }
        public int VagasRestantes { get; set; }
    }
}
