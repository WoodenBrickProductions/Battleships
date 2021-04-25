using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.WSA;

public enum GameState
{
    ShipPlacement, PlayerAttack, EnemyAttack, PlayerWin, PlayerDefeat
}

public class GameController : MonoBehaviour
{
    private Plane _mousePlane = new Plane(Vector3.up, 0);
    [SerializeField] private GameState gameState;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject playerBoardOrigin;
    [SerializeField] private GameObject enemyBoardOrigin;
    [SerializeField] private GameObject tile;
    
    private BoatController _selectedBoat;
    private int _selectableLayerMask;
    private int _tileLayerMask;
    private List<BoatController> _placedBoats;
    private List<BoatController> _enemyBoats;
    private TileTrigger _selectedTile;

    private GameObject smallBoat;
    
    // Gameplay phase;

    

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.ShipPlacement;
        _selectableLayerMask = LayerMask.GetMask("PlayerBoat");
        _tileLayerMask = LayerMask.GetMask("TileSelection");
        _placedBoats = new List<BoatController>();
        print("Trying to make a boat");
        // FIX FIX FIX FIX FIX FIX FIX FIX FIX 
        // FIX FIX FIX FIX FIX FIX FIX FIX FIX 
        // FIX FIX FIX FIX FIX FIX FIX FIX FIX 
        // FIX FIX FIX FIX FIX FIX FIX FIX FIX 
        // FIX FIX FIX FIX FIX FIX FIX FIX FIX 
        // FIX FIX FIX FIX FIX FIX FIX FIX FIX 
        // FIX FIX FIX FIX FIX FIX FIX FIX FIX 
        // FIX FIX FIX FIX FIX FIX FIX FIX FIX 
        // Asset loading doesn't work
        // smallBoat = (GameObject) GameObject.Instantiate(Resources.Load("Prefabs/Position.prefab"));
        print("Creating Tile");
        print("Creating Board");
        CreateBoardTiles();
        CreateEnemyBoard();

    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.ShipPlacement:
                ShipPlacement();
                break;
            case GameState.PlayerAttack:
                PlayerAttack();
                break;
            case GameState.EnemyAttack:
                PlayerAttack();
                break;
        }
        
        // Temporary
        if (Input.GetKeyDown(KeyCode.D))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _selectableLayerMask))
            {
                audioManager.Play("Destruction");
                Destroy(hit.collider.gameObject);
            }

        }
    }

    private void ChangeGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.ShipPlacement:
                break;
            case GameState.PlayerAttack:
                cameraController.ChangeCameraPosition();
                break;
            case GameState.EnemyAttack:
                cameraController.ChangeCameraPosition();
                break;
            case GameState.PlayerWin:
                break;
            case GameState.PlayerDefeat:
                break;
        }
        this.gameState = gameState;
    }

    public void ShipPlacement()
    {
        if (_placedBoats.Count == 10)
        {
            ChangeGameState(GameState.PlayerAttack);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            // Ship selection on left mouse click
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _selectableLayerMask))
            {
                _selectedBoat = hit.collider.GetComponentInParent<BoatController>();
                print(_selectedBoat.name);
                if (!_selectedBoat.IsMovable())
                {
                    _selectedBoat = null;
                }
            }
            else
            {
                _selectedBoat = null;
            }
        }
        
        if (Input.GetMouseButton(0) && _selectedBoat != null)
        {
            // Ship movement
            _selectedBoat.transform.position = GetWorldPositionFromScreen(Input.mousePosition);
            if (Input.GetKeyDown(KeyCode.R))
            {
                _selectedBoat.transform.Rotate(0, 90, 0);
            }
        }

        if (Input.GetMouseButtonUp(0) && _selectedBoat != null)
        {
            // Ship placement to position
            if (_selectedBoat.ChangedPosition())
            {
                audioManager.Play("Success");
                _placedBoats.Add(_selectedBoat);
            }
            else
            {
                audioManager.Play("Gameover");
            }
            _selectedBoat.SnapToGridPosition();
            _selectedBoat = null;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _selectableLayerMask))
            {
                _selectedBoat = hit.collider.GetComponentInParent<BoatController>();
                print(_selectedBoat.name + " removing");
                if (!_selectedBoat.IsMovable())
                {
                    _selectedBoat.ResetPosition();
                    _placedBoats.Remove(_selectedBoat);
                    _selectedBoat = null;
                }
            }
        }
    }

    public void CreateBoardTiles()
    {
        GameObject tiles = playerBoardOrigin.transform.GetChild(0).gameObject;
        for (int i = 0; i < 100; i++)
        {
            GameObject newTile = GameObject.Instantiate(tile);
            newTile.transform.parent = tiles.transform;
            newTile.transform.position = playerBoardOrigin.transform.position + new Vector3((i / 10) + 0.5f, 0, (i % 10) + 0.5f);
        }
    }

    public void CreateEnemyBoard()
    {
        GameObject tiles = enemyBoardOrigin.transform.GetChild(0).gameObject;
        for (int i = 0; i < 100; i++)
        {
            GameObject newTile = GameObject.Instantiate(tile);
            newTile.transform.parent = tiles.transform;
            newTile.transform.position = enemyBoardOrigin.transform.position + new Vector3((i / 10) + 0.5f, 0, (i % 10) + 0.5f);
        }

        // FINISH FINISH FINISH FINISH FINISH
        // FINISH FINISH FINISH FINISH FINISH
        // FINISH FINISH FINISH FINISH FINISH
        // FINISH FINISH FINISH FINISH FINISH
        // FINISH FINISH FINISH FINISH FINISH
        // FINISH FINISH FINISH FINISH FINISH
        // FINISH FINISH FINISH FINISH FINISH
        // FINISH FINISH FINISH FINISH FINISH
        
        // for (int i = 0; i < 4; i++)
        // {
        //     GameObject boat = enemyBoardOrigin.transform.GetChild(1).GetChild(0).gameObject;
        // }
        
    }
    
    public void PlayerAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _tileLayerMask))
            {
                TileTrigger tile = hit.collider.GetComponentInParent<TileTrigger>();
                if (tile.IsMarked())
                {
                    print("ADD ERROR SOUND");
                    print("This tile is already clicked");
                    // audioManager.Play("Error");
                    return;
                }

                GameObject occupiedObject = tile.GetOccupiedObject();
                if (occupiedObject != null)
                {
                    IHittable boatPiece = tile.GetOccupiedObject().GetComponentInChildren<IHittable>();
                    boatPiece.Hit();
                    BoatController boat = tile.GetOccupiedObject().GetComponentInParent<BoatController>();
                    if (boat.IsDestroyed())
                    {
                        audioManager.Play("Destruction");
                        _placedBoats.Remove(boat);
                        // Destroy(hit.collider.GetComponentInParent<BoatController>().gameObject );
                    }
                    tile.SetMarked();
                }
                else
                {
                    tile.SetMarked();
                    ChangeGameState(GameState.EnemyAttack);
                }
            }
            else
            {
                print("ADD ERROR SOUND");
                print("Player clicked somewhere else");
                // audioManager.Play("Error");
            }
        }
    }
    
    private Vector3 GetWorldPositionFromScreen(Vector3 screenPosition)
    {
        float distance;
        var ray = Camera.main.ScreenPointToRay(screenPosition);
        if (_mousePlane.Raycast(ray, out distance))
            //Raycast does the calculation, while getPoint(distance) returns a point at calculated distance.
            return ray.GetPoint(distance);
        return new Vector3();
    }
}
