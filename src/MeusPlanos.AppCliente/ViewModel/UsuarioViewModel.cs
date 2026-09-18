using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MeusPlanos.AppCliente.ViewModel.Base;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.AppCliente.ViewModel;

public class UsuarioViewModel : BaseViewModel, IEntidade<Guid>
{
    public UsuarioViewModel()
    {
        PasswordHash = "123456";
    }

    public Guid Id { get; set; }
    [Required(ErrorMessage = "O preenchimento do campo {0} é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O preenchimento do campo {0} é obrigatório.")]
    public string Sobrenome { get; set; }

    [DisplayName("E-mail")]
    [Required(ErrorMessage = "O preenchimento do campo {0} é obrigatório.")]
    [EmailAddress(ErrorMessage = "Por gentileza, forneça um endereço de e-mail válido.")]
    public string Email { get; set; }

    [DisplayName("Usuário")]
    [Required(ErrorMessage = "O preenchimento do campo {0} é obrigatório.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "O preenchimento do campo {0} é obrigatório.")]
    public string PasswordHash { get; set; }

    [DisplayName("Cadastro")]
    public DateTimeOffset DataCriacao { get; set; }
    [DisplayName("Alteração")]
    public DateTimeOffset? DataAlteracao { get; set; }
    public DateTimeOffset? DataDelecao { get; set; }

    [DisplayName("Nome")]
    public string NomeCompleto => $"{Nome} {Sobrenome}";

    public string CodigoRedefinicaoSenha { get; set; }

    public ICollection<UsuarioPapelViewModel> Papeis { get; set; }
}