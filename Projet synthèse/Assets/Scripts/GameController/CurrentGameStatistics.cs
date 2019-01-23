using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GameController
{
    public class CurrentGameStatistics
    {
        private int numberOfDeaths;

        
        private int numberOfShotsToGoal;

        private int numberOfProvoquedDrops;
        private int numberOfInterceptions;
        private float possessionTime;
        private int numberOfBonusesAquired;
        private int numberOfAssists;
        private int numberOfMissilesShot;
        private int numberOfJumpsFromSprings;
        private int numberOfLightningBoltUsed;
        private float lastHitByEnemyBonus;

        public int NumberOfDeaths
        {
            get { return numberOfDeaths; }
        }


        public int NumberOfShotsToGoal
        {
            get { return numberOfShotsToGoal; }
        }

        public int NumberOfProvoquedDrops
        {
            get { return numberOfProvoquedDrops; }
        }

        public int NumberOfInterceptions
        {
            get { return numberOfInterceptions; }
        }

        public float PossessionTime
        {
            get { return possessionTime; }
        }

        public int NumberOfBonusesAquired
        {
            get { return numberOfBonusesAquired; }
        }

        public int NumberOfAssists
        {
            get { return numberOfAssists; }
        }

        public int NumberOfMissilesShot
        {
            get { return numberOfMissilesShot; }
        }

        public int NumberOfJumpsFromSprings
        {
            get { return numberOfJumpsFromSprings; }
        }

        public int NumberOfLightningBoltUsed
        {
            get { return numberOfLightningBoltUsed; }
        }

        public float LastHitByEnemyBonus
        {
            get { return lastHitByEnemyBonus; }
        }

        public void AddDeath()
        {
            numberOfDeaths++;
        }

        public void AddShotToGoal()
        {
            numberOfShotsToGoal++;
        }

        public void AddProvoquedDrop()
        {
            numberOfProvoquedDrops++;
        }

        public void AddInterception()
        {
            numberOfInterceptions++;
        }

        public void AddTimeToPossessionTime(float _deltaTime)
        {
            possessionTime += _deltaTime;
        }

        public void AddBonusAcquired()
        {
            numberOfBonusesAquired++;
        }

        public void AddAssist()
        {
            numberOfAssists++;
        }

        public void AddMissileShot()
        {
            numberOfMissilesShot++;
        }

        public void AddJumpFromSpring()
        {
            numberOfJumpsFromSprings++;
        }

        public void SetLastHitByBonus(float _timeSinceLevelLoad)
        {
            lastHitByEnemyBonus = _timeSinceLevelLoad;
        }

        public void AddLightningBoltUsed()
        {
            numberOfLightningBoltUsed++;
        }
    }
}
