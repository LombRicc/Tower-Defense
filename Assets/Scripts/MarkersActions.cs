using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkersActions : MonoBehaviour
{
    internal Transform turret;
    public Button confirm;

    private void Update()
    {
        if(turret != null && confirm != null)
        {
            switch (GameManager.instance.selectedTurret.GetComponent<TurretBehavior>().type)
            {
                case TurretBehavior.TurretType.Pistol:
                    if (turret.GetComponent<TurretBehavior>().occupiedTiles.Count < 1)
                    {
                        confirm.interactable = false;
                        return;
                    }
                    break;
                case TurretBehavior.TurretType.Missile:
                    if (turret.GetComponent<TurretBehavior>().occupiedTiles.Count < 4)
                    {
                        confirm.interactable = false;
                        return;
                    }
                    break;
                case TurretBehavior.TurretType.Sniper:
                    if (turret.GetComponent<TurretBehavior>().occupiedTiles.Count < 4)
                    {
                        confirm.interactable = false;
                        return;
                    }
                    break;
            }
            foreach(Tile tile in turret.GetComponent<TurretBehavior>().occupiedTiles)
            {
                if(tile == null || tile.roadOn)
                {
                    confirm.interactable = false;
                    return;
                }
            }
            confirm.interactable = true;
        }
    }

    public void RotateAction()
    {
        turret.eulerAngles = new Vector3(turret.eulerAngles.x, turret.eulerAngles.y + 90f, turret.eulerAngles.z);
        turret.GetComponent<TurretBehavior>().orientation += 1;
        if(turret.GetComponent<TurretBehavior>().orientation == TurretBehavior.TurretOrientation.None)
        {
            turret.GetComponent<TurretBehavior>().orientation = 0;
        }
        Camera.main.GetComponent<InputController>().SimulateTouch(2);
    }

    public void CancelPlacingAction()
    {
        var gm = GameManager.instance;
        if (gm.originalTurret != null)
            gm.originalTurret.GetComponent<TurretBehavior>().enabled = true;

        foreach (Tile tile in turret.GetComponent<TurretBehavior>().occupiedTiles)
        {
            tile.occupied = false;
        }
        Destroy(turret.gameObject);
        gm.selectedTurret = null;
        gm.placingTurret = false;
        gm.placingTurretGo = null;
        gm.originalTurret = null;
        gameObject.SetActive(false);
    }

    public void PlaceAction()
    {
        var gm = GameManager.instance;
        if(gm.originalTurret == null)
        {
            switch (gm.selectedTurret.GetComponent<TurretBehavior>().type)
            {
                case TurretBehavior.TurretType.Pistol:
                    gm.gold -= gm.pistolCost;
                    break;
                case TurretBehavior.TurretType.Missile:
                    gm.gold -= gm.missileCost;
                    break;
                case TurretBehavior.TurretType.Sniper:
                    gm.gold -= gm.sniperCost;
                    break;
            }
        }
        else
        {
            Destroy(gm.originalTurret);
            gm.originalTurret = null;
        }
        foreach (BoxCollider collider in turret.GetComponentsInChildren<BoxCollider>())
            collider.enabled = true;
        turret.GetComponent<TurretBehavior>().enabled = true;
        turret.GetComponent<TurretBehavior>().placed = true;
        gm.selectedTurret = null;
        gm.placingTurret = false;
        gm.placingTurretGo = null;
        gameObject.SetActive(false);
    }

    public void MoveAction()
    {
        gameObject.SetActive(false);
        turret.GetComponent<TurretBehavior>().enabled = false;
        foreach (Tile tile in turret.GetComponent<TurretBehavior>().occupiedTiles)
        {
            tile.occupied = false;
        }
        Camera.main.GetComponent<InputController>().tile.GetComponent<Tile>().occupied = true;
        turret.GetComponent<TurretBehavior>().magazine.rotation = turret.GetComponent<TurretBehavior>().defaultRotation;
        GameManager.instance.selectedTurret = turret.gameObject;
        GameManager.instance.originalTurret = turret.gameObject;
        Camera.main.GetComponent<InputController>().SimulateTouch(1);
    }

    public void CancelSelectionAction()
    {
        gameObject.SetActive(false);
        turret.GetComponent<TurretBehavior>().enabled = true;
    }

    public void DestroyAction()
    {
        switch (turret.GetComponent<TurretBehavior>().type)
        {
            case TurretBehavior.TurretType.Pistol:
                GameManager.instance.gold += GameManager.instance.pistolCost / 2;
                break;
            case TurretBehavior.TurretType.Missile:
                GameManager.instance.gold += GameManager.instance.missileCost / 2;
                break;
            case TurretBehavior.TurretType.Sniper:
                GameManager.instance.gold += GameManager.instance.sniperCost / 2;
                break;
        }
        foreach(Tile tile in turret.GetComponent<TurretBehavior>().occupiedTiles)
        {
            tile.occupied = false;
        }
        gameObject.SetActive(false);
        Destroy(turret.gameObject);
    }
}
