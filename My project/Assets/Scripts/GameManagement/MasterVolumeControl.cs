using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using CMF;
using UnityEngine.SceneManagement;


public class MasterVolumeControl : MonoBehaviour, ResetScript
{
    // A master list of all audio sources in the scene
    public List<AudioSource> allAudioSourcesInScene = new List<AudioSource>();

    // Connections for the UI slider and text components
    public Slider masterVolumeSlider;
    public Slider ambientVolumeSlider;
    public Slider spatialVolumeSlider;
    public Slider oneShotVolumeSlider;

    public TMP_Text masterText;
    public TMP_Text ambientText;
    public TMP_Text spatialText;
    public TMP_Text oneShotText;

    // Reference volumes for designer presets to be maintained in their relative volume levels
    public List<float> referenceVolumes = new List<float>();

    // Volume subset factors
    float masterVolumeFactor = 1.0f;
    float ambientVolumeFactor = 1.0f;
    float spatialVolumeFactor = 1.0f;
    float oneShotVolumeFactor = 1.0f;

    // Custom logic for also adjusting the volume of our specific character controller
    public AudioControl playerAudio;
    float playerControllerAudioReferenceVal;


    public float refVolume;

    // Start is called before the first frame update
    void Start()
    {
        SetAllData();
    }

    public void SetAllData()
    {
        allAudioSourcesInScene.Clear();
        referenceVolumes.Clear();

        AudioSource[] allAudioArray = GameObject.FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioArray)
        {
            allAudioSourcesInScene.Add(audioSource);
        }

        // Populate reference volume list
        if (allAudioSourcesInScene != null && allAudioSourcesInScene.Count > 0)
        {
            for (int i = 0; i < allAudioSourcesInScene.Count; i++)
            {
                referenceVolumes.Add(allAudioSourcesInScene[i].volume);
            }
        }

        // Snag player controller audio reference
        playerAudio = GameObject.Find("Player").GetComponent<AudioControl>();
        if (playerAudio != null)  // Also adjust custom player controller audio if relevant
        {
            playerControllerAudioReferenceVal = playerAudio.audioClipVolume;
        }

        // Ensure all sound is set to base state
        UnmuteAll();
        SetAllVolumes();

        refVolume = allAudioSourcesInScene[0].volume;
    }

    public void UpdateMasterVolume()
    {
        float newValue = masterVolumeSlider.value;
        masterVolumeFactor = newValue / 100;

        masterText.text = "Master Volume: " + (int)newValue;

        SetAllVolumes();
    }

    public void UpdateAmbientVolume()
    {
        float newValue = ambientVolumeSlider.value;
        ambientVolumeFactor = newValue / 100;

        ambientText.text = "Ambient Volume: " + (int)newValue;

        SetAllVolumes();
    }

    public void UpdateSpatialVolume()
    {
        float newValue = spatialVolumeSlider.value;
        spatialVolumeFactor = newValue / 100;

        spatialText.text = "Spatial Volume: " + (int)newValue;

        SetAllVolumes();
    }

    public void UpdateOneShotVolume()
    {
        float newValue = oneShotVolumeSlider.value;
        oneShotVolumeFactor = newValue / 100;

        oneShotText.text = "One Shot Volume: " + (int)newValue;

        SetAllVolumes();
    }

    void SetAllVolumes()
    {
        if (allAudioSourcesInScene != null && allAudioSourcesInScene.Count > 0)
        {
            for (int i = 0; i < allAudioSourcesInScene.Count; i++)
            {
                if (allAudioSourcesInScene[i].spatialBlend >= 1.0) // If fully spatial, assume it is a spatial audio
                {
                    allAudioSourcesInScene[i].volume = referenceVolumes[i] * masterVolumeFactor * spatialVolumeFactor;
                }
                else if (allAudioSourcesInScene[i].playOnAwake)  // If plays automatically, assume it is ambient
                {
                    allAudioSourcesInScene[i].volume = referenceVolumes[i] * masterVolumeFactor * ambientVolumeFactor;
                }
                else  // Otherwise, assume it is one-shot
                {
                    allAudioSourcesInScene[i].volume = referenceVolumes[i] * masterVolumeFactor * oneShotVolumeFactor;
                }
            }
        }

        if(playerAudio != null)  // Also adjust custom player controller audio if relevant
        {
            playerAudio.audioClipVolume = playerControllerAudioReferenceVal * masterVolumeFactor * oneShotVolumeFactor;
        }
    }

    public void MuteAll()
    {
        foreach(AudioSource audioSource in allAudioSourcesInScene)
        {
            audioSource.mute = true;
        }
    }

    public void UnmuteAll()
    {
        foreach (AudioSource audioSource in allAudioSourcesInScene)
        {
            audioSource.mute = false;
        }
    }

}
