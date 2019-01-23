using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AchievementModule
{
    class OnlineAchievement : AchievementModule
    {
        private const string GET_ALL_ACHIEVEMENTS_API = "getAllAchievements.php";
        private const string GET_ALL_USER_ACHIEVEMENTS_API = "AchievementsApi.php";
        private const string UPDATE_ACHIEVEMENT = "updateAchievementsApi.php";

        RestClientAchievement restClientAchievement = new RestClientAchievement();

        public override IList<Achievement> GetListOfExistingAchievement()
        {
            return restClientAchievement.GetAchievementsFromDB(GET_ALL_ACHIEVEMENTS_API, -1, ConnectionModule.ConnectionModule.HttpResquest.POST);
        }

        public override IList<Achievement> GetListOfExistingUserAchievement(int _userid)
        {
            return restClientAchievement.GetAchievementsFromDB(GET_ALL_USER_ACHIEVEMENTS_API, _userid, ConnectionModule.ConnectionModule.HttpResquest.POST);
        }

        public override void UpdateAchievement(int _achievementId, int _userId, int _progress)
        {
            JSONObject updateAchievement = restClientAchievement.CreateAchievementJsonObject(_achievementId, _userId, _progress);
            restClientAchievement.UpdateDB(updateAchievement, UPDATE_ACHIEVEMENT, ConnectionModule.ConnectionModule.HttpResquest.POST);
        }
    }
}
