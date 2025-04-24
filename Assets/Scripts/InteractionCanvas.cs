using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionCanvas : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descriptionText;

    [SerializeField] private Image image0;
    [SerializeField] private Image image1;

    private Canvas _canvas;
    private OVRCameraRig _cameraRig;
    
    
    private const float SPAWN_DISTANCE_FROM_CAMERA = 1.5f;
    private void Awake()
    {
        _cameraRig = FindObjectOfType<OVRCameraRig>(false);
        _canvas = GetComponent<Canvas>();
        
        
        //StartCoroutine(SnapCanvasInFrontOfCamera());
    }

    private void Update()
    {
        Billboard();
    }
    public void SetInteractionCanvasTextAndImages(InteractionPointData interactionPointData)
    {
        titleText.text = interactionPointData.Name;
        descriptionText.text = interactionPointData.Text;

        if (interactionPointData.image0)
        {
            image0.sprite = interactionPointData.image0;
        }
        else
        {
            image0.color = new Vector4(1, 1, 1, 0);
        }

        if (interactionPointData.image1)
        {
            image1.sprite = interactionPointData.image1;
        }
        else
        {
            image1.color = new Vector4(1, 1, 1, 0);
        }
    }
    
    private void Billboard()
    {
        if (!_canvas)
        {
            return;
        }

        var direction = _canvas.transform.position - _cameraRig.centerEyeAnchor.transform.position;
        var rotation = Quaternion.LookRotation(direction);
        _canvas.transform.rotation = rotation;
    }
    

}
