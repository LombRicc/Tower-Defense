using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIReadValue : MonoBehaviour
{
    public enum UIType
    {
        Wave,
        Soldier,
        Knight,
        Bird,
        Dragon,
        Health,
        Gold,
        PistolCost,
        MissileCost,
        SniperCost
    }

    [SerializeField] UIType type;

    // Update is called once per frame
    void Update()
    {
        var text = "";
        bool unsufficientCost = false; 
        var gm = GameManager.instance;
        switch (type)
        {
            case UIType.Wave:
                text = gm.waveNumber.ToString();
                break;
            case UIType.Soldier:
                text = gm.soldierNumber.ToString();
                break;
            case UIType.Knight:
                text = gm.knightNumber.ToString();
                break;
            case UIType.Bird:
                text = gm.birdNumber.ToString();
                break;
            case UIType.Dragon:
                text = gm.dragonNumber.ToString();
                break;
            case UIType.Health:
                text = gm.health.ToString();
                break;
            case UIType.Gold:
                text = gm.gold.ToString();
                break;
            case UIType.PistolCost:
                if(gm.gold < gm.pistolCost)
                    unsufficientCost = true;
                text = gm.pistolCost.ToString();
                break;
            case UIType.MissileCost:
                if (gm.gold < gm.missileCost)
                    unsufficientCost = true;
                text = gm.missileCost.ToString();
                break;
            case UIType.SniperCost:
                if (gm.gold < gm.sniperCost)
                    unsufficientCost = true;
                text = gm.sniperCost.ToString();
                break;
        }

        GetComponent<TextMeshProUGUI>().text = text;

        if (unsufficientCost)
            GetComponent<TextMeshProUGUI>().color = Color.red;
        else
            GetComponent<TextMeshProUGUI>().color = Color.black;
    }
}
