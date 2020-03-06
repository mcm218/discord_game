using UnityEngine;

public abstract class Tool
{
    public string toolType { get; private set; }
    public int toolStrength { get; private set; }
    public int durability { get; private set; }
    public Sprite sprite { get; private set; }

    public void adjustDurability(int num)
    {
        durability += num;
        if (durability <= 0)
        {
            breakTool();
        }
    }
    public abstract void breakTool();
}