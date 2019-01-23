﻿using System.Collections.Generic;
﻿using Assets.Scripts.GameCharacter;
﻿using UnityEngine;
using Assets.Scripts.GameCharacter.AI;
﻿using Assets.Scripts.GameCharacter.HumanPlayer;
﻿using Assets.Scripts.GameObjectsBehavior;
﻿using Assets.Scripts.Projectiles;
﻿using UnityEngine.UI;

namespace Assets.Scripts.GameController
{
    public class StartGame : MonoBehaviour
    {
        public int numberOfPlayers = 1;
        public GameObject scoreBoard;
        public GameObject playerList;
        public List<GameObject> shipPrefabsList;
        public GameObject missileList;
        public GameObject springList;
        public GameObject homingMissileList;
        public GameObject aIPrefab;
        public GameObject spawnPointList;
        public GameObject uICanvas;
        public GameObject ball;
        public GameObject redStrategicPoint;
        public GameObject blueStrategicPoint;
        public GameObject blueGoal;
        public GameObject redGoal;
        public GameObject thunderBoltList;

        private void Start()
        {
            GamestatesController.ball = ball;
            MagnetManager.ballBehaviour = ball.GetComponent<BallBehaviour>();
            GameObserver currentGameObserver = new GameObserver();
            CurrentGame.currentGameObserver = currentGameObserver;

            //remove when all UI is done
            if (GameParameters.numberOfPlayers != 0)
            {
                numberOfPlayers = GameParameters.numberOfPlayers;
            }

            AchievementsController.uICanvas = uICanvas;
            uICanvas.transform.GetChild(numberOfPlayers - 1).gameObject.SetActive(true);
            CreatePlayerList();
            SplitScreen();
            AddPlayerTags();
            GamestatesController.scoreBoard = scoreBoard;
            GamestatesController.playerList = playerList;
            GamestatesController.blueGoal = blueGoal;
            GamestatesController.redGoal = redGoal;
            GamestatesController.ChangeGameState(GamestatesController.GameStates.FACEOFF);
        }

        private void CreatePlayerList()
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (numberOfPlayers == 4 && i == 2)
                {
                    numberOfPlayers = 5;
                    AddAIPlayer(i);
                }
                else
                {
                    AddHumanPlayer(i);
                }
            }
            if (numberOfPlayers == 5)
            {
                numberOfPlayers = 4;
            }

            for (int i = playerList.transform.childCount; i < 6; i++)
            {
                AddAIPlayer(i);
            }
        }

        private void SplitScreen()
        {
            if (numberOfPlayers == 2)
            {
                playerList.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect(0.0f,
                    0.5f, 1f, 0.5f);
                playerList.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect(0.0f,
                    0.0f, 1f, 0.5f);
            }
            else if (numberOfPlayers == 3)
            {
                playerList.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect(0.0f,
                    0.5f, 1f, 0.5f);
                playerList.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect(0.0f,
                    0.0f, 0.5f, 0.5f);
                playerList.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect(0.5f,
                    0.0f, 0.5f, 0.5f);
            }
            else if (numberOfPlayers == 4)
            {
                playerList.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect(0.0f,
                    0.5f, 0.5f, 0.5f);
                playerList.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect(0.5f,
                    0.5f, 0.5f, 0.5f);
                playerList.transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect(0.0f,
                    0.0f, 0.5f, 0.5f);
                playerList.transform.GetChild(4).GetChild(0).GetChild(0).GetComponent<Camera>().rect = new Rect(0.5f,
                    0.0f, 0.5f, 0.5f);
            }
        }

        private AIPlayerBehaviour.Team GetTeam(int _i)
        {
            if (_i > 2)
            {
                return AIPlayerBehaviour.Team.BLUE;
            }
            else
            {
                return AIPlayerBehaviour.Team.RED;
            }
        }

        private GameObject AssignSpawn(int i)
        {
            return spawnPointList.transform.GetChild(i).gameObject;
        }

        private GameObject AssignBonusBox(int i)
        {
            if (numberOfPlayers == 5)
            {
                return uICanvas.transform.GetChild(numberOfPlayers - 2).GetChild(i - 1).GetChild(1).GetChild(0).gameObject;
            }
            return uICanvas.transform.GetChild(numberOfPlayers - 1).GetChild(i).GetChild(1).GetChild(0).gameObject;
        }

        private void AssignTeamBasedObjectives(AIPlayerBehaviour _ai)
        {
            if (_ai.GetTeam() == AIPlayerBehaviour.Team.BLUE)
            {
                _ai.targetGoal = redGoal.transform;
                _ai.protectedGoal = blueGoal.transform;
                _ai.strategicOffensivePointList = blueStrategicPoint.transform;
                _ai.strategicDefensivePointList = redStrategicPoint.transform;
                _ai.transform.GetComponent<MeshRenderer>().material = Resources.Load("BlueShip") as Material;
            }
            else
            {
                _ai.targetGoal = blueGoal.transform;
                _ai.protectedGoal = redGoal.transform;
                _ai.strategicOffensivePointList = redStrategicPoint.transform;
                _ai.strategicDefensivePointList = blueStrategicPoint.transform;
                _ai.transform.GetComponent<MeshRenderer>().material = Resources.Load("RedShip") as Material;
            }
        }

        private void AssignTeamBasedObjectives(HumanPlayerBehaviour _human)
        {
            if (_human.GetTeam() == AIPlayerBehaviour.Team.BLUE)
            {
                _human.targetGoal = redGoal.transform;
                _human.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material =
                    Resources.Load("BlueShip") as Material;
            }
            else
            {
                _human.targetGoal = blueGoal.transform;
                _human.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material =
                    Resources.Load("RedShip") as Material;
            }
        }

        private void AddPlayerTags()
        {
            playerList.transform.GetChild(0).tag = ObjectTags.Player1Tag;
            if (numberOfPlayers > 1)
            {
                playerList.transform.GetChild(1).tag = ObjectTags.Player2Tag;
                if (numberOfPlayers > 2)
                {
                    playerList.transform.GetChild(2).tag = ObjectTags.Player3Tag;
                    if (numberOfPlayers > 3)
                    {
                        playerList.transform.GetChild(2).tag = ObjectTags.AITag;
                        playerList.transform.GetChild(3).tag = ObjectTags.Player3Tag;
                        playerList.transform.GetChild(4).tag = ObjectTags.Player4Tag;
                    }
                }
            }

            foreach (Transform child in playerList.transform)
            {
                if (child.tag == ObjectTags.Player1Tag)
                {
                    child.GetChild(2).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player1BallIndicator");
                    child.GetChild(3).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player1");
                }
                else if (child.tag == ObjectTags.Player2Tag)
                {
                    child.GetChild(2).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player2BallIndicator");
                    child.GetChild(3).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player2");
                }
                else if (child.tag == ObjectTags.Player3Tag)
                {
                    child.GetChild(2).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player3BallIndicator");
                    child.GetChild(3).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player3");
                }
                else if (child.tag == ObjectTags.Player4Tag)
                {
                    child.GetChild(2).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player4BallIndicator");
                    child.GetChild(3).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player4");
                }
            }
        }

        private Material ChangeShipColor(AIPlayerBehaviour.Team _teamColor)
        {
            if (_teamColor == AIPlayerBehaviour.Team.BLUE)
            {
                return Resources.Load("BlueShip") as Material;
            }
            else
            {
                return Resources.Load("RedShip") as Material;
            }
        }

        private void AddHumanPlayer(int _index)
        {
            GameObject humanToAdd;
            if (GameParameters.playersSelectedModelIndex == null)
            {
                humanToAdd = Instantiate(shipPrefabsList[Random.Range(0, 3)]);
            }
            else
            {
                humanToAdd = Instantiate(shipPrefabsList[GameParameters.playersSelectedModelIndex[_index]]);
            }

            humanToAdd.transform.GetComponent<BonusShooting>().missileList = missileList;
            humanToAdd.transform.GetComponent<BonusShooting>().springList = springList;
            humanToAdd.transform.GetComponent<BonusShooting>().homingMissileList = homingMissileList;
            humanToAdd.transform.GetComponent<HumanPlayerBehaviour>().team = GetTeam(_index);
            humanToAdd.transform.GetComponent<PickUpBonus>().scoreBoard = scoreBoard;
            humanToAdd.transform.GetComponent<PlayerMovement>().playerSpawn = AssignSpawn(_index);
            humanToAdd.transform.GetComponent<PickUpBonus>().bonusImage = AssignBonusBox(_index);
            humanToAdd.transform.GetChild(2).GetComponent<BallPositionIndicator>().ball = ball.transform;
            humanToAdd.transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material = ChangeShipColor(GetTeam(_index));
            humanToAdd.transform.parent = playerList.transform;
            humanToAdd.transform.GetComponent<HumanPlayerBehaviour>().ball = ball.transform;
            if (numberOfPlayers == 5)
            {
                humanToAdd.transform.GetComponent<HumanPlayerBehaviour>().polarityText = uICanvas.transform.GetChild(numberOfPlayers - 2).GetChild(_index - 1).GetChild(2).GetChild(1).gameObject.GetComponent<Text>();
            }
            else
            {
                humanToAdd.transform.GetComponent<HumanPlayerBehaviour>().polarityText = uICanvas.transform.GetChild(numberOfPlayers - 1).GetChild(_index).GetChild(2).GetChild(1).gameObject.GetComponent<Text>();
            }
            humanToAdd.transform.GetComponent<VisualEffects>().thunderBoltVisual = thunderBoltList.transform.GetChild(_index).gameObject;
            AssignTeamBasedObjectives(humanToAdd.GetComponent<HumanPlayerBehaviour>());
        }

        private void AddAIPlayer(int _index)
        {
            GameObject AIToAdd;
            AIToAdd = Instantiate(aIPrefab);
            AIToAdd.transform.GetComponent<BonusShooting>().missileList = missileList;
            AIToAdd.transform.GetComponent<BonusShooting>().springList = springList;
            AIToAdd.transform.GetComponent<BonusShooting>().homingMissileList = homingMissileList;
            AIToAdd.GetComponent<AIPlayerBehaviour>().team = GetTeam(_index);
            AIToAdd.transform.GetComponent<AIPickUpBonus>().scoreBoard = scoreBoard;
            AIToAdd.GetComponent<AIPlayerBehaviour>().ball = ball.transform;
            AssignTeamBasedObjectives(AIToAdd.GetComponent<AIPlayerBehaviour>());
            AIToAdd.GetComponent<AIPlayerBehaviour>().spawnPoint = AssignSpawn(_index);
            AIToAdd.transform.parent = playerList.transform;
            AIToAdd.GetComponent<AIPlayerBehaviour>().distanceDifferenceNeededForPass = 20f;
            AIToAdd.GetComponent<AIPlayerBehaviour>().estimatedThrowRange = 20f;
            AIToAdd.GetComponent<VisualEffects>().thunderBoltVisual = thunderBoltList.transform.GetChild(_index).gameObject;

        }

    }
}