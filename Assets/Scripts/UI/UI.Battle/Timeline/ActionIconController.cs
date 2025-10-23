using UnityEngine;

public class ActionIconController : MonoBehaviour
{
    [SerializeField] private EnemyUI enemyUI;
    [SerializeField] private HPResource hp;

    private void Start()
    {
        enemyUI.Bind(hp);
    }

    public void PlanAction(Sprite icon, string actionName)
    {
        enemyUI.SetActionIcon(icon, actionName);
    }

    public void ExecuteAction(BattleContext ctx)
    {
        enemyUI.SetActionIconVisible(false);
    }

    public void ShowActionHighlight(bool highlight)
    {
        enemyUI.HighlightAction(highlight);
    }
}
