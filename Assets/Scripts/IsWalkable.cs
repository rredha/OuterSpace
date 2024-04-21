using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWalkable : MonoBehaviour {
    [SerializeField] private List<Path> possiblePaths = new List<Path>();
    public Transform previousBlock;

    [SerializeField] private Transform gizmoTopPoint;

    private void OnDrawGizmos() {
       Gizmos.color = Color.blue;
       Gizmos.DrawSphere(gizmoTopPoint.position, 0.10f);

       foreach (Path possiblePath in possiblePaths) {
           if (possiblePath.isActive == true) {
               Transform gizmoCenterPoint = possiblePath.target.GetChild(0);
               Gizmos.DrawLine(gizmoCenterPoint.position, gizmoTopPoint.position);
           }
       }
   }

    public List<Path> GetPossiblePaths() {
        return possiblePaths;
    }
}
