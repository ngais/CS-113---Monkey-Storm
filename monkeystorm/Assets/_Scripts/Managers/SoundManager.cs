using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public static SoundManager soundManagerScript;
    public AudioSource sfx;
    public AudioClip[] jump;
    public AudioClip[] swoosh;
    public AudioClip celebrate;
    public AudioClip builder;
    public AudioClip shake;

    private void Awake()
    {
        if (soundManagerScript == null)
        {
            DontDestroyOnLoad(gameObject);
            soundManagerScript = this;
        }
        else if (soundManagerScript != this)
            Destroy(gameObject);
    }

    /// <summary>
    /// Plays a random jump sound for the players jump
    /// </summary>
    public void PlayerJump(AudioSource audioSource)
    {
        audioSource.clip = jump[Random.Range(0, jump.Length)];
        if(!audioSource.isPlaying)
            audioSource.Play();
    }

    /// <summary>
    /// Plays a constrution sound when building an object such as a house
    /// </summary>
    public void Construct()
    {
        sfx.clip = builder;
        sfx.Play();
    }

    /// <summary>
    /// Plays a leaf shaking sound when the player contacts a branch.
    /// </summary>
    public void Shake()
    {
        sfx.clip = shake;
        if(!sfx.isPlaying)
        {
            sfx.Play();
        }
    }

    /// <summary>
    /// Plays a sound when something is thrown
    /// </summary>
    public void Throw(AudioSource audioSource)
    {
        audioSource.clip = swoosh[Random.Range(0, swoosh.Length)];
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    /// <summary>
    /// Plays a gorilla sound and returns the length of the clip
    /// </summary>
    public float GorillaNoise(AudioSource audioSource)
    {
        audioSource.Play();
        return audioSource.clip.length;
    }

    /// <summary>
    /// Plays a celebratory sound at the end of the level
    /// </summary>
    public void Celebrate()
    {
        sfx.clip = celebrate;
        sfx.Play();
    }
}
