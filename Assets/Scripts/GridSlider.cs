using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridSlider : MonoBehaviour
{
    public TextMeshProUGUI sliderText;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Slider>().value = GameManager.instance.tilesXrow;
        sliderText.text = GetComponent<Slider>().value.ToString();
    }
}
