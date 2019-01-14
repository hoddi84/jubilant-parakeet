/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 Title          :   LightBehavior.
 Description    :   Simple behavior and controls for a single light.
 Copyright Aldin. All Rights reserved. 
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

using System.Collections;
using UnityEngine;

public class LightBehavior : MonoBehaviour
{

    public AudioClip _LightOnClip;                  // Audioclip when the light is turning on.
    public AudioClip _LightOffClip;                 // Audioclip when the light is turning off.
    public AudioClip _LightStaticClip;              // Audioclip when the light is turned on. (default)
    public AudioClip _CreepyVoiceClip;              // Audioclip when the lights are shutting down.

    public Light _REFLight;                         // The Light component.
    public AudioSource _REFLightAudioSrc;           // Audiosource that is attached to the light.
    public MeshRenderer _REFLightBulbRenderer;      // MeshRenderer of the light bulb.

    private Coroutine _lightBehaviorCoroutine;      // Coroutine that will run the light behaviour.

    private void Start()
    {
        _lightBehaviorCoroutine = StartCoroutine(Behavior());
    }

    [Button]
    public void TurnOffLight(bool permanent = false)
    {
        /*
         * Light becomes completely dark, emits no sound.
         */
        _REFLightBulbRenderer.material.DisableKeyword("_EMISSION");
        _REFLight.enabled = false;
        _REFLightAudioSrc.Stop();
        _REFLightAudioSrc.loop = false;
        _REFLightAudioSrc.clip = _LightOffClip;
        _REFLightAudioSrc.Play();

        if (permanent)
        {
            /*
             * Stop coroutine that randomly turns the light off and on.
             * Play creepy sound when light is permanently shut down.
             */
            StopCoroutine(_lightBehaviorCoroutine);
            PlaySound(_CreepyVoiceClip, false);
        }
    }

    [Button]
    public void TurnOnLight()
    {
        /*
         * Light turns on. Sound played.
         */
        _REFLight.enabled = true;
        _REFLightBulbRenderer.material.EnableKeyword("_EMISSION");
        PlaySound(_LightOnClip, false);
    }

    public void PlaySound(AudioClip clip, bool loop)
    {
        /*
         * Play a sound, with a specified clip and specify if it should loop.
         */
        _REFLightAudioSrc.Stop();
        _REFLightAudioSrc.clip = clip;
        _REFLightAudioSrc.loop = loop;
        _REFLightAudioSrc.Play();
    }

    private IEnumerator Behavior()
    {
        /*
         * Randomly turns the light off and back on.
         */
        while(true)
        {
            float delayLightOff = Random.Range(10f, 20f);
            float delayLightOn = Random.Range(0.5f, 2.5f);

            yield return new WaitForSeconds(delayLightOff);

            TurnOffLight();

            yield return new WaitForSeconds(delayLightOn + _LightOffClip.length);

            TurnOnLight();

            yield return new WaitForSeconds(_LightOnClip.length);

            PlaySound(_LightStaticClip, true);

            yield return null;
        }
    }
}