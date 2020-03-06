public class PlayerStats
{
    // Stats
    public Stat Strength;
    public Stat Endurance;
    public Stat Intelligence;
    public Stat Agility;
    public Stat Luck;

    // Skills
    public Skill Farming;
    public Skill Melee;
    public Skill Guns;
    public Skill Science;
    public Skill Crafting;

    // Perks

    public PlayerStats()
    {
        Strength = new Stat();
        Endurance = new Stat();
        Intelligence = new Stat();
        Agility = new Stat();
        Luck = new Stat();
        
        Farming = new Skill();
        Melee = new Skill();
        Guns = new Skill();
        Science = new Skill();
        Crafting = new Skill();
    }
    public PlayerStats(int PlayerID)
    {
        // if old player, load stats
        // else
        Strength = new Stat();
        Endurance = new Stat();
        Intelligence = new Stat();
        Agility = new Stat();
        Luck = new Stat();
        
        Farming = new Skill();
        Melee = new Skill();
        Guns = new Skill();
        Science = new Skill();
        Crafting = new Skill();
    }
}
