using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTurret : MonoBehaviour
{
    enum TurretTypeUI
    {
        Pistol,
        Missile,
        Sniper
    }

    [SerializeField] TurretTypeUI type;

    public void SelectionAction()
    {
        var gm = GameManager.instance;
        if(gm.placingTurretGo != null)
        {
            Destroy(gm.placingTurretGo);
        }
        if (GetComponent<Image>().enabled)
        {
            gm.selectedTurret = null;
            GetComponent<Image>().enabled = false;
        }
        else
        {
            switch (type)
            {
                case TurretTypeUI.Pistol:
                    if (gm.gold >= gm.pistolCost)
                    {
                        GetComponent<Image>().enabled = true;
                        gm.selectedTurret = gm.pistol;
                    }
                    else
                    {
                        gm.selectedTurret = null;
                    }
                    break;
                case TurretTypeUI.Missile:
                    if (gm.gold >= gm.missileCost)
                    {
                        GetComponent<Image>().enabled = true;
                        gm.selectedTurret = gm.missile;
                    }
                    else
                    {
                        gm.selectedTurret = null;
                    }
                    break;
                case TurretTypeUI.Sniper:
                    if (gm.gold >= gm.sniperCost)
                    {
                        GetComponent<Image>().enabled = true;
                        gm.selectedTurret = gm.sniper;
                    }
                    else
                    {
                        gm.selectedTurret = null;
                    }
                    break;
            }
        }
    }
}
