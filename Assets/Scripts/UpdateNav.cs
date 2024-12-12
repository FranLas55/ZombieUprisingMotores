using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class UpdateNav : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.surface.BuildNavMesh();
    }
}
