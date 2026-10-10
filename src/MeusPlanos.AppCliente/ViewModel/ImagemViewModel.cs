using System;
using System.ComponentModel.DataAnnotations;
using MeusPlanos.AppCliente.ViewModel.Base;
using Microsoft.AspNetCore.Http;

namespace MeusPlanos.AppCliente.ViewModel;

public class ImagemViewModel : BaseViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Nome do arquivo original")]
    [MaxLength(500)]
    public string NomeOriginal { get; set; }

    [Display(Name = "Nome do arquivo")]
    [MaxLength(500)]
    public string NomeArquivo { get; set; }

    [Display(Name = "Caminho")]
    [MaxLength(1000)]
    public string CaminhoRelativo { get; set; }

    [MaxLength(200)]
    public string ContentType { get; set; }

    [Display(Name = "Tamanho (bytes)")]
    public long TamanhoBytes { get; set; }

    [MaxLength(1000)]
    public string Descricao { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }

    [ScaffoldColumn(false)]
    public IFormFile ArquivoUpload { get; set; }

    [ScaffoldColumn(false)]
    public string UrlAcesso { get; set; }

    [ScaffoldColumn(false)]
    public string TamanhoFormatado => TamanhoBytes switch
    {
        < 1024 => $"{TamanhoBytes} B",
        < 1024 * 1024 => $"{TamanhoBytes / 1024d:F1} KB",
        _ => $"{TamanhoBytes / (1024d * 1024d):F1} MB"
    };
}
