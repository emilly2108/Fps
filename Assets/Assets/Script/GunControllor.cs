using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControllor : MonoBehaviour
{
    [SerializeField]
    private Gun CurrentGun;
    private float CurrentFireRate;
    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }



    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }
    private void GunFireRateCalc()
    {
        if(CurrentFireRate>0)
        {
            CurrentFireRate -= Time.deltaTime;
        }
    }

    private void TryFire()
    {
        if(Input.GetButton("Fire1") && CurrentFireRate <= 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        CurrentFireRate = CurrentGun.fireRate;
        Shoot();
    }

    private void Shoot()
    {
        PlaySE(CurrentGun.fire_Sound);
        CurrentGun.muzzleFlash.Play();
        Debug.Log("ÃÑ¾Ë ¹ß»çÇÔ");
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
