using UnityEngine;

public class CardDetailPopupManager : MonoBehaviour, ICardDetailPopupManager
{
    public static CardDetailPopupManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject popupCanvasRoot;   // Canvasごとシーンをまたいで残すルートオブジェクト
    [SerializeField] private CardDetailPopup cardDetailPopup; // ポップアップ本体のスクリプト

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            ServiceLocator.Register<ICardDetailPopupManager>(this);
        }
        else
        {
            // すでに存在している場合は重複生成された方を破壊する
            Destroy(popupCanvasRoot);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// どのシーンからでもこのメソッドを呼ぶだけでポップアップが開く
    /// </summary>
    public void OpenPopup(CardData cardData)
    {
        if (cardDetailPopup != null)
        {
            cardDetailPopup.Show(cardData);
        }
    }

    public void ClosePopup()
    {
        if (cardDetailPopup != null)
        {
            cardDetailPopup.ClosePopup();
        }
    }
}