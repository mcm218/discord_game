public class PlayerStats
{
    // Physical Attributes
    public Stat Height;
    public Stat HairColor;
    // Stats
    public Stat Strength;
    public Stat Agility;

    // Skills
    public Skill Farming;
    public Skill Fishing;

    public PlayerStats()
    {
        Height = new Stat(false);
        HairColor = new Stat(false);

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
