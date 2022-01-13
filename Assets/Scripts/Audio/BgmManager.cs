using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip clipBgmMainMenu;
    [SerializeField] private AudioClip clipBgmPrepare;
    [SerializeField] private AudioClip clipBgmCombat;
    [SerializeField] private AudioClip clipBgmRestart; // When player dead.

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void BgmPlayMainMenu()
    {
        audioSource.clip = clipBgmMainMenu;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void BgmPlayPrepare()
    {
        audioSource.clip = clipBgmPrepare;
        audioSource.loop = true;
	    audioSource.Play();
    }

    public void BgmPlayCombat()
    {
        audioSource.clip = clipBgmCombat;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void BgmStop()
    {
        audioSource.Stop();
    }

}

