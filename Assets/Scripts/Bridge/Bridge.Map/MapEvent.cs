using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Events/MapEvent")]
public class MapEvent : EventBase
{
    public override void Execute(string optionId)
    {
        var option = Options.Find(o => o.Id == optionId);
        if (option == null)
        {
            Debug.LogWarning($"Option not found: {optionId}");
            return;
        }

        Debug.Log($"[Event: {EventId}] 選択肢 {option.Text} を選びました");

        // 結果テキストをUIで表示したい場合
        // EventManager.Instance.ShowResult(option.ResultText);

        // 条件に応じた結果処理（例: ゴールド増減など）
        // switch (option.Id)
        // {
        //     case "A":
        //         GameManager.Instance.Gold += 50;
        //         break;
        //     case "B":
        //         GameManager.Instance.HP -= 10;
        //         break;
        // }
    }
}
