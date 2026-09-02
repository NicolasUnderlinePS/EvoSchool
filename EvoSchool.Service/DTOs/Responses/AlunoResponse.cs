using System;

namespace EvoSchool.Service.DTOs.Responses
{
    public class AlunoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string DataNascimento { get; set; }
        public string Ativo { get; set; }
        public string DataCadastro { get; set; }
    }
}
