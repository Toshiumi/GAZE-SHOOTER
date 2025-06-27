// Copyright (c) 2023 AYANO_TFT. See vn3license.pdf

Shader "AY_Shader/Skybox/StarCatalogMesh"
{
    Properties
    {
        [HideInInspector][Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 0

        [HideInInspector]_BlendMode ("__mode", Float) = 0.0
        [HideInInspector]_SrcBlend ("__src", Float) = 1.0
        [HideInInspector]_DstBlend ("__dst", Float) = 0.0
        [HideInInspector]_ZWrite ("__zw", Float) = 1.0

        [HideInInspector]cYear("Year", Float) = 2023
        [HideInInspector]cMonth("Month", Float) = 1
        [HideInInspector]cDay("Day", Float) = 1
        [HideInInspector]cHour("Hour", Float) = 0
        [HideInInspector]cMinute("Minute", Float) = 0
        [HideInInspector]timeZone("timeZone", Float) = 9

        _SiderealTime("SiderealTime", Float) = 0.0
        _Latitude("Latitude", Range(-90.0, 90.0)) = 35.67
        _Longitude("Longitude", Range(0.0, 360.0)) = 139.74
        _Rotation("Rotation", Range(0.0, 360)) = 0.0

        // [Toggle(_LDATN)]_LDATN("LightDir Attenuation", Float) = 0
        
        [SingleLineTexture]_CubeMap1 ("CubeMap1", CUBE) = "" {}
        [SingleLineTexture]_CubeMap2 ("CubeMap2", CUBE) = "" {}
        [SingleLineTexture]_CubeMap3 ("CubeMap3", CUBE) = "" {}
        [SingleLineTexture]_PositionTex ("PositionTex", 2D) = "white" {}
        _BlueColor("BlueColor", Color) = (0.5613207,0.6558278,1.0)
        _RedColor("RedColor", Color) = (1,0.6257204,0.572549)
        _BGColor("BGColor", Color) = (0, 0, 0)
        _ColorBoost("ColorBoost", Range(1, 100)) = 10.0
        _bv_correct("bv_correct", Range(0.1, 2)) = 0.7
        _BlinkRate("BlinkRate", Range(0, 0.5)) = 0.2
        _BlinkSpeed("BlinkSpeed", Range(0.1, 10.0)) = 2.0
        _Power("Power", Range(3.0, 10.0)) = 6.0

        [HideInInspector]_Numerator("Numerator", Float) = 1.0
        [IntRange]_LoopNum("LoopNum", Range(1, 12)) = 12
        // _Scale("Scale", Float) = 3000
        // [Toggle(_ISTEST)]_Test("IsTest", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "PreviewType"="Plane"}
        LOD 100

        Blend[_SrcBlend][_DstBlend]
        ZWrite [_ZWrite]
        Cull [_Cull]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // #pragma multi_compile _ _ISTEST
            // #pragma multi_compile _ _LDATN

            #include "UnityCG.cginc"

            #include "StarCatalogCore.cginc"

            ENDCG
        }
    }
    CustomEditor "StarCatalogMeshGUI"
}
