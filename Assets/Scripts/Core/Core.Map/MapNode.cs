using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode
{
    public int x; // 列番号（0〜4）
    public int y; // 階層（高さ、0がスタート階層）
    public bool isReachable = false; // 次の階層への接続
    public NodeType type; // 戦闘、休憩、イベントなど
}

