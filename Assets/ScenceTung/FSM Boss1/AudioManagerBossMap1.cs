using UnityEngine;

public class AudioManagerBossMap1 : MonoBehaviour
{
    public AudioSource source;
    public AudioClip runSound, attack1Sound, attack2Sound, attack3Sound, attack4Sound, skill1Sound,
        skill2Sound, skill3Sound, skill4Sound, deathSound, roarSound;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
}
