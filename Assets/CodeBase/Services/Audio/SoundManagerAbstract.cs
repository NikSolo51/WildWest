﻿using UnityEngine;

namespace CodeBase.Services.Audio
{
    public abstract class SoundManagerAbstract : MonoBehaviour,ISoundService
    {
        public Sound[] sounds;
        public AudioClip[] clips;
        public abstract void PlaySound(string soundName);
        public abstract void StopSound(string soundName);
        public abstract void StopAllSounds();
        public abstract void SetVolume(float volume);
        public abstract void SetPitch(float pitch);
        public abstract void MuteSound(string soundName);
        public abstract void PauseSound(string soundName);
        public abstract void UnmuteSound(string soundName);
        public abstract float GetSoundLenght(string soundName);
    }
}