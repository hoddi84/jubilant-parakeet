/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 Title          :   ScriptableObject Light Settings template.
 Description    :   Settings for lights, changes in play mode will be saved.
 Copyright Aldin. All Rights reserved. 
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

using UnityEngine;

[CreateAssetMenu(fileName = "LightController", menuName = "Data/LightController")]
public class SO_LightSettings : ScriptableObject {

    public int LightIntensity = 1;              // Light Intensity.
    public float LightRange = 1;                // Light Range.
    public Color LightColor = Color.white;      // Light Color.
    [Range(0f, 2f)]                             //
    public float LightHeightOffset = 0f;        // Height offset that can be added to a light.
    public LightmapBakeType LightBakeType;      // The Lightmode of the light. (Realtime, Mixed or Baked).
}
