using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AchievementModule
{
    public abstract class AchievementModule
    {
        public static string ALL_ACHIEVEMENT_DIRECTORY = "/JSON/all_achievements/";
        public static string USER_ACHIEVEMENT_DIRECTORY = "/JSON/user_achievements/";
        public abstract IList<Achievement> GetListOfExistingAchievement();
        public abstract IList<Achievement> GetListOfExistingUserAchievement(int _userid);
        public abstract void UpdateAchievement(int _achievementId, int _userId, int _progress);
    }
}
