public class PlayerStats
{
    // Primary Skills
    public Stat Strength;
    public Stat Agility;

    // Secondary Skills
    public Skill Farming;
    public Skill Fishing;

    public PlayerStats()
    {
        Strength = new Stat();
        Agility = new Stat();
        Farming = new Skill();
        Fishing = new Skill();
    }
    public PlayerStats(int PlayerID)
    {
        // if old player, load stats
        // else
        Strength = new Stat();
        Agility = new Stat();
        Farming = new Skill();
        Fishing = new Skill();
    }
}
