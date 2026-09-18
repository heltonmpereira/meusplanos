using System;

namespace MeusPlanos.Definicao.Modelo.Usuario;

public class AlterarSenha
{
    public Guid Id { get; set; }
    public string SenhaAtual { get; set; }
    public string NovaSenha { get; set; }
}