using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
	public AudioClip[] clips;

	public RangedFloat volume;

	[MinMaxRange(0, 2)]
	public RangedFloat pitch;

	AudioClip oneshotClip;

	public override void Play(AudioSource source)
	{
		if (clips.Length == 0) return;

		source.clip = clips[Random.Range(0, clips.Length)];
		source.volume = Random.Range(volume.minValue, volume.maxValue);
		source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
		source.Play();
	}

	public void PlayOneShot(AudioSource source)
	{
		if (clips.Length == 0) return;

		oneshotClip = clips[Random.Range(0, clips.Length)];
		source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
		source.PlayOneShot(oneshotClip, Random.Range(volume.minValue, volume.maxValue));
	}

	[ContextMenu("Usable Default")]
	void UsableDefault(){
		volume.maxValue = 1f;
		volume.minValue = 0.95f;
		pitch.maxValue = 1.03f;
		pitch.minValue = 0.97f;
	}
}