using System.Collections.Generic;

public class CardContext
{
    public IBattleSystem BattleSystem;
    public IBattleUnit User;
    public List<IBattleUnit> Enemies;
    public ISourceCard SourceCard;
    public List<IBattleUnit> Targets;
}
