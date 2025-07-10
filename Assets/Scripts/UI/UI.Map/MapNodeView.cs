using System;
using UnityEngine;
using UnityEngine.UI;

public class MapNodeView : MonoBehaviour
{
    public MapNode node;
    private Button button;

    // イベントで通知する（上層 → 下層を避けるため）
    public event Action<MapNode> OnClicked;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnClicked?.Invoke(node));
    }

    public void Setup(MapNode nodeData)
    {
        node = nodeData;
        UpdateInteractable();
    }

    public void UpdateInteractable()
    {
        button.interactable = node.isReachable;
    }
}
