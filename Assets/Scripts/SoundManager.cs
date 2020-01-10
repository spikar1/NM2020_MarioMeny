using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AC = UnityEngine.AudioClip;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public AudioSource aSource;
    private void Awake() {
        aSource = GetComponent<AudioSource>();
    }

    public AC defaultPunch;
    public AC swordChargeUp;
    public AC swordSwing;
    public AC jetpackLoop;
    public AC blizzard;
    public AC hammer;
    public AC defaultBlock;
    public AC reflectiveShield;
    public AC[] jumpSounds;

    AC GetJumpSound() {
        var r = Random.Range(0, jumpSounds.Length);
        return jumpSounds[r];
    }

    public void PlayJumpSound() {
        aSource.PlayOneShot(GetJumpSound());
    }

    public void PlaySwordChargeUp() {
        aSource.clip = swordChargeUp;
        aSource.Play();
        
    }
    public void PlaySwordSwing() {
        aSource.Stop();
        aSource.PlayOneShot(swordSwing);
    }

    internal void PlayJetPackLoop() {
        aSource.PlayOneShot(jetpackLoop);
    }

    internal void PlayDefaultPunch() {
        aSource.PlayOneShot(defaultPunch);
    }

    internal void PlayBlizzardSound() {
        aSource.PlayOneShot(blizzard);
    }

    internal void PlayHammerSound() {
        aSource.PlayOneShot(hammer);
    }

    internal void PlayReflectiveShield() {
        aSource.PlayOneShot(reflectiveShield);
    }

    internal void PlayDefaultBlock() {
        aSource.PlayOneShot(defaultBlock);
    }
}
