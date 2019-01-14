/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 Title          :   EmptyEventListener.
 Description    :   Listener for a scriptableObject event that takes no parameters.
 Copyright Aldin. All Rights reserved. 
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

using UnityEngine;
using System;

public class EventEmptyListener : MonoBehaviour {

	[Serializable] public class ResponseEvent : UnityEngine.Events.UnityEvent { }

    public SO_EmptyEvent _REFEventEmpty;    // Event that takes no parameters.
    public ResponseEvent EventResponse;     // Response to the event.

    public void OnEventRaised()
    {
        /*
         * Invoke the response to the empty event.
         */
        EventResponse.Invoke();
    }

    private void OnEnable()
    {
        /*
         * Add as a listener to the empty event.
         */
        _REFEventEmpty.RegisterListener(this);
    }

    private void OnDisable()
    {
        /*
         * Remove as a listener from the empty event.
         */
        _REFEventEmpty.UnregisterListener(this);
    }
}
