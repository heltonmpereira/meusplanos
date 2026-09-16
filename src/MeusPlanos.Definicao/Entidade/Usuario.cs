using System;
using System.Collections.Generic;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.Definicao.Entidade
{
    public class Usuario : IEntidade<Guid>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string CodigoRedefinicaoSenha { get; set; }
        public bool Deletado { get; set; }

        public ICollection<UsuarioPapel> Papeis { get; set; }
    }
}