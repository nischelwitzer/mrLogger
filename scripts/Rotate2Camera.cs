
using UnityEngine;
using System.Collections;

public class Rotate2Camera : MonoBehaviour
{
    [SerializeField] private Transform referencePos;
    [SerializeField] private bool mirror;
  
    void Awake()
    {
        if (!referencePos)
            referencePos = Camera.main.transform;
    }

    void Update()
    {
        Vector3 direction = referencePos.position - this.transform.position;
        direction.y = 0; // Y-Achse bleibt unverändert
        this.transform.rotation = Quaternion.LookRotation(direction);
        if (mirror) transform.Rotate(0, 180, 0);
    }
}