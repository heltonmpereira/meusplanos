namespace MeusPlanos.Definicao.Interface.Entidade
{
    public interface IEntidade<TPK>
    {
        TPK Id { get; set; }
        bool Deletado { get; set; }
    }
}