using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    public AudioSource musicSource;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private Slider sfxSlider;

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    void Start()
    {
        //Carrega todas as faixas no dicionário
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];

        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }

        LoadVolume();

        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }

    //Função usada para tocar os efeitos sonoros
    public void PlaySFX(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }

    //Atualiza e salva o valor dos volumes de música de fundo e efeitos sonoros
    public void UpdateVolume()
    {
        musicSource.volume = musicSlider.value;
        sfxSource.volume = sfxSlider.value;

        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
    }


    //Carrega os volumes de música de fundo e efeitos sonoros
    public void LoadVolume()
    {
        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.5f);

        musicSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;
    }
}
