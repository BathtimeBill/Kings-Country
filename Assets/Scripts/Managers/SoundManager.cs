using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Tree Sounds")]
    public AudioClip[] treePlaceSound;
    public AudioClip[] treeGrowSound;
    public AudioClip[] chopSounds;
    public AudioClip[] treeLandSounds;
    public AudioClip[] treeFallSounds;
    [Header("Combat Sounds")]
    public AudioClip bowDrawSound;
    public AudioClip bowReleaseSound;
    public AudioClip[] impactSounds;
    public AudioClip[] gruntSounds;
    public AudioClip[] leshyStompSounds;
    public AudioClip[] screamSounds;
    [Header("Footstep Sounds")]
    public AudioClip[] humanFootsteps;
    public AudioClip[] forestFootsteps;
    public AudioClip[] knightFootsteps;
    public AudioClip[] leshyFootsteps;
    public AudioClip[] flapSounds;
    [Header("UI Sounds")]
    public AudioClip buttonClickSound;
    public AudioClip gameOverSound;
    public AudioClip upgradeSound;
    public AudioClip openMenuSound;
    public AudioClip closeMenuSound;
    public AudioClip warningSound;
    public AudioClip waveOverSound;
    public AudioClip waveBeginSound;
    public AudioClip menuDragSound;
    public AudioClip textGroupSound;
    public AudioClip nextTutorialSound;
    [Header("Wildlife Distress Sounds")]
    public AudioClip[] deerDistressSounds;
    public AudioClip[] boarDistressSounds;
    public AudioClip rabbitDistressSound;
    [Header("Home Tree Sounds")]
    public AudioClip summonSound;
    [Header("Ambience and Lightning")]
    public AudioClip[] lightningSounds;
    public AudioClip forestSound;
    public AudioClip rainSound;
    public AudioSource weatherAudioSource;
    public AudioClip stormerSummonSound;
    [Header("Audio")]
    public GameObject SFXPool;
    public int soundPoolCurrent;
    public AudioSource[] soundPool;



    public void PlaySound(AudioClip _clip)
    {
        if (soundPoolCurrent == soundPool.Length - 1)
            soundPoolCurrent = 0;
        else
            soundPoolCurrent += 1;

        soundPool[soundPoolCurrent].clip = _clip;
        soundPool[soundPoolCurrent].Play();
    }
    public AudioClip GetScreamSound()
    {
        return screamSounds[Random.Range(0, screamSounds.Length)];
    }
    public AudioClip GetLightningSound()
    {
        return lightningSounds[Random.Range(0, lightningSounds.Length)];
    }
    public AudioClip GetFlapSound()
    {
        return flapSounds[Random.Range(0, flapSounds.Length)];
    }
    public AudioClip GetTreeFallSound()
    {
        return treeFallSounds[Random.Range(0, treeFallSounds.Length)];
    }
    public AudioClip GetTreeLandSound()
    {
        return treeLandSounds[Random.Range(0, treeLandSounds.Length)];
    }
    public AudioClip GetLeshyStompSound()
    {
        return leshyStompSounds[Random.Range(0, leshyStompSounds.Length)];
    }
    public AudioClip GetLeshyFootstepSound()
    {
        return leshyFootsteps[Random.Range(0, leshyFootsteps.Length)];
    }
    public AudioClip GetKnightFootstepSound()
    {
        return knightFootsteps[Random.Range(0, knightFootsteps.Length)];
    }
    public AudioClip GetHumanFootstepSound()
    {
        return humanFootsteps[Random.Range(0, humanFootsteps.Length)];
    }
    public AudioClip GetForestFootstepSound()
    {
        return forestFootsteps[Random.Range(0, forestFootsteps.Length)];
    }

    public AudioClip GetChopSounds()
    {
        return chopSounds[Random.Range(0, chopSounds.Length)];
    }
    public AudioClip GetGruntSounds()
    {
        return gruntSounds[Random.Range(0, gruntSounds.Length)];
    }
    public AudioClip GetImpactSounds()
    {
        return impactSounds[Random.Range(0, impactSounds.Length)];
    }
    public AudioClip GetTreePlaceSound()
    {
        return treePlaceSound[Random.Range(0, treePlaceSound.Length)];
    }
    public AudioClip GetTreeGrowSound()
    {
        return treeGrowSound[Random.Range(0, treeGrowSound.Length)];
    }
    public AudioClip GetDeerDistressSound()
    {
        return deerDistressSounds[Random.Range(0, deerDistressSounds.Length)];
    }
    public AudioClip GetBoarDistressSound()
    {
        return boarDistressSounds[Random.Range(0, boarDistressSounds.Length)];
    }

}
