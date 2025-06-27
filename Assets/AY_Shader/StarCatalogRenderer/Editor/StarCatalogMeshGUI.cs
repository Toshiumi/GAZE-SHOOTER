// Copyright (c) 2023 AYANO_TFT. See vn3license.pdf

using UnityEngine;
using UnityEditor;
using System;

public class StarCatalogMeshGUI : ShaderGUI
{
    enum BlendMode
    {
        Opaque,
        Additive,
        SoftAdditive,
    }

    static void ApplyBlendMode(Material material, BlendMode blendMode)
    {
        switch (blendMode)
        {
            case BlendMode.Opaque:
                material.SetOverrideTag("RenderType", "");
                material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.renderQueue = -1;
                break;

            case BlendMode.Additive:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_ZWrite", 0);
                material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
                break;

            case BlendMode.SoftAdditive:
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_ZWrite", 0);
                material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
                break;

            default:
                throw new ArgumentOutOfRangeException("blendMode", blendMode, null);
        }
    }
    void DrawBlendMode(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        var blendMode = FindProperty("_BlendMode", properties);
        var mode = (BlendMode) blendMode.floatValue;

        using (var scope = new EditorGUI.ChangeCheckScope())
        {
            mode = (BlendMode) EditorGUILayout.Popup("Blend Mode", (int) mode, Enum.GetNames(typeof(BlendMode)));

            if (scope.changed)
            {
                blendMode.floatValue = (float) mode;
                foreach (UnityEngine.Object obj in blendMode.targets)
                {
                    ApplyBlendMode(obj as Material, mode);
                }
            }
        }
    }

    void SetSiderialTime(MaterialProperty[] properties)
    {
        var cYear = FindProperty("cYear", properties);
        var cMonth = FindProperty("cMonth", properties);
        var cDay = FindProperty("cDay", properties);
        var cHour = FindProperty("cHour", properties);
        var cMinute = FindProperty("cMinute", properties);
        var timeZone = FindProperty("timeZone", properties);

        Double Year = cYear.floatValue;
        Double Month = cMonth.floatValue;
        Double Hour = cHour.floatValue - timeZone.floatValue;

        //3月起点として1月は13、2月は14、YはY=Y-1とする。
        if(Month <= 2)
        {
            Year -= 1;
            Month += 12;
        }
        
        //修正ユリウス日（単位：日）
        Double mjd = Math.Floor(365.25 * Year) + Math.Floor(Year / 400.0)
         - Math.Floor(Year / 100.0) + Math.Floor(30.59 * (Month - 2))
         + (Double)cDay.floatValue + 1721088.5 + Hour / 24 + (Double)cMinute.floatValue / 1440 - 2400000.5;// + cSecond / 86400
        //グリニッジ恒星時
        Double st = 0.67239 + 1.00273781 * (mjd - 40000.0);
        st = st % 1.0 * 360.0;

        var _SiderealTime = FindProperty("_SiderealTime", properties);
        _SiderealTime.floatValue = (float)st;
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        var _Cull = FindProperty("_Cull", properties);
        materialEditor.ShaderProperty(_Cull, "Cull Mode");

        DrawBlendMode(materialEditor, properties);

        var cYear = FindProperty("cYear", properties);
        var cMonth = FindProperty("cMonth", properties);
        var cDay = FindProperty("cDay", properties);
        var cHour = FindProperty("cHour", properties);
        var cMinute = FindProperty("cMinute", properties);
        var timeZone = FindProperty("timeZone", properties);

        cYear.floatValue = (int)EditorGUILayout.IntSlider("Year", (int)cYear.floatValue, 2000, 2100);
        cMonth.floatValue = (int)EditorGUILayout.IntSlider("Month", (int)cMonth.floatValue, 1, 12);
        cDay.floatValue = (int)EditorGUILayout.IntSlider("Day", (int)cDay.floatValue, 1, 31);
        cHour.floatValue = (int)EditorGUILayout.IntSlider("Hour", (int)cHour.floatValue, 0, 23);
        cMinute.floatValue = (int)EditorGUILayout.IntSlider("Minute", (int)cMinute.floatValue, 0, 59);
        timeZone.floatValue = (int)EditorGUILayout.IntSlider("timeZone", (int)timeZone.floatValue, -12, 14);

        if(GUILayout.Button("Set Siderial Time"))
        {
            SetSiderialTime(properties);
        }

        base.OnGUI(materialEditor, properties);
    }
    public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
    {
        base.AssignNewShaderToMaterial(material, oldShader, newShader);
        ApplyBlendMode(material, (BlendMode) material.GetFloat("_BlendMode"));
    }
}
