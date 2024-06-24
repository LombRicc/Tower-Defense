using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

[RequireComponent(typeof(SwipeManager))]
public class InputController : MonoBehaviour
{
    private Transform _camera;
    private Vector3 originalPosition;
    private float originalFieldOfView;

    public float slideAmount;
    public LayerMask tileMask;
    public LayerMask turretMask;
    public RectTransform canvasRect;
    public RectTransform markers;
    public RectTransform markersPlaced;
    private float markerOffset = 5f;
    private GameManager gm;
    public Transform tile;
    private Transform turret;
    private Vector3 markerPosition1;
    private Vector3 markerPosition2;

    void OnEnable()
    {
        gm = GameManager.instance;
        originalPosition = transform.position;
        originalFieldOfView = GetComponent<Camera>().fieldOfView;
        SwipeManager swipeManager = GetComponent<SwipeManager>();
        swipeManager.onSwipe += HandleSwipe;
        _camera = transform;
        slideAmount = gm.tilesXrow;
        _camera.Translate(Vector3.up * slideAmount/2f);
        _camera.Translate(Vector3.right * slideAmount/2f);
        GetComponent<Camera>().orthographicSize = slideAmount-5 + 15;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if (gm.selectedTurret)
            {
                if(gm.placingTurretGo == null)
                {
                    gm.placingTurretGo = Instantiate(gm.selectedTurret);
                    gm.placingTurretGo.SetActive(false);
                }
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileMask) && !hit.transform.GetComponent<Tile>().occupied)
                {
                    if (!gm.placingTurret)
                    {
                        HandleTurret(hit);
                    }
                    else if (hit.transform != tile && hit.transform.position != markerPosition1 && hit.transform.position != markerPosition2)
                    {
                        tile.GetComponent<Tile>().occupied = false;
                        foreach (Tile tile in gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles)
                        {
                            tile.occupied = false;
                        }
                        gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Clear();
                        gm.placingTurret = false;
                    }
                }
            }
            else
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, turretMask))
                {
                    gm.playerPaneCollapsed.SetActive(true);
                    gm.playerPaneExpanded.SetActive(false);
                    turret = hit.transform;

                    Vector2 canvasPos;
                    Vector2 screenPoint = Camera.main.WorldToScreenPoint(new Vector3(turret.position.x, turret.position.y, turret.position.z + markerOffset));

                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);

                    markersPlaced.gameObject.SetActive(true);
                    markersPlaced.localPosition = canvasPos;
                    markersPlaced.GetComponentInChildren<MarkersActions>().turret = turret;

                    markerPosition1 = new Vector3(hit.transform.position.x - 5.5f, hit.transform.position.y, hit.transform.position.z + 5.5f);
                    markerPosition2 = new Vector3(hit.transform.position.x + 5.5f, hit.transform.position.y, hit.transform.position.z + 5.5f);
                }
            }
        }
    }

    public void SimulateTouch(int numberOfTouches)
    {
        for(int i = 0; i < numberOfTouches; i++)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if (gm.placingTurretGo == null)
            {
                gm.placingTurretGo = Instantiate(gm.selectedTurret);
                gm.placingTurretGo.transform.eulerAngles = new Vector3(gm.placingTurretGo.transform.eulerAngles.x, gm.placingTurretGo.transform.eulerAngles.y + 90f, gm.placingTurretGo.transform.eulerAngles.z);
                gm.placingTurretGo.GetComponent<TurretBehavior>().orientation += 1;
                if (gm.placingTurretGo.GetComponent<TurretBehavior>().orientation == TurretBehavior.TurretOrientation.None)
                    gm.placingTurretGo.GetComponent<TurretBehavior>().orientation = 0;
                gm.placingTurretGo.SetActive(false);
            }
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileMask) && hit.transform.GetComponent<Tile>().occupied)
            {
                if (!gm.placingTurret)
                {
                    HandleTurret(hit);
                }
                else if (hit.transform == tile && hit.transform.position != markerPosition1 && hit.transform.position != markerPosition2)
                {
                    foreach (Tile tile in gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles)
                    {
                        tile.occupied = false;
                    }
                    tile.GetComponent<Tile>().occupied = true;
                    gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Clear();
                    gm.placingTurret = false;
                }
            }
        }
    }

    void HandleTurret(RaycastHit hit)
    {
        gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Clear();
        gm.playerPaneCollapsed.SetActive(true);
        gm.playerPaneExpanded.SetActive(false);
        tile = hit.transform;
        gm.grid.FindNeighbors(tile.GetComponent<Tile>());
        switch (gm.placingTurretGo.GetComponent<TurretBehavior>().type)
        {
            case TurretBehavior.TurretType.Pistol:
                tile.GetComponent<Tile>().occupied = true;
                gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(tile.GetComponent<Tile>());
                break;
            case TurretBehavior.TurretType.Missile:
                tile.GetComponent<Tile>().occupied = true;
                gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(tile.GetComponent<Tile>());
                var adjacentTile = new Tile();
                switch (gm.placingTurretGo.GetComponent<TurretBehavior>().orientation)
                {
                    case TurretBehavior.TurretOrientation.Left:
                        adjacentTile = gm.grid.tiles[tile.GetComponent<Tile>().X - 1 < 0 ? this.tile.GetComponent<Tile>().X : tile.GetComponent<Tile>().X - 1, this.tile.GetComponent<Tile>().Y];
                        adjacentTile.occupied = true;
                        if(!gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Contains(adjacentTile))
                            gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(adjacentTile);
                        adjacentTile = gm.grid.tiles[tile.GetComponent<Tile>().X, tile.GetComponent<Tile>().Y + 1 > gm.tilesXrow-1 ? this.tile.GetComponent<Tile>().Y : this.tile.GetComponent<Tile>().Y + 1];
                        adjacentTile.occupied = true;
                        if (!gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Contains(adjacentTile))
                            gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(adjacentTile);
                        adjacentTile = gm.grid.tiles[tile.GetComponent<Tile>().X - 1 < 0 ? this.tile.GetComponent<Tile>().X : tile.GetComponent<Tile>().X - 1, tile.GetComponent<Tile>().Y + 1 > gm.tilesXrow - 1 ? this.tile.GetComponent<Tile>().Y : this.tile.GetComponent<Tile>().Y + 1];
                        adjacentTile.occupied = true;
                        if (!gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Contains(adjacentTile))
                            gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(adjacentTile);
                        break;
                    case TurretBehavior.TurretOrientation.Up:
                        adjacentTile = gm.grid.tiles[tile.GetComponent<Tile>().X + 1 > gm.tilesXrow - 1 ? this.tile.GetComponent<Tile>().X : tile.GetComponent<Tile>().X + 1, this.tile.GetComponent<Tile>().Y];
                        adjacentTile.occupied = true;
                        if (!gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Contains(adjacentTile))
                            gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(adjacentTile);
                        adjacentTile = gm.grid.tiles[tile.GetComponent<Tile>().X, tile.GetComponent<Tile>().Y + 1 > gm.tilesXrow - 1 ? this.tile.GetComponent<Tile>().Y : this.tile.GetComponent<Tile>().Y + 1];
                        adjacentTile.occupied = true;
                        if (!gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Contains(adjacentTile))
                            gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(adjacentTile);
                        adjacentTile = gm.grid.tiles[tile.GetComponent<Tile>().X + 1 > gm.tilesXrow - 1 ? this.tile.GetComponent<Tile>().X : tile.GetComponent<Tile>().X + 1, tile.GetComponent<Tile>().Y + 1 > gm.tilesXrow - 1 ? this.tile.GetComponent<Tile>().Y : tile.GetComponent<Tile>().Y + 1];
                        adjacentTile.occupied = true;
                        if (!gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Contains(adjacentTile))
                            gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(adjacentTile);
                        break;
                    case TurretBehavior.TurretOrientation.Right:
                        adjacentTile = gm.grid.tiles[tile.GetComponent<Tile>().X + 1 > gm.tilesXrow - 1 ? this.tile.GetComponent<Tile>().X : tile.GetComponent<Tile>().X + 1, this.tile.GetComponent<Tile>().Y];
                        adjacentTile.occupied = true;
                        if (!gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Contains(adjacentTile))
                            gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(adjacentTile);
                        adjacentTile = gm.grid.tiles[tile.GetComponent<Tile>().X, tile.GetComponent<Tile>().Y - 1 < 0 ? this.tile.GetComponent<Tile>().Y : this.tile.GetComponent<Tile>().Y - 1];
                        adjacentTile.occupied = true;
                        if (!gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Contains(adjacentTile))
                            gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(adjacentTile);
                        adjacentTile = gm.grid.tiles[tile.GetComponent<Tile>().X + 1 > gm.tilesXrow - 1 ? this.tile.GetComponent<Tile>().X : tile.GetComponent<Tile>().X + 1, tile.GetComponent<Tile>().Y - 1 < 0 ? this.tile.GetComponent<Tile>().Y : tile.GetComponent<Tile>().Y - 1];
                        adjacentTile.occupied = true;
                        if (!gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Contains(adjacentTile))
                            gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(adjacentTile);
                        break;
                    case TurretBehavior.TurretOrientation.Down:
                        adjacentTile = gm.grid.tiles[tile.GetComponent<Tile>().X - 1 < 0 ? this.tile.GetComponent<Tile>().X : tile.GetComponent<Tile>().X - 1, this.tile.GetComponent<Tile>().Y];
                        adjacentTile.occupied = true;
                        if (!gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Contains(adjacentTile))
                            gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(adjacentTile);
                        adjacentTile = gm.grid.tiles[tile.GetComponent<Tile>().X, tile.GetComponent<Tile>().Y - 1 < 0 ? this.tile.GetComponent<Tile>().Y : this.tile.GetComponent<Tile>().Y - 1];
                        adjacentTile.occupied = true;
                        if (!gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Contains(adjacentTile))
                            gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(adjacentTile);
                        adjacentTile = gm.grid.tiles[tile.GetComponent<Tile>().X - 1 < 0 ? this.tile.GetComponent<Tile>().X : tile.GetComponent<Tile>().X - 1, tile.GetComponent<Tile>().Y - 1 < 0 ? this.tile.GetComponent<Tile>().Y : tile.GetComponent<Tile>().Y - 1];
                        adjacentTile.occupied = true;
                        if (!gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Contains(adjacentTile))
                            gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(adjacentTile);
                        break;
                }
                break;
            case TurretBehavior.TurretType.Sniper:
                tile.GetComponent<Tile>().occupied = true;
                gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(tile.GetComponent<Tile>());
                foreach (Tile tile in tile.GetComponent<Tile>().neighbors)
                {

                    if (tile != null && !tile.occupied && tile != this.tile.GetComponent<Tile>().neighbors[(int)gm.placingTurretGo.GetComponent<TurretBehavior>().orientation > 2 ? (int)gm.placingTurretGo.GetComponent<TurretBehavior>().orientation : Mathf.Abs((int)gm.placingTurretGo.GetComponent<TurretBehavior>().orientation - 2)])
                    {
                        tile.occupied = true;
                        gm.placingTurretGo.GetComponent<TurretBehavior>().occupiedTiles.Add(tile);
                    }
                }
                break;
        }
        gm.placingTurret = true;
        gm.placingTurretGo.transform.SetPositionAndRotation(tile.position, gm.placingTurretGo.transform.rotation);
        gm.placingTurretGo.SetActive(true);

        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(new Vector3(tile.position.x, tile.position.y, tile.position.z + markerOffset));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);

        markers.gameObject.SetActive(true);
        markers.localPosition = canvasPos;
        markers.GetComponentInChildren<MarkersActions>().turret = gm.placingTurretGo.transform;

        markerPosition1 = new Vector3(hit.transform.position.x - 5.5f, hit.transform.position.y, hit.transform.position.z + 5.5f);
        markerPosition2 = new Vector3(hit.transform.position.x + 5.5f, hit.transform.position.y, hit.transform.position.z + 5.5f);
    }

    void HandleSwipe(SwipeAction swipeAction)
    {
        if (!GameManager.instance.placingTurret)
        {
            //Debug.LogFormat("HandleSwipe: {0}", swipeAction);
            if (swipeAction.direction == SwipeDirection.Up || swipeAction.direction == SwipeDirection.UpRight)
            {
                // move down
                if (_camera != null && _camera.transform.position.z - slideAmount > 0)
                {
                    _camera.Translate(Vector3.down * slideAmount);
                } 
            }
            else if (swipeAction.direction == SwipeDirection.Right || swipeAction.direction == SwipeDirection.DownRight)
            {
                // move left
                if (_camera != null && _camera.transform.position.x - slideAmount > slideAmount)
                {
                    _camera.Translate(Vector3.left * slideAmount);
                }  
            }
            else if (swipeAction.direction == SwipeDirection.Down || swipeAction.direction == SwipeDirection.DownLeft)
            {
                // move up
                if (_camera != null && _camera.transform.position.z < slideAmount * 4)
                {
                    _camera.Translate(Vector3.up * slideAmount);
                }
            }
            else if (swipeAction.direction == SwipeDirection.Left || swipeAction.direction == SwipeDirection.UpLeft)
            {
                // move right
                if (_camera != null && _camera.transform.position.x + slideAmount < slideAmount * 4)
                {
                    _camera.Translate(Vector3.right * slideAmount);
                }
            }
        }
    }

    public void ResetCamera()
    {
        transform.position = originalPosition;
        GetComponent<Camera>().fieldOfView = originalFieldOfView;
    }
}