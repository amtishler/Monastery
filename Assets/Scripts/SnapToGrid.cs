using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    //Enables Snapping
    [SerializeField] private bool enableSnapping = false;
    [SerializeField] private bool snapX = false;
    [SerializeField] private bool snapY = false;
    [SerializeField] private Vector2 gridSize;

    //Works while the grid is visible
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying && enableSnapping && snapY && !snapX) GridSnapY();
        if (!Application.isPlaying && enableSnapping && snapX && !snapY) GridSnapX();
        if (!Application.isPlaying && enableSnapping && snapX && snapY) GridSnapXY();
    }

    //Snaps to the middle of a unit on the y-axis
    private void GridSnapY()
    {
        var position = new Vector2
        (
            Mathf.Floor(this.transform.position.x / this.gridSize.x) * this.gridSize.x + 0.5f,
            Mathf.RoundToInt(this.transform.position.y / this.gridSize.y) * this.gridSize.y
        );

        this.transform.position = position;
    }

    //Snaps to the middle of a unit on the x-axis
    private void GridSnapX()
    {
        var position = new Vector2
        (
            Mathf.RoundToInt(this.transform.position.x / this.gridSize.x) * this.gridSize.x,
            Mathf.Floor(this.transform.position.y / this.gridSize.y) * this.gridSize.y + 0.5f
        );

        this.transform.position = position;
    }
    
    //Snaps to the intersection of the xy-axis
    private void GridSnapXY()
    {
        var position = new Vector2
        (
            Mathf.RoundToInt(this.transform.position.x / this.gridSize.x) * this.gridSize.x,
            Mathf.RoundToInt(this.transform.position.y / this.gridSize.y) * this.gridSize.y
        );

        this.transform.position = position;
    }
}
