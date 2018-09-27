using UnityEngine;
using System.Collections;

public class IS_Flicker : MonoBehaviour {

    public bool b_ChangeMaterial;
    public float f_OpenInterval;
    public bool b_RandomizeOpenInterval;
    public float f_OpenIntervalRand;

    

    public float f_WaitBetweenFlicks;
    public bool b_RandomizeWaitBF;
    public float f_WaitBFRand;

    public float f_FlickedIntensity;
    public bool b_LerpIntensity;
    public float f_LerpTimeForward;
    public float f_LerpTimeBack;


    private float f_OriginalIntensity;
    private Light l_This;

    public bool b_WaitOnEnable;

    public bool b_PlaySound;
    private AudioSource as_This;

	public Renderer targetRenderer;
	public int i_MaterialIndex;

	public Material mat_CloseMat;
	public Material mat_OpenMat;

	private float f_SavedInterval;
	private float f_SavedWaitBW;

    void OnEnable()
    {
		f_SavedInterval = f_OpenInterval;
		f_SavedWaitBW = f_WaitBetweenFlicks;
        l_This = GetComponent<Light>();
        f_OriginalIntensity = l_This.intensity;
        as_This = GetComponent<AudioSource>();
        if (b_WaitOnEnable)
            StartCoroutine(WaitForFlicker());
        else
            StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        if (b_ChangeMaterial) 
		{
			Material[] mats = targetRenderer.materials;
			mats[i_MaterialIndex] = mat_CloseMat;
			targetRenderer.materials = mats;
		}
            

        if(b_LerpIntensity)
        {
            float i = 0.0f;
            float rate = 1.0f / f_LerpTimeForward;
            float currentIntensity = l_This.intensity;

            while (i < 1.0f)
            {
                i += Time.deltaTime * rate;
                l_This.intensity = Mathf.Lerp(currentIntensity, f_FlickedIntensity, i);
                yield return null;
            }
        }
        else
        {
            l_This.intensity = f_FlickedIntensity;
        }

        if(b_PlaySound)
        {
            
        }

        

        if (b_RandomizeOpenInterval)
            f_SavedInterval = Random.Range(f_OpenInterval - f_OpenIntervalRand, f_OpenInterval + f_OpenIntervalRand);

        yield return new WaitForSeconds(f_SavedInterval);

		if (b_ChangeMaterial) 
		{
			Material[] mats = targetRenderer.materials;
			mats[i_MaterialIndex] = mat_OpenMat;
			targetRenderer.materials = mats;
		}

        if (b_LerpIntensity)
        {
            float i = 0.0f;
            float rate = 1.0f / f_LerpTimeBack;
            float currentIntensity = l_This.intensity;

            while (i < 1.0f)
            {
                i += Time.deltaTime * rate;
                l_This.intensity = Mathf.Lerp(currentIntensity, f_OriginalIntensity, i);
                yield return null;
            }
        }
        else
            l_This.intensity = f_OriginalIntensity;

        StartCoroutine(WaitForFlicker());
    }

    IEnumerator WaitForFlicker()
    {
        if (b_RandomizeWaitBF)
            f_SavedWaitBW = Random.Range(f_WaitBetweenFlicks + f_WaitBFRand, f_WaitBetweenFlicks - f_WaitBFRand);

		yield return new WaitForSeconds(f_SavedWaitBW);

        StartCoroutine(Flicker());
    }
}
