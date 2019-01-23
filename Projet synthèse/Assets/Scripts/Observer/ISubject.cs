using UnityEngine;

namespace Assets.Scripts.Observer
{
    public interface ISubject
    {
        void AddObserver(Observer _observer);

        void RemoveObserver(Observer _observer);

        void NotifyAllObservers(Subject.NotifyReason _reason, GameObject _player);
    }

}