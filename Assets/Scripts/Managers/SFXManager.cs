using UnityEngine;

public class SFXManager : MonoBehaviour
{

    [SerializeField] private AudioClip[] audioclips;
    //arrays para espadas
    //arrays para gritos sonidos humanos
    //
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(int index)
    {
        audioSource.PlayOneShot(audioclips[index]);
        //audioSource.pitch
    }
}
