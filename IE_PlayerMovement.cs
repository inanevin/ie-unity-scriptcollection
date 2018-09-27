/* 
Author: İnan Evin
www.inanevin.com
Global Game Jam 2018 - Unity Technologies Helsinki
Author owns all the rights to the source code. No commercial usage is allowed without permission.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class IE_PlayerMovement : MonoBehaviour
{
    public IE_HeadBobController _HeadBobController;
    public IE_HeadBobController _GunBobController;
    public float f_WalkingMagnitude = 0.2f;
    public float f_JogginMagnitude = 2.0f;
    public Vector2 v2_Speeds = new Vector2(8, 12);
    public float f_Gravity = 9.81f;
    public float f_JoggingMultiplier;
    public float f_StaminaDecrease;
    public float f_StaminaIncrease;
    [Header("Breath Audio")]
    public float f_BreathingMaxVolume = 0.6f;
    public float f_BreathingVolumeIncrease = 0.2f;
    public float f_BreathingVolumeDecrease = 0.2f;
    public AudioSource as_Breathing;
    [Header("Hearthbeat Audio")]
    public float f_HearthbeatMaxVolume = 0.6f;
    public float f_HearthbeatVolumeIncrease = 0.2f;
    public float f_HearthbeatVolumeDecrease = 0.2f;
    public AudioSource as_Hearthbeat;
    [Header("Footstep Audio")]
    public AudioSource as_Footstep;
    public AudioClip[] ac_Footsteps;
    private float f_LastStepped;
    public float f_FootstepWalkRate = 0.5f;
    public float f_FootstepJogRate = 0.25f;
    private float f_FootstepRate;
    private float f_Stamina = 100.0f;
    private float f_MovementSpeedMultiplier = 1.0f;
    private Vector3 v3_MoveDirection = Vector2.zero;
    private CharacterController cc_This;

    private void Awake()
    {
        cc_This = GetComponent<CharacterController>();
        f_FootstepRate = f_FootstepWalkRate;
    }

    private void Update()
    {

        if (cc_This.isGrounded)
        {
            // Jogging Logic w/ Stamina
            if (Input.GetKey(KeyCode.LeftShift))
            {
                
                if (f_Stamina > 0.0f)
                    f_Stamina -= Time.deltaTime * f_StaminaDecrease;

                if (f_Stamina > 1.0f)
                {
                    f_MovementSpeedMultiplier = f_JoggingMultiplier;
                    f_FootstepRate = f_FootstepJogRate;

                    if (f_Stamina < 50.0f)
                    {
                        if (as_Breathing.volume < f_BreathingMaxVolume)
                            as_Breathing.volume += Time.deltaTime * f_BreathingVolumeIncrease;

                        if (as_Hearthbeat.volume < f_HearthbeatMaxVolume)
                            as_Hearthbeat.volume += Time.deltaTime * f_HearthbeatVolumeIncrease;
                    }
                }
                else
                {
                    f_MovementSpeedMultiplier = 1.0f;
                    f_FootstepRate = f_FootstepWalkRate;

                    if (as_Breathing.volume > 0.001f)
                        as_Breathing.volume -= Time.deltaTime * f_BreathingVolumeDecrease;

                    if (as_Hearthbeat.volume > 0.001f)
                        as_Hearthbeat.volume -= Time.deltaTime * f_HearthbeatVolumeDecrease;
                }
            }
            else
            {
                if (f_MovementSpeedMultiplier != 1.0f)
                {
                    f_MovementSpeedMultiplier = 1.0f;
                    f_FootstepRate = f_FootstepWalkRate;
                }

                if (f_Stamina < 99.0f)
                    f_Stamina += Time.deltaTime * f_StaminaIncrease;

                if (as_Breathing.volume > 0.001f)
                    as_Breathing.volume -= Time.deltaTime * f_BreathingVolumeDecrease;

                if (as_Hearthbeat.volume > 0.001f)
                    as_Hearthbeat.volume -= Time.deltaTime * f_HearthbeatVolumeDecrease;
            }                                                  

            // Movement Direction
            v3_MoveDirection = new Vector3(v2_Speeds.x * Input.GetAxis("Horizontal"), 0, v2_Speeds.y * Input.GetAxis("Vertical")) * f_MovementSpeedMultiplier;
            v3_MoveDirection = transform.TransformDirection(v3_MoveDirection);

            // Footstep
            if (cc_This.velocity.magnitude > f_WalkingMagnitude)
            {
                if (Time.time > f_FootstepRate + f_LastStepped)
                {
                    f_LastStepped = Time.time;
                    as_Footstep.PlayOneShot(ac_Footsteps[Random.Range(0, ac_Footsteps.Length)]);
                }

                if (cc_This.velocity.magnitude > f_JogginMagnitude)
                {
                    _HeadBobController._PlayerState = IE_HeadBobController.PlayerState.Running;
                    _GunBobController._PlayerState = IE_HeadBobController.PlayerState.Running;
                }
                else
                {
                    _GunBobController._PlayerState = IE_HeadBobController.PlayerState.Walking;
                    _HeadBobController._PlayerState = IE_HeadBobController.PlayerState.Walking;
                }
            }
            else
            {
                _HeadBobController._PlayerState = IE_HeadBobController.PlayerState.Idle;
                _GunBobController._PlayerState = IE_HeadBobController.PlayerState.Idle;
            }

        }

        // Movement Logic
        v3_MoveDirection.y -= Time.deltaTime * f_Gravity;
        cc_This.Move(v3_MoveDirection * Time.deltaTime);

    }
}
