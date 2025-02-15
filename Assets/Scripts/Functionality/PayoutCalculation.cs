using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class PayoutCalculation : MonoBehaviour
{
    [SerializeField]
    private int x_Distance;
    [SerializeField]
    private int y_Distance;

    [SerializeField]
    private Transform LineContainer;
    [SerializeField]
    private GameObject Line_Prefab;

    [SerializeField]
    private Vector2 InitialLinePosition = new Vector2(-315, 100);

    [SerializeField] private ManageLineButtons[] leftPaylineButtons;
    [SerializeField] private ManageLineButtons[] rightPaylineButtons;
    [SerializeField] private Color[] colors;
    [SerializeField]List<GameObject> DontDestroy = new List<GameObject>();

    internal List<List<int>> paylines = new List<List<int>>();

    private void Start()
    {

        List<int> lines = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        for (int i = 0; i < leftPaylineButtons.Length; i++)
        {
            int line = Random.Range(0, lines.Count);
            leftPaylineButtons[i].num = lines[line];
            rightPaylineButtons[i].num = lines[line];
            leftPaylineButtons[i].num_text.text = (lines[line] + 1).ToString();
            rightPaylineButtons[i].num_text.text = (lines[line] + 1).ToString();

            leftPaylineButtons[i].GenerateLine = GeneratePayoutLines;
            rightPaylineButtons[i].GenerateLine = GeneratePayoutLines;

            leftPaylineButtons[i].DestroyLine = ResetLines;
            rightPaylineButtons[i].DestroyLine = ResetLines;
            lines.RemoveAt(line);
        }

    }


    internal void GeneratePayoutLines(int index, bool dontDestroy=false)
    {
        GameObject MyLineObj = Instantiate(Line_Prefab, LineContainer);
        MyLineObj.transform.localPosition = new Vector2(InitialLinePosition.x, InitialLinePosition.y);
        UILineRenderer MyLine = MyLineObj.GetComponent<UILineRenderer>();
        for (int i = 0; i < paylines[index].Count; i++)
        {
            var points = new Vector2() { x = i * x_Distance, y = paylines[index][i] * -y_Distance };
            var pointlist = new List<Vector2>(MyLine.Points);
            pointlist.Add(points);
            MyLine.Points = pointlist.ToArray();
        }
        var newpointlist = new List<Vector2>(MyLine.Points);
        newpointlist.RemoveAt(0);
        MyLine.Points = newpointlist.ToArray();
        MyLine.color=colors[index];

        if(dontDestroy)
        DontDestroy.Add(MyLineObj);
        // if(isStatic)
        // {
        //     TempObj = MyLineObj;
        // }
    }

    //delete all lines
    internal void ResetLines(bool hard=false)
    {
        foreach (Transform child in LineContainer)
        {
            if(!hard){

                if(!DontDestroy.Contains(child.gameObject))
            Destroy(child.gameObject);

            }else
            Destroy(child.gameObject);
        }
    }
}
