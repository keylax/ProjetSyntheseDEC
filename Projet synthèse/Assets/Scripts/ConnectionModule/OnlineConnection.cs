using Assets.Scripts.AchievementModule;
using Assets.Scripts.QueryStack;

namespace Assets.Scripts.ConnectionModule
{
    public class OnlineConnection : ConnectionModule
    {
        public override string Connect(string _userName, string _password)
        {
            RestClientConnection rest = new RestClientConnection();
            OnlineStack onlineStack;
            AchievementModule.AchievementModule achievementModule = new OnlineAchievement();
            string responce = rest.TryLogin(_userName, _password, ConnectionModule.HttpResquest.POST);

            if (responce == ConnectedUser.LOGIN_SUCCESFUL)
            {
                onlineStack = new OnlineStack();
                GameParameters.ArenaList = rest.GetAllArenaOnDB(ConnectionModule.HttpResquest.POST);
                GameParameters.AchievementsList = achievementModule.GetListOfExistingAchievement();
                ConnectedUser.connectedUser.UserAchievementsList = achievementModule.GetListOfExistingUserAchievement(ConnectedUser.connectedUser.id);
                onlineStack.EmptyStack();
                return responce;
            }
            return responce;
        }
    }

}


