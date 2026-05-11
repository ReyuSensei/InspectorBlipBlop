using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class RandomImageGenerator : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> images = new List<Sprite>();
    [SerializeField]
    private Image image;
    private int currentImg;
    void Start()
    {
        currentImg = Random.Range(0, images.Count);
        image.sprite = images[currentImg];
    }


    void Update()
    {
        
    }
}
