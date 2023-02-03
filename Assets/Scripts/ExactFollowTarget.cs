using UnityEngine;

public class ExactFollowTarget : MonoBehaviour
{
   [SerializeField] private Transform _target;
   [SerializeField] private Vector3 _offset;
   [SerializeField] private bool _rotation;

   private void LateUpdate()
   {
      transform.position = _target.position - _offset;
      if(!_rotation)
         transform.rotation = _target.rotation;
      else
      {
         transform.LookAt(_target.forward);
      }
   }
}
