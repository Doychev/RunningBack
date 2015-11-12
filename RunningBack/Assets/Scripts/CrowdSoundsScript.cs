using UnityEngine;
using System.Collections;

public class CrowdSoundsScript : MonoBehaviour {

	public AudioClip booing;
	public AudioClip cheering;

	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}

	void playAudioClip(AudioClip audioClip) {
		source.PlayOneShot (audioClip, 1f);
	}

	public void playBooing() {
		playAudioClip (booing);
	}
	
	public void playCheering() {
		playAudioClip (cheering);
	}
	
}
