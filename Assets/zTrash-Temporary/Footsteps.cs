using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{

    [SerializeField] private AudioClip[] waterClips;
    [SerializeField] private AudioClip[] woodClips;
    [SerializeField] private AudioClip[] dirtClips;
    [SerializeField] private AudioClip[] snowClips;

    [SerializeField] ParticleSystem waterRippleStep;

    private AudioSource audioSource;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Step()
    {
        AudioClip clip = GetRandomDirtClip();

        if (player.floor == "Water")
        {
            clip = GetRandomWaterClip();
            Instantiate(waterRippleStep, 
                new Vector3(player.transform.position.x, player.transform.position.y+0.5f, player.transform.position.z), Quaternion.Euler(-90, 0, 0));
            audioSource.PlayOneShot(clip, 0.033f);
        }
        if (player.floor == "Dirt")
        {
            clip = GetRandomDirtClip();
            audioSource.PlayOneShot(clip, 0.28f);
        }
    }

    private AudioClip GetRandomWaterClip()
    { return waterClips[UnityEngine.Random.Range(0, waterClips.Length)]; }

    private AudioClip GetRandomWoodClip()
    { return woodClips[UnityEngine.Random.Range(0, woodClips.Length)]; }

    private AudioClip GetRandomDirtClip()
    { return dirtClips[UnityEngine.Random.Range(0, dirtClips.Length)]; }

    private AudioClip GetRandomSnowClip()
    { return snowClips[UnityEngine.Random.Range(0, snowClips.Length)]; }

}
