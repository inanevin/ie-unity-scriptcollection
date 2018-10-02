/*	Author: Inan Evin
	www.inanevin.com	
*/

using UnityEngine;
using System.Collections;

public class RSS_WeaponInputPositioner : MonoBehaviour
{

    #region Member Variable(s)

    private Transform t_Target;


    public Vector3 v3_InitialPosition = Vector3.zero;
    public Vector3 v3_InitialEulerAngles = Vector3.zero;
    public float f_POS_InterpolationSpeed;
    public float f_ROT_InterpolationSpeed;
    [Header("Keyboard Settings")]
    public Vector2 v2_KEY_POS_InputPowers;
    public Vector3 v3_KEY_ROT_InputPowers;

    [Header("Mouse Settings")]
    public Vector2 v2_MS_POS_InputPowers;
    public Vector3 v3_MS_ROT_InputPowers;

    public Vector2 v2_POS_MinLimits;
    public Vector2 v2_POS_MaxLimits;
    public Vector2 v2_ROT_MinLimits;
    public Vector2 v2_ROT_MaxLimits;

    public bool b_FreeAimSwitchable;
    private Vector2 v2_MS_POS_FreeAimInputs;
    private Vector2 v2_MS_ROT_FreeAimInputs;
    public string s_MS_FreeAimSwitchButton;

    private Vector2 v2_KEY_Inputs;
    private Vector2 v2_MS_Inputs;
    private float f_POS_UsedInterpSpeed;
    private float f_ROT_UsedInterpSpeed;
    private bool b_FreeAimSwitchedFlag;

    private float f_INIT_MultiplierValue;
  
    [System.NonSerialized]
    public float f_KEY_POS_AimMultiplier = 1.0f;
    [System.NonSerialized]
    public float f_KEY_ROT_AimMultiplier = 1.0f;
    [System.NonSerialized]
    public float f_MS_POS_AimMultiplier = 1.0f;
    [System.NonSerialized]
    public float f_MS_ROT_AimMultiplier = 1.0f;

    public Transform Target
    {
        get { return t_Target; }
        set { t_Target = value; }
    }

    #endregion


    #region Initializer(s)

    void Awake()
    {

        t_Target = transform;

        f_INIT_MultiplierValue = 1.0f;
        v3_InitialPosition = t_Target.localPosition;
        v3_InitialEulerAngles = t_Target.localEulerAngles;
    }


    #endregion


    #region Control(s)

    void Update()
    {
 
        v2_KEY_Inputs.x = Input.GetAxis("Horizontal");
        v2_KEY_Inputs.y = Input.GetAxis("Vertical");

        v2_MS_Inputs.x = Input.GetAxis("Mouse X");
        v2_MS_Inputs.y = Input.GetAxis("Mouse Y");

        Vector3 usedPositionMouseAddition;
        Vector3 usedRotationMouseAddition;


        usedPositionMouseAddition = new Vector3(v2_MS_Inputs.x * v2_MS_POS_InputPowers.x, v2_MS_Inputs.y * v2_MS_POS_InputPowers.y, 0);
        usedRotationMouseAddition = new Vector3(v2_MS_Inputs.y * v3_MS_ROT_InputPowers.x, v2_MS_Inputs.x * v3_MS_ROT_InputPowers.y, v2_MS_Inputs.x * v3_MS_ROT_InputPowers.z);
        f_POS_UsedInterpSpeed = f_POS_InterpolationSpeed;
        f_ROT_UsedInterpSpeed = f_ROT_InterpolationSpeed;
        Vector3 targetLocalPos = ((new Vector3(v2_KEY_Inputs.x * v2_KEY_POS_InputPowers.x, v2_KEY_Inputs.y * v2_KEY_POS_InputPowers.y, 0) * f_KEY_POS_AimMultiplier) / 100.0f) + ((usedPositionMouseAddition * f_MS_POS_AimMultiplier) / 100.0f);
        Vector3 targetLocalRot = (new Vector3(v2_KEY_Inputs.y * v3_KEY_ROT_InputPowers.y, v2_KEY_Inputs.x * v3_KEY_ROT_InputPowers.x, v2_KEY_Inputs.x * v3_KEY_ROT_InputPowers.z) * f_KEY_ROT_AimMultiplier) + usedRotationMouseAddition * f_MS_ROT_AimMultiplier;

        t_Target.localPosition = Vector3.Lerp(t_Target.localPosition, v3_InitialPosition + targetLocalPos, Time.deltaTime * f_POS_UsedInterpSpeed);
        t_Target.localRotation = Quaternion.Slerp(t_Target.localRotation, Quaternion.Euler(v3_InitialEulerAngles + targetLocalRot), Time.deltaTime * f_ROT_UsedInterpSpeed);
    }


    #endregion


    #region Coroutine(s)



    #endregion


    #region ExternallySet




    #endregion


    #region Finalizer(s)




    #endregion

}
