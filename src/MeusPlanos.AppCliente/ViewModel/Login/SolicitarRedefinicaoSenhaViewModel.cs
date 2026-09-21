using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MeusPlanos.AppCliente.ViewModel.Base;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.AppCliente.ViewModel.Login;

public class SolicitarRedefinicaoSenhaViewModel : BaseViewModel, IEntidade<Guid>
{
    public Guid Id { get; set; }

    [DisplayName("Usuário")]
    [Required(ErrorMessage = "O preenchimento do campo {0} é obrigatório.")]
    public string Username { get; set; }

    [DisplayName("E-mail")]
    [Required(ErrorMessage = "O preenchimento do campo {0} é obrigatório.")]
    [EmailAddress(ErrorMessage = "Por gentileza, forneça um endereço de e-mail válido.")]
    public string Email { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }
}