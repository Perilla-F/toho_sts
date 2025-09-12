import pandas as pd
import json

def convert_excel_to_json(excel_path, output_path):
    df = pd.read_excel(excel_path)

    result = []

    for _, row in df.iterrows():
        enemy = {
            "EnemyId": row["EnemyId"],
            "BattlerName": row["BattlerName"],
            "MaxHp": int(row["MaxHp"]),
            "EnemyAI": []
        }

        # 最大2つのアクションを想定（必要ならループで増やせます）
        for i in range(1, 10):  # action1〜action9 まで対応可能
            action_name_key = f"action{i}_name"
            action_type_key = f"action{i}_type"
            action_amount_key = f"action{i}_amount"

            if pd.isna(row.get(action_name_key)):
                break  # 空なら打ち切り

            EnemyAI = {
                "name": row[action_name_key],
                "effects": [
                    {
                        "type": row[action_type_key],
                        "amount": int(row[action_amount_key])
                    }
                ]
            }
            enemy["actions"].append(EnemyAI)

        result.append(enemy)

    with open(output_path, "w", encoding="utf-8") as f:
        json.dump(result, f, indent=2, ensure_ascii=False)

    print(f"✅ JSON出力完了: {output_path}")

# 実行例
if __name__ == "__main__":
    convert_excel_to_json("enemies.xlsx", "enemy_data.json")
