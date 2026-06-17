using UnityEngine;
using Cysharp.Threading.Tasks;

public class BattleViewRoot : MonoBehaviour
{
    [SerializeField] public HandView HandView;
    [SerializeField] public DeckView DeckView;
    [SerializeField] public DiscardAreaView DiscardAreaView;
    [SerializeField] public TimelineView TimelineView;
    [SerializeField] public ManaView ManaView;
    [SerializeField] public TurnMessagePanel TurnMessagePanel;
    [SerializeField] public TurnEndButton TurnEndButton;

    public async UniTask BattleStart()
    {
        await TurnMessagePanel.ShowMessage("戦闘開始");
    }

    public async UniTask TurnStart(int turn)
    {
        await TurnMessagePanel.ShowMessage("第" + turn + "巡目");
    }

}
