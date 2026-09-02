namespace EvoSchool.Service.DTOs.Requests
{
    public class UpdateAlunoRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
    }
}
