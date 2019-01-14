/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 Title          :   MonitorPanelController.
 Description    :   Simple logic for the monitor panel.
 Copyright Aldin. All Rights reserved. 
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

using UnityEngine;
using VRTK;

public class MonitorPanelController : MonoBehaviour
{

    public Material _Material_Monitor;                          // The material of the monitor.
    public RenderTexture[] _RenderTexture;                      // The render textures that the security cameras read into.

    public AudioSource _BtnUpAudioSrc;                          // The AudioSource of the upper button on the panel.
    public AudioSource _BtnDownAudioSrc;                        // The AudioSource of the lower button on the panel.

    public VRTK_InteractableObject _REFBtnUpInteractableObj;    // The upper button on the panel.
    public VRTK_InteractableObject _REFBtnDownInteractableObj;  // The lower button on the panel.

    private int _currentCamera = 0;                             // Current security camera index.

    private void Start()
    {
        // Unused
        _REFBtnUpInteractableObj.InteractableObjectGrabbed += new InteractableObjectEventHandler(ChannelUp);
        _REFBtnDownInteractableObj.InteractableObjectGrabbed += new InteractableObjectEventHandler(ChannelDown);
    }

    public void ChannelUp(object sender, InteractableObjectEventArgs interactableObjectEventArgs)
    {
        /*
         * Increase the current security camera index.
         */ 
        _currentCamera++;

        if (_currentCamera >= _RenderTexture.Length)
        {
            _currentCamera = 0;
        }

        /*
         * Change the monitor material's texture to the correct render texture.
         * Play a sound when the button on the panel is used.
         */ 
        ChangeMaterialTexture(_RenderTexture[_currentCamera]);
        _BtnUpAudioSrc.Play();
    }

    public void ChannelDown(object sender, InteractableObjectEventArgs interactableObjectEventArgs)
    {
        /*
         * Decrease the current security camera index.
         */
        _currentCamera--;

        if (_currentCamera <= -1)
        {
            _currentCamera = _RenderTexture.Length - 1;
        }

        /*
         * Change the monitor material's texture to the correct render texture.
         * Play a sound when the button on the panel is used.
         */
        ChangeMaterialTexture(_RenderTexture[_currentCamera]);
        _BtnDownAudioSrc.Play();
    }

    private void ChangeMaterialTexture(Texture texture)
    {
        /*
         * Set the texture on the panel monitor material.
         */
        _Material_Monitor.SetTexture("_MainTex", texture);
    }
}
