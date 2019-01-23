using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.GameObjectsBehavior;
using Assets.Scripts.Game_Objects;

public class PrefabFactory : MonoBehaviour
{
    public GameObject Ball;
    public GameObject BonusSpawn;
    public GameObject EndlessHole;
    public GameObject Floor;
    public GameObject Goal;
    public GameObject PlayerSpawn;
    public GameObject Ramps;
    public GameObject UnaccessibleBloc;

    public void CreateMap(Map _map)
    {
        FloorFactory floorFactory = new FloorFactory();
        UnaccessibleBlocFactory unaccessibleBlocFactory = new UnaccessibleBlocFactory();
        GoalFactory goalFactory = new GoalFactory();
        RampsFactory rampsFactory = new RampsFactory();
        BallFactory ballFactory = new BallFactory();
        BonusSpawnFactory bonusSpawnFactory = new BonusSpawnFactory();
        PlayerSpawnFactory playerSpawnFactory = new PlayerSpawnFactory();
        EndlessHoleFactory endlessHoleFactory = new EndlessHoleFactory();

        int currentFloor = 0;
        Vector3 position;
        foreach (List<Tile> level in _map.ActiveMap)
        {
            for (int i = 0; i < _map.Width; ++i)
            {
                for (int j = 0; j < _map.Height; ++j)
                {

                    position = new Vector3(i * 3, currentFloor * 3, j * 3);

                    if (level[_map.Height * i + j] is TileFloor)
                    {
                        floorFactory.InsatanciateGameObject(Floor, level[_map.Height * i + j], position);
                    }
                    if (level[_map.Height * i + j] is TileUnpassableTerrain)
                    {
                        unaccessibleBlocFactory.InsatanciateGameObject(UnaccessibleBloc, level[_map.Height * i + j], position);
                    }
                    if (level[_map.Height * i + j] is TileGoal)
                    {
                        goalFactory.InsatanciateGameObject(Goal, level[_map.Height * i + j], position);
                    }
                    if (level[_map.Height * i + j] is TileRamp)
                    {
                        rampsFactory.InsatanciateGameObject(Ramps, level[_map.Height * i + j], position);
                    }
                    if (level[_map.Height * i + j] is TileBallSpawn)
                    {
                        ballFactory.InsatanciateGameObject(Ball, level[_map.Height * i + j], position);
                    }
                    if (level[_map.Height * i + j] is TileBonus)
                    {
                        bonusSpawnFactory.InsatanciateGameObject(BonusSpawn, level[_map.Height * i + j], position);
                    }
                    if (level[_map.Height * i + j] is TilePlayerSpawn)
                    {
                        playerSpawnFactory.InsatanciateGameObject(PlayerSpawn, level[_map.Height * i + j], position);
                    }
                    if (level[_map.Height * i + j] is TileBottomLessPit)
                    {
                        endlessHoleFactory.InsatanciateGameObject(EndlessHole, level[_map.Height * i + j], position);
                    }

                }
            }
            currentFloor++;
        }
    }

    private interface IPrefabFactory
    {
        void InsatanciateGameObject(GameObject _gameObject, Tile _tile, Vector3 _position);
    }

    private class FloorFactory : IPrefabFactory
    {
        private int nbOfInsatanciation = 1;
        private string nameOfObject = "Floor ";

        public void InsatanciateGameObject(GameObject _gameObject, Tile _tile, Vector3 _position)
        {
            GameObject newInstance = (GameObject)Instantiate(_gameObject, _position, Quaternion.identity);
            if (newInstance.GetComponent<ObjectBehavior>() != null)
            {
                newInstance.GetComponent<ObjectBehavior>().SetStartPosition(_position);
            }
            //Name the new object
            newInstance.name = nameOfObject + nbOfInsatanciation;
            nbOfInsatanciation++;
        }
    }

    private class UnaccessibleBlocFactory : IPrefabFactory
    {
        private int nbOfInsatanciation = 1;
        private string nameOfObject = "Unaccessible Bloc ";

        public void InsatanciateGameObject(GameObject _gameObject, Tile _tile, Vector3 _position)
        {
            GameObject newInstance = (GameObject)Instantiate(_gameObject, _position, Quaternion.identity);
            if (newInstance.GetComponent<ObjectBehavior>() != null)
            {
                newInstance.GetComponent<ObjectBehavior>().SetStartPosition(_position);
            }
            //Name the new object
            newInstance.name = nameOfObject + nbOfInsatanciation;
            nbOfInsatanciation++;
        }
    }

    private class GoalFactory : IPrefabFactory
    {
        private int nbOfInsatanciation = 1;
        private string nameOfObject = "Goal ";

        public void InsatanciateGameObject(GameObject _gameObject, Tile _tile, Vector3 _position)
        {
            TileGoal tile = _tile as TileGoal;
            GameObject newInstance =
                (GameObject)
                    Instantiate(_gameObject, _position, Quaternion.identity);
            if (newInstance.GetComponent<GoalBehaviour>() != null)
            {
                newInstance.GetComponent<GoalBehaviour>().SetStartPosition(_position);
                newInstance.GetComponent<GoalBehaviour>().SetDirection(tile.FirstEntranceDirection);
            }
            //Name the new object
            newInstance.name = nameOfObject + nbOfInsatanciation;
            nbOfInsatanciation++;
        }
    }

    private class RampsFactory : IPrefabFactory
    {
        private int nbOfInsatanciation = 1;
        private string nameOfObject = "Ramps ";

        public void InsatanciateGameObject(GameObject _gameObject, Tile _tile, Vector3 _position)
        {
            TileRamp tile = _tile as TileRamp;

            if (tile.UpDirection == Tile.Direction.Right)
            {
                _position.z -= (6 * 3);
                _position.x -= (3 * 3);
            }
            if (tile.UpDirection == Tile.Direction.Left)
            {
                _position.z += (8 * 3);
                _position.x += (6 * 3);
            }
            if (tile.UpDirection == Tile.Direction.Down)
            {
                _position.z -= (3 * 3);
                _position.x += (2 * 3);
            }
            if (tile.UpDirection == Tile.Direction.Up)
            {
                _position.z -= (6 * 3);
                _position.x -= (2 * 3);
            }

            GameObject newInstance = (GameObject)Instantiate(_gameObject, _position, Quaternion.identity);
            if (newInstance.GetComponent<ObjectBehavior>() != null)
            {
                newInstance.GetComponent<ObjectBehavior>().SetStartPosition(_position);
            }
            //Name the new object
            newInstance.name = nameOfObject + nbOfInsatanciation;
            nbOfInsatanciation++;
        }
    }

    private class BallFactory : IPrefabFactory
    {
        private int nbOfInsatanciation = 1;
        private string nameOfObject = "Ball ";

        public void InsatanciateGameObject(GameObject _gameObject, Tile _tile, Vector3 _position)
        {
            GameObject newInstance =
                (GameObject)
                    Instantiate(_gameObject, _position, Quaternion.identity);
            if (newInstance.GetComponent<BallBehaviour>() != null)
            {
                newInstance.GetComponent<BallBehaviour>().SetStartPosition(_position);
            }
            //Name the new object
            newInstance.name = nameOfObject + nbOfInsatanciation;
            nbOfInsatanciation++;
        }
    }

    private class BonusSpawnFactory : IPrefabFactory
    {
        private int nbOfInsatanciation = 1;
        private string nameOfObject = "Bonus Spawn ";

        public void InsatanciateGameObject(GameObject _gameObject, Tile _tile, Vector3 _position)
        {
            GameObject newInstance =
                (GameObject)
                    Instantiate(_gameObject, _position, Quaternion.identity);
            if (newInstance.GetComponent<ObjectBehavior>() != null)
            {
                newInstance.GetComponent<ObjectBehavior>().SetStartPosition(_position);
            }
            //Name the new object
            newInstance.name = nameOfObject + nbOfInsatanciation;
            nbOfInsatanciation++;
        }
    }

    private class PlayerSpawnFactory : IPrefabFactory
    {
        private int nbOfInsatanciation = 1;
        private string nameOfObject = "Player Spawn ";

        public void InsatanciateGameObject(GameObject _gameObject, Tile _tile, Vector3 _position)
        {
            GameObject newInstance =
                (GameObject)
                    Instantiate(_gameObject, _position, Quaternion.identity);
            if (newInstance.GetComponent<ObjectBehavior>() != null)
            {
                newInstance.GetComponent<ObjectBehavior>().SetStartPosition(_position);
            }
            //Name the new object
            newInstance.name = nameOfObject + nbOfInsatanciation;
            nbOfInsatanciation++;
        }
    }
    private class EndlessHoleFactory : IPrefabFactory
    {
        private int nbOfInsatanciation = 1;
        private string nameOfObject = "Endless Hole ";

        public void InsatanciateGameObject(GameObject _gameObject, Tile _tile, Vector3 _position)
        {
            GameObject newInstance =
                (GameObject)
                    Instantiate(_gameObject, _position, Quaternion.identity);
            if (newInstance.GetComponent<EndlessHoleBehaviour>() != null)
            {
                newInstance.GetComponent<EndlessHoleBehaviour>().SetStartPosition(_position);
            }
            //Name the new object
            newInstance.name = nameOfObject + nbOfInsatanciation;
            nbOfInsatanciation++;
        }

    }
}