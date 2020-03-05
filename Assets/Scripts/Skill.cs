public class Skill
{
    public Stat[] stats { get; private set; }
    public int level { get; private set; }
    public int experience { get; set; }


    public Skill(int _level = 1)
    {
        level = _level;
        stats = new Stat[0];
    }

    public Skill(Stat[] _stats, int _level = 1)
    {
        level = _level;
        stats = _stats;
    }

    public void adjustExperience(int change)
    {
        experience += change;
        // check if experience meets level up requirement
        while (experience > level * 100)
        {
            // if true, level up and subtract exp requirement from exp total
            experience -= level * 100;
            level++;
            // check if player has enough experience to level up again
        }
    }
}