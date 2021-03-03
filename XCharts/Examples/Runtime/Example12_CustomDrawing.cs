﻿/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
using XUGL;


namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example12_CustomDrawing : MonoBehaviour
    {
        LineChart chart;
        void Awake()
        {
            chart = gameObject.GetComponent<LineChart>();
            if (chart == null) return;

            chart.onCustomDraw = delegate (VertexHelper vh)
            {
                var dataPoints = chart.series.list[0].dataPoints;
                if (dataPoints.Count > 0)
                {
                    var pos = dataPoints[3];
                    var zeroPos = new Vector3(chart.grid.runtimeX, chart.grid.runtimeY);
                    var startPos = new Vector3(pos.x, zeroPos.y);
                    var endPos = new Vector3(pos.x, zeroPos.y + chart.grid.runtimeWidth);
                    UGL.DrawLine(vh, startPos, endPos, 1, Color.blue);
                    UGL.DrawCricle(vh, pos, 5, Color.blue);
                }
            };
        }
    }
}