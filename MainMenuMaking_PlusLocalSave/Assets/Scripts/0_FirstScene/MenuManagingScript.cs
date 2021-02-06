using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct MenuEntity
{
    public float Volume;
};

public class MenuManagingScript : MonoBehaviour
{
    public MenuEntity mE = new MenuEntity();
    public Slider volumeSlider;
    public Text volumeText;
    AudioSource volume;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        mE.Volume = volumeSlider.value;
        volume.volume=volumeSlider.value;
        volumeText.text = ((int)(volumeSlider.value*100)).ToString();
    }
}
