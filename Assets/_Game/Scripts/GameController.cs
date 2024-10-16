using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Dreamteck.Splines;

public class GameController : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] Transform background;
    float distanceToBG;

    Vector3 touchWorldPosition = Vector3.zero;

    Node currentNode;
    SplineRenderer currentSplineRenderer = null;
    SplineComputer currentSplineComputer = null;

    bool isGameStart = false;

    void Awake()
    {
        distanceToBG = _camera.transform.position.y - background.position.y;
    }

    void Update()
    {
        if (GameManager.Ins.IsState(GameState.Gameplay))
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchWorldPosition = GetWorldPosition(Input.mousePosition);

                currentNode = GetClosestNode(touchWorldPosition);

                isGameStart = true;
            }

            if (Input.GetMouseButton(0) && isGameStart)
            {
                touchWorldPosition = GetWorldPosition(Input.mousePosition);

                DrawSpline(currentNode);
            }

            if (Input.GetMouseButtonUp(0) && isGameStart)
            {
                LevelManager.Ins.CheckFinish();
                isGameStart = false;
            }
        }      
    }

    Vector3 GetWorldPosition(Vector3 screenPosition)
    {
        Vector3 screenPositionWithZ = new Vector3(screenPosition.x, screenPosition.y, distanceToBG);
        Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPositionWithZ);
        return worldPosition;
    }

    int GetClosest(Vector3 point, List<Vector3> listPoint)
    {
        int closestPointIndex = -1;
        float closestDistanceSqr = Mathf.Infinity;

        for (int i = 0; i < listPoint.Count; i++)
        {
            float distanceSqr = (listPoint[i] - point).sqrMagnitude;

            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestPointIndex = i;
            }
        }

        return closestPointIndex;
    }

    Node GetClosestNode(Vector3 worldPosition)
    {
        List<Node> listNode = LevelManager.Ins.currentLevel.GetListNode();
        List<Vector3> listNodePosition = new();

        for (int i = 0; i < listNode.Count; i++)
        {
            listNodePosition.Add(listNode[i].transform.position);
        }

        int closestIndex = GetClosest(worldPosition, listNodePosition);

        return listNode[closestIndex];
    }

    void DrawSpline(Node node)
    {
        Node.Connection[] connectionsFull = node.GetConnections();

        List<Node.Connection> connectionsReady = new();
        List<Vector3> closestPointInSpline = new();
        
        for (int i = 0; i < connectionsFull.Length; i++)
        {
            if (LevelManager.Ins.currentLevel.CheckSplineReady(connectionsFull[i].spline))
            {
                connectionsReady.Add(connectionsFull[i]);
                closestPointInSpline.Add(connectionsFull[i].spline.Project(touchWorldPosition).position);
            }
        }

        if (connectionsReady.Count == 0)
        {
            LevelManager.Ins.CheckFinish();
            isGameStart = false;
            return;
        }

        int closestConnectionIndex = GetClosest(touchWorldPosition, closestPointInSpline);

        SplineComputer splineComputer = connectionsReady[closestConnectionIndex].spline;
        SplineRenderer splineRenderer = LevelManager.Ins.currentLevel.GetSplineRenderer(splineComputer);

        if (splineComputer != currentSplineComputer)
        {
            if (currentSplineComputer != null && LevelManager.Ins.currentLevel.CheckSplineReady(currentSplineComputer))
            {
                currentSplineRenderer.clipFrom = 0;
                currentSplineRenderer.clipTo = 0;
            }

            currentSplineComputer = splineComputer;
            currentSplineRenderer = splineRenderer;
        }

        double percent = splineComputer.Project(touchWorldPosition).percent;
        int pointIndex = connectionsReady[closestConnectionIndex].pointIndex;

        if (pointIndex == 0)
        {
            currentSplineRenderer.clipFrom = 0;
            currentSplineRenderer.clipTo = percent;

            if (percent > 0.7f)
            {
                currentSplineRenderer.clipTo = 1;
                currentNode = currentSplineComputer.GetNode(1);
                LevelManager.Ins.currentLevel.RemoveFinishedSpline(currentSplineComputer);                
            }
        }
        else if (pointIndex == 1)
        {
            currentSplineRenderer.clipFrom = percent;
            currentSplineRenderer.clipTo = 1;

            if (percent < 0.3f)
            {
                currentSplineRenderer.clipFrom = 0;
                currentNode = currentSplineComputer.GetNode(0);
                LevelManager.Ins.currentLevel.RemoveFinishedSpline(currentSplineComputer);
            }
        }
    }
}
