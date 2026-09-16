using System.Collections.Generic;

namespace MeusPlanos.Definicao.Interface.Servico.Resposta
{
    public interface IRespostaPaginadaServico<T> : IRespostaServico<IList<T>>
    {
        int ItensPorPagina { get; set; }
        int NumeroPagina { get; set; }
        int TotalPaginas { get; set; }
        int TotalRegistros { get; set; }
    }
}