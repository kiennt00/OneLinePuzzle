using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] List<Node> listNode = new();
    [SerializeField] List<SplineComputer> listSplineComputer = new();

    private HashSet<SplineComputer> setSplineComputerReady = new();
    private Dictionary<SplineComputer, SplineRenderer> dictSpline = new();

    private List<MeshRenderer> listMeshRenderer = new();

    private void Awake()
    {
        for (int i = 0; i < listSplineComputer.Count; i++)
        {
            SplineRenderer splineRenderer = listSplineComputer[i].GetComponent<SplineRenderer>();
            dictSpline.Add(listSplineComputer[i], splineRenderer);

            listMeshRenderer.Add(listSplineComputer[i].GetComponent<MeshRenderer>());
        }

        InitLevel();
    }

    public void InitLevel()
    {
        StartCoroutine(IEInitLevel());
    }

    IEnumerator IEInitLevel()
    {
        foreach (var kvp in dictSpline)
        {
            kvp.Value.clipFrom = 0;
            kvp.Value.clipTo = 0;
        }
        InitSplineReady();

        yield return null;

        for (int i = 0; i < listMeshRenderer.Count; i++)
        {
            listMeshRenderer[i].material = LevelManager.Ins.green;
        }
    }

    public SplineRenderer GetSplineRenderer(SplineComputer splineComputer)
    { 
        return dictSpline[splineComputer];
    }

    public List<Node> GetListNode()
    {
        return listNode;
    }

    public void InitSplineReady()
    {
        setSplineComputerReady.Clear();

        for (int i = 0; i < listSplineComputer.Count; i++)
        {
            setSplineComputerReady.Add(listSplineComputer[i]);
        }
    }

    public bool CheckSplineReady(SplineComputer splineComputer)
    {
        return setSplineComputerReady.Contains(splineComputer);
    }

    public void RemoveFinishedSpline(SplineComputer splineComputer)
    {
        setSplineComputerReady.Remove(splineComputer);
    }

    public bool CheckFinish()
    {
        return setSplineComputerReady.Count == 0;
    }

    public void Lose()
    {        
        //UIManager.Ins.OpenUI<UILose>();
        GameManager.Ins.ChangeGameState(GameState.Lose);

        for (int i = 0; i < listMeshRenderer.Count; i++)
        {
            listMeshRenderer[i].material = LevelManager.Ins.red;
        }

        StartCoroutine(IEResetLevel());        
    }

    IEnumerator IEResetLevel()
    {
        yield return Constants.WFS_0_S_5;
        InitLevel();
        GameManager.Ins.ChangeGameState(GameState.Gameplay);
        //UIManager.Ins.CloseUI<UILose>();
    }
}
