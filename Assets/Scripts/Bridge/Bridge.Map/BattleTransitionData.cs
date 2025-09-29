using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップ→戦闘遷移時に渡すデータ
/// </summary>
[Serializable]
public class BattleTransitionData
{
    /// <summary>遷移先のセル種別（Battle / Elite / Boss）</summary>
    public CellType CellType;

    /// <summary>Encounter ID（戦闘データ識別用）</summary>
    public string EncounterId;

    /// <summary>ScrollRect の垂直位置（0〜1）</summary>
    public float ScrollPosition;

    /// <summary>プレイヤーアイコンの anchoredPosition</summary>
    public Vector2 PlayerMarkerPos;

    /// <summary>マップ上のセル状態一覧</summary>
    public List<MapCellState> CellStates;
}

/// <summary>
/// マップセルの状態保持用
/// </summary>
[Serializable]
public class MapCellState
{
    public Vector2Int GridPos;
    public bool Cleared;
    public bool EventCompleted;
    public CellType Type;
}
