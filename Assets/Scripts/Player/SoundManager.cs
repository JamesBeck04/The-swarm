using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    public AudioSource ShootingChannel;
    public AudioSource ReloadSound;
    public AudioSource EmptySound;

    public AudioClip PistolShot;
    public AudioClip ShotgunShot;

    public AudioSource ShotgunReloadSound;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon) {
            case WeaponModel.Pistol:
                ShootingChannel.PlayOneShot(PistolShot);
                break;
            case WeaponModel.Shotgun:
                ShootingChannel.PlayOneShot(ShotgunShot);
                break;
        }

    
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol:
                ReloadSound.Play();
                break;
            case WeaponModel.Shotgun:
                ShotgunReloadSound.Play();
                break;
        }


    }
}
