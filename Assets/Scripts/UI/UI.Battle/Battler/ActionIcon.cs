using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _iconSpace;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private GameObject _highlightEffect;

    [SerializeField] private Sprite _attackIcon;
    [SerializeField] private Sprite _blockIcon;
    [SerializeField] private Sprite _buffIcon;
    [SerializeField] private Sprite _debuffIcon;
    [SerializeField] private Sprite _white;

    public int number;
    public int time;
    public int damage;

    /// <summary>
    /// 敵UIに行動アイコンをセット
    /// </summary>
    /// <param name="type">攻撃, 防御, バフ, デバフなど</param>
    /// <param name="time">行動までの時間</param>
    /// <param name="number">この敵の何番目の行動か</param>
    /// <param name="damage">攻撃の時のダメージ量</param>
    public void SetIcon(EnemyActionType type, int time, int number, int damage = 0)
    {
        _iconSpace.sprite = GetIcon(type);
        _timer.text = time.ToString();
        this.time = time;
        this.number = number;
        this.damage = damage;
        if (type == EnemyActionType.Attack) _damageText.text = damage.ToString();
    }

    /// <summary>
    /// アイコンの決定
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Sprite GetIcon(EnemyActionType type)
    {
        switch (type)
        {
            case EnemyActionType.Attack:
                return _attackIcon;
            case EnemyActionType.Block:
                return _blockIcon;
            case EnemyActionType.Buff:
                return _buffIcon;
            case EnemyActionType.Debuff:
                return _debuffIcon;
            default:
                return _white;
        }
    }

    public void HighlightIcon(bool highlight)
    {
        _highlightEffect.SetActive(highlight);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string title = "攻撃";
        string desc = $"{time}ステップ後に{damage}点ダメージの攻撃";

        TooltipManager.Instance.Show(title, desc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.Hide();
    }
}