using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Observer
{
    public class Subject: ISubject
    {
        public enum NotifyReason
        {
            RED_GOAL,
            BLUE_GOAL,
            MISSILE_USED,
            SPRING_USED,
            LIGHTNINGBOLT_USED,
            FELL_IN_PIT,
            SHOT_TO_GOAL,
            HIT_BY_MISSILE,
            HIT_BY_HOMINGMISSILE,
            HIT_BY_LIGHTNINGBOLT,
            HIT_BY_MRFREEZE,
            HIT_BY_POLARITYREVERSER,
            BONUS_ACQUIRED,
            BALL_INTERCEPTED,
            GAME_OVER
        }

        protected List<Observer> observers;

        public Subject()
        {
            observers = new List<Observer>();
        }

        public void AddObserver(Observer _observer)
        {
            if (!observers.Contains(_observer))
            {
                observers.Add(_observer);
            }
        }

        public void RemoveObserver(Observer _observer)
        {
            if (observers.Contains(_observer))
            {
                observers.Remove(_observer);
            }
        }

        public void NotifyAllObservers(NotifyReason _reason, GameObject _player)
        {
            foreach (Observer o in observers)
            {
                o.Notify(this, _reason, _player);
            }
        }
        
    }
}