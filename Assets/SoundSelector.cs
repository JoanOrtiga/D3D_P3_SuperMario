using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSelector : MonoBehaviour
{
    public AudioClip lifeUp;
    public AudioClip lifeDown;
    public AudioClip coin;
    private AudioSource sonido;
    private void Start()
    {
        sonido = this.GetComponent<AudioSource>();
    }
    public void StarUp()
    {
        sonido.clip = lifeUp;
        sonido.Play();
    }
    public void StarDown()
    {
        sonido.clip = lifeDown;
        sonido.Play();
    }
    public void Coin()
    {
        sonido.clip = coin;
        sonido.Play();
    }
}
