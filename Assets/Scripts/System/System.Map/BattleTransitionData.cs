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
    public EncounterType Type;

    /// <summary>Encounter ID（戦闘データ識別用）</summary>
    public string EncounterId;

    /// <summary>戦闘開始時のX座標</summary>
    public int CellX;

    /// <summary>戦闘開始時のY座標</summary>
    public int CellY;

    /// <summary>マップ上のセル状態一覧</summary>
    public Dictionary<Vector2Int, MapCellState> CellStates;
}
