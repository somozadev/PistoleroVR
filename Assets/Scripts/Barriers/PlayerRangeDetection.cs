using UnityEngine;
using UnityEngine.Events;

public class PlayerRangeDetection : MonoBehaviour
{
    [SerializeField] protected UnityEvent _playerEnterRange;
    [SerializeField]protected UnityEvent _playerLeaveRange;
    [SerializeField] protected float range;
    [SerializeField] protected Color color;
    public FillType fillType;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
            _playerEnterRange?.Invoke();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
            _playerLeaveRange.Invoke();
    }


    
    public void AddListenerPlayerEnter(UnityAction action){ _playerEnterRange.AddListener(action);}
    public void RemoveListenerPlayerEnter(UnityAction action){_playerEnterRange.RemoveListener(action);}
    public void AddListenerPLayerLeave(UnityAction action){_playerLeaveRange.AddListener(action);}
    public void RemoveListenerPLayerLeave(UnityAction action){_playerLeaveRange.RemoveListener(action);}


    protected void OnDrawGizmos()
    {
        Gizmos.color = color;
        if (fillType == FillType.wiredSphere)
        {
            GetComponent<SphereCollider>().radius = range;
            Gizmos.DrawWireSphere(transform.position, range);
        }
        else
        {
            BoxCollider bc = GetComponent<BoxCollider>();
            var bcTrf = bc.transform;
            var rotationMatrix = Matrix4x4.TRS(bcTrf.position, bcTrf.rotation, bcTrf.lossyScale);
            Gizmos.matrix = rotationMatrix;
            if (fillType == FillType.wiredCube)
                Gizmos.DrawWireCube(bc.center, bc.size);
            else if (fillType == FillType.filledCube)
                Gizmos.DrawCube(bc.center, bc.size);            
        }
        
        
       
        
    } public enum FillType
    {
        wiredSphere,
        wiredCube,
        filledCube
    }
}