using Assets.Scripts.GameCharacter.AI;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public AIPlayerBehaviour.Team team;
    public float millisecondRespawnTimer = 10f;
    public GameObject bonusSpawnPoint;
    
    private float timeBeforeRespawn;
    private MeshRenderer bonusRenderer;
    private Collider bonusCollider;

	void Start () 
    {
        bonusCollider = transform.GetComponent<Collider>();
        bonusRenderer = transform.GetComponent<MeshRenderer>();
        timeBeforeRespawn = millisecondRespawnTimer;
        Spawn();
	}
	
	void Update () 
    {
        
        if (transform.GetComponent<Collider>().enabled == false)
        {
            if (timeBeforeRespawn > 0)
            {
                timeBeforeRespawn -= Time.deltaTime;
            }
            else
            {
                timeBeforeRespawn = millisecondRespawnTimer;
                Spawn();
            }
        }
	}

    private void Spawn()
    {
        transform.position = bonusSpawnPoint.transform.position;
        ShowBonus();
    }

    private void ShowBonus()
    {
        bonusCollider.enabled = true;
        bonusRenderer.enabled = true;
    }
}