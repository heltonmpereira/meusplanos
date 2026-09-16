using System;

namespace MeusPlanos.Definicao.Modelo.Usuario
{
    public class UsuarioRedefinirSenha
    {
        public Guid Id { get; set; }
        public string CodigoRedefinicaoSenha { get; set; }
        public string Password { get; set; }
    }
}