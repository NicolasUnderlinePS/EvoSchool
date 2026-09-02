using System;

namespace EvoSchool.Service.DTOs.Requests
{
    public class InsertAlunoRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; }
    }
}
