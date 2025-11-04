using UnityEngine;

public class BattleViewRoot : MonoBehaviour, IBattleViewRoot
{
    [SerializeField] public IHandView HandView { get; }
    [SerializeField] public IDeckView DeckView { get; }
    [SerializeField] public IDiscardAreaView DiscardAreaView { get; }
    [SerializeField] public ITimelineView TimelineView { get; }
    [SerializeField] public ManaView ManaView;
    [SerializeField] public TurnMessagePanel TurnMessagePanel;
    [SerializeField] public TurnEndButton TurnEndButton;

    public void BattleStart()
    {
    }

}
