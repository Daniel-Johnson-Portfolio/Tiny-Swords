using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SoundManager : MonoBehaviour
{
    public enum Sounds
    {
        ButtonConfirm,
        ButtonBack,
        ButtonHover,

    }

    public static SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public Sounds sound;
        public AudioClip audioClip;
    }

    public static void PlayeSound(Sounds sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(Sounds sound)
    {
        foreach (SoundAudioClip soundAudioClip in soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    public static void addbuttonSound(GameObject button)
    {
        button.AddComponent<AudioSource>();
        button.GetComponent<AudioSource>().playOnAwake = false;
        button.GetComponent<AudioSource>().clip = GetAudioClip(Sounds.ButtonHover);
    }



}
