/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 Title          :   LightController.
 Description    :   Placed on parent gameObject that contains groups of lights you wish to affect all at once.
 Copyright Aldin. All Rights reserved. 
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class LightController : MonoBehaviour {

    public SO_LightSettings _LightSettings;         // LightSettings template.
    public SO_IntVariable _LightTurnOffDelay;       // Delay the lights turn off in this light group.
    public Light[] _REFLights;                      // Lights in the light group.

    private int _lightCount;                        // The number of lights in the light group.

    private void Awake()
    {
        _lightCount = _REFLights.Length;
    }

    private void Update()
    {
        /*
         * Retrieve the lights in the light group.
         */
        if (_REFLights.Length == 0 || _lightCount != _REFLights.Length)
        {
            _REFLights = GetComponentsInChildren<Light>();
            _lightCount = _REFLights.Length;
        }

        /*
         * Adjust the lights based on the light settings template configuration.
         */
        for (int i = 0; i < _REFLights.Length; i++)
        {
            _REFLights[i].intensity = _LightSettings.LightIntensity;
            _REFLights[i].color = _LightSettings.LightColor;
            _REFLights[i].range = _LightSettings.LightRange;
            _REFLights[i].lightmapBakeType = _LightSettings.LightBakeType;

            Vector3 t = _REFLights[i].transform.position;

            _REFLights[i].transform.position = new Vector3(t.x, _LightSettings.LightHeightOffset, t.z);
        }
    }

    public void TurnOffLights()
    {
        /*
         * Start a coroutine that systematically turns of the lights.
         */
        StartCoroutine(TurnOffLightsSystem());
    }

    private IEnumerator TurnOffLightsSystem()
    {
        /*
         * Turns off the lights, one after another, with a specified delay.
         */
        for (int i = _REFLights.Length - 1; i >= 0; i--)
        {
            LightBehavior lightBehavior = _REFLights[i].gameObject.GetComponentInParent<LightBehavior>();
            lightBehavior.TurnOffLight(true);

            yield return new WaitForSeconds(_LightTurnOffDelay.IntVariable * 12 / (_REFLights.Length - 1));
        }
        yield return null;
    }
}
