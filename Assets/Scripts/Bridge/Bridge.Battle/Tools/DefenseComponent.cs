using UnityEngine;

public class DefenseComponent
{
    public int Block;
    public int SimpleBlock;

    public int Consume(int damage)
    {
        // SimpleBlock(6) -> Block(5) の順に削る例
        int remaining = damage;

        // SimpleBlockを削る
        int consumeSimple = Mathf.Min(remaining, SimpleBlock);
        SimpleBlock -= consumeSimple;
        remaining -= consumeSimple;

        // Blockを削る
        int consumeBlock = Mathf.Min(remaining, Block);
        Block -= consumeBlock;
        remaining -= consumeBlock;

        return remaining; // 残ったダメージを返す
    }
}