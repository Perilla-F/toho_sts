using UnityEngine;

public class DefenseComponent
{
    public int Block;
    public int SimpleBlock;

    public int Consume(int damage)
    {
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

    public void ApplyBlock(int amount)
    {
        Block += amount;
    }

    public void ApplySimpleBlock(int amount)
    {
        SimpleBlock += amount;
    }

    public void ClearBlock()
    {
        Block = 0;
    }

    public void ClearSimpleBlock()
    {
        SimpleBlock = 0;
    }
}