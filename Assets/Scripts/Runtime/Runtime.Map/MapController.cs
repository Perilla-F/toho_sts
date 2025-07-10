using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public static MapController Instance { get; private set; }

    public GridMapGenerator mapGenerator;
    public Transform mapRoot;
    public GameObject nodePrefab { get; private set; }

    private MapNode currentNode;
    private MapNodeView[,] nodeViews;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateAndShowMap();

        MapNodeView view = Instantiate(nodePrefab, mapRoot).GetComponent<MapNodeView>();
        view.Setup(currentNode);
        view.OnClicked += OnNodeSelected;
    }

    void GenerateAndShowMap()
    {
        mapGenerator.GenerateMap();
        var grid = mapGenerator.grid;

        int columns = mapGenerator.columns;
        int rows = mapGenerator.rows;
        nodeViews = new MapNodeView[columns, rows];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                var node = grid[x, y];

                GameObject obj = Instantiate(nodePrefab, mapRoot);
                var view = obj.GetComponent<MapNodeView>();
                view.Setup(node);
                nodeViews[x, y] = view;

                // UI配置（例: Gridに合わせてRectTransform移動）
                RectTransform rect = obj.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(x * 100, y * 100);
            }
        }

        // スタートノード選択可能に
        for (int x = 0; x < columns; x++)
        {
            grid[x, 0].isReachable = true;
            nodeViews[x, 0].UpdateInteractable();
        }
    }

    public void OnNodeSelected(MapNode selected)
    {
        Debug.Log($"Selected node: ({selected.x}, {selected.y}) Type: {selected.type}");

        // 現在地を更新
        currentNode = selected;

        // 一度すべて選択不可に
        foreach (var view in nodeViews)
        {
            view.node.isReachable = false;
            view.UpdateInteractable();
        }

        // 次に進めるノードを有効に
        var next = GetReachableNodes(currentNode);
        foreach (var node in next)
        {
            node.isReachable = true;
            nodeViews[node.x, node.y].UpdateInteractable();
        }
    }

    private List<MapNode> GetReachableNodes(MapNode current)
    {
        List<MapNode> result = new();
        int nextY = current.y + 1;
        if (nextY >= mapGenerator.rows) return result;

        for (int dx = -1; dx <= 1; dx++)
        {
            int nx = current.x + dx;
            if (nx >= 0 && nx < mapGenerator.columns)
            {
                result.Add(mapGenerator.grid[nx, nextY]);
            }
        }

        return result;
    }
}
