using Cysharp.Threading.Tasks;

public interface IBattleSystem
{
    public IHeroUnit Hero { get; }
    UniTask Draw(int count);
}
