using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask = new LayerMask();

    private Camera mainCamera;

    private List<Unit> selectedUnits = new List<Unit>();

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
        foreach (Unit unit in selectedUnits)
        {
            unit.Deselect();
        }

        selectedUnits.Clear();
    }

    private void ClearSelectionArea()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

        if(!hit.collider.TryGetComponent<Unit>(out Unit unit)) return;

        if(!unit.hasAuthority) return;

        Debug.Log("hit a unit");
        selectedUnits.Add(unit);

        foreach(Unit selectedUnit in selectedUnits)
        {
            selectedUnit.Select();
        }
    }
}
