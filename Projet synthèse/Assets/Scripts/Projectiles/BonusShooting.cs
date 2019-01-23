using Assets.Scripts.GameCharacter;
using Assets.Scripts.GameCharacter.AI;
using Assets.Scripts.GameCharacter.HumanPlayer;
using Assets.Scripts.GameController;
using Assets.Scripts.GameObjectsBehavior;
using Assets.Scripts.Observer;
using UnityEngine;

namespace Assets.Scripts.Projectiles
{
    public class BonusShooting : SubjectMonobehaviour
    {
        public GameObject projectileSpawn;
        public GameObject missileList;
        public GameObject springList;
        public GameObject homingMissileList;

        private Transform projectile;
        private Rigidbody projectileRb;

        private const float LIGHTNING_BOLT_STUN_DURATION = 2.5f;

        public void Start()
        {
            AddObserver(CurrentGame.currentGameObserver);
        }

        public void ShootBonus(PickUpBonus.Bonuses _bonusToShoot)
        {
            projectile = null;

            switch (_bonusToShoot)
            {
                case PickUpBonus.Bonuses.SPRING:
                    ShootSpring();
                    break;
                case PickUpBonus.Bonuses.MISSILE:
                    ShootMissile();
                    break;
                case PickUpBonus.Bonuses.HOMING_MISSILE:
                    ShootHomingMissile();
                    break;
                case PickUpBonus.Bonuses.LIGHTNING_BOLT:
                    UseLightningBolt();
                    break;
                case PickUpBonus.Bonuses.MAGNET_UPGRADE:
                    UseMagnetUpgrade();
                    break;
                case PickUpBonus.Bonuses.MR_FREEZE:
                    UseMrFreeze();
                    break;
                case PickUpBonus.Bonuses.POLARITY_REVERSER:
                    UsePolarityReverser();
                    break;
                case PickUpBonus.Bonuses.SPEED_BOOST:
                    UseSpeedBoost();
                    break;
                case PickUpBonus.Bonuses.STAR:
                    UseStar();
                    break;
            }

        }

        private void ShootSpring()
        {
            SoundManager.PlaySFX("Throw");
            for (int i = 0; i < springList.transform.childCount; i++)
            {
                if (springList.transform.GetChild(i).GetComponent<SpringBehaviour>().active == false)
                {
                    projectile = springList.transform.GetChild(i);
                    springList.transform.GetChild(i).GetComponent<SpringBehaviour>().active = true;
                    projectile.position = projectileSpawn.transform.position + transform.forward;
                    projectileRb = projectile.GetComponent<Rigidbody>();
                    projectileRb.velocity = projectileSpawn.transform.forward * 20;
                    projectileRb.velocity += GetComponent<Rigidbody>().velocity;
                    break;
                }
            }
        }

        private void ShootMissile()
        {
            SoundManager.PlaySFX("ProjectileShot");
            NotifyAllObservers(Subject.NotifyReason.MISSILE_USED, gameObject);
            for (int i = 0; i < missileList.transform.childCount; i++)
            {
                if (missileList.transform.GetChild(i).GetComponent<MissileBehaviour>().active == false)
                {
                    projectile = missileList.transform.GetChild(i);
                    missileList.transform.GetChild(i).GetComponent<MissileBehaviour>().active = true;
                    missileList.transform.GetChild(i).GetComponent<MissileBehaviour>().teamOfSource = GetComponent<IGameCharacter>().GetTeam();
                    projectile.position = projectileSpawn.transform.position + transform.forward;

                    if (ObjectTags.AITag != tag)
                    {
                        projectile.rotation = transform.GetChild(1).rotation;
                    }
                    else
                    {
                        projectile.rotation = transform.rotation;
                    }
                    
                    projectileRb = projectile.GetComponent<Rigidbody>();
                    projectileRb.velocity = projectileSpawn.transform.forward * 50;
                    break;
                }
            }
        }

        private void ShootHomingMissile()
        {
            SoundManager.PlaySFX("ProjectileShot");
            NotifyAllObservers(Subject.NotifyReason.MISSILE_USED, gameObject);
            float shortestDistanceFromSource = 100000000;
            Transform missileTarget = null;
            foreach (Transform player in transform.parent)
            {
                if (player.GetComponent<IGameCharacter>().GetTeam() != GetComponent<IGameCharacter>().GetTeam() && Vector3.Distance(player.position, transform.position) < shortestDistanceFromSource)
                {
                    shortestDistanceFromSource = Vector3.Distance(player.position, transform.position);
                    missileTarget = player;
                }
            }

            if (missileTarget != null)
            {
                for (int i = 0; i < homingMissileList.transform.childCount; i++)
                {
                    if (homingMissileList.transform.GetChild(i).GetComponent<HomingMissileBehaviour>().active == false)
                    {
                        projectile = homingMissileList.transform.GetChild(i);
                        homingMissileList.transform.GetChild(i).GetComponent<HomingMissileBehaviour>().active = true;
                        homingMissileList.transform.GetChild(i).GetComponent<HomingMissileBehaviour>().SetTarget(missileTarget);
                        homingMissileList.transform.GetChild(i).GetComponent<HomingMissileBehaviour>().teamOfSource = this.GetComponent<IGameCharacter>().GetTeam();
                        projectile.position = projectileSpawn.transform.position + transform.forward;
                        projectileRb = projectile.GetComponent<Rigidbody>();
                        projectileRb.velocity = projectileSpawn.transform.forward * 20;
                        break;
                    }
                }
            }
        }

        private void UseLightningBolt()
        {
            AIPlayerBehaviour.Team team = transform.GetComponent<IGameCharacter>().GetTeam();
            SoundManager.PlaySFX("Thunder");

            NotifyAllObservers(Subject.NotifyReason.LIGHTNINGBOLT_USED, gameObject);
            foreach (Transform player in transform.parent)
            {
                if (player.GetComponent<IGameCharacter>().GetTeam() != team)
                {
                    if (player.tag != ObjectTags.AITag)
                    {
                        player.GetComponent<StunPlayer>().StunThePlayer(LIGHTNING_BOLT_STUN_DURATION);
                    }
                    else
                    {
                        player.GetComponent<AIPlayerBehaviour>().StunPlayer(LIGHTNING_BOLT_STUN_DURATION);
                    }
                    player.GetComponent<VisualEffects>().SetThunderBolt();
                    NotifyAllObservers(Subject.NotifyReason.HIT_BY_LIGHTNINGBOLT, player.gameObject);
                }
            }

        }

        private void UsePolarityReverser()
        {
            AIPlayerBehaviour.Team team = transform.GetComponent<IGameCharacter>().GetTeam();

            foreach (Transform player in transform.parent)
            {
                if (player.GetComponent<IGameCharacter>().GetTeam() != team)
                {
                    if (player.tag != ObjectTags.AITag)
                    {
                        player.GetChild(4).GetComponent<HumanMagnetManager>().InvertMagnetPolarity(true);
                        player.GetChild(4).GetComponent<HumanMagnetManager>().InvertPolarityInUI();
                    }
                    else
                    {
                        player.GetChild(1).GetComponent<AIMagnetManager>().InvertMagnetPolarity(true);
                    }
                    NotifyAllObservers(Subject.NotifyReason.HIT_BY_POLARITYREVERSER, player.gameObject);
                }
            }

        }

        private void UseMagnetUpgrade()
        {
            if (gameObject.tag != ObjectTags.AITag)
            {
                transform.GetChild(4).GetComponent<HumanMagnetManager>().UpgradeMagnetForce();
            }
            else
            {
                transform.GetChild(1).GetComponent<AIMagnetManager>().UpgradeMagnetForce();
            }
        }

        private void UseSpeedBoost()
        {
            SoundManager.PlaySFX("Boost");
            if (gameObject.tag != ObjectTags.AITag)
            {
                GetComponent<PlayerMovement>().BoostSpeed();
            }
            else
            {
                GetComponent<AIPlayerBehaviour>().BoostSpeed();
            }
        }

        private void UseMrFreeze()
        {
            AIPlayerBehaviour.Team team = transform.GetComponent<IGameCharacter>().GetTeam();
            SoundManager.PlaySFX("freeze");

            foreach (Transform player in transform.parent)
            {
                if (player.GetComponent<IGameCharacter>().GetTeam() != team)
                {
                    player.GetComponent<FreezeGameCharacter>().FreezePlayer();
                    NotifyAllObservers(Subject.NotifyReason.HIT_BY_MRFREEZE, player.gameObject);
                }
            }
        }

        private void UseStar()
        {
            GetComponent<Invincibility>().MakeInvincible();
            SoundManager.PlaySFX("starman");

            if (gameObject.tag != ObjectTags.AITag)
            {
                GetComponent<PlayerMovement>().BoostSpeed();
                GetComponent<HumanPlayerBehaviour>().transform.GetChild(3).GetChild(0).GetComponent<MeshRenderer>().material =
                    Resources.Load("StarMaterial") as Material;
            }
            else
            {
                GetComponent<AIPlayerBehaviour>().BoostSpeed();
                GetComponent<AIPlayerBehaviour>().transform.GetComponent<MeshRenderer>().material =
                   Resources.Load("StarMaterial") as Material;
            }
        }
    }
}