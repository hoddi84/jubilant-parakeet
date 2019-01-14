/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 Title          :   LightBehavior.
 Description    :   Simple behavior and controls for a single light.
 Copyright Aldin. All Rights reserved. 
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

using System.Collections;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LightBehavior : MonoBehaviour
{

    public AudioClip _LightOnClip;                  // Audioclip when the light is turning on.
    public AudioClip _LightOffClip;                 // Audioclip when the light is turning off.
    public AudioClip _LightStaticClip;              // Audioclip when the light is turned on. (default)
    public AudioClip _CreepyVoiceClip;              // Audioclip when the lights are shutting down.

    private Light _light;                           // The Light component.
    private AudioSource _lightAudioSrc;              // Audiosource that is attached to the light.
    private MeshRenderer _lightRenderer;            // MeshRenderer of the light bulb.

    private Coroutine _lightBehaviorCoroutine;      // Coroutine that will run the light behaviour.

    private void Awake()
    {
        _light = GetComponentInChildren<Light>();
        _lightAudioSrc = GetComponentInChildren<AudioSource>();
        _lightRenderer = GetComponentInChildren<MeshRenderer>();
    }

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
        _lightRenderer.material.DisableKeyword("_EMISSION");
        _light.enabled = false;
        _lightAudioSrc.Stop();
        _lightAudioSrc.loop = false;
        _lightAudioSrc.clip = _LightOffClip;
        _lightAudioSrc.Play();

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
        _light.enabled = true;
        _lightRenderer.material.EnableKeyword("_EMISSION");
        PlaySound(_LightOnClip, false);
    }

    public void PlaySound(AudioClip clip, bool loop)
    {
        /*
         * Play a sound, with a specified clip and specify if it should loop.
         */
        _lightAudioSrc.Stop();
        _lightAudioSrc.clip = clip;
        _lightAudioSrc.loop = loop;
        _lightAudioSrc.Play();
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

#if UNITY_EDITOR
[CustomEditor(typeof(LightBehavior), true)]
public class LightBehaviorEditor : Editor
{
    SerializedProperty _pLightOnClip;
    SerializedProperty _pLightOffClip;
    SerializedProperty _pLightStaticClip;
    SerializedProperty _pCreepyVoiceClip;

    public void OnEnable()
    {
        _pLightOnClip = serializedObject.FindProperty("_LightOnClip");
        _pLightOffClip = serializedObject.FindProperty("_LightOffClip");
        _pLightStaticClip = serializedObject.FindProperty("_LightStaticClip");
        _pCreepyVoiceClip = serializedObject.FindProperty("_CreepyVoiceClip");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        LightBehavior t = (LightBehavior)target;

        Color defaultGUIColor = GUI.color;

        GUIStyle smallLabelStyle = new GUIStyle(GUI.skin.box)
        {
            fontSize = 7,
            alignment = TextAnchor.MiddleCenter,
        };

        GUIStyle boxGroupWithLabelStyle = new GUIStyle(GUI.skin.box)
        {
            margin = new RectOffset(5, 5, 10, 5),
            padding = new RectOffset(5, 5, 12, 5)
        };

        GUI.color = Color.grey;

        GUIHelper.GroupedBoxFielWithLabel(() =>
        {
            GUI.color = defaultGUIColor;

            GUIHelper.GroupedBoxVertical(() =>
            {
                GUIHelper.PropertyFieldWithLabel(_pLightOnClip, _pLightOnClip.displayName);
                GUIHelper.PropertyFieldWithLabel(_pLightOffClip, _pLightOffClip.displayName);
                GUIHelper.PropertyFieldWithLabel(_pLightStaticClip, _pLightStaticClip.displayName);
                GUIHelper.PropertyFieldWithLabel(_pCreepyVoiceClip, _pCreepyVoiceClip.displayName);
            });
        }, "Assets", smallLabelStyle, boxGroupWithLabelStyle);

        GUI.color = Color.grey;

        GUIHelper.GroupedBoxFielWithLabel(() =>
        {
            GUI.color = defaultGUIColor;

            GUIHelper.GroupedBoxHorizontal(() =>
            {
                MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                for (int i = 0; i < methods.Length; i++)
                {
                    object[] customAttributes = methods[i].GetCustomAttributes(typeof(ButtonAttribute), true);

                    if (customAttributes.Length > 0)
                    {
                        if (GUILayout.Button(methods[i].Name))
                        {
                            ((LightBehavior)target).Invoke(methods[i].Name, 0f);
                        }
                    }
                }
            });
        }, "Controls", smallLabelStyle, boxGroupWithLabelStyle);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif