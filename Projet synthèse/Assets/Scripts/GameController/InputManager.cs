using Assets.Scripts.GameCharacter.HumanPlayer;
using Assets.Scripts.GameObjectsBehavior;
using UnityEngine;

namespace Assets.Scripts.GameController
{
    public static class InputManager
    {
        private const string player1Vertical = "Player1Vertical";
        private const string player2Vertical = "Player2Vertical";
        private const string player3Vertical = "Player3Vertical";
        private const string player4Vertical = "Player4Vertical";

        private const string player1Horizontal = "Player1Horizontal";
        private const string player2Horizontal = "Player2Horizontal";
        private const string player3Horizontal = "Player3Horizontal";
        private const string player4Horizontal = "Player4Horizontal";

        private const string player1Steering = "Player1Rotation";
        private const string player2Steering = "Player2Rotation";
        private const string player3Steering = "Player3Rotation";
        private const string player4Steering = "Player4Rotation";

        private const string player1Aiming = "Player1Aiming";
        private const string player2Aiming = "Player2Aiming";
        private const string player3Aiming = "Player3Aiming";
        private const string player4Aiming = "Player4Aiming";

        private const string player1Fire = "Player1Fire";
        private const string player2Fire = "Player2Fire";
        private const string player3Fire = "Player3Fire";
        private const string player4Fire = "Player4Fire";

        private const string player1ChangeCamera = "Player1Camera";
        private const string player2ChangeCamera = "Player2Camera";
        private const string player3ChangeCamera = "Player3Camera";
        private const string player4ChangeCamera = "Player4Camera";

        private const string player1ChangePolarity = "Player1Polarity";
        private const string player2ChangePolarity = "Player2Polarity";
        private const string player3ChangePolarity = "Player3Polarity";
        private const string player4ChangePolarity = "Player4Polarity";

        private const string player1Magnet = "Player1Magnet";
        private const string player2Magnet = "Player2Magnet";
        private const string player3Magnet = "Player3Magnet";
        private const string player4Magnet = "Player4Magnet";

        private const string player1Start = "Player1Start";
        private const string player2Start = "Player2Start";
        private const string player3Start = "Player3Start";
        private const string player4Start = "Player4Start";

        private const string player1Submit = "Player1Submit";
        private const string player2Submit = "Player2Submit";
        private const string player3Submit = "Player3Submit";
        private const string player4Submit = "Player4Submit";

        private const string player1Back = "Player1B";
        private const string player2Back = "Player2B";
        private const string player3Back = "Player3B";
        private const string player4Back = "Player4B";

        private const string menuHorizontal = "Horizontal";
        private const string menuVertical = "Vertical";

        public static float GetPlayerHorizontal(string _tag)
        {
            float axis = 0;
            if (GamestatesController.currentState != GamestatesController.GameStates.IN_MENUS && GameObject.FindGameObjectWithTag(_tag).GetComponent<StunPlayer>().stunned)
            {
                return 0;
            }

            if (GamestatesController.currentState != GamestatesController.GameStates.FACEOFF && GamestatesController.currentState != GamestatesController.GameStates.GAMEOVER)
            {
                if (_tag == ObjectTags.Player1Tag)
                {
                    axis = Input.GetAxis(player1Horizontal);
                }
                else if (_tag == ObjectTags.Player2Tag)
                {
                    axis = Input.GetAxis(player2Horizontal);
                }
                else if (_tag == ObjectTags.Player3Tag)
                {
                    axis = Input.GetAxis(player3Horizontal);
                }
                else
                {
                    axis = Input.GetAxis(player4Horizontal);
                }
            }
            return axis;
        }

        public static float GetPlayerVertical(string _tag)
        {
            float axis = 0;

            if (GamestatesController.currentState != GamestatesController.GameStates.IN_MENUS && GameObject.FindGameObjectWithTag(_tag).GetComponent<StunPlayer>().stunned)
            {
                return 0;
            }
            if (GamestatesController.currentState != GamestatesController.GameStates.FACEOFF && GamestatesController.currentState != GamestatesController.GameStates.GAMEOVER)
            {
                if (_tag == ObjectTags.Player1Tag)
                {
                    axis = Input.GetAxis(player1Vertical);
                }
                else if (_tag == ObjectTags.Player2Tag)
                {
                    axis = Input.GetAxis(player2Vertical);
                }
                else if (_tag == ObjectTags.Player3Tag)
                {
                    axis = Input.GetAxis(player3Vertical);
                }
                else
                {
                    axis = Input.GetAxis(player4Vertical);
                }
            }
            return axis;
        }

        public static float GetPlayerSteering(string _tag)
        {
            float axis = 0;

            if (GamestatesController.currentState != GamestatesController.GameStates.IN_MENUS && GameObject.FindGameObjectWithTag(_tag).GetComponent<StunPlayer>().stunned)
            {
                return 0;
            }
            if (GamestatesController.currentState != GamestatesController.GameStates.FACEOFF && GamestatesController.currentState != GamestatesController.GameStates.GAMEOVER)
            {
                if (_tag == ObjectTags.Player1Tag)
                {
                    axis = Input.GetAxis(player1Steering);
                }
                else if (_tag == ObjectTags.Player2Tag)
                {
                    axis = Input.GetAxis(player2Steering);
                }
                else if (_tag == ObjectTags.Player3Tag)
                {
                    axis = Input.GetAxis(player3Steering);
                }
                else
                {
                    axis = Input.GetAxis(player4Steering);
                }
            }

            return axis;
        }

        public static float GetPlayerAimAxis(string _tag)
        {
            float axis = 0;

            if (GamestatesController.currentState != GamestatesController.GameStates.IN_MENUS && GameObject.FindGameObjectWithTag(_tag).GetComponent<StunPlayer>().stunned)
            {
                return 0;
            }
            if (GamestatesController.currentState != GamestatesController.GameStates.FACEOFF && GamestatesController.currentState != GamestatesController.GameStates.GAMEOVER)
            {
                if (_tag == ObjectTags.Player1Tag)
                {
                    axis = Input.GetAxis(player1Aiming);
                }
                else if (_tag == ObjectTags.Player2Tag)
                {
                    axis = Input.GetAxis(player2Aiming);
                }
                else if (_tag == ObjectTags.Player3Tag)
                {
                    axis = Input.GetAxis(player3Aiming);
                }
                else
                {
                    axis = Input.GetAxis(player4Aiming);
                }
            }
            return axis;
        }

        public static bool GetPlayerFire(string _tag)
        {
            bool buttonDown = false;

            if (GamestatesController.currentState != GamestatesController.GameStates.IN_MENUS && GameObject.FindGameObjectWithTag(_tag).GetComponent<StunPlayer>().stunned)
            {
                return false;
            }
            if (GamestatesController.currentState != GamestatesController.GameStates.FACEOFF && GamestatesController.currentState != GamestatesController.GameStates.GAMEOVER)
            {
                if (_tag == ObjectTags.Player1Tag)
                {
                    buttonDown = Input.GetButtonDown(player1Fire);
                }
                else if (_tag == ObjectTags.Player2Tag)
                {
                    buttonDown = Input.GetButtonDown(player2Fire);
                }
                else if (_tag == ObjectTags.Player3Tag)
                {
                    buttonDown = Input.GetButtonDown(player3Fire);
                }
                else
                {
                    buttonDown = Input.GetButtonDown(player4Fire);
                }
            }
            return buttonDown;
        }

        public static bool GetPlayerChangeCamera(string _tag)
        {
            bool buttonDown = false;
            if (GamestatesController.currentState != GamestatesController.GameStates.FACEOFF && GamestatesController.currentState != GamestatesController.GameStates.GAMEOVER)
            {
                if (_tag == ObjectTags.Player1Tag)
                {
                    buttonDown = Input.GetButtonDown(player1ChangeCamera);
                }
                else if (_tag == ObjectTags.Player2Tag)
                {
                    buttonDown = Input.GetButtonDown(player2ChangeCamera);
                }
                else if (_tag == ObjectTags.Player3Tag)
                {
                    buttonDown = Input.GetButtonDown(player3ChangeCamera);
                }
                else
                {
                    buttonDown = Input.GetButtonDown(player4ChangeCamera);
                }
            }
            return buttonDown;
        }

        public static bool GetPlayerChangePolarity(string _tag)
        {
            bool buttonDown = false;
            if (GamestatesController.currentState != GamestatesController.GameStates.IN_MENUS && GameObject.FindGameObjectWithTag(_tag).GetComponent<StunPlayer>().stunned)
            {
                return false;
            }
            if (GamestatesController.currentState != GamestatesController.GameStates.FACEOFF && GamestatesController.currentState != GamestatesController.GameStates.GAMEOVER)
            {
                if (_tag == ObjectTags.Player1Tag)
                {
                    buttonDown = Input.GetAxis(player1ChangePolarity) == 1;
                }
                else if (_tag == ObjectTags.Player2Tag)
                {
                    buttonDown = Input.GetAxis(player2ChangePolarity) == 1;
                }
                else if (_tag == ObjectTags.Player3Tag)
                {
                    buttonDown = Input.GetAxis(player3ChangePolarity) == 1;
                }
                else
                {
                    buttonDown = Input.GetAxis(player4ChangePolarity) == 1;
                }
            }
            return buttonDown;
        }

        public static bool GetPlayerStart(string _tag)
        {
            if (_tag == ObjectTags.Player1Tag)
            {
                return Input.GetButtonDown(player1Start);
            }
            else if (_tag == ObjectTags.Player2Tag)
            {
                return Input.GetButtonDown(player2Start);
            }
            else if (_tag == ObjectTags.Player3Tag)
            {
                return Input.GetButtonDown(player3Start);
            }
            else
            {
                return Input.GetButtonDown(player4Start);
            }
        }

        public static bool GetPlayerSubmit(string _tag)
        {
            if (_tag == ObjectTags.Player1Tag)
            {
                return Input.GetButtonDown(player1Submit);
            }
            else if (_tag == ObjectTags.Player2Tag)
            {
                return Input.GetButtonDown(player2Submit);
            }
            else if (_tag == ObjectTags.Player3Tag)
            {
                return Input.GetButtonDown(player3Submit);
            }
            else
            {
                return Input.GetButtonDown(player4Submit);
            }
        }

        public static bool GetPlayerBack(string _tag)
        {
            if (_tag == ObjectTags.Player1Tag)
            {
                return Input.GetButtonDown(player1Back);
            }
            else if (_tag == ObjectTags.Player2Tag)
            {
                return Input.GetButtonDown(player2Back);
            }
            else if (_tag == ObjectTags.Player3Tag)
            {
                return Input.GetButtonDown(player3Back);
            }
            else
            {
                return Input.GetButtonDown(player4Back);
            }
        }

        public static float GetMenuVertical()
        {
            return Input.GetAxis(menuVertical);
        }

        public static float GetMenuHorizontal()
        {
            return Input.GetAxis(menuHorizontal);
        }

        public static float GetPlayerMagnetForce(string _tag)
        {
            if (GamestatesController.currentState == GamestatesController.GameStates.IN_MENUS && GameObject.FindGameObjectWithTag(_tag).GetComponent<StunPlayer>().stunned)
            {
                return 0;
            }

            if (GamestatesController.currentState == GamestatesController.GameStates.FACEOFF ||
                GamestatesController.currentState == GamestatesController.GameStates.GAMEOVER)
                return 0f;

            if (_tag == ObjectTags.Player1Tag)
            {
                return Input.GetAxis(player1Magnet);
            }
            else if (_tag == ObjectTags.Player2Tag)
            {
                return Input.GetAxis(player2Magnet);
            }
            else if (_tag == ObjectTags.Player3Tag)
            {
                return Input.GetAxis(player3Magnet);
            }
            else
            {
                return Input.GetAxis(player4Magnet);
            }
        }

    }
}