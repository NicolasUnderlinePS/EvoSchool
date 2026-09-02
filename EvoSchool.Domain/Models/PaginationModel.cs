using System;
using System.Collections.Generic;

namespace EvoSchool.Domain.Commons
{
    public class PaginationModel<T>
    {
        public List<T> Itens { get; set; } = new List<T>();
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalItens { get; set; }
        public int TamanhoPagina { get; set; }

        public PaginationModel() { }

        public PaginationModel(List<T> itens, int totalItens, int paginaAtual, int tamanhoPagina)
        {
            Itens = itens;
            TotalItens = totalItens;
            PaginaAtual = paginaAtual;
            TamanhoPagina = tamanhoPagina;
            TotalPaginas = (int)Math.Ceiling(totalItens / (double)tamanhoPagina);
        }
    }
}
