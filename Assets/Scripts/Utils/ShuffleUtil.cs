using System.Collections.Generic;
using UnityEngine;

public static class ShuffleUtil
{
    // 拡張メソッド：Listをシャッフル（Fisher–Yatesアルゴリズム）
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }
}
