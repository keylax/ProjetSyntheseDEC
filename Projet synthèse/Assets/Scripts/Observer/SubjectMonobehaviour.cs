using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Observer
{
    public class SubjectMonobehaviour: MonoBehaviour, ISubject
    {
        protected List<Observer> observers;

        public SubjectMonobehaviour()
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

        public void NotifyAllObservers(Subject.NotifyReason _reason, GameObject _player)
        {
            foreach (Observer o in observers)
            {
                o.Notify(this, _reason, _player);
            }
        }
        
    }
}