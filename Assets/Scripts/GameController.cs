using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    ShipPlacement, PlayerAttack, EnemyAttack, PlayerWin, PlayerDefeat
}

public class GameController : MonoBehaviour
{
    private Plane _mousePlane = new Plane(Vector3.up, 0);
    [SerializeField] private GameState gameState;
    private BoatController _selectedBoat;
    private int _selectableLayerMask;
    private List<BoatController> _placedBoats;
    

    
    
    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.ShipPlacement;
        _selectableLayerMask = LayerMask.GetMask("PlayerBoat");
        _placedBoats = new List<BoatController>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.ShipPlacement:
                ShipPlacement();
                break;
            
        }
    }

    private void ChangeGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.ShipPlacement:
                break;
            case GameState.PlayerAttack:
                break;
            case GameState.EnemyAttack:
                break;
            case GameState.PlayerWin:
                break;
            case GameState.PlayerDefeat:
                break;
        }
    }

    public void ShipPlacement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _selectableLayerMask))
            {
                _selectedBoat = hit.collider.GetComponentInChildren<BoatController>();
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
            _selectedBoat.transform.position = GetWorldPositionFromScreen(Input.mousePosition);
            if (Input.GetKeyDown(KeyCode.R))
            {
                _selectedBoat.transform.Rotate(0, 90, 0);
            }
        }

        if (Input.GetMouseButtonUp(0) && _selectedBoat != null)
        {
            if (_selectedBoat.ChangedPosition())
            {
                _placedBoats.Add(_selectedBoat);
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
                _selectedBoat = hit.collider.GetComponentInChildren<BoatController>();
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
