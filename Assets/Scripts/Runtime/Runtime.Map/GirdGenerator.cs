using UnityEngine;

public class GridMapGenerator : MonoBehaviour
{
    public int columns = 5;
    public int rows = 10;

    public MapNode[,] grid; // [x,y]でアクセス

    public void GenerateMap()
    {
        grid = new MapNode[columns, rows];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                grid[x, y] = new MapNode
                {
                    x = x,
                    y = y,
                    type = GetRandomNodeType()
                };
            }
        }
    }

    private NodeType GetRandomNodeType()
    {
        return (NodeType)Random.Range(0, System.Enum.GetValues(typeof(NodeType)).Length);
    }
}
