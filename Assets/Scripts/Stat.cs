public class Stat
{
    public bool canLevel { get; private set; }
    public int level { get; private set; }
    public int experience { get; set; }


    public Stat(int _level = 1, bool _canLevel = true)
    {
        level = _level;
        canLevel = _canLevel;
    }


    public void adjustExperience(int change)
    {
        experience += change;
        // check if experience meets level up requirement
        while (experience > level * 1000)
        {
            // if true, level up and subtract exp requirement from exp total
            experience -= level * 1000;
            level++;
            // check if player has enough experience to level up again
        }
    }
}