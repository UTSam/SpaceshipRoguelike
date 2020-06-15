/*
    Authors:
      Jelle van Urk
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Player : MonoBehaviour
{
    [SerializeField] private AudioSource _specialAnimationAudio;
    [SerializeField] private AudioSource _shootingAnimationAudio;
    [SerializeField] private AudioSource _onImpact;
    [SerializeField] private AudioSource _onDestroy;
    [SerializeField] private AudioSource _onStart;

    public void PlaySpecialAnimationAudio()
    {
        if (_specialAnimationAudio != null) this._specialAnimationAudio.Play();
    }

    public void PlayShootingSFX()
    {
        if (_shootingAnimationAudio != null) this._shootingAnimationAudio.Play();
    }

    public void PlayOnImpact()
    {
        if (_onImpact != null) this._onImpact.Play();
    }

    public void PlayOnDeath()
    {
        if (_onDestroy != null) this._onDestroy.Play();
    }

    public void PlayOnStart()
    {
        if (_onStart != null) this._onStart.Play();
    }
}

