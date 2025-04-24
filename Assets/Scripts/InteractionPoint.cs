using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPoint : MonoBehaviour
{
    [SerializeField] Material regularMaterial;
    [SerializeField] Material hoveredMaterial;
    private MeshRenderer _mesh;
    [SerializeField] private int index;
    private bool _selected;
    
    
    public static event Action<InteractionPoint> OnInteractionPointSelected;

    private void Awake()
    {
        _mesh = GetComponentInChildren<MeshRenderer>();
        _selected = false;
    }

    private void OnEnable()
    {
        OnInteractionPointSelected += HandleOtherInteractionPointSelected;
    }

    private void OnDisable()
    {
        OnInteractionPointSelected -= HandleOtherInteractionPointSelected;
    }

    private void HandleOtherInteractionPointSelected(InteractionPoint selectedPoint)
    {
        if (selectedPoint != this && _selected)
        {
            _selected = false;
            _mesh.material = regularMaterial;
        }
    }
    
    public void OnHoverEnter()
    {
        if (!_selected)
        {
            _mesh.material = hoveredMaterial;
        }
    }

    public void OnHoverExit()
    {
        if (!_selected)
        {
            _mesh.material = regularMaterial;
        }
    }
        
    public void OnClick()
    {
        if (_selected)
        {
            ExperienceManager.Instance.ClearInteractions();
            _mesh.material = regularMaterial;
            _selected = false;
        }
        else
        {
            OnInteractionPointSelected?.Invoke(this);
            
            ExperienceManager.Instance.SpawnInteraction(index);
            _selected = true;
            _mesh.material = hoveredMaterial;
        }
    }
}
