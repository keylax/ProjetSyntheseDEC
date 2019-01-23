using System.IO;
using UnityEngine;

namespace Assets.Scripts.AchievementModule
{
    public class Achievement
    {
        public int id { get; set; }
        public string name { get; set; }
        public string objective { get; set; }
        public int points { get; set; }
        public string image { get; set; }
        public int progress { get; set; }

        public static float GetProgressById(int _id)
        {
            float progress = 0;
            foreach (Achievement achievement in ConnectedUser.connectedUser.UserAchievementsList)
            {
                if (achievement.id == _id)
                {
                    return ((float)((double)achievement.progress));
                }
            }
            return progress;
        }

        public static int GetIdByName(string _name)
        {
            int id = 0;
            foreach (Achievement achievement in ConnectedUser.connectedUser.UserAchievementsList)
            {
                string formattedName = achievement.name;
                formattedName = formattedName.Replace("\"", "");
                if (formattedName == _name)
                {
                    id = achievement.id;
                }
            }
            return id;
        }

        public static Achievement GetUserAchievementByName(string _name)
        {
            Achievement achievementToReturn = new Achievement();
            foreach (Achievement achievement in ConnectedUser.connectedUser.UserAchievementsList)
            {
                string formattedName = achievement.name;
                formattedName = formattedName.Replace("\"", "");
                if (formattedName == _name)
                {
                    achievementToReturn = achievement;
                }
            }
            return achievementToReturn;
        }

        public static Achievement GetAchievementByName(string _name)
        {
            Achievement achievementToReturn = new Achievement();
            foreach (Achievement achievement in GameParameters.AchievementsList)
            {
                string formattedName = achievement.name;
                formattedName = formattedName.Replace("\"", "");
                if (formattedName == _name)
                {
                    achievementToReturn = achievement;
                }
            }
            return achievementToReturn;
        }


        public static bool AchievementExistsInUserAchievement(string _name)
        {
            bool exists = false;
            foreach (Achievement achievement in ConnectedUser.connectedUser.UserAchievementsList)
            {
                string formattedName = achievement.name;
                formattedName = formattedName.Replace("\"", "");
                if (formattedName == _name)
                {
                    exists = true;
                }
            }
            return exists;
        }

        public static void SaveAchievementProgressToFile()
        {

            foreach (Achievement achievement in ConnectedUser.connectedUser.UserAchievementsList)
            {
                UpdateProgressInFile(achievement.id, achievement.progress);
            }

        }

        private static void UpdateProgressInFile(int _id, int _progress)
        {
            string path = Application.dataPath + "/JSON/user_achievements/" + _id + ".json";
            string jsonContent = File.ReadAllText(path);
            JSONObject achievement = new JSONObject(jsonContent);

            achievement.SetField("progress", _progress);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter file = File.CreateText(path))
            {
                file.Write(achievement.ToString());
                file.Flush();
            }
        }
    }
}
