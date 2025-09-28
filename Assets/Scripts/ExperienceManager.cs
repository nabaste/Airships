using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using UnityEngine.Video;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] private GameObject airship;
    [SerializeField] private InteractionPointData[] interactionData;
    [SerializeField] private GameObject interactionCanvasPrefab;
    [SerializeField] private GameObject videoCanvasPrefab;
    private Vector3 _mainAssetLocation;
    private OVRCameraRig _cameraRig;

    private GameObject _mainAssetInstance;
    private GameObject _interactionPrefabInstance;
    private GameObject _interactionCanvasInstance;
    private GameObject _videoCanvasInstance;
    
    
    private const float SPAWN_DISTANCE_FROM_CAMERA = 0.75f;
    public static ExperienceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        _cameraRig = FindObjectOfType<OVRCameraRig>(false);
    }
    

    
    private void Start()
    {
        MRUK.Instance.RoomCreatedEvent.AddListener(SpawnAirship);
    }

    private void SpawnAirship(MRUKRoom room)
    {
        var playerPosition = _cameraRig.centerEyeAnchor.transform.position;
       _mainAssetLocation = room.FloorAnchor.GetAnchorCenter();
       // _mainAssetLocation.y = playerPosition.y * 0.75f;
       _mainAssetLocation.y = 1.75f;
       _mainAssetInstance = Instantiate(airship, _mainAssetLocation, Quaternion.identity);
       _mainAssetInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void SpawnInteraction(int index)
    {
        ClearInteractions();

        if (interactionData[index].Text != "")
        {
            _interactionCanvasInstance = Instantiate(interactionCanvasPrefab, GetInteractionSpawnLocation(false),
                Quaternion.identity);
            _interactionCanvasInstance.GetComponent<InteractionCanvas>().SetInteractionCanvasTextAndImages(interactionData[index]);
        }

        if (interactionData[index].Prefab)
        {
            _interactionPrefabInstance = Instantiate(interactionData[index].Prefab, GetInteractionSpawnLocation(true), Quaternion.identity);
        }

        if (interactionData[index].videoClip)
        {
            _videoCanvasInstance = Instantiate(videoCanvasPrefab, GetInteractionSpawnLocation(true), Quaternion.identity);
            _videoCanvasInstance.GetComponent<InteractionCanvas>().SetInteractionCanvasTextAndImages(interactionData[index], true);
        }
        
    }
    private Vector3 GetInteractionSpawnLocation(bool isPrefab, bool isVideo = false)
    {
        Vector3 spawnLocation = _cameraRig.centerEyeAnchor.transform.position + _cameraRig.centerEyeAnchor.transform.forward * SPAWN_DISTANCE_FROM_CAMERA;
        if (isPrefab)
        {
            spawnLocation.y = _cameraRig.centerEyeAnchor.transform.position.y * 0.5f;
        }

        if (isVideo)
        {
            spawnLocation.y = _cameraRig.centerEyeAnchor.transform.position.y * 0.85f;
        }
        
        return spawnLocation;
    }

    public void ClearInteractions()
    {
        if (_interactionPrefabInstance)
        {
            Destroy(_interactionPrefabInstance);
        }

        if (_interactionCanvasInstance)
        {
            Destroy(_interactionCanvasInstance);
        }

        if (_videoCanvasInstance)
        {
            Destroy(_videoCanvasInstance);
        }
    }
    
    private IEnumerator SnapCanvasInFrontOfCamera()
    {
        yield return new WaitUntil(
            () => _cameraRig && _cameraRig.centerEyeAnchor.transform.position != Vector3.zero);
        transform.position = _cameraRig.centerEyeAnchor.transform.position +
                             _cameraRig.centerEyeAnchor.transform.forward * SPAWN_DISTANCE_FROM_CAMERA;
    }

}
