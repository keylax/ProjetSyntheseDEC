
using UnityEngine;

namespace Assets.Scripts.Observer
{
    public interface Observer
    {
        void Notify(ISubject _subject, Subject.NotifyReason _notifyReason, GameObject _player);
    }
}
