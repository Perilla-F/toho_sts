public interface IBattleViewRoot
{
    public IDeckView DeckView { get; }
    public IHandView HandView { get; }
    public IDiscardAreaView DiscardAreaView { get; }
    public ITimelineView TimelineView { get; }
}