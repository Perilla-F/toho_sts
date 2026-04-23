using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [SerializeField] private GameObject _tooltipWindow;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private RectTransform _canvasRect; // 親CanvasのRect

    private RectTransform _windowRect;

    private void Awake()
    {
        Instance = this;
        _windowRect = _tooltipWindow.GetComponent<RectTransform>();
        Hide(); // 最初は隠しておく
    }

    private void Update()
    {
        if (_tooltipWindow.activeSelf)
        {
            UpdatePosition();
        }
    }

    public void Show(string title, string description)
    {
        _tooltipWindow.SetActive(true);
        _titleText.text = title;
        _descriptionText.text = description;
        UpdatePosition(); // 表示した瞬間に位置を合わせる
    }

    public void Hide()
    {
        _tooltipWindow.SetActive(false);
    }

    private void UpdatePosition()
    {
        // マウス位置をCanvas上の座標に変換
        Vector2 mousePos = Input.mousePosition;

        // ツールチップがマウスの右下に出るようにオフセットを加える
        _windowRect.position = mousePos + new Vector2(10f, -10f);
    }
}