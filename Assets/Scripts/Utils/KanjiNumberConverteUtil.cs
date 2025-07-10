using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KanjiNumberConverteUtil
{
    private static readonly string[] KanjiDigits =
        { "零", "壱", "弐", "参", "肆", "伍", "陸", "漆", "捌", "玖" };
    private static readonly string[] KanjiUnits =
        { "", "拾", "佰", "仟" };
    private static readonly string[] KanjiBigUnits =
        { "", "萬", "億", "兆" }; // 万以上にも対応

    /// <summary>
    /// int→String(大字)
    /// </summary>
    public static string ConvertToKanjiWithUnits(int number)
    {
        if (number == 0) return KanjiDigits[0];

        string result = "";
        string numStr = number.ToString();
        int len = numStr.Length;

        // 万・億などのブロック（4桁ごと）で処理
        int blockCount = 0;

        while (numStr.Length > 0)
        {
            // 下4桁ずつ切り出す
            int blockLength = Mathf.Min(4, numStr.Length);
            string block = numStr.Substring(numStr.Length - blockLength, blockLength);
            numStr = numStr.Substring(0, numStr.Length - blockLength);

            string blockResult = ConvertBlockToKanji(block);
            if (!string.IsNullOrEmpty(blockResult))
            {
                result = blockResult + KanjiBigUnits[blockCount] + result;
            }

            blockCount++;
        }

        return result;
    }

    // 千以下（最大4桁）の位取り漢数字変換
    private static string ConvertBlockToKanji(string block)
    {
        int length = block.Length;
        string result = "";

        for (int i = 0; i < block.Length; i++)
        {
            int digit = block[i] - '0';
            int unitIndex = length - i - 1;

            if (digit == 0)
                continue;

            // 「一十」→「十」などの略記ルール
            if (digit == 1 && unitIndex == 1 && block.Length == 2)
            {
                result += KanjiUnits[unitIndex];
            }
            else
            {
                result += KanjiDigits[digit] + KanjiUnits[unitIndex];
            }
        }

        return result;
    }
}
