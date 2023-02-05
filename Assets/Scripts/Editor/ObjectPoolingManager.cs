using System.Collections.Generic;
using General;
using UnityEditor;

namespace Editor
{
   [CustomEditor(typeof(General.ObjectPoolingManager))]
   [CanEditMultipleObjects]
   public class ObjectPoolingManager : UnityEditor.Editor
   {

      public override void OnInspectorGUI()
      {
         var myObjectPoolingManager = target as General.ObjectPoolingManager;
         Dictionary<string, General.ObjectPooling> objectPools = myObjectPoolingManager.objectPools;
         if (objectPools != null)
         {
            foreach (KeyValuePair<string, General.ObjectPooling> kvp in objectPools)
            {
            
               EditorGUILayout.BeginHorizontal();
               EditorGUILayout.TextField(kvp.Key);
               EditorGUILayout.ObjectField(kvp.Value, typeof(ObjectPooling),true);
               EditorGUILayout.EndHorizontal();
            }
         }
         base.OnInspectorGUI();  
      }
   }
}
