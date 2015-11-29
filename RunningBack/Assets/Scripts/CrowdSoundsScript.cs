using UnityEngine;
using System.Collections;

public class CrowdSoundsScript : MonoBehaviour {

	public AudioClip booing;
	public AudioClip cheering;

	private AudioSource source;
    private AudioSource generalSound;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
        GameObject generalSoundObject = GameObject.FindGameObjectWithTag("AudioManager");
        generalSound = generalSoundObject.GetComponent<AudioSource>();
	}

	void playAudioClip(AudioClip audioClip) {
        generalSound.volume = 0.5f;
		source.PlayOneShot (audioClip, 1f);
        StartCoroutine(increaseVolume(audioClip.length));
    }

    public IEnumerator increaseVolume(float audioClipLength)
    {
        yield return new WaitForSeconds(audioClipLength);
        generalSound.volume = 1f;
    }

    public void playBooing() {
		playAudioClip (booing);
	}
	
	public void playCheering() {
		playAudioClip (cheering);
	}
	
}
