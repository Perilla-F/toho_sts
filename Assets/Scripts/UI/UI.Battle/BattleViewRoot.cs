using UnityEngine;

public class BattleViewRoot : MonoBehaviour
{
    [SerializeField] public HandView HandView;
    [SerializeField] public DeckView DeckView;
    [SerializeField] public DiscardAreaView DiscardAreaView;
    [SerializeField] public TimelineView TimelineView;
    [SerializeField] public ManaView ManaView;
    [SerializeField] public TurnMessagePanel TurnMessagePanel;

}
