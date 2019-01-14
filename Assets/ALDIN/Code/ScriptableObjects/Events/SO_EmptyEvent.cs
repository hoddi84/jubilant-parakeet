/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 Title          :   SO_EmptyEvent.
 Description    :   Scriptable Object event that takes no parameters.
 Copyright Aldin. All Rights reserved. 
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmptyEvent", menuName = "Data/Event/EmptyEvent")]
public class SO_EmptyEvent : ScriptableObject {

    private List<EventEmptyListener> _listeners = new List<EventEmptyListener>(); // List containing subscribed listeners.

    public void RaiseEvent()
    {
        /*
         * Tell the listeners this event has been raised.
         */
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(EventEmptyListener listener)
    {
        /*
         * Add listener to the list of subscribers.
         */
        _listeners.Add(listener);
    }

    public void UnregisterListener(EventEmptyListener listener)
    {
        /*
         * Remove listener from the list of subscribers.
         */
        _listeners.Remove(listener);
    }
}
