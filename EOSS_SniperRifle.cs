/*	Author: Inan Evin
	www.inanevin.com
    
    Main logic class for specifically a sniper rifle behaviour.
    Written for th Extra-Ordinary Sniper System project.
*/

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class EOSS_SniperRifle : MonoBehaviour {

    #region Member Variable(s)

    [Header("General")]
    public Camera cam_MainCamera;
    public GameObject go_GNRL_WeaponMesh;
    private Vector3 v3_GNRL_CurrentPosition;
    private Transform t_ThisTransform;
    private Transform t_MainCameraTransform;
    private bool b_GNRL_IsAiming;


    [Header("Aim Settings")]
    public Transform t_AIM_AimTransform;
    public bool b_AIM_SwitchMeshMaterial;
    public bool b_AIM_ChangePosition;
    public bool b_AIM_ChangeRotation;
    public bool b_AIM_ChangeFOVAfter;
    public bool b_AIM_UseTexture;
    public bool b_AIM_CanAimWhileReloading;
    public float f_AIM_InSpeed;
    public float f_AIM_OutSpeed;
    public float f_AIM_TextureAppearTime;
    public float f_AIM_TextureDisappearTime;
    public float f_AIM_MaterialSwitchTimeIn;
    public float f_AIM_MaterialSwitchTimeOut;
    public float f_AIM_AimRate;
    public float f_AIM_TargetFOV;
    public float f_AIM_FOVChangeInSpeed;
    public float f_AIM_FOVChangeOutSpeed;
    public float f_AIM_CBB_POSMultiplier;
    public float f_AIM_CBB_ROTMultiplier;
    public float f_AIM_WBB_POSMultiplier;
    public float f_AIM_WBB_ROTMultiplier;
    public int i_AIM_MaterialIndex;
    public Vector3 v3_AIM_HipPosition;
    public Vector3 v3_AIM_AimPosition;
    public Vector3 v3_AIM_AimEuler;
    public Vector3 v3_AIM_HipEuler;
    public enum AimButtonStyle { Hold, Toggle};
    public AimButtonStyle en_AIM_ButtonStyle;
    public Camera cam_AIM_AimedCamera;
    public string s_AIM_ButtonName;
    public Material mat_AIM_ScopeRenderTexture;
    public Material mat_AIM_ScopeNormal;
    public Renderer rend_AIM_Target;
    public Image img_AIM_AimedTexture;


    private delegate void ReceiveAimInput();
    private ReceiveAimInput delvo_ReceiveAimInput;
    private delegate void INITAfterWait();
    private INITAfterWait delvo_INITAfterWait;
    private bool b_AIM_CanAim = true;
    private bool b_AIM_HoldFlag = false;
    private bool b_AIM_ToggleFlag = false;
    private Coroutine co_AIM_AimIn;
    private Coroutine co_AIM_AimOut;
    private Coroutine co_AIM_SwitchMaterial;
    private Coroutine co_AIM_ToggleTexture;
    private float f_AIM_LastAimed;
    private float f_AIM_CamFOV;

    [Header("Firing Settings")]
    public float f_FRE_FireRate;
    public enum FiringButtonStyle { Toggle, Hold};
    public FiringButtonStyle en_FRE_ButtonStyle;
    public string s_FRE_FireButton;
    public Animator anim_FRE_Weapon;
    public bool b_FRE_UseFireAnimation;
    public bool b_FRE_RandomizeAnimations;
    public string s_FRE_AnimTrigger;
    public string[] s_FRE_AnimTriggers;

    private int i_HASH_FRE_FireAnim;
    private int[] i_HASH_FRE_FireAnims;

    private bool b_FRE_CanFire = true;
    private float f_FRE_LastFired;
    private delegate void ReceiveFireInput();
    private ReceiveFireInput delvo_ReceiveFireInput;


    [Header("Camera Blowback Settings")]

    public bool b_CBB_UseCameraBlowback;
    public bool b_CBB_RandomizePositionBB;
    public bool b_CBB_RandomizeRotationBB;
    public enum CBB_POS_PowerAlteringType { ChangeLinearly, Randomize, Constant};
    public CBB_POS_PowerAlteringType en_CBB_POS_PAT;
    public enum CBB_ROT_PowerAlteringType { ChangeLinearly, Randomize, Constant };
    public CBB_POS_PowerAlteringType en_CBB_ROT_PAT;
    public enum CBB_TransformAffectType { Position, Rotation, Both};
    public CBB_TransformAffectType en_CBB_TAT;
    public Vector3 v3_CBB_Pos;
    public Vector3 v3_CBB_Eul;
    public Vector3 v3_CBB_MIN_Pos;
    public Vector3 v3_CBB_MAX_Pos;
    public Vector3 v3_CBB_MIN_Rot;
    public Vector3 v3_CBB_MAX_Rot;
    public Transform t_CBB_CameraBBTransform;

    public float f_CBB_POS_Power;
    public float f_CBB_POS_MIN_Power;
    public float f_CBB_POS_MAX_Power;
    public float f_CBB_POS_FROM_Power;
    public float f_CBB_POS_TO_Power;
    public float f_CBB_POS_PowerLinearSpeed;
    public float f_CBB_POS_PowerDecreaseSpeed;

    public float f_CBB_ROT_Power;
    public float f_CBB_ROT_MIN_Power;
    public float f_CBB_ROT_MAX_Power;
    public float f_CBB_ROT_FROM_Power;
    public float f_CBB_ROT_TO_Power;
    public float f_CBB_ROT_PowerLinearSpeed;
    public float f_CBB_ROT_PowerDecreaseSpeed;

    public float f_CBB_POS_InSpeed;
    public float f_CBB_POS_OutSpeed;
    public float f_CBB_ROT_InSpeed;
    public float f_CBB_ROT_OutSpeed;
    
    private Coroutine co_CBB_BB;
    private Coroutine co_CBB_DecreasePower;
    private Vector3 v3_CBB_InitialTransformPos;
    private Vector3 v3_CBB_InitialTransformEul;
    private float f_CBB_POS_PowerIndicator;
    private float f_CBB_ROT_PowerIndicator;

    [Header("Weapon Blowback Settings")]

    public bool b_WBB_UseWeaponBlowback;
    public bool b_WBB_RandomizePositionBB;
    public bool b_WBB_RandomizeRotationBB;
    public enum WBB_POS_PowerAlteringType { ChangeLinearly, Randomize, Constant };
    public WBB_POS_PowerAlteringType en_WBB_POS_PAT;
    public enum WBB_ROT_PowerAlteringType { ChangeLinearly, Randomize, Constant };
    public WBB_POS_PowerAlteringType en_WBB_ROT_PAT;
    public enum WBB_TransformAffectType { Position, Rotation, Both };
    public WBB_TransformAffectType en_WBB_TAT;
    public Vector3 v3_WBB_Pos;
    public Vector3 v3_WBB_Eul;
    public Vector3 v3_WBB_MIN_Pos;
    public Vector3 v3_WBB_MAX_Pos;
    public Vector3 v3_WBB_MIN_Rot;
    public Vector3 v3_WBB_MAX_Rot;
    public Transform t_WBB_CameraBBTransform;

    public float f_WBB_POS_Power;
    public float f_WBB_POS_MIN_Power;
    public float f_WBB_POS_MAX_Power;
    public float f_WBB_POS_FROM_Power;
    public float f_WBB_POS_TO_Power;
    public float f_WBB_POS_PowerLinearSpeed;
    public float f_WBB_POS_PowerDecreaseSpeed;

    public float f_WBB_ROT_Power;
    public float f_WBB_ROT_MIN_Power;
    public float f_WBB_ROT_MAX_Power;
    public float f_WBB_ROT_FROM_Power;
    public float f_WBB_ROT_TO_Power;
    public float f_WBB_ROT_PowerLinearSpeed;
    public float f_WBB_ROT_PowerDecreaseSpeed;

    public float f_WBB_POS_InSpeed;
    public float f_WBB_POS_OutSpeed;
    public float f_WBB_ROT_InSpeed;
    public float f_WBB_ROT_OutSpeed;

    private Coroutine co_WBB_BB;
    private Coroutine co_WBB_DecreasePower;
    private Vector3 v3_WBB_InitialTransformPos;
    private Vector3 v3_WBB_InitialTransformEul;
    private float f_WBB_POS_PowerIndicator;
    private float f_WBB_ROT_PowerIndicator;

    [Header("Accuracy Settings")]
    public bool b_HACC_SpreadAccuracy;
    public bool b_HACC_RandomizeAccuracy;
    public enum HipAccuracyPowerChangeType { ChangeLinearly, Randomize, Constant};
    public HipAccuracyPowerChangeType en_HACC_PowerChangeType;
    public Vector3 v3_HACC_Spread;
    public Vector3 v3_HACC_MIN_Spread;
    public Vector3 v3_HACC_MAX_Spread;
    public float f_HACC_Power;
    public float f_HACC_FROM_Power;
    public float f_HACC_TO_Power;
    public float f_HACC_MIN_Power;
    public float f_HACC_MAX_Power;
    public float f_HACC_PowerChangeInSpeed;
    public float f_HACC_PowerChangeBackSpeed;
    private float f_HACC_PowerIndicator;
    private Coroutine co_HACC_DecreasePower;

    [Header("Reload Settings")]
    public int i_RLD_BulletAmountInOneMagazine;
    public int i_RLD_INIT_BulletAmount;
    public enum AmmoRefreshType { Realistic, Economic};
    public AmmoRefreshType en_RLD_AmmoRefreshType;
    public int i_RLD_INIT_MagazineAmount;
    public int i_RLD_MAX_MagazineAmount;
    public Animator anim_RLD_WeaponAnimator;
    public string s_RLD_BoltActionAnimTrigger;
    public string s_RLD_MagazineAnimTrigger;
    public string[] s_RLD_BoltActionAnimTriggers;
    public string[] s_RLD_MagazineAnimTriggers;
    public string s_RLD_ReloadButton;
    public enum BoltReloadINITType { OnReloadButton, OnFireButton, OnReloadOrFireButton, Automatic };
    public BoltReloadINITType en_RLD_BoltReloadINITType;
    public enum MagReloadINITType { OnReloadButton, OnFireButton, OnReloadOrFireButton, Automatic };
    public MagReloadINITType en_RLD_MagReloadINITType;
    public float f_RLD_BoltActionTime;
    public float[] f_RLD_BoltActionTimes;
    public float f_RLD_MagazineTime;
    public float[] f_RLD_MagazineTimes;
    public float f_RLD_WaitBeforeBoltAutoReload;
    public float f_RLD_WaitBeforeMagAutoReload;

    public bool b_RLD_UseBoltAction;
    public bool b_RLD_RandomizeBoltActionAnimations;
    public bool b_RLD_RandomizeMagazineAnimations;
    public bool b_RLD_CheckIsAimingOnBoltActionReload;
    public bool b_RLD_CheckIsAimingOnMagazineReload;

    private int i_RLD_CURR_MagazineCount;
    private int i_RLD_CURR_BulletCount;
    private int i_RLD_TotalBulletCount;
    private int i_HASH_RLD_BoltAction;
    private int i_HASH_RLD_Magazine;
    private int[] i_HASH_RLD_BoltActions;
    private int[] i_HASH_RLD_Magazines;

    private bool b_RLD_IsBoltActionReloading;
    private bool b_RLD_IsMagazineReloading;
    private bool b_RLD_IsInReload;
    private Coroutine co_RLD_BoltActionReload;
    private Coroutine co_RLD_MagazineReload;


    [Header("Raycast Settings")]
    public float f_RC_RifleDistance;
    public LayerMask lm_RC_InclusionMask;
    public float f_RC_XUnit;

    [Header("Bullet Drop Settings")]
    public float f_BLD_DropPerXUnits;

    [Header("Bullet Delay Settings")]
    public float f_BLDE_DelayPerXUnits;

    [Header("Bullet Penetration Settings")]
    public bool b_PNT_UsePenetration;
    public float f_PNT_PenetrationPower;
    public enum PenetratedBulletSpreadDirection { SameDirection, RandomDirection };
    public PenetratedBulletSpreadDirection en_PNT_PenetratedSpreadDir;
    public bool b_PNT_IsPowerAffectedByDistance;
    public float f_PNT_XUnits;
    public float f_PNT_PowerDropPerXUnits;
    public float f_PNT_PenetrableThickness;
    public float f_PNT_PenetratedRayDistancePercentage;
    public float f_PNT_RayPointsOffset;
    public Vector3 v3_PNT_MIN_DirSpread;
    public Vector3 v3_PNT_MAX_DirSpread;

    [Header("Bullet Ricochet Settings")]
    public bool b_RCH_EnableRicochet;
    public float f_RCH_RicochetBulletRayDistancePercentage;
    public int i_RCH_MaxRicochetCount;
    public enum RicochetBulletDirectionType { VectorReflect, Random};
    public RicochetBulletDirectionType en_RCH_BulletDirType;
    [Range(0.01f, 1.0f)]
    public float f_RCH_RicochetChance;
    public float f_RCH_RicochetPower;
    public bool b_RCH_IsPowerAffectedByDistance;
    public float f_RCH_XUnits;
    public float f_RCH_PowerDropPerXUnits;

    [Header("Bullet Effect Settings")]
    public GameObject go_BLE_BulletPrefab;
    public bool b_BLE_UseObjectPooling;
    public bool b_BLE_Enabled;
    public EOSS_BulletEffectPooler _BLE_Pooler;
    public float f_BLE_NoHitTime;
    public Transform t_BLE_BulletExtractPosition;
    public float f_BLE_BulletEffectChance;
    public enum AppearType { AllShots, EmptyShotsOnly, HitsOnly };

    [Header("Debug Settings")]
    public bool b_DBG_RayDebugEnabled;
    public bool b_DBG_DebugShootRays;
    public bool b_DBG_DebugPenetrationRays;
    public bool b_DBG_DebugRicochetRay;
    public Color col_DBG_InitialShootRayColor;
    public Color col_DBG_InitialPenetrationRayColor;
    public Color col_DBG_ReverseRayColor;
    public Color col_DBG_PenetratedRayColor;
    public Color col_DBG_RicochetRayColor;

    private struct BlowbackData
    {
        public Transform t_BBD_Target;
        public bool b_BBD_PosBB;
        public bool b_BBD_RotBB;
        public bool b_BDD_CBB_POS_DecreasePower;
        public bool b_BDD_CBB_ROT_DecreasePower;
        public bool b_BDD_WBB_POS_DecreasePower;
        public bool b_BDD_WBB_ROT_DecreasePower;
        public Vector3 v3_BDD_TargetPos;
        public Vector3 v3_BDD_TargetEul;
        public Vector3 v3_BDD_InitialPos;
        public Vector3 v3_BDD_InitialEul;
        public float f_BDD_POS_InSpeed;
        public float f_BDD_POS_OutSpeed;
        public float f_BDD_ROT_InSpeed;
        public float f_BDD_ROT_OutSpeed;
        public float f_BDD_POS_Power;
        public float f_BDD_ROT_Power;
        public float f_BDD_POS_BLW_Power;
        public float f_BDD_ROT_BLW_Power;
        public float f_BDD_POS_PWR_DecreaseSpeed;
        public float f_BDD_ROT_PWR_DecreaseSpeed;

        }

    #endregion


    #region Initializer(s)

    void Awake()
    {
        t_ThisTransform = transform;
        t_MainCameraTransform = cam_MainCamera.transform;
        f_AIM_LastAimed = Time.time;
        f_FRE_LastFired = Time.time;
        f_BLDE_DelayPerXUnits /= 1000.0f;
        f_BLD_DropPerXUnits /= 1000.0f;

        if (b_AIM_SwitchMeshMaterial)
        {
            Material[] mats = rend_AIM_Target.materials;
            mats[i_AIM_MaterialIndex] = mat_AIM_ScopeNormal;
            rend_AIM_Target.materials = mats;
        }

        if (b_AIM_UseTexture)
            img_AIM_AimedTexture.enabled = false;

        if (en_AIM_ButtonStyle == AimButtonStyle.Hold)
            delvo_ReceiveAimInput = AIM_ReceiveHoldInput;
        else
            delvo_ReceiveAimInput = AIM_ReceiveToggleInput;

        if (en_FRE_ButtonStyle == FiringButtonStyle.Hold)
            delvo_ReceiveFireInput = FRE_ReceiveHoldInput;
        else
            delvo_ReceiveFireInput = FRE_ReceiveToggleInput;

        if(en_CBB_TAT == CBB_TransformAffectType.Position || en_CBB_TAT == CBB_TransformAffectType.Both)
        {
            if (en_CBB_POS_PAT == CBB_POS_PowerAlteringType.ChangeLinearly)
                f_CBB_POS_Power = f_CBB_POS_FROM_Power;

            if ((f_CBB_POS_TO_Power - f_CBB_POS_FROM_Power) < 0)
                f_CBB_POS_PowerIndicator = -1;
            else
                f_CBB_POS_PowerIndicator = 1;

        }

        if (en_CBB_TAT == CBB_TransformAffectType.Rotation || en_CBB_TAT == CBB_TransformAffectType.Both)
        {
            if (en_CBB_ROT_PAT == CBB_POS_PowerAlteringType.ChangeLinearly)
                f_CBB_ROT_Power = f_CBB_ROT_FROM_Power;

            if ((f_CBB_ROT_TO_Power - f_CBB_ROT_FROM_Power) < 0)
                f_CBB_ROT_PowerIndicator = -1;
            else
                f_CBB_ROT_PowerIndicator = 1;
        }

        if (en_WBB_TAT == WBB_TransformAffectType.Position || en_WBB_TAT == WBB_TransformAffectType.Both)
        {
            if (en_WBB_POS_PAT == WBB_POS_PowerAlteringType.ChangeLinearly)
                f_WBB_POS_Power = f_WBB_POS_FROM_Power;

            if ((f_WBB_POS_TO_Power - f_WBB_POS_FROM_Power) < 0)
                f_WBB_POS_PowerIndicator = -1;
            else
                f_WBB_POS_PowerIndicator = 1;

        }

        if (en_WBB_TAT == WBB_TransformAffectType.Rotation || en_WBB_TAT == WBB_TransformAffectType.Both)
        {
            if (en_WBB_ROT_PAT == WBB_POS_PowerAlteringType.ChangeLinearly)
                f_WBB_ROT_Power = f_WBB_ROT_FROM_Power;


            if ((f_WBB_ROT_TO_Power - f_WBB_ROT_FROM_Power) < 0)
                f_WBB_ROT_PowerIndicator = -1;
            else
                f_WBB_ROT_PowerIndicator = 1;
        }

        if (b_HACC_SpreadAccuracy)
        {
            if (en_HACC_PowerChangeType == HipAccuracyPowerChangeType.ChangeLinearly)
                f_HACC_Power = f_HACC_FROM_Power;

            if ((f_HACC_TO_Power - f_HACC_FROM_Power) < 0)
                f_HACC_PowerIndicator = -1;
            else
                f_HACC_PowerIndicator = 1;
        }

        i_RLD_CURR_MagazineCount = i_RLD_INIT_MagazineAmount - 1;
        i_RLD_CURR_BulletCount = i_RLD_INIT_BulletAmount;
        i_RLD_TotalBulletCount = ((i_RLD_INIT_MagazineAmount - 1) * i_RLD_BulletAmountInOneMagazine) + i_RLD_INIT_BulletAmount;
        i_HASH_RLD_BoltAction = Animator.StringToHash(s_RLD_BoltActionAnimTrigger);
        i_HASH_RLD_Magazine = Animator.StringToHash(s_RLD_MagazineAnimTrigger);

        if (b_RLD_RandomizeBoltActionAnimations)
            StartCoroutine(CO_RLD_RegisterHashArrayForBoltActionAnimations());
        if (b_RLD_RandomizeMagazineAnimations)
            StartCoroutine(CO_RLD_RegisterHashArrayForMagazineAnimations());
        if (b_FRE_UseFireAnimation && b_FRE_RandomizeAnimations)
            StartCoroutine(CO_FRE_RegisterHashArrayForFireAnimations());

        i_HASH_FRE_FireAnim = Animator.StringToHash(s_FRE_AnimTrigger);

        v3_CBB_InitialTransformPos = t_CBB_CameraBBTransform.localPosition;
        v3_CBB_InitialTransformEul = t_CBB_CameraBBTransform.localEulerAngles;

        v3_WBB_InitialTransformPos = t_WBB_CameraBBTransform.localPosition;
        v3_WBB_InitialTransformEul = t_WBB_CameraBBTransform.localEulerAngles;



    }

    #endregion


    #region Control(s)

    void Update()
    {
        if ((b_AIM_CanAim) && (b_RLD_IsBoltActionReloading && b_AIM_CanAimWhileReloading) || (!b_RLD_IsBoltActionReloading))
            delvo_ReceiveAimInput();

        if (b_FRE_CanFire && !b_RLD_IsInReload)
            delvo_ReceiveFireInput();

        RLD_ReceiveReloadInput();

        if (Input.GetKeyDown("k"))
            Time.timeScale = 0.03f;
        if (Input.GetKeyDown("l"))
            Time.timeScale = 1.0f;
    }



    #region AIM Control(s)

    void AIM_SwitchMeshMaterial(Material targetMat, bool camEnableStatus)
    {
        Material[] mats = rend_AIM_Target.materials;
        cam_AIM_AimedCamera.enabled = camEnableStatus;
        mats[i_AIM_MaterialIndex] = targetMat;
        rend_AIM_Target.materials = mats;
    }

    void AIM_ToggleTexture(bool on)
    {
        if(on)
        {
            go_GNRL_WeaponMesh.SetActive(false);
            img_AIM_AimedTexture.enabled = true;
        }
        else
        {
            go_GNRL_WeaponMesh.SetActive(true);
            img_AIM_AimedTexture.enabled = false;
        }
    }


    void AIM_ReceiveHoldInput()
    {
        if (Input.GetButton(s_AIM_ButtonName) && !b_AIM_HoldFlag && Time.time > f_AIM_LastAimed + f_AIM_AimRate)
        {
            f_AIM_LastAimed = Time.time;

            if (co_AIM_AimOut != null)
                StopCoroutine(co_AIM_AimOut);

            co_AIM_AimIn = StartCoroutine(CO_AIM_AimIn());
            b_AIM_HoldFlag = true;
        }

        if (Input.GetButtonUp(s_AIM_ButtonName))
        {
            if (co_AIM_AimIn != null)
                StopCoroutine(co_AIM_AimIn);

            co_AIM_AimOut = StartCoroutine(CO_AIM_AimOut());
            b_AIM_HoldFlag = false;
        }
    }

    void AIM_ReceiveToggleInput()
    {
        if (Input.GetButtonDown(s_AIM_ButtonName) && Time.time > f_AIM_LastAimed + f_AIM_AimRate)
        {
            f_AIM_LastAimed = Time.time;
            b_AIM_ToggleFlag = !b_AIM_ToggleFlag;

            if (b_AIM_ToggleFlag)
            {
                if (co_AIM_AimOut != null)
                    StopCoroutine(co_AIM_AimOut);

                co_AIM_AimIn = StartCoroutine(CO_AIM_AimIn());
            }
            else
            {
                if (co_AIM_AimIn != null)
                    StopCoroutine(co_AIM_AimIn);

                co_AIM_AimOut = StartCoroutine(CO_AIM_AimOut());
            }
        }
    }

    #endregion

    #region Fire Control(s)

    void FRE_ReceiveHoldInput()
    {
        if(Input.GetButton(s_FRE_FireButton) && Time.time > f_FRE_LastFired + f_FRE_FireRate)
        {
            if(!b_RLD_IsBoltActionReloading && !b_RLD_IsMagazineReloading)
            {
                f_FRE_LastFired = Time.time;
                RLD_CheckAmmo();
            }
            else if(b_RLD_IsBoltActionReloading && (en_RLD_BoltReloadINITType == BoltReloadINITType.OnFireButton || en_RLD_BoltReloadINITType == BoltReloadINITType.OnReloadOrFireButton) && i_RLD_CURR_MagazineCount > 0)
                co_RLD_BoltActionReload = StartCoroutine(CO_RLD_BoltActionReload());
            else if (b_RLD_IsMagazineReloading && (en_RLD_MagReloadINITType == MagReloadINITType.OnFireButton || en_RLD_MagReloadINITType == MagReloadINITType.OnReloadOrFireButton) && i_RLD_CURR_MagazineCount > 0)
                co_RLD_MagazineReload = StartCoroutine(CO_RLD_MagazineReload());
        }
    }

    void FRE_ReceiveToggleInput()
    {
        if (Input.GetButtonDown(s_FRE_FireButton) && Time.time > f_FRE_LastFired + f_FRE_FireRate)
        {
            if (!b_RLD_IsBoltActionReloading && !b_RLD_IsMagazineReloading)
            {
                f_FRE_LastFired = Time.time;
                RLD_CheckAmmo();
            }
            else if (b_RLD_IsBoltActionReloading && (en_RLD_BoltReloadINITType == BoltReloadINITType.OnFireButton || en_RLD_BoltReloadINITType == BoltReloadINITType.OnReloadOrFireButton) && i_RLD_CURR_MagazineCount > 0)
                co_RLD_BoltActionReload = StartCoroutine(CO_RLD_BoltActionReload());
            else if (b_RLD_IsMagazineReloading && (en_RLD_MagReloadINITType == MagReloadINITType.OnFireButton || en_RLD_MagReloadINITType == MagReloadINITType.OnReloadOrFireButton) && i_RLD_CURR_MagazineCount > 0)
                co_RLD_MagazineReload = StartCoroutine(CO_RLD_MagazineReload());

        }
    }


    void FRE_Fire()
    {
        // Anim

        if(b_FRE_UseFireAnimation)
        {
            int triggerHash;

            if (b_FRE_RandomizeAnimations)
            {
                int index = UnityEngine.Random.Range(0, i_HASH_FRE_FireAnims.Length);
                triggerHash = i_HASH_FRE_FireAnims[index];
            }
            else
                triggerHash = i_HASH_FRE_FireAnim;

            anim_FRE_Weapon.SetTrigger(triggerHash);
        }

        // BlowBacks
        
        if(b_CBB_UseCameraBlowback)
        {
            BlowbackData bbData = new BlowbackData();
            bbData.t_BBD_Target = t_CBB_CameraBBTransform;

            if (en_CBB_TAT == CBB_TransformAffectType.Position || en_CBB_TAT == CBB_TransformAffectType.Both)
            {
                if (en_CBB_POS_PAT == CBB_POS_PowerAlteringType.ChangeLinearly)
                {
                    f_CBB_POS_Power += f_CBB_POS_PowerLinearSpeed * f_CBB_POS_PowerIndicator;

                    if ((f_CBB_POS_TO_Power - f_CBB_POS_Power) * f_CBB_POS_PowerIndicator < 0)
                        f_CBB_POS_Power = f_CBB_POS_TO_Power;

                    bbData.b_BDD_CBB_POS_DecreasePower = true;
                    bbData.f_BDD_POS_BLW_Power = f_CBB_POS_FROM_Power;
                    bbData.f_BDD_POS_PWR_DecreaseSpeed = f_CBB_POS_PowerDecreaseSpeed;
                }
                else if (en_CBB_POS_PAT == CBB_POS_PowerAlteringType.Randomize)
                    f_CBB_POS_Power = UnityEngine.Random.Range(f_CBB_POS_MIN_Power, f_CBB_POS_MAX_Power);



                Vector3 newPosBB;
                if (b_CBB_RandomizePositionBB)
                {
                    newPosBB = new Vector3(
                    UnityEngine.Random.Range(v3_CBB_MIN_Pos.x, v3_CBB_MAX_Pos.x),
                    UnityEngine.Random.Range(v3_CBB_MIN_Pos.y, v3_CBB_MAX_Pos.y),
                    UnityEngine.Random.Range(v3_CBB_MIN_Pos.z, v3_CBB_MAX_Pos.z)
                    );
                }
                else
                    newPosBB = v3_CBB_Pos;

                bbData.f_BDD_POS_Power = f_CBB_POS_Power;
                newPosBB *= f_CBB_POS_Power;
                if (b_GNRL_IsAiming)
                    newPosBB *= f_AIM_CBB_POSMultiplier;
                bbData.v3_BDD_TargetPos = newPosBB;
                bbData.b_BBD_PosBB = true;
                bbData.f_BDD_POS_InSpeed = f_CBB_POS_InSpeed;
                bbData.f_BDD_POS_OutSpeed = f_CBB_POS_OutSpeed;
                bbData.v3_BDD_InitialPos = v3_CBB_InitialTransformPos;
            }

            if (en_CBB_TAT == CBB_TransformAffectType.Rotation || en_CBB_TAT == CBB_TransformAffectType.Both)
            {
                if (en_CBB_ROT_PAT == CBB_POS_PowerAlteringType.ChangeLinearly)
                {
                    f_CBB_ROT_Power += f_CBB_ROT_PowerLinearSpeed * f_CBB_ROT_PowerIndicator;

                    if ((f_CBB_ROT_TO_Power - f_CBB_ROT_Power) * f_CBB_ROT_PowerIndicator < 0)
                        f_CBB_ROT_Power = f_CBB_ROT_TO_Power;

                    bbData.b_BDD_CBB_ROT_DecreasePower = true;
                    bbData.f_BDD_ROT_BLW_Power = f_CBB_ROT_FROM_Power;
                    bbData.f_BDD_ROT_PWR_DecreaseSpeed = f_CBB_ROT_PowerDecreaseSpeed;
                }
                else if (en_CBB_ROT_PAT == CBB_POS_PowerAlteringType.Randomize)
                    f_CBB_ROT_Power = UnityEngine.Random.Range(f_CBB_ROT_MIN_Power, f_CBB_ROT_MAX_Power);

                Vector3 newRotBB;
                if (b_CBB_RandomizeRotationBB)
                {
                    newRotBB = new Vector3(
                    UnityEngine.Random.Range(v3_CBB_MIN_Rot.x, v3_CBB_MAX_Rot.x),
                    UnityEngine.Random.Range(v3_CBB_MIN_Rot.y, v3_CBB_MAX_Rot.y),
                    UnityEngine.Random.Range(v3_CBB_MIN_Rot.z, v3_CBB_MAX_Rot.z)
                    );
                }
                else
                    newRotBB = v3_CBB_Eul;

                bbData.f_BDD_ROT_Power = f_CBB_ROT_Power;
                newRotBB *= f_CBB_ROT_Power;
                if (b_GNRL_IsAiming)
                    newRotBB *= f_AIM_CBB_ROTMultiplier;
                bbData.v3_BDD_TargetEul = newRotBB;
                bbData.b_BBD_RotBB = true;
                bbData.f_BDD_ROT_InSpeed = f_CBB_ROT_InSpeed;
                bbData.f_BDD_ROT_OutSpeed = f_CBB_ROT_OutSpeed;
                bbData.v3_BDD_InitialEul = v3_CBB_InitialTransformEul;
            }

            if (co_CBB_BB != null)
                StopCoroutine(co_CBB_BB);

            co_CBB_BB = StartCoroutine(CO_Blowback(bbData));
        }

        if (b_WBB_UseWeaponBlowback)
        {
            if(!b_GNRL_IsAiming || (b_GNRL_IsAiming && !b_AIM_UseTexture))
            {

                BlowbackData bbData = new BlowbackData();
                bbData.t_BBD_Target = t_WBB_CameraBBTransform;

                if (en_WBB_TAT == WBB_TransformAffectType.Position || en_WBB_TAT == WBB_TransformAffectType.Both)
                {
                    if (en_WBB_POS_PAT == WBB_POS_PowerAlteringType.ChangeLinearly)
                    {
                        f_WBB_POS_Power += f_WBB_POS_PowerLinearSpeed * f_WBB_POS_PowerIndicator;

                        if ((f_WBB_POS_TO_Power - f_WBB_POS_Power) * f_WBB_POS_PowerIndicator < 0)
                            f_WBB_POS_Power = f_WBB_POS_TO_Power;

                        bbData.b_BDD_WBB_POS_DecreasePower = true;
                        bbData.f_BDD_POS_BLW_Power = f_WBB_POS_FROM_Power;
                        bbData.f_BDD_POS_PWR_DecreaseSpeed = f_WBB_POS_PowerDecreaseSpeed;
                    }
                    else if (en_WBB_POS_PAT == WBB_POS_PowerAlteringType.Randomize)
                        f_WBB_POS_Power = UnityEngine.Random.Range(f_WBB_POS_MIN_Power, f_WBB_POS_MAX_Power);

                    Vector3 newPosBB;
                    if (b_WBB_RandomizePositionBB)
                    {
                        newPosBB = new Vector3(
                        UnityEngine.Random.Range(v3_WBB_MIN_Pos.x, v3_WBB_MAX_Pos.x),
                        UnityEngine.Random.Range(v3_WBB_MIN_Pos.y, v3_WBB_MAX_Pos.y),
                        UnityEngine.Random.Range(v3_WBB_MIN_Pos.z, v3_WBB_MAX_Pos.z)
                        );
                    }
                    else
                        newPosBB = v3_WBB_Pos;

                    bbData.f_BDD_POS_Power = f_WBB_POS_Power;
                    newPosBB *= f_WBB_POS_Power;
                    if (b_GNRL_IsAiming)
                        newPosBB *= f_AIM_WBB_POSMultiplier;
                    bbData.v3_BDD_TargetPos = newPosBB;
                    bbData.b_BBD_PosBB = true;
                    bbData.f_BDD_POS_InSpeed = f_WBB_POS_InSpeed;
                    bbData.f_BDD_POS_OutSpeed = f_WBB_POS_OutSpeed;
                    bbData.v3_BDD_InitialPos = v3_WBB_InitialTransformPos;
                }

                if (en_WBB_TAT == WBB_TransformAffectType.Rotation || en_WBB_TAT == WBB_TransformAffectType.Both)
                {
                    if (en_WBB_ROT_PAT == WBB_POS_PowerAlteringType.ChangeLinearly)
                    {
                        f_WBB_ROT_Power += f_WBB_ROT_PowerLinearSpeed * f_WBB_ROT_PowerIndicator;

                        if ((f_WBB_ROT_TO_Power - f_WBB_ROT_Power) * f_WBB_ROT_PowerIndicator < 0)
                            f_WBB_ROT_Power = f_WBB_ROT_TO_Power;

                        bbData.b_BDD_WBB_ROT_DecreasePower = true;
                        bbData.f_BDD_ROT_BLW_Power = f_WBB_ROT_FROM_Power;
                        bbData.f_BDD_ROT_PWR_DecreaseSpeed = f_WBB_ROT_PowerDecreaseSpeed;
                    }
                    else if (en_WBB_ROT_PAT == WBB_POS_PowerAlteringType.Randomize)
                        f_WBB_ROT_Power = UnityEngine.Random.Range(f_WBB_ROT_MIN_Power, f_WBB_ROT_MAX_Power);

                    Vector3 newRotBB;
                    if (b_WBB_RandomizeRotationBB)
                    {
                        newRotBB = new Vector3(
                        UnityEngine.Random.Range(v3_WBB_MIN_Rot.x, v3_WBB_MAX_Rot.x),
                        UnityEngine.Random.Range(v3_WBB_MIN_Rot.y, v3_WBB_MAX_Rot.y),
                        UnityEngine.Random.Range(v3_WBB_MIN_Rot.z, v3_WBB_MAX_Rot.z)
                        );
                    }
                    else
                        newRotBB = v3_WBB_Eul;

                    bbData.f_BDD_ROT_Power = f_WBB_ROT_Power;
                    newRotBB *= f_WBB_ROT_Power;
                    if (b_GNRL_IsAiming)
                        newRotBB *= f_AIM_WBB_POSMultiplier;
                    bbData.v3_BDD_TargetEul = newRotBB;
                    bbData.b_BBD_RotBB = true;
                    bbData.f_BDD_ROT_InSpeed = f_WBB_ROT_InSpeed;
                    bbData.f_BDD_ROT_OutSpeed = f_WBB_ROT_OutSpeed;
                    bbData.v3_BDD_InitialEul = v3_WBB_InitialTransformEul;
                }

                if (co_WBB_BB != null)
                    StopCoroutine(co_WBB_BB);

                co_WBB_BB = StartCoroutine(CO_Blowback(bbData));
            }

            
        }

        // Audio && Effects

        FRE_INITSound();
        FRE_INITEffects();
        // Raycast
        Vector3 accuracySpread = HACC_CalculateAccuracy();
        Ray newRay = cam_MainCamera.ViewportPointToRay(new Vector3(0.5f + accuracySpread.x, 0.5f + accuracySpread.y, 0));

        GameObject bullet;

        if (b_BLE_Enabled)
            bullet = Instantiate(go_BLE_BulletPrefab, t_BLE_BulletExtractPosition.position, t_BLE_BulletExtractPosition.rotation) as GameObject;
        else
            bullet = null;
        StartCoroutine(CO_RC_CastRays(newRay, f_PNT_PenetrationPower, f_RCH_RicochetPower, bullet));

    }

    void FRE_INITSound()
    {

    }

    void FRE_INITEffects()
    {

    }




    #endregion

    #region Reload Control(s)

    void RLD_ReceiveReloadInput()
    {
        if(Input.GetButtonDown(s_RLD_ReloadButton) && !b_RLD_IsInReload)
        {
            if(b_RLD_IsBoltActionReloading && (en_RLD_BoltReloadINITType == BoltReloadINITType.OnReloadButton || en_RLD_BoltReloadINITType == BoltReloadINITType.OnReloadOrFireButton) && i_RLD_CURR_MagazineCount > 0)
                co_RLD_BoltActionReload = StartCoroutine(CO_RLD_BoltActionReload());
            else if (b_RLD_IsMagazineReloading && (en_RLD_MagReloadINITType == MagReloadINITType.OnReloadButton || en_RLD_MagReloadINITType == MagReloadINITType.OnReloadOrFireButton) && i_RLD_CURR_MagazineCount > 0)
                co_RLD_MagazineReload = StartCoroutine(CO_RLD_MagazineReload());
            else if(!b_RLD_IsMagazineReloading && !b_RLD_IsBoltActionReloading)
            {
                if (i_RLD_CURR_MagazineCount > 0)
                {
                    if(i_RLD_CURR_BulletCount == i_RLD_BulletAmountInOneMagazine)
                    {
                        if(en_RLD_AmmoRefreshType == AmmoRefreshType.Realistic)
                            co_RLD_MagazineReload = StartCoroutine(CO_RLD_MagazineReload());
                    }
                    else
                        co_RLD_MagazineReload = StartCoroutine(CO_RLD_MagazineReload());

                }
            }
            
           
        }
    }

    void RLD_CheckAmmo()
    {

        if (i_RLD_CURR_BulletCount > 0)
        {
            i_RLD_CURR_BulletCount--;
            i_RLD_TotalBulletCount--;
            FRE_Fire();

            if (b_RLD_UseBoltAction)
            {
                b_RLD_IsBoltActionReloading = true;

                if (en_RLD_BoltReloadINITType == BoltReloadINITType.Automatic)
                    co_RLD_BoltActionReload = StartCoroutine(CO_RLD_BoltActionReload());

            }

            if(i_RLD_CURR_BulletCount == 0)
            {
                b_RLD_IsMagazineReloading = true;

                if (en_RLD_MagReloadINITType == MagReloadINITType.Automatic && i_RLD_CURR_MagazineCount > 0)
                    co_RLD_MagazineReload = StartCoroutine(CO_RLD_MagazineReload());
            }
        }
        else
        {
            b_RLD_IsMagazineReloading = true;

            if (en_RLD_MagReloadINITType == MagReloadINITType.Automatic && i_RLD_CURR_MagazineCount > 0)
                co_RLD_MagazineReload = StartCoroutine(CO_RLD_MagazineReload());
        }

    }

    #endregion

    #region Accuracy Control(s)

    Vector3 HACC_CalculateAccuracy()
    {
        if (b_GNRL_IsAiming || !b_HACC_SpreadAccuracy)
            return Vector3.zero;

        Vector3 newAcc;

        if (b_HACC_RandomizeAccuracy)
        {
            newAcc = new Vector3(
                UnityEngine.Random.Range(v3_HACC_MIN_Spread.x, v3_HACC_MAX_Spread.x),
                UnityEngine.Random.Range(v3_HACC_MIN_Spread.y, v3_HACC_MAX_Spread.y),
                UnityEngine.Random.Range(v3_HACC_MIN_Spread.z, v3_HACC_MAX_Spread.z)
                    );
        }
        else
            newAcc = v3_HACC_Spread;

        if (en_HACC_PowerChangeType == HipAccuracyPowerChangeType.ChangeLinearly)
        {

            f_HACC_Power += f_HACC_PowerChangeInSpeed * f_HACC_PowerIndicator;
            if ((f_HACC_TO_Power - f_HACC_Power) * f_HACC_PowerIndicator < 0)
                f_HACC_Power = f_HACC_TO_Power;

        }
        else if (en_HACC_PowerChangeType == HipAccuracyPowerChangeType.Randomize)
            f_HACC_Power = UnityEngine.Random.Range(f_HACC_MIN_Power, f_HACC_MAX_Power);

        return newAcc * f_HACC_Power;
    }

    #endregion

    #region Raycast Control(s)


    void RC_ApplySettings(RaycastHit hit, Vector3 inputDirection, float penetrationPower, float ricochetPower, GameObject bullet)
    {
        HIT_CastHit(hit.point, false);

        if (b_PNT_UsePenetration)
            PNT_ApplyPenetration(hit, Vector3.zero, inputDirection, penetrationPower, ricochetPower, bullet);

        if(b_RCH_EnableRicochet)
            RCH_CheckRicochet(hit, inputDirection, penetrationPower, ricochetPower, bullet);

       
    }



    #endregion

    #region Penetration Control(s)

    void PNT_ApplyPenetration(RaycastHit hit, Vector3 from, Vector3 inputDirection, float penetrationPower, float ricochetPower, GameObject bullet)
    {
        if (hit.transform.GetComponent<EOSS_PenetrableObject>() != null)
        {
            EOSS_PenetrableObject obj = hit.transform.GetComponent<EOSS_PenetrableObject>();
            float currentPower = penetrationPower;
            if (b_PNT_IsPowerAffectedByDistance)
            {
                float distance = (hit.point - t_MainCameraTransform.position).magnitude;
                currentPower -= (distance / f_PNT_XUnits) * f_PNT_PowerDropPerXUnits;
            }

            if (currentPower > obj.f_Durability)
            {
                currentPower -= obj.f_HitBulletPowerDecrease;

                Vector3 firstPoint = hit.point + inputDirection * f_PNT_RayPointsOffset;
                Vector3 firstDirection = inputDirection;
                Ray firstRay = new Ray(firstPoint, firstDirection);
                RaycastHit firstHit;

                Vector3 reversePoint;
                Vector3 reverseDirection = inputDirection * -1;

                if(b_DBG_RayDebugEnabled && b_DBG_DebugPenetrationRays)
                    Debug.DrawRay(firstRay.origin , firstRay.direction * f_PNT_PenetrableThickness, col_DBG_InitialPenetrationRayColor, 15.0f);

                if (Physics.Raycast(firstRay, out firstHit, f_PNT_PenetrableThickness, lm_RC_InclusionMask))
                    reversePoint = firstHit.point;

                else
                    reversePoint = hit.point + inputDirection * f_PNT_PenetrableThickness;

                RaycastHit reverseHit;
                Ray reverseRay = new Ray(reversePoint, reverseDirection);

                if (b_DBG_RayDebugEnabled && b_DBG_DebugPenetrationRays)
                    Debug.DrawRay(reverseRay.origin, reverseRay.direction * f_PNT_PenetrableThickness, Color.blue, 15.0f);

                if (Physics.Raycast(reverseRay, out reverseHit, f_PNT_PenetrableThickness, lm_RC_InclusionMask))
                {
                    HIT_CastHit(reverseHit.point, true);
                    Vector3 point = reverseHit.point;
                    Vector3 direction = reverseRay.direction * -1;
                    
                    if (en_PNT_PenetratedSpreadDir == PenetratedBulletSpreadDirection.RandomDirection)
                    {
                        Vector3 randVector = new Vector3(
                            UnityEngine.Random.Range(v3_PNT_MIN_DirSpread.x, v3_PNT_MAX_DirSpread.x),
                            UnityEngine.Random.Range(v3_PNT_MIN_DirSpread.y, v3_PNT_MAX_DirSpread.y),
                            UnityEngine.Random.Range(v3_PNT_MIN_DirSpread.z, v3_PNT_MAX_DirSpread.z)
                            );

                        Vector3 randomPoint = point + direction * Mathf.Abs(randVector.z);
                        randomPoint += new Vector3(randVector.x, randVector.y, 0);
                        direction = randomPoint - point;
                    }

                    float bulletEffectDistance = f_RC_RifleDistance;
                    float bulletEffectTime = f_BLE_NoHitTime;
                    Ray penetratedRay = new Ray(point, direction);
                    Vector3 toPoint = penetratedRay.origin + penetratedRay.direction * currentPower / f_PNT_PenetratedRayDistancePercentage;
                    if (b_DBG_RayDebugEnabled && b_DBG_DebugPenetrationRays)
                        Debug.DrawRay(penetratedRay.origin, penetratedRay.direction * currentPower / f_PNT_PenetratedRayDistancePercentage, col_DBG_PenetratedRayColor, 15.0f);

                    StartCoroutine(CO_RC_CastRays(penetratedRay, currentPower, ricochetPower, bullet));
                    /*
                    if (Physics.Raycast(penetratedRay, out penetratedHit, currentPower * f_RC_RifleDistance / f_PNT_PenetratedRayDistancePercentage, lm_RC_InclusionMask))
                    {
                        
                        RC_ApplySettings(penetratedHit, penetratedRay.direction, currentPower);
                        toPoint = penetratedHit.point;
                        bulletEffectDistance = penetratedHit.distance;
                        bulletEffectTime = BLE_GetBulletTransportTime(bulletEffectDistance);
                    }
                    else
                    {
                       if (b_BLE_EnableBulletEffect && BLE_CheckBulletEffectAppearChance() && BLE_CheckBulletEffectDistance(bulletEffectDistance))
                        {
                            int distance = Mathf.RoundToInt(((toPoint - t_BLE_BulletExtractPosition.position).magnitude));
                            int bezierSize = Mathf.RoundToInt(distance / f_BLE_MidPointDivider);
                            StartCoroutine(CO_BLE_BulletTransport(penetratedRay.origin, toPoint, t_MainCameraTransform.rotation, bulletEffectTime, bezierSize));
                        }
                    }*/

                }
            }
            else
            {
               /* if (b_BLE_EnableBulletEffect)
                    HIT_CheckHit(hit, inputDirection, false, from, hit.point, hit.distance, BLE_GetBulletTransportTime(hit.distance));
                else
                    HIT_CheckHit(hit, inputDirection, false);
                if (b_RCH_EnableRicochet && ricochetAfterPenetration)
                    RCH_CheckRicochet(hit, inputDirection);*/
            }
        }
        else
        {
            /*if (b_BLE_EnableBulletEffect)
                HIT_CheckHit(hit, inputDirection, false, from, hit.point, hit.distance, BLE_GetBulletTransportTime(hit.distance));
            else
                HIT_CheckHit(hit, inputDirection, false);

            if (b_RCH_EnableRicochet && ricochetAfterPenetration)
                RCH_CheckRicochet(hit, inputDirection);*/
        }
    }


    #endregion

    #region Ricochet Control(s)

    void RCH_CheckRicochet(RaycastHit hit, Vector3 inputDirection, float penetrationPower, float ricochetPower, GameObject bullet)
    {
        float rand = UnityEngine.Random.Range(0.0f, 0.99f);
        if (rand < f_RCH_RicochetChance)
        {
            if (hit.transform.GetComponent<EOSS_RicochetObject>() != null)
            {
                EOSS_RicochetObject obj = hit.transform.GetComponent<EOSS_RicochetObject>();
                float currentPower = ricochetPower;
                if (b_RCH_IsPowerAffectedByDistance)
                {
                    float distance = (hit.point - t_MainCameraTransform.position).magnitude;
                    currentPower -= (distance / f_RCH_XUnits) * f_RCH_PowerDropPerXUnits;
                }

                if (currentPower > obj.f_RicochetThreshold)
                {
                    currentPower -= obj.f_RicochetPowerDecrease;

                    RaycastHit currentHit = hit;
                    Vector3 point = hit.point;
                    Vector3 inputDir = inputDirection;
                    Vector3 dir;
                    if (en_RCH_BulletDirType == RicochetBulletDirectionType.Random)
                    {
                        Vector3 backPoint = point - inputDirection * -2f;
                        Vector3 randomPoint = backPoint + UnityEngine.Random.insideUnitSphere;
                        dir = randomPoint - backPoint;
                    }
                    else
                        dir = Vector3.Reflect(inputDir, currentHit.normal);

                    Ray reflectedRay = new Ray(point, dir);
                    if (b_DBG_RayDebugEnabled && b_DBG_DebugRicochetRay)
                        Debug.DrawRay(point, dir * ricochetPower, col_DBG_RicochetRayColor, 10.0f);

                    StartCoroutine(CO_RC_CastRays(reflectedRay, penetrationPower, ricochetPower, bullet));
                }
            }
        }
    }
    
    

    #endregion

    #region Hit Control(s)

 
    void HIT_CastHit(Vector3 hitPoint, bool reverseHit)
    {
        Debug.Log("Hit");
        Debug.DrawRay(hitPoint, Vector3.up * 10, Color.yellow, 100.0f);
        Debug.DrawRay(hitPoint, Vector3.down * 10, Color.yellow, 100.0f);
        Debug.DrawRay(hitPoint, Vector3.right * 10, Color.yellow, 100.0f);
        Debug.DrawRay(hitPoint, Vector3.right * -10, Color.yellow, 100.0f);

    }

    #endregion

    #region Bullet Effect Control(s)

    bool BLE_CalculateBEChance()
    {
        float rand = UnityEngine.Random.Range(0.0f, 0.99f);

        return false;
    }

    #endregion

    #endregion


    #region Coroutine(s)

    #region AIM Coroutine(s)


    IEnumerator CO_AIM_AimIn()
    {
        if (b_AIM_SwitchMeshMaterial)
        {
            if (co_AIM_SwitchMaterial != null)
                StopCoroutine(co_AIM_SwitchMaterial);

            co_AIM_SwitchMaterial = StartCoroutine(CO_WaitAndINIT(f_AIM_MaterialSwitchTimeIn, () => AIM_SwitchMeshMaterial(mat_AIM_ScopeRenderTexture, true)));
        }

        if (b_AIM_UseTexture)
        {
            if (co_AIM_ToggleTexture != null)
                StopCoroutine(co_AIM_ToggleTexture);

            co_AIM_ToggleTexture = StartCoroutine(CO_WaitAndINIT(f_AIM_TextureAppearTime, () => AIM_ToggleTexture(true)));
        }

        float i = 0.0f;
        float j = 0.0f;
        float currentFOV = cam_MainCamera.fieldOfView;
        f_AIM_CamFOV = currentFOV;
        Vector3 currentPosition = t_AIM_AimTransform.localPosition;
        Quaternion currentRotation = t_AIM_AimTransform.localRotation;

        if (b_AIM_ChangeFOVAfter)
            j = 1.1f;

        while (i < 1.0f || j < 1.0f)
        {
            i += Time.deltaTime * f_AIM_InSpeed;

            if (!b_AIM_ChangeFOVAfter)
            {
                j += Time.deltaTime * f_AIM_FOVChangeInSpeed;
                cam_MainCamera.fieldOfView = Mathf.Lerp(currentFOV, f_AIM_TargetFOV, j);
            }

            if (b_AIM_ChangePosition)
                t_AIM_AimTransform.localPosition = Vector3.Lerp(currentPosition, v3_AIM_AimPosition, i);

            if (b_AIM_ChangeRotation)
                t_AIM_AimTransform.localRotation = Quaternion.Slerp(currentRotation, Quaternion.Euler(v3_AIM_AimEuler), i);

            yield return null;
        }

        b_GNRL_IsAiming = true;
        
        if (b_AIM_ChangeFOVAfter)
        {
            j = 0.0f;
            while (j < 1.0f)
            {
                j += Time.deltaTime * f_AIM_FOVChangeInSpeed;
                cam_MainCamera.fieldOfView = Mathf.Lerp(currentFOV, f_AIM_TargetFOV, j);
                yield return null;
            }
        }
    }

    IEnumerator CO_AIM_AimOut()
    {

        if (b_AIM_SwitchMeshMaterial)
        {
            if (co_AIM_SwitchMaterial != null)
                StopCoroutine(co_AIM_SwitchMaterial);

            co_AIM_SwitchMaterial = StartCoroutine(CO_WaitAndINIT(f_AIM_MaterialSwitchTimeOut, () => AIM_SwitchMeshMaterial(mat_AIM_ScopeNormal, false)));
        }

        if (b_AIM_UseTexture)
        {
            if (co_AIM_ToggleTexture != null)
                StopCoroutine(co_AIM_ToggleTexture);

            co_AIM_ToggleTexture = StartCoroutine(CO_WaitAndINIT(f_AIM_TextureDisappearTime, () => AIM_ToggleTexture(false)));
        }

        float i = 0.0f;
        float j = 0.0f;
        float currentFOV = cam_MainCamera.fieldOfView;
        Vector3 currentPosition = t_AIM_AimTransform.localPosition;
        Quaternion currentRotation = t_AIM_AimTransform.localRotation;

        while (i < 1.0f || j < 1.0f)
        {
            i += Time.deltaTime * f_AIM_InSpeed;
            j += Time.deltaTime * f_AIM_FOVChangeOutSpeed;

            cam_MainCamera.fieldOfView = Mathf.Lerp(currentFOV, f_AIM_CamFOV, j);

            if (b_AIM_ChangePosition)
                t_AIM_AimTransform.localPosition = Vector3.Lerp(currentPosition, v3_AIM_HipPosition, i);

            if (b_AIM_ChangeRotation)
                t_AIM_AimTransform.localRotation = Quaternion.Slerp(currentRotation, Quaternion.Euler(v3_AIM_HipEuler), i);

            yield return null;
        }
        b_GNRL_IsAiming = false;

    }


    #endregion

    #region BB Coroutine(s)

    IEnumerator CO_Blowback(BlowbackData bbData)
    {
        Transform target = bbData.t_BBD_Target;
        float i = 0.0f;
        float j = 0.0f;
        float inSpeedPos = bbData.f_BDD_POS_InSpeed;
        float outSpeedPos = bbData.f_BDD_POS_OutSpeed;
        float inSpeedRot = bbData.f_BDD_ROT_InSpeed;
        float outSpeedRot = bbData.f_BDD_ROT_OutSpeed;
        bool posBB = bbData.b_BBD_PosBB;
        bool rotBB = bbData.b_BBD_RotBB;
        bool decreaseCBBPosPower = bbData.b_BDD_CBB_POS_DecreasePower;
        bool decreaseCBBRotPower = bbData.b_BDD_CBB_ROT_DecreasePower;
        bool decreaseWBBPosPower = bbData.b_BDD_WBB_POS_DecreasePower;
        bool decreaseWBBRotPower = bbData.b_BDD_WBB_ROT_DecreasePower;
        Vector3 currentPos = target.localPosition;
        Vector3 initialPos = bbData.v3_BDD_InitialPos;
        Vector3 initialEul = bbData.v3_BDD_InitialEul;
        Vector3 targetPos = bbData.v3_BDD_TargetPos + initialPos;
        Quaternion currentRot = target.localRotation;
        Quaternion targetRot = Quaternion.Euler(bbData.v3_BDD_TargetEul + initialEul);


        if (!posBB)
            i = 1.1f;
        if (!rotBB)
            j = 1.1f;

        while(i < 1.0f || j < 1.0f)
        {
            i += Time.deltaTime * inSpeedPos;
            j += Time.deltaTime * inSpeedRot;

            if (posBB)
                target.localPosition = Vector3.Lerp(currentPos, targetPos, i);
            if (rotBB)
                target.localRotation = Quaternion.Slerp(currentRot, targetRot, j);

            yield return null;
        }

        currentPos = target.localPosition;
        currentRot = target.localRotation;
        Quaternion initialRot = Quaternion.Euler(initialEul);

        if (posBB)
            i = 0.0f;
        else
            i = 1.1f;
        if (rotBB)
            j = 0.0f;
        else
            j = 1.1f;


        while (i < 1.0f || j < 1.0f)
        {
            i += Time.deltaTime * outSpeedPos;
            j += Time.deltaTime * outSpeedRot;

            if (posBB)
                target.localPosition = Vector3.Lerp(currentPos, initialPos, i);
            if (rotBB)
                target.localRotation = Quaternion.Slerp(currentRot, initialRot, j);

            yield return null;
        }

        if (co_CBB_DecreasePower != null)
            StopCoroutine(co_CBB_DecreasePower);


        if (decreaseCBBPosPower && !decreaseCBBRotPower)
            co_CBB_DecreasePower = StartCoroutine(CO_CBB_DecreasePower(true, false));
        else if(!decreaseCBBPosPower && decreaseCBBRotPower)
            co_CBB_DecreasePower = StartCoroutine(CO_CBB_DecreasePower(false, true));
        else if(decreaseCBBPosPower && decreaseCBBRotPower)
            co_CBB_DecreasePower = StartCoroutine(CO_CBB_DecreasePower(true, true));

        if (decreaseWBBPosPower && !decreaseWBBRotPower)
            co_WBB_DecreasePower = StartCoroutine(CO_WBB_DecreasePower(true, false));
        else if (!decreaseWBBPosPower && decreaseWBBRotPower)
            co_WBB_DecreasePower = StartCoroutine(CO_WBB_DecreasePower(false, true));
        else if (decreaseWBBPosPower && decreaseWBBRotPower)
            co_WBB_DecreasePower = StartCoroutine(CO_WBB_DecreasePower(true, true));

    }


    IEnumerator CO_CBB_DecreasePower(bool decreasePosPower, bool decreaseRotPower)
    {
        float i = 0.0f;
        float j = 0.0f;
        float currPowerPos = f_CBB_POS_Power;
        float currPowerRot = f_CBB_ROT_Power;

        if (!decreasePosPower)
            i = 1.1f;
        if (!decreaseRotPower)
            j = 1.1f;
        
        while (i < 1.0f || j < 1.0f)
        {
            i += Time.deltaTime * f_CBB_POS_PowerDecreaseSpeed;
            j += Time.deltaTime * f_CBB_ROT_PowerDecreaseSpeed;

            if(decreasePosPower)
                f_CBB_POS_Power = Mathf.Lerp(currPowerPos, f_CBB_POS_FROM_Power, i);
            if (decreaseRotPower)
                f_CBB_ROT_Power = Mathf.Lerp(currPowerRot, f_CBB_ROT_FROM_Power, j);
            yield return null;
        }
    }

    IEnumerator CO_WBB_DecreasePower(bool decreasePosPower, bool decreaseRotPower)
    {
        float i = 0.0f;
        float j = 0.0f;
        float currPowerPos = f_WBB_POS_Power;
        float currPowerRot = f_WBB_ROT_Power;

        if (!decreasePosPower)
            i = 1.1f;
        if (!decreaseRotPower)
            j = 1.1f;

        while (i < 1.0f || j < 1.0f)
        {
            i += Time.deltaTime * f_WBB_POS_PowerDecreaseSpeed;
            j += Time.deltaTime * f_WBB_ROT_PowerDecreaseSpeed;

            if (decreasePosPower)
                f_WBB_POS_Power = Mathf.Lerp(currPowerPos, f_WBB_POS_FROM_Power, i);
            if (decreaseRotPower)
                f_WBB_ROT_Power = Mathf.Lerp(currPowerRot, f_WBB_ROT_FROM_Power, j);
            yield return null;
        }
    }

    IEnumerator CO_HACC_DecreasePower()
    {
        float i = 0.0f;
        float currPower = f_HACC_Power;
        while(i < 1.0f)
        {
            i += Time.deltaTime * f_HACC_PowerChangeBackSpeed;
            f_HACC_Power = Mathf.Lerp(currPower, f_HACC_FROM_Power, i);
            yield return null;
        }
    }

    #endregion

    #region Reload Coroutine(s)

    IEnumerator CO_RLD_BoltActionReload()
    {

        if (en_RLD_BoltReloadINITType == BoltReloadINITType.Automatic)
            yield return new WaitForSeconds(f_RLD_WaitBeforeBoltAutoReload);

        int triggerHash;
        float time;

        if (b_RLD_RandomizeBoltActionAnimations)
        {
            int index = UnityEngine.Random.Range(0, i_HASH_RLD_BoltActions.Length);
            triggerHash = i_HASH_RLD_BoltActions[index];
            time = f_RLD_BoltActionTimes[index];
        }
        else
        {
            triggerHash = i_HASH_RLD_BoltAction;
            time = f_RLD_BoltActionTime;

        }


        if(b_RLD_CheckIsAimingOnBoltActionReload && !b_AIM_CanAimWhileReloading && b_GNRL_IsAiming)
            co_AIM_AimOut = StartCoroutine(CO_AIM_AimOut());

        b_RLD_IsInReload = true;
        anim_RLD_WeaponAnimator.SetTrigger(triggerHash);
        yield return new WaitForSeconds(time);
        b_RLD_IsBoltActionReloading = false;
        b_RLD_IsInReload = false;

        if (i_RLD_CURR_BulletCount < 1)
        {
            b_RLD_IsMagazineReloading = true;
            if(en_RLD_MagReloadINITType == MagReloadINITType.Automatic)
                co_RLD_MagazineReload = StartCoroutine(CO_RLD_MagazineReload());
        }


    }


    IEnumerator CO_RLD_MagazineReload()
    {

        if (en_RLD_MagReloadINITType == MagReloadINITType.Automatic)
            yield return new WaitForSeconds(f_RLD_WaitBeforeMagAutoReload);

        int triggerHash;
        float time;

        if (b_RLD_RandomizeMagazineAnimations)
        {
            int index = UnityEngine.Random.Range(0, i_HASH_RLD_Magazines.Length);
            triggerHash = i_HASH_RLD_Magazines[index];
            time = f_RLD_MagazineTimes[index];
        }
        else
        {
            triggerHash = i_HASH_RLD_Magazine;
            time = f_RLD_MagazineTime;

        }


        if (b_RLD_CheckIsAimingOnMagazineReload && !b_AIM_CanAimWhileReloading && b_GNRL_IsAiming)
            co_AIM_AimOut = StartCoroutine(CO_AIM_AimOut());

        b_RLD_IsInReload = true;
        anim_RLD_WeaponAnimator.SetTrigger(triggerHash);
        yield return new WaitForSeconds(time);
        b_RLD_IsMagazineReloading = false;


        if (en_RLD_AmmoRefreshType == AmmoRefreshType.Realistic)
            i_RLD_TotalBulletCount -= i_RLD_CURR_BulletCount;

        i_RLD_CURR_MagazineCount = Mathf.FloorToInt(((i_RLD_TotalBulletCount - i_RLD_CURR_BulletCount) / i_RLD_BulletAmountInOneMagazine));

        

        if (i_RLD_CURR_MagazineCount == 0)
            i_RLD_CURR_BulletCount = i_RLD_TotalBulletCount;
        else
            i_RLD_CURR_BulletCount = i_RLD_BulletAmountInOneMagazine;

        if (i_RLD_CURR_BulletCount < 0)
            i_RLD_CURR_BulletCount = 0;
        if (i_RLD_CURR_MagazineCount < 0)
            i_RLD_CURR_MagazineCount = 0;

        b_RLD_IsInReload = false;
    }

    IEnumerator CO_RLD_RegisterHashArrayForBoltActionAnimations()
    {
        i_HASH_RLD_BoltActions = new int[s_RLD_BoltActionAnimTriggers.Length];

        for(int x = 0; x < i_HASH_RLD_BoltActions.Length; x++)
        {
            i_HASH_RLD_BoltActions[x] = Animator.StringToHash(s_RLD_BoltActionAnimTriggers[x]);
            yield return null;
        }
      
    }

    IEnumerator CO_RLD_RegisterHashArrayForMagazineAnimations()
    {
        i_HASH_RLD_Magazines = new int[s_RLD_MagazineAnimTriggers.Length];

        for (int x = 0; x < i_HASH_RLD_Magazines.Length; x++)
        {
            i_HASH_RLD_Magazines[x] = Animator.StringToHash(s_RLD_MagazineAnimTriggers[x]);
            yield return null;
        }
    }
    #endregion

    #region Fire Coroutine(s)

    IEnumerator CO_FRE_RegisterHashArrayForFireAnimations()
    {
        i_HASH_FRE_FireAnims = new int[s_FRE_AnimTriggers.Length];

        for(int x = 0; x < i_HASH_FRE_FireAnims.Length; x++)
        {
            i_HASH_FRE_FireAnims[x] = Animator.StringToHash(s_FRE_AnimTriggers[x]);
            yield return null;
        }
    }

    #endregion

    #region Raycast Coroutine(s)


    IEnumerator CO_RC_CastRays(Ray ray, float penetrationPower, float ricochetPower, GameObject bullet)
    {

        Ray initialRay = ray;
        RaycastHit hit;
        int maximumRayCount = Mathf.CeilToInt((f_RC_RifleDistance / f_RC_XUnit));
        bullet.transform.position = initialRay.origin;
        Coroutine moveBullet = null;
        int i = 0;

        while (i < maximumRayCount)
        {
            i++;

            if (b_DBG_RayDebugEnabled && b_DBG_DebugShootRays)
                Debug.DrawRay(initialRay.origin, initialRay.direction * f_RC_XUnit, col_DBG_InitialShootRayColor, 15.0f);

            bool hitAcquired = Physics.Raycast(initialRay.origin, initialRay.direction, out hit, f_RC_XUnit, lm_RC_InclusionMask);

            if (moveBullet != null)
                StopCoroutine(moveBullet);

            Vector3 targetPos = initialRay.origin + initialRay.direction * f_RC_XUnit;

            if (hitAcquired)
            {
                targetPos = hit.point;

                if(bullet != null)
                    moveBullet = StartCoroutine(CO_RC_MoveBullet(bullet.transform.position, targetPos, bullet, f_BLDE_DelayPerXUnits));

                RC_ApplySettings(hit, initialRay.direction, penetrationPower, ricochetPower, bullet);
                break;
            }
            if (bullet != null)
                moveBullet = StartCoroutine(CO_RC_MoveBullet(bullet.transform.position, initialRay.origin, bullet, f_BLDE_DelayPerXUnits));

            initialRay.origin += initialRay.direction * f_RC_XUnit;
            initialRay.direction = new Vector3(initialRay.direction.x, initialRay.direction.y - f_BLD_DropPerXUnits, initialRay.direction.z);
            yield return new WaitForSeconds(f_BLDE_DelayPerXUnits);
        }

    }

    #endregion

    #region Bullet Effect Coroutine(s)

    IEnumerator CO_RC_MoveBullet(Vector3 from, Vector3 to, GameObject bullet, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        Quaternion qCurrent = bullet.transform.rotation;
        Quaternion qTarget = Quaternion.LookRotation(to - from);
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            bullet.transform.rotation = Quaternion.Slerp(qCurrent, qTarget, i);
            bullet.transform.position = Vector3.Lerp(from, to, i);
            yield return null;
        }
    }

    #endregion


    IEnumerator CO_WaitAndINIT(float time, Action onComplete)
    {
        yield return new WaitForSeconds(time);

        if(onComplete != null)
            onComplete();
    }

    #endregion


    #region ExternallySet



    #endregion


    #region Finalizer(s)



    #endregion

}
