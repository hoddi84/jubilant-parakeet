/* - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
 Title          :   ElevatorController.
 Description    :   Simple logic for the elevator and required components. 
 Copyright Aldin. All Rights reserved. 
 * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class ElevatorController : MonoBehaviour
{
    public AudioSource _REFElevatorMovingAudioSrc;          // Sound after elevator has been summoned and started.
    public AudioSource _REFElevatorSummonAudioSrc;          // Sound when elevator is being summoned.
    public AudioSource _REFElevatorFloorDingAudioSrc;       // Sound when elevator passes floors.
    public AudioSource _REFElevatorStopAudioSrc;            // Sound when elevator stops.
    public AudioSource _REFElevatorStartAudioSrc;           // Sound after elevator has been summoned.
    public AudioSource _REFElevatorEndScreamAudioSrc;       // Sound when elevator is about to stop.
    public AudioSource _REFElevatorEndingScreamAudioSrc;    // Sound when elevator door has opened.

    public Transform _REFCameraRigEye;                      // The Eye transform of the SteamVR CameraRig.

    public GameObject[] _REFElevatorFloorMarkers;           // The triangle markers above the elevator.

    public GameObject _REFElevatorRightDoor;                // The right door gameObject of the elevator.
    public GameObject _REFElevatorLeftDoor;                 // The left door gameObject of the elevator.
    public Transform _REFElevatorRightDoorOpen;             // The transform of the right door at the open position
    public Transform _REFElevatorLeftDoorOpen;              // The transform of the left door at the open position.

    public SO_EmptyEvent _ElevatorSummonedEvent;            // Event that gets raised when the elevator is summoned.
    public SO_IntVariable _ElevatorFloorDelay;              // Delay for the elevator time between floors.

    public float _ElevatorOpeningSpeed = 1f;                // Speed at which the elevator doors open.
    public float _EndingScreamMoveSpeed = 1f;               // Speed at which the sound travels towards the CameraRig after the doors open.

    private bool _isElevatorOpen = false;                    // State of the elevator doors, true if they are open.
    private bool _isElevatorStopped = false;                 // State of the elevator after it has been summoned, true if stopped.
    private bool _isElevatorSummoned = false;                // State of the elevator, if it has been summoned then true.

    private Transform _elevatorRightDoorClosed;             // Reference to the closed position of the right elevator door.
    private Transform _elevatorLeftDoorClosed;              // Reference to the open position of the left elevator door.

    private Transform _endingScreamPosition;                // Reference to the transform of the sound that is played after the doors open.

    private void Start()
    {
        _elevatorRightDoorClosed = _REFElevatorRightDoor.transform;
        _elevatorLeftDoorClosed = _REFElevatorLeftDoor.transform;

        _endingScreamPosition = _REFElevatorEndingScreamAudioSrc.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SummonElevator();
        }

        if (_isElevatorOpen)
        {
            /* 
             * Elevator doors open.
             */
            _REFElevatorRightDoor.transform.position = Vector3.Lerp(_elevatorRightDoorClosed.position, _REFElevatorRightDoorOpen.position, Time.deltaTime * _ElevatorOpeningSpeed);
            _REFElevatorLeftDoor.transform.position = Vector3.Lerp(_elevatorLeftDoorClosed.position, _REFElevatorLeftDoorOpen.position, Time.deltaTime * _ElevatorOpeningSpeed);
        }

        if (_isElevatorStopped)
        {
            /*
             * Sound that plays after the doors are opened moves towards the SteamVR CameraRig.
             */
            _REFElevatorEndingScreamAudioSrc.transform.position = Vector3.Lerp(_endingScreamPosition.position, _REFCameraRigEye.position, Time.deltaTime * _EndingScreamMoveSpeed);
        }
    }

    public void SummonElevator()
    {
        /*
         * Start the coroutine for the elevator behavior.
         */
        if (!_isElevatorSummoned)
        {
            StartCoroutine(BeginElevatorBehavior());
            _isElevatorSummoned = true;
        }

    }

    public void SummonElevator(object sender, InteractableObjectEventArgs interactableObjectEventArgs)
    {
        /*
         * Start the coroutine for the elevator behavior.
         */
        if (!_isElevatorSummoned)
        {
            StartCoroutine(BeginElevatorBehavior());
            _isElevatorSummoned = true;
        }
        
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    private void OpenElevator()
    {
        _isElevatorOpen = true;
    }

    private void StopElevator()
    {
        _isElevatorStopped = true;
    }

    private IEnumerator BeginElevatorBehavior()
    {
        /*
         * Play the elevator summoned sound.
         * Raise elevator summoned event.
         */
        _REFElevatorSummonAudioSrc.Play();
        _ElevatorSummonedEvent.RaiseEvent();

        yield return new WaitForSeconds(0.5f);

        /*
         * Play the start sound of the elevator.
         */
        _REFElevatorStartAudioSrc.Play();

        yield return new WaitUntil(() => !_REFElevatorStartAudioSrc.isPlaying);

        /*
         * Play the moving sound of the elevator.
         */
        _REFElevatorMovingAudioSrc.Play();

        for (int i = 0; i < _REFElevatorFloorMarkers.Length; i++)
        {

            yield return new WaitForSeconds(_ElevatorFloorDelay.IntVariable);

            /*
             * Play a sound and color the triangles above the elevator.
             */
            _REFElevatorFloorDingAudioSrc.Play();
            _REFElevatorFloorMarkers[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green);

            /*
             * Play a sound when the elevator is about to stop.
             */
            if (i == _REFElevatorFloorMarkers.Length - 2)
            {
                _REFElevatorEndScreamAudioSrc.Play();
            }
        }

        yield return new WaitForSeconds(_ElevatorFloorDelay.IntVariable);

        /*
         * Stop elevator moving sound.
         */
        _REFElevatorMovingAudioSrc.Stop();

        yield return new WaitForSeconds(0.5f);

        /*
         * Play elevator stop sound.
         */
        _REFElevatorStopAudioSrc.Play();

        /*
         * Open elevator.
         */
        OpenElevator();

        /*
         * Play the sound after the elevator doors open.
         */
        _REFElevatorEndingScreamAudioSrc.Play();

        /*
         * Set the state of the elevator to stop.
         */
        StopElevator();

        yield return new WaitUntil(() => !_REFElevatorEndingScreamAudioSrc.isPlaying);

        /*
         * Reload the scene.
         */
        ReloadScene();
    }
}

