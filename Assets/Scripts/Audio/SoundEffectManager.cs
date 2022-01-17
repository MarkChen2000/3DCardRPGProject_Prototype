using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip clipSoundSpellBuff;
    [SerializeField] private AudioClip clipSoundSpellAttack;
    [SerializeField] private AudioClip clipSoundNormalAttack;
    [SerializeField] private AudioClip clipSoundPlayerHurt;
    [SerializeField] private AudioClip clipSoundPlayerRoll;
    [SerializeField] private AudioClip clipSoundMonsterHurt;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void SoundPlaySpellBuff()
    {
        audioSource.clip = clipSoundSpellBuff;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void SoundPlaySpellAttack()
    {
        audioSource.clip = clipSoundSpellAttack;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void SoundPlayNormalAttack()
    {
        audioSource.clip = clipSoundNormalAttack;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void SoundPlayPlayerHurt()
    {
        audioSource.clip = clipSoundPlayerHurt;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void SoundPlayPlayerRoll()
    {
        audioSource.clip = clipSoundPlayerRoll;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void SoundPlayMonsterHurt()
    {
        audioSource.clip = clipSoundMonsterHurt;
        audioSource.loop = false;
        audioSource.Play();
    }


    public void SoundStop()
    {
        audioSource.Stop();
    }

}

