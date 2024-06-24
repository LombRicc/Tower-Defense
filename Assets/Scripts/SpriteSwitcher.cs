using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{
    public Sprite playSprite;
    private Sprite originalSprite;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        originalSprite = image.sprite;
    }

    public void SwitchSprite()
    {
        if(image.sprite ==  originalSprite)
        {
            image.sprite = playSprite;
        }
        else
        {
            image.sprite = originalSprite;
        }
    }
}
