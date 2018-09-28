/*
Author: Inan Evin
www.inanevin.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class IE_AudioClipRandomizer : MonoBehaviour
{

    private AudioSource as_This;
    public AudioClip[] ac_Clips;

    void Awake()
    { as_This = GetComponent<AudioSource>(); }
	
    void OnEnable()
    {
        AudioClip clip = ac_Clips[Random.Range(0, ac_Clips.Length)];
        as_This.PlayOneShot(clip);
    }
}
