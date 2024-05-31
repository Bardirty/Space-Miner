using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]private AudioSource[] sources;
    private void Start()
    {
        for(int i = 0; i < sources.Length; ++i)
            sources[i].volume = PlayerPrefs.GetFloat("VolumePreference");
    }
}