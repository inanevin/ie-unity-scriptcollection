using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IE_TimedDeactivator : MonoBehaviour
{

    public float f_Time;
    public bool b_RunsOnEnable;

    void OnEnable()
    {
        if (b_RunsOnEnable) PVO_Deactivate();
    }

    public void PVO_Deactivate()
    {
        Invoke("Deac", f_Time);
    }

    void Deac()
    {
        gameObject.SetActive(false);
    }
}
