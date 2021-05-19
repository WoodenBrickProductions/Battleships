using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;
using Random = UnityEngine.Random;

public enum GameState
{
    ShipPlacement, PlayerAttack, EnemyAttack, PlayerWin, PlayerDefeat, EnemyBoardPlacement
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
    [SerializeField] private MenuController menuController;
    
    //UI
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject lose;

    private float attackTimer = 1.25f;

    private bool _gameOver = false;
    private BoatController _selectedBoat;
    private int _selectableLayerMask;
    private int _tileLayerMask;
    private List<BoatController> _placedBoats;
    private List<BoatController> _enemyBoats;
    private TileTrigger _selectedTile;
    private AIController _aiController;

    private bool placingEnemyShips = false;

    private int[,] playerBoardMatrix;
    
    [SerializeField] GameObject[] boatsGameObjects;
    
    
    // Gameplay phase;

    

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.ShipPlacement;
        _selectableLayerMask = LayerMask.GetMask("PlayerBoat");
        _tileLayerMask = LayerMask.GetMask("TileSelection");
        _placedBoats = new List<BoatController>();
        _enemyBoats = new List<BoatController>();
        _aiController = GetComponent<AIController>();
        playerBoardMatrix = new int[10, 10];
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
            case GameState.EnemyBoardPlacement:
                if (!placingEnemyShips)
                {
                    placingEnemyShips = true;
                    StartCoroutine("PlaceEnemyShips");
                }
                break;
            case GameState.ShipPlacement:
                ShipPlacement();
                break;
            case GameState.PlayerAttack:
                PlayerAttack();
                break;
            case GameState.EnemyAttack:
                if (attackTimer <= 0)
                {
                    EnemyAttack();
                    attackTimer = 1.25f;
                }
                else
                {
                    attackTimer -= 1 * Time.deltaTime;
                }
                break;
            case GameState.PlayerWin:
                PlayerWin();
                break;
            case GameState.PlayerDefeat:
                PlayerDefeat();
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

    
    public void PlayerWin()
    {
        if (!_gameOver)
        {
            print("VICTORY");
            audioManager.Play("Victory");
            win.SetActive(true);
            Invoke(nameof(GoToMenu), 3f);
            _gameOver = true;
        }
    }

    public void PlayerDefeat()
    {
        if (!_gameOver)
        {
            print("DEFEAT");
            audioManager.Play("Gameover");
            lose.SetActive(true);
            Invoke(nameof(GoToMenu), 3f);
            _gameOver = false;
        }
    }
    
    private void ChangeGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.ShipPlacement:
                break;
            case GameState.PlayerAttack:
                cameraController.ChangeCameraPosition(CameraPosition.Enemy);
                break;
            case GameState.EnemyAttack:
                cameraController.ChangeCameraPosition(CameraPosition.Player);
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
        if (_placedBoats.Count == 10 || Input.GetKeyDown(KeyCode.Space))
        {
            ChangeGameState(GameState.EnemyBoardPlacement);
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
            // Internal placement logic check
            _selectedBoat.SnapToGridPosition();

            // Ship placement to position
            if (_selectedBoat.ChangedPosition())
            {
                audioManager.Play("Success");
                _placedBoats.Add(_selectedBoat);
            }
            else
            {
                audioManager.Play("Error");
            }
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

        Transform boats = enemyBoardOrigin.transform.GetChild(2);

        
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 3 - i; j < 4; j++)
            {
                GameObject boat = GameObject.Instantiate(enemyBoardOrigin.transform.GetChild(1).GetChild(i).gameObject, boats);
                _enemyBoats.Add(boat.GetComponentInParent<BoatController>());
            }
        }
        
        
    }

    public void PlaceEnemyShips()
    {
        Vector3[] positions = _aiController.CreateBoatPositions();

        for (int i = 0; i < 10; i++)
        {
            Vector3 origin = enemyBoardOrigin.transform.position;
            Vector3 boatLocation = origin +  new Vector3(positions[i].x, 0, positions[i].z);
            int rotation = (int) positions[i].y;
            Transform boat = _enemyBoats[i].transform;
            boat.position = boatLocation;
            boat.Rotate(0, 90*rotation, 0 );
        }
        
        // foreach (BoatController boat in _enemyBoats)
        // {
        //     PlaceBoat(boat);
        // }
        
        print("CHANGING TO PLAYER ATTACK");
        
        ChangeGameState(GameState.PlayerAttack);
    }

    private bool PlaceBoat(BoatController boat)
    {
        GameObject tiles = enemyBoardOrigin.transform.GetChild(0).gameObject;
        Vector3 origin = enemyBoardOrigin.transform.position;
        int k = 0;
        // do
        // {
            k++;
            int pos = (int) ((Random.value * 100 + 1) % 100);
            int rot = (int) (Random.value * 4);
            boat.transform.Rotate(0, 90*rot, 0 );
            boat.transform.position = origin + new Vector3((pos / 10) + 0.5f, 0, (pos % 10) + 0.5f);
            boat.SnapToGridPosition();
            print("I TRIED");
        // } while (!boatController.ChangedPosition() || k > 10);
        
        if (k > 99) return false;
        
        return true;
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
                    audioManager.Play("Attack");
                    if (boat.IsDestroyed())
                    {
                        audioManager.Play("Destruction");
                        _enemyBoats.Remove(boat);
                        // Destroy(hit.collider.GetComponentInParent<BoatController>().gameObject );
                    }
                    tile.SetMarked(true);
                }
                else
                {
                    audioManager.Play("Miss");
                    tile.SetMarked(false);
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
        
        

        if (_enemyBoats.Count == 0)
        {
            ChangeGameState(GameState.PlayerWin);
        }
    }

    private void EnemyAttack()
    {
        bool failed = true;
        while (failed)
        {
            int a = (int) Mathf.Clamp(Random.value * 10, 0f, 9f);
            int b = (int) Mathf.Clamp(Random.value * 10, 0f, 9f);

            failed = false;
            if (playerBoardMatrix[a, b] >= 1)
            {
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(playerBoardOrigin.transform.position + new Vector3(a + 0.5f, 5, b + 0.5f), 
                    -Vector3.up, out hit, Mathf.Infinity, _tileLayerMask))
                {
                    TileTrigger tile = hit.collider.GetComponentInParent<TileTrigger>();
                    if (tile.IsMarked())
                    {
                        playerBoardMatrix[a, b] = 1;
                        failed = true;
                        print("ADD ERROR SOUND");
                        print("This tile is already clicked");
                        // audioManager.Play("Error");
                    }
                    else
                    {
                        GameObject occupiedObject = tile.GetOccupiedObject();
                        if (occupiedObject != null)
                        {
                            IHittable boatPiece = tile.GetOccupiedObject().GetComponentInChildren<IHittable>();
                            boatPiece.Hit();
                            BoatController boat = tile.GetOccupiedObject().GetComponentInParent<BoatController>();
                            audioManager.Play("Attack");
                            if (boat.IsDestroyed())
                            {
                                audioManager.Play("Destruction");
                                _placedBoats.Remove(boat);
                                // Destroy(hit.collider.GetComponentInParent<BoatController>().gameObject );
                            }
                            tile.SetMarked(true);
                            playerBoardMatrix[a, b] = 2;
                        }
                        else
                        {
                            audioManager.Play("Miss");
                            tile.SetMarked(false);
                            playerBoardMatrix[a, b] = 1;
                            ChangeGameState(GameState.PlayerAttack);
                        }
                    }
                }
            }
        }
        
        if (_placedBoats.Count == 0)
        {
            ChangeGameState(GameState.PlayerDefeat);
        }
    }

    public void GoToMenu()
    {
        menuController.ChangeScene(0);
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
