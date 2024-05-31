using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSource;
    [SerializeField] private AudioClip[] clips;
    public void IsHovered()
    {
        audioSource[0].clip = clips[0];
        audioSource[0].Play();
    }
    public void IsPressed()
    {
        audioSource[1].clip = clips[1];
        audioSource[1].Play();
    }
}
