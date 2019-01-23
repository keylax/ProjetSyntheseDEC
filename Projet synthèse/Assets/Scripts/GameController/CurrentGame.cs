using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.GameCharacter.AI;
using Assets.Scripts.GameObjectsBehavior;

namespace Assets.Scripts.GameController
{
    public static class CurrentGame
    {
        public enum GameType {QUICKMATCH, CUSTOMGAME}

        public static List<GameGoal> goals = new List<GameGoal>();
        public static GameObject lastBallPossessor;
        public static GameObject secondLastBallPossessor;
        public static GameObserver currentGameObserver;
        public static CurrentGameStatistics Statistics = new CurrentGameStatistics();
        public static AIPlayerBehaviour.Team? winningTeam = null;
        public static CurrentGame.GameType gameType;

        public static void ResetGame()
        {
            goals.Clear();
            lastBallPossessor = null;
            secondLastBallPossessor = null;
            Statistics = new CurrentGameStatistics();
        }

        public static int GetRedScore()
        {
            int score = 0;
            foreach (GameGoal goal in goals)
            {
                if (goal.team == AIPlayerBehaviour.Team.RED)
                {
                    score++;
                }
            }
            return score;
        }

        public static int GetBlueScore()
        {
            int score = 0;
            foreach (GameGoal goal in goals)
            {
                if (goal.team == AIPlayerBehaviour.Team.BLUE)
                {
                    score++;
                }
            }
            return score;
        }

        public static List<GameGoal> GetPlayer1Goals()
        {
            List<GameGoal> player1Goals = new List<GameGoal>();
            foreach (GameGoal goal in goals)
            {
                if (goal.team == AIPlayerBehaviour.Team.RED && goal.scorer.tag == ObjectTags.Player1Tag)
                {
                    player1Goals.Add(goal);
                }
            }
            return player1Goals;
        }

        public static List<GameGoal> GetPlayer1Points()
        {
            List<GameGoal> player1Points = new List<GameGoal>();
            foreach (GameGoal goal in goals)
            {
                if (goal.team == AIPlayerBehaviour.Team.RED && (goal.scorer.tag == ObjectTags.Player1Tag || (goal.assist != null && goal.assist.tag == ObjectTags.Player1Tag)))
                {
                    player1Points.Add(goal);
                }
            }
            return player1Points;
        }

        public static void AddStatsToDictionary(ref Dictionary<string,int> _gameStats )
        {
            _gameStats["Goals"] = GetPlayer1Goals().Count;
            _gameStats["Deaths"] = Statistics.NumberOfDeaths;
            _gameStats["Interceptions"] = Statistics.NumberOfInterceptions;
            _gameStats["Bonuses Aquired"] = Statistics.NumberOfBonusesAquired;
            _gameStats["Shots To Goal"] = Statistics.NumberOfShotsToGoal;
            _gameStats["Provoqued Drops"] = Statistics.NumberOfProvoquedDrops;
            _gameStats["Assists"] = Statistics.NumberOfAssists;
            _gameStats["Possession Time"] = (int)Statistics.PossessionTime;
            _gameStats["Missiles Shot"] = Statistics.NumberOfMissilesShot;
            _gameStats["Springs Used"] = Statistics.NumberOfJumpsFromSprings;
        } 
    }
}
