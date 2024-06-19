using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIReadValue : MonoBehaviour
{
    enum Type
    {
        Wave,
        Soldier,
        Knight,
        Bird,
        Dragon,
        Health,
        Gold
    }

    [SerializeField] Type type;

    // Update is called once per frame
    void Update()
    {
        var text = "";
        if(type == Type.Wave)
        {
            text = GameManager.instance.waveNumber.ToString();
        }

        GetComponent<TextMeshProUGUI>().text = text;
    }
}
