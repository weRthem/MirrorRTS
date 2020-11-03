using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask = new LayerMask();

    private Camera mainCamera;

    public List<Unit> SelectedUnits {get; private set;} = new List<Unit>();

    private void Start()
    {
        mainCamera = Camera.main;
    }
    
    private void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Start the selection area
            DeselectUnits();
        }
        else if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
            Debug.Log("looking for unit");
        }

    }

    private void DeselectUnits()
    {
        foreach (Unit unit in SelectedUnits)
        {
            unit.Deselect();
        }

        SelectedUnits.Clear();
    }

    private void ClearSelectionArea()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

        if(!hit.collider.TryGetComponent<Unit>(out Unit unit)) return;

        if(!unit.hasAuthority) return;

        Debug.Log("hit a unit");
        SelectedUnits.Add(unit);

        foreach(Unit selectedUnit in SelectedUnits)
        {
            selectedUnit.Select();
        }
    }
}
