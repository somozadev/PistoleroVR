using General;
using UnityEngine;

public class UILookToTarget : MonoBehaviour
{
    [SerializeField] private UILookTarget target;
    [SerializeField] private float rotSpeed = 10;

    void Start()
    {
        if (target == null)
            target = FindObjectOfType<UILookTarget>();
    }

    void Update()
    {
        if (target != null)
            transform.LookAt(target.transform);
    }
}