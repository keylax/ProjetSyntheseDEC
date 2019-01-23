using UnityEngine;
using Assets.Scripts.Observer;
using Assets.Scripts.GameController;
using Assets.Scripts.GameCharacter;
using Assets.Scripts.GameCharacter.AI;
using Assets.Scripts.GameObjectsBehavior;

public class HomingMissileBehaviour : SubjectMonobehaviour
{
    public GameObject missileList;
    public bool active;
    public AIPlayerBehaviour.Team teamOfSource;
    private Transform target;

	void Start () 
    {
        target = null;
        active = false;
        AddObserver(CurrentGame.currentGameObserver);
	}
	
	
	void Update () 
    {
        if (transform.position.y < -35)
        {
            active = false;
            target = null;
        }
        if (active == false)
        {
            gameObject.transform.position = missileList.transform.position;
        }
        else 
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            transform.LookAt(target.position);
            GetComponent<Rigidbody>().AddForce(transform.forward * 50000 * Time.deltaTime);
        }
	}

    public void SetTarget(Transform _target)
    {
        target = _target;
    }


    void OnTriggerEnter(Collider _collider)
    {
        if (active && _collider.gameObject.name != "Magnet")
        {
            if (ObjectTags.IsPlayer(_collider.gameObject.tag))
            {
                _collider.transform.GetComponent<IGameCharacter>().StunPlayer(2f);
                if (teamOfSource != _collider.transform.GetComponent<IGameCharacter>().GetTeam())
                {
                    NotifyAllObservers(Subject.NotifyReason.HIT_BY_MISSILE, _collider.gameObject);
                }
            }
            target = null;
            active = false;
            GameObject prefab = Resources.Load("Explosion") as GameObject;
            GameObject explosion = Instantiate(prefab);
            explosion.transform.position = transform.position;
            Destroy(explosion, 5);
        }
    }
}
