/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 Title          :   SO_IntVariable.
 Description    :   ScriptableObject int variable.
 Copyright Aldin. All Rights reserved. 
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "Data/IntVariable")]
public class SO_IntVariable : ScriptableObject {

    public int IntVariable = 0;         // ScriptableObject int variable.
}
