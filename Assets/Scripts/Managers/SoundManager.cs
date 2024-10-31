using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public AudioClip[] spitSpraySounds;
    [Header("Vocals")]
    public AudioClip[] golemVocals;
    public AudioClip[] leshyVocals;
    public AudioClip[] orcusVocals;
    public AudioClip[] satyrVocals;
    public AudioClip[] skessaVocals;
    public AudioClip[] goblinVocals;
    public AudioClip[] fidhainVocals;
    [Header("Death Sounds")]
    public AudioClip[] satyrDeathVocals;
    public AudioClip[] orcusDeathVocals;
    public AudioClip[] leshyDeathVocals;
    [Header("Footstep Sounds")]
    public AudioClip[] humanFootsteps;
    public AudioClip[] forestFootsteps;
    public AudioClip[] knightFootsteps;
    public AudioClip[] leshyFootsteps;
    public AudioClip[] flapSounds;
    public AudioClip[] golemFootsteps;
    [Header("UI Sounds")]
    public AudioClip transitionToNightSound;
    public AudioClip transitionToDaySound;
    public AudioClip buttonClickSound;
    public AudioClip buttonHoverSound;
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
    public AudioClip taskCompleteSound;
    public AudioClip tutorialChangeSound;
    public AudioClip timeSpeedUpSound;
    public AudioClip timeStopSound;
    public AudioClip targetEnemySound;
    public AudioClip attackSound;
    public AudioClip defendSound;
    public AudioClip formationSound;
    public AudioClip stopSound;
    public AudioClip controlGroup;
    public AudioClip controlGroupSelect;
    public AudioClip levelSelectedSound;
    public AudioClip holdButtonFill;
    [Header("Wildlife Distress Sounds")]
    public AudioClip[] deerDistressSounds;
    public AudioClip[] boarDistressSounds;
    public AudioClip rabbitDistressSound;
    [Header("Home Tree Sounds")]
    public AudioClip summonSound;
    [Header("Ambience and Lightning")]
    public AudioClip[] lightningSounds;
    public AudioClip forestSoundDay;
    public AudioClip forestSoundNight;
    public AudioClip rainSound;
    public AudioSource weatherAudioSource;
    public AudioClip stormerSummonSound;
    [Header("Audio")]
    public GameObject SFXPool;
    public int soundPoolCurrent;
    public AudioSource[] soundPool;
    [Header("Towers")]
    public AudioClip[] spitSounds;
    public AudioClip[] spitExplosionSounds;
    [Header("Lords")]
    public AudioClip[] whooshSounds;
    public AudioClip horseGallopSound;




    public void PlaySound(AudioClip _clip)
    {
        if (soundPoolCurrent == soundPool.Length - 1)
            soundPoolCurrent = 0;
        else
            soundPoolCurrent += 1;

        soundPool[soundPoolCurrent].clip = _clip;
        soundPool[soundPoolCurrent].Play();
    }

    public AudioClip GetSatyrDeathSound()
    {
        return satyrDeathVocals[Random.Range(0, satyrDeathVocals.Length)];
    }
    public AudioClip GetOrcusDeathSound()
    {
        return orcusDeathVocals[Random.Range(0, orcusDeathVocals.Length)];
    }
    public AudioClip GetLeshyDeathSound()
    {
        return leshyDeathVocals[Random.Range(0, leshyDeathVocals.Length)];
    }



    public AudioClip GetWhooshSounds()
    {
        return whooshSounds[Random.Range(0, whooshSounds.Length)];
    }
    public AudioClip GetSpitSounds()
    {
        return spitSounds[Random.Range(0, spitSounds.Length)];
    }
    public AudioClip GetSpitExplosionSounds()
    {
        return spitExplosionSounds[Random.Range(0, spitExplosionSounds.Length)];
    }
    public AudioClip GetGoblinVocal()
    {
        return goblinVocals[Random.Range(0, goblinVocals.Length)];
    }
    public AudioClip GetGolemVocal()
    {
        return golemVocals[Random.Range(0, golemVocals.Length)];
    }
    public AudioClip GetGolemFootsteps()
    {
        return golemFootsteps[Random.Range(0, golemFootsteps.Length)];
    }
    public AudioClip GetLeshyVocal()
    {
        return leshyVocals[Random.Range(0, leshyVocals.Length)];
    }
    public AudioClip GetOrcusVocal()
    {
        return orcusVocals[Random.Range(0, orcusVocals.Length)];
    }
    public AudioClip GetSatyrVocal()
    {
        return satyrVocals[Random.Range(0, satyrVocals.Length)];
    }
    public AudioClip GetFidhainVocal()
    {
        return fidhainVocals[Random.Range(0, fidhainVocals.Length)];
    }
    public AudioClip GetSkessaVocal()
    {
        return skessaVocals[Random.Range(0, skessaVocals.Length)];
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

    private void OnCombatSelected(CombatID _combatID)
    {
        switch (_combatID)
        {
            case CombatID.Attack:
                PlaySound(attackSound);
                break;
            case CombatID.Defend:
                PlaySound(defendSound);
                break;
            case CombatID.Formation:
                PlaySound(formationSound);
                break;
            case CombatID.Stop:
                PlaySound(stopSound); 
                break;
        }
    }
    
    private void OnEnable()
    {
        GameEvents.OnCombatSelected += OnCombatSelected;
    }

    private void OnDisable()
    {
        GameEvents.OnCombatSelected -= OnCombatSelected;
    }
}
