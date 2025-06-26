using Lofelt.NiceVibrations;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  public static SoundManager Instance { get; private set; }
  public AudioSource music;
  public AudioSource sound;
  public AudioClip[] audioClip;
  [Header("Sfx stuffs")]
  [SerializeField] AudioClip hittingSfx;

  private void Start()
  {
    Instance = this;
  }

  public void OnOffMusic(float volume)
  {
    music.volume = volume;
  }
  public void OnOffSound(float volume)
  {
    sound.volume = volume;
  }
  public void PlaySound(int idSound)
  {
    sound.PlayOneShot(audioClip[idSound], 1f);
  }
  public void OpenKindButton()
  {
    PlaySound(2);
  }

  public void CloseKindButton()
  {
    PlaySound(1);
  }

  void PlaySfx(AudioClip audioClip, HapticPatterns.PresetType presetType)
  {
    if (GameManager.Instance.IsHapticOn)
    {
      HapticPatterns.PlayPreset(presetType);
    }
    if (!GameManager.Instance.IsSoundOn) return;

    AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, 1);
  }

  public void PlayHittingSfx()
  {
    PlaySfx(hittingSfx, HapticPatterns.PresetType.LightImpact);
  }
}
