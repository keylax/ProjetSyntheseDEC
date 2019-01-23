using System.Collections;
using UnityEngine;
using System.Linq;
using Assets.Scripts.AchievementModule;
using Assets.Scripts.GameCharacter;
using Assets.Scripts.GameCharacter.AI;
using Assets.Scripts.GameObjectsBehavior;
using Assets.Scripts.Observer;
using UnityEngine.UI;

namespace Assets.Scripts.GameController
{
    public class AchievementsController: MonoBehaviour
    {

        public static GameObject uICanvas;

        private GameObject achievementUnlockVisual;
        private bool isAchievementVisualUp;
        
        private const float RAYCAST_DISTANCE = 3f;
        
        public void CheckAchievementProgression(Subject.NotifyReason _notifyReason)
        {
            switch (_notifyReason)
            {
                case Subject.NotifyReason.RED_GOAL:
                    CheckOhBabyATripleCompletion();
                    CheckWomboComboCompletion();
                    CheckStillPlayingCompletion();
                    CheckSlamDunkCompletion();
                    CheckThanksBroCompletion();
                    CheckMomGetTheCameraCompletion();
                    CheckMLGQuickScoperCompletion();
                    CheckPassiveAgressiveCompletion();
                    CheckYouDaRealMVPCompletion();
                    CheckProPlayerCompletion();
                    break;

                case Subject.NotifyReason.MISSILE_USED:
                    CheckExplosionManiacCompletion();
                    break;

                case Subject.NotifyReason.SPRING_USED:
                    CheckUse100SpringsCompletion();
                    break;

                case Subject.NotifyReason.GAME_OVER:
                    CheckFeelsGoodManCompletion();
                    CheckNewbiePhaseOverCompletion();
                    CheckWoWMuchGoodCompletion();
                    CheckTotalDominationCompletion();
                    CheckCompletionistCompletion();
                    CheckEpicComebackCompletion();
                    CheckStarPlayerCompletion();
                    break;

                case Subject.NotifyReason.SHOT_TO_GOAL:
                    CheckProPlayerCompletion();
                    CheckYouDaRealMVPCompletion();
                    break;

                case Subject.NotifyReason.FELL_IN_PIT:
                    CheckRipInPepperonisCompletion();
                    break;

                case Subject.NotifyReason.LIGHTNINGBOLT_USED:
                    CheckJusticeRainsFromAboveCompletion();
                    break;
            }
        }


        void CheckWoWMuchGoodCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("WowMuchGood"))
            {
                if (!IsAlreadyUnlocked("WowMuchGood"))
                {
                    if (ConnectedUser.connectedUser.totalNumberOfWins == 10)
                    {
                        UnlockAchievement("WowMuchGood");
                    }
                    else
                    {
                        UpdateProgress("WowMuchGood", 10);
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("WowMuchGood");
                CheckWoWMuchGoodCompletion();
            }

        }

        void CheckNewbiePhaseOverCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("NewbiePhaseOver"))
            {
                if (!IsAlreadyUnlocked("NewbiePhaseOver"))
                {
                    if (ConnectedUser.connectedUser.totalNumberOfGamePlayed == 10)
                    {
                        UnlockAchievement("NewbiePhaseOver");
                    }
                    else
                    {
                        UpdateProgress("NewbiePhaseOver", 10);
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("NewbiePhaseOver");
                CheckNewbiePhaseOverCompletion();
            }
        }

        void CheckStillPlayingCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("StillPlaying?"))
            {
                if (!IsAlreadyUnlocked("StillPlaying?"))
                {
                    if (ConnectedUser.connectedUser.totalNumberOfGoals + CurrentGame.GetPlayer1Goals().Count >= 500)
                    {
                        UnlockAchievement("StillPlaying?");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("StillPlaying?");
                CheckStillPlayingCompletion();
            }
        }

        void CheckExplosionManiacCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("ExplosionManiac"))
            {
                if (!IsAlreadyUnlocked("ExplosionManiac"))
                {
                    if (ConnectedUser.connectedUser.totalNumberOfMissilesShot +
                        CurrentGame.Statistics.NumberOfMissilesShot >= 100)
                    {
                        UnlockAchievement("ExplosionManiac");
                    }
                    else
                    {
                        UpdateProgress("ExplosionManiac", 1);
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("ExplosionManiac");
                CheckExplosionManiacCompletion();
            }
        }

        void CheckUse100SpringsCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("Use100Springs"))
            {
                if (!IsAlreadyUnlocked("Use100Springs"))
                {
                    if (ConnectedUser.connectedUser.totalNumberOfJumpsFromSprings +
                        CurrentGame.Statistics.NumberOfJumpsFromSprings >= 100)
                    {
                        UnlockAchievement("Use100Springs");
                    }
                    else
                    {
                        UpdateProgress("Use100Springs", 1);
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("Use100Springs");
                CheckUse100SpringsCompletion();
            }
        }

        void CheckRipInPepperonisCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("RipInPepperonis"))
            {
                if (!IsAlreadyUnlocked("RipInPepperonis"))
                {
                    UnlockAchievement("RipInPepperonis");
                }
            }
            else
            {
                AddAchievementToUserAchievements("RipInPepperonis");
                CheckRipInPepperonisCompletion();
            }
        }

        void CheckOhBabyATripleCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("OhBabyATriple"))
            {
                if (!IsAlreadyUnlocked("OhBabyATriple"))
                {
                    for (int i = 0; i < CurrentGame.GetPlayer1Goals().Count; i++)
                    {
                        if (i <= CurrentGame.GetPlayer1Goals().Count - 3)
                        {
                            if ((CurrentGame.GetPlayer1Goals()[i + 2].goalTimeStamp -
                                 CurrentGame.GetPlayer1Goals()[i].goalTimeStamp) < 300)
                            {
                                UnlockAchievement("OhBabyATriple");
                            }
                        }
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("OhBabyATriple");
                CheckOhBabyATripleCompletion();
            }
        }

        void CheckWomboComboCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("WomboCombo"))
            {
                if (!IsAlreadyUnlocked("WomboCombo"))
                {
                    for (int i = 0; i < CurrentGame.GetPlayer1Points().Count; i++)
                    {
                        if (i <= CurrentGame.GetPlayer1Points().Count - 5)
                        {
                            if ((CurrentGame.GetPlayer1Points()[i + 4].goalTimeStamp - CurrentGame.GetPlayer1Points()[i].goalTimeStamp) < 300)
                            {
                                UnlockAchievement("WomboCombo");
                            }
                        }
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("WomboCombo");
                CheckWomboComboCompletion();
            }
        }

        void CheckFeelsGoodManCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("FeelsGoodMan"))
            {
                if (!IsAlreadyUnlocked("FeelsGoodMan"))
                {
                    if (ConnectedUser.connectedUser.numberOfQuickMatchWonInARow == 4)
                    {
                        UnlockAchievement("FeelsGoodMan");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("FeelsGoodMan");
                CheckFeelsGoodManCompletion();
            }
        }

        void CheckTotalDominationCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("TotalDomination"))
            {
                if (!IsAlreadyUnlocked("TotalDomination"))
                {
                    if (CurrentGame.gameType == CurrentGame.GameType.QUICKMATCH && (CurrentGame.GetRedScore() - CurrentGame.GetBlueScore()) >= 5)
                    {
                        UnlockAchievement("TotalDomination");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("TotalDomination");
                CheckTotalDominationCompletion();
            }
        }

        void CheckSlamDunkCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("SlamDunk"))
            {
                if (!IsAlreadyUnlocked("SlamDunk"))
                {
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(new Vector3(CurrentGame.goals.Last().scorerPosition.x + 1.5f, CurrentGame.goals.Last().scorerPosition.y + 0.5f, CurrentGame.goals.Last().scorerPosition.z + 1.5f), Vector3.down, RAYCAST_DISTANCE);
                    bool wasHit = false;

                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.transform.gameObject.GetComponentInParent<Transform>().tag == ObjectTags.Environment)
                        {
                            wasHit = true;
                        }
                    }

                    if (!wasHit)
                    {
                        UnlockAchievement("SlamDunk");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("SlamDunk");
                CheckSlamDunkCompletion();
            }
        }

        void CheckCompletionistCompletion()
        {
            bool isCompleted = true;
            if (Achievement.AchievementExistsInUserAchievement("Completionist"))
            {
                if (!IsAlreadyUnlocked("Completionist"))
                {

                    if (!IsAlreadyUnlocked("OhBabyATriple")
                        || !IsAlreadyUnlocked("WomboCombo")
                        || !IsAlreadyUnlocked("NewbiePhaseOver")
                        || !IsAlreadyUnlocked("ExplosionManiac")
                        || !IsAlreadyUnlocked("TotalDomination"))
                    {
                        isCompleted = false;
                    }

                    if (isCompleted)
                    {
                        UnlockAchievement("Completionist");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("Completionist");
                CheckCompletionistCompletion();
            }
        }

        void CheckProPlayerCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("ProPlayer"))
            {
                if (!IsAlreadyUnlocked("ProPlayer"))
                {
                    if (CurrentGame.Statistics.NumberOfShotsToGoal == 15)
                    {
                        UnlockAchievement("ProPlayer");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("ProPlayer");
                CheckProPlayerCompletion();
            }
        }

        void CheckYouDaRealMVPCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("YouDaRealMVP"))
            {
                if (!IsAlreadyUnlocked("YouDaRealMVP"))
                {
                    if (ConnectedUser.connectedUser.totalNumberOfShotsToGoal + CurrentGame.Statistics.NumberOfShotsToGoal >= 1000)
                    {
                        UnlockAchievement("YouDaRealMVP");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("YouDaRealMVP");
                CheckYouDaRealMVPCompletion();
            }
        }

        void CheckThanksBroCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("ThanksBro"))
            {
                if (!IsAlreadyUnlocked("ThanksBro"))
                {
                    if (CurrentGame.goals.Last().goalTimeStamp - CurrentGame.Statistics.LastHitByEnemyBonus < 5f)
                    {
                        UnlockAchievement("ThanksBro");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("ThanksBro");
                CheckThanksBroCompletion();
            }
        }

        void CheckMomGetTheCameraCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("MomGetTheCamera"))
            {
                if (!IsAlreadyUnlocked("MomGetTheCamera"))
                {
                    RaycastHit[] hits;
                    hits = Physics.RaycastAll(new Vector3(CurrentGame.goals.Last().scorerPosition.x + 1.5f, CurrentGame.goals.Last().scorerPosition.y + 0.5f, CurrentGame.goals.Last().scorerPosition.z + 1.5f), Vector3.down, RAYCAST_DISTANCE);
                    bool wasHit = false;

                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.transform.gameObject.GetComponentInParent<Transform>().tag == ObjectTags.EndlessHole)
                        {
                            wasHit = true;
                        }
                    }

                    if (wasHit)
                    {
                        UnlockAchievement("MomGetTheCamera");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("MomGetTheCamera");
                CheckMomGetTheCameraCompletion();
            }
        }

        void CheckMLGQuickScoperCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("MLGQuickScoper"))
            {
                if (!IsAlreadyUnlocked("MLGQuickScoper"))
                {
                    if (CurrentGame.goals.Last().scorer.tag == ObjectTags.Player1Tag)
                    {
                        if (GetCurrentGameTime() > GetGameLength() - 5)
                        {
                            UnlockAchievement("MLGQuickScoper");
                        }
                    }
                    else if (CurrentGame.goals.Last().assist != null)
                    {
                        if (CurrentGame.goals.Last().assist.tag == ObjectTags.Player1Tag)
                        {
                            if (GetCurrentGameTime() > GetGameLength() - 5)
                            {
                                UnlockAchievement("MLGQuickScoper");
                            }
                        }
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("MLGQuickScoper");
                CheckMLGQuickScoperCompletion();
            }
        }

        float GetCurrentGameTime()
        {
            return GamestatesController.scoreBoard.transform.GetChild(6).GetComponent<TimerCountdown>().timeInSeconds;
        }

        float GetGameLength()
        {
            return GamestatesController.scoreBoard.transform.GetChild(6).GetComponent<TimerCountdown>().startingTimeInSeconds;
        }

        void CheckEpicComebackCompletion()
        {
            int goalsBehind = 0;
            bool wasBehindBy3Goals = false;
            if (Achievement.AchievementExistsInUserAchievement("EpicComeback"))
            {
                if (!IsAlreadyUnlocked("EpicComeback"))
                {
                    foreach (GameGoal goal in CurrentGame.goals)
                    {
                        if (goal.team == AIPlayerBehaviour.Team.BLUE)
                        {
                            goalsBehind++;
                        }
                        else
                        {
                            goalsBehind--;
                        }

                        if (goalsBehind == 3)
                        {
                            wasBehindBy3Goals = true;
                        }
                    }

                    if (wasBehindBy3Goals && CurrentGame.winningTeam == AIPlayerBehaviour.Team.RED)
                    {
                        UnlockAchievement("EpicComeback");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("EpicComeback");
                CheckEpicComebackCompletion();
            }
        }

        void CheckJusticeRainsFromAboveCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("JusticeRainsFromAbove"))
            {
                if (!IsAlreadyUnlocked("JusticeRainsFromAbove"))
                {
                    if (CurrentGame.Statistics.NumberOfLightningBoltUsed == 5)
                    {
                        UnlockAchievement("JusticeRainsFromAbove");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("JusticeRainsFromAbove");
                CheckJusticeRainsFromAboveCompletion();
            }
        }

        void CheckStarPlayerCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("StarPlayer"))
            {
                if (!IsAlreadyUnlocked("StarPlayer"))
                {
                    if (CurrentGame.Statistics.PossessionTime + ConnectedUser.connectedUser.totalPossessionTime >= 3600)
                    {
                        UnlockAchievement("StarPlayer");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("StarPlayer");
                CheckStarPlayerCompletion();
            }
        }

        void CheckPassiveAgressiveCompletion()
        {
            if (Achievement.AchievementExistsInUserAchievement("PassiveAgressive"))
            {
                if (!IsAlreadyUnlocked("PassiveAgressive"))
                {
                    if (CurrentGame.goals.Last().scorer.tag == ObjectTags.Player1Tag && CurrentGame.goals.Last().scorer.transform.GetChild(4).GetComponent<MagnetManager>().IsMagnetPulling())
                    {
                        UnlockAchievement("PassiveAgressive");
                    }
                }
            }
            else
            {
                AddAchievementToUserAchievements("PassiveAgressive");
                CheckPassiveAgressiveCompletion();
            }
        }

        private void UnlockAchievement(string _achievementName)
        {
            Achievement.GetUserAchievementByName(_achievementName).progress = 100;
            DisplayAchievementUnlockOnUI(_achievementName);
        }

        private void UpdateProgress(string _achievementName, int _progresstoAdd)
        {
            Achievement.GetUserAchievementByName(_achievementName).progress += _progresstoAdd;
        }

        private void AddAchievementToUserAchievements(string _achievementName)
        {
            Achievement newUserAchievement = new Achievement();
            newUserAchievement = Achievement.GetAchievementByName(_achievementName);
            ConnectedUser.connectedUser.UserAchievementsList.Add(newUserAchievement);
        }

        private bool IsAlreadyUnlocked(string _achievementName)
        {
            return (Achievement.GetUserAchievementByName(_achievementName).progress) >= 100;
        }


        private void DisplayAchievementUnlockOnUI(string _achievementName)
        {
            if (!isAchievementVisualUp)
            {
                isAchievementVisualUp = true;
                achievementUnlockVisual = uICanvas.transform.GetChild(5).gameObject;
                achievementUnlockVisual.SetActive(true);
                achievementUnlockVisual.transform.GetChild(0).GetComponent<Text>().text = _achievementName;
                GUITween.MoveFrom(achievementUnlockVisual, GUITween.Hash("position", new Vector3(achievementUnlockVisual.GetComponent<RectTransform>().localPosition.x + Screen.width, achievementUnlockVisual.GetComponent<RectTransform>().localPosition.y, achievementUnlockVisual.GetComponent<RectTransform>().localPosition.z), "islocal", true, "time", 2.0f / 1, "delay", 0, "ignoretimescale", false));
                StartCoroutine(HideAchievementUnlockAfterDelay());
            }
        }

        IEnumerator HideAchievementUnlockAfterDelay()
        {
            yield return new WaitForSeconds(5);
            achievementUnlockVisual.SetActive(false);
            isAchievementVisualUp = false;
        } 

    }

}