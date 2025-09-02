using System.Collections.Generic;

public class CardContext
{
    public IBattleSystem BattleSystem;
    public IHeroUnit User;
    public List<IEnemyUnit> Enemies;
    public ISourceCard SourceCard;
}
