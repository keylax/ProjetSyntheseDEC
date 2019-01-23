using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GameStatisticsModule
{
    abstract class GameStatisticsModule
    {
        public abstract void UpdateUserStats(int _win, int _loss, string _userName);
        public abstract void UpdateGame(string _winningTeam, int _arenaId, ConnectionModule.ConnectionModule.HttpResquest httpResquestType);
        public abstract void UpdateGameStats(Dictionary<string, int> _playerOverallStat, int _userId);
        public abstract Dictionary<string, int> GetOverallStats(int _userId);
    }

}
