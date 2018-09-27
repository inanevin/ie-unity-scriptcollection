using UnityEngine;
using System.Collections;

public class IS_Thunder : MonoBehaviour {

    public Light l_Target;
    public float f_TargetIntensity;

    public bool b_RandomizeSoundWait;
    public float f_SoundWaitTime;
    public float f_RandomizeOffsetSound;

    
    public bool b_RandomizeTime;
    public float f_Time;
    public float f_RandomizeTimeOffset;

    public bool b_RandomizeFrequency;
    public float f_Frequency;
    public float f_RandomizeOffset;
    
    public AudioSource as_Source;
    public AudioClip[] ac_Clips;

    private float f_InitialIntensity;
    private float f_WaitTime;
    private float f_FrequencyWait;
    private float f_SoundWait;
	[System.NonSerialized]
	public bool b_CanLightUp = true;

    void Start()
    {
        StartCoroutine(LightUp());
    }
    IEnumerator LightUp()
    {
		yield return new WaitForSeconds (1);
        while(b_CanLightUp)
        {
            l_Target.intensity = f_TargetIntensity;

            if (b_RandomizeTime)
                f_WaitTime = Random.Range(f_Time - f_RandomizeTimeOffset, f_Time + f_RandomizeTimeOffset);
            else
                f_WaitTime = f_Time;

            yield return new WaitForSeconds(f_WaitTime);

            l_Target.intensity = f_InitialIntensity;

            if (b_RandomizeSoundWait)
                f_SoundWait = Random.Range(f_SoundWaitTime - f_RandomizeOffsetSound, f_SoundWaitTime + f_RandomizeOffsetSound);
            else
                f_SoundWait = f_SoundWaitTime;


            yield return new WaitForSeconds(f_SoundWait);

            int i = Random.Range(1, ac_Clips.Length);

            as_Source.clip = ac_Clips[i];
            as_Source.Play();

            ac_Clips[0] = ac_Clips[i];

            if (b_RandomizeFrequency)
                f_FrequencyWait = Random.Range(f_Frequency - f_RandomizeOffset, f_Frequency + f_RandomizeOffset);
            else
                f_FrequencyWait = f_Frequency;

            yield return new WaitForSeconds(f_FrequencyWait);
        }
    }

    public void Disable()
    {
        this.enabled = false;
    }
}
