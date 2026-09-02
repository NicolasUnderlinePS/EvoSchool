using System;
using System.ComponentModel.DataAnnotations;

namespace EvoSchool.Service.DTOs.Requests
{
    public class InsertAlunoRequest
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; }
    }
}
