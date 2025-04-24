using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionPoint", menuName = "Airships/InteractionPointData", order = 1)]
public class InteractionPointData : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Text;
    
    public GameObject Prefab;
    
    public Sprite image0;
    public Sprite image1;
}
