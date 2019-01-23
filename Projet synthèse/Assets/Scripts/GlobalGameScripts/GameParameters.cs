using System.Collections.Generic;
using Assets.Scripts.AchievementModule;
using Assets.Scripts.ConnectionModule;

public static class GameParameters
{
    public static int numberOfPlayers { get; set; }

    public static int[] playersSelectedModelIndex { get; set; }

    public static float MasterVolume { get; set; }

    public static float SoundEffectsVolume { get; set; }

    public static float AnnouncerVolume { get; set; }

    public static float MusicVolume { get; set; }

    public static string ChosenArena { get; set; }

    public static bool VolumeHasBeenChanged { get; set; }

    public static IList<Arena> ArenaList { get; set; }

    public static IList<Achievement> AchievementsList { get; set; }

    public static int GetArenaIdByName(string _name)
    {
        string formattedName;
        int id = 0;
        foreach (Arena arena in ArenaList)
        {
            formattedName = arena.name;
            formattedName = formattedName.Replace("\"", "");

            if (formattedName == _name)
            {
                id = arena.id;
            }
        }
        return id;
    }

}