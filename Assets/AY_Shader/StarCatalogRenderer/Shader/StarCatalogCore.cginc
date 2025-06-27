// Copyright (c) 2023 AYANO_TFT. See vn3license.pdf

struct appdata
{
    float4 vertex : POSITION;
    float3 uv : TEXCOORD0;
};

struct v2f
{
    float3 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
    #ifndef IsSkyboxMode
        float3 worldPos : TEXCOORD1;
    #endif
};

sampler2D _PositionTex;
UNITY_DECLARE_TEXCUBE(_CubeMap1);
UNITY_DECLARE_TEXCUBE(_CubeMap2);
UNITY_DECLARE_TEXCUBE(_CubeMap3);
float3 _BlueColor, _RedColor, _BGColor;

float _ColorBoost, _bv_correct, _BlinkRate, _BlinkSpeed, _Numerator, _Power;
float _Longitude, _Latitude, _SiderealTime, _Rotation;
float _LoopNum;

float2x2 rot2d(float rad)
{
    float s=sin(rad),c=cos(rad);
    return float2x2(c,s,-s,c);
}

static const uint UINT_MAX = 0xffffffff;
static const uint4 k = uint4(0x35415acb, 0x43823fed, 0x83175cde, 0x53413aec);
static const uint4 u = uint4(1, 2, 3, 4);

uint3 uhash33(uint3 n)
{
    n ^= (n.yzx << u);
    n ^= (n.yzx >> u);
    n *= k;
    n ^= (n.yzx << u);
    return n * k;
}

float hash31(float3 p)
{
    uint3 n = asuint(p);
    return float(uhash33(n).x) / float(UINT_MAX);
}

v2f vert (appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);

    #ifdef IsSkyboxMode
        v.uv.xz = mul(v.uv.xz, rot2d(radians( _Rotation)));
        v.uv.xy = mul(v.uv.xy, rot2d(radians(_Latitude - 90)));
        v.uv.xz = mul(v.uv.xz, rot2d(radians( _Longitude + _SiderealTime)));
    #else
        o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    #endif

    o.uv = v.uv;
    return o;
}

fixed4 frag (v2f i) : SV_Target
{
    //精度が必要なのでFrag内でNormalizeする
    #ifdef IsSkyboxMode
        i.uv = normalize(i.uv);
    #else
        i.uv = normalize(i.worldPos.xyz - _WorldSpaceCameraPos);
        i.uv.xz = mul(i.uv.xz, rot2d(radians( _Rotation)));
        i.uv.xy = mul(i.uv.xy, rot2d(radians(_Latitude - 90)));
        i.uv.xz = mul(i.uv.xz, rot2d(radians( _Longitude + _SiderealTime)));
    #endif

    //_CubeMap1 RGBAHalf(16bit*4ch)
    //_CubeMap1.xy : 視線方向で1番目と2番目に明るい星の_PositionTex上のuv座標（fixed>half圧縮）
    //_CubeMap1.zw : 視線方向で3番目と4番目に明るい星の_PositionTex上のuv座標（fixed>half圧縮）
    
    //_CubeMap2 _CubeMap1と同様に5番目から8番目に明るい星のuv座標

    //_PositionTex RGBAHalf 星を16384個に減らして256*256なのでこれに対するuv座標は8bit*2で足りる
    //(ID表記にしても同様だがuv表記のほうがシェーダー上の計算が少ない)
    //(実際の利用は4分の1なので一応8bit*6bitまでは減らせる)
    //_PositionTex.rgb : 各星の球面上の座標 正規化されているので一応もう7bitの圧縮は可能
    //_PositionTex.a : 各星の明るさと色（fixed>half圧縮）

    //視線方向から近い星の_PositionTex上のuv座標を取得、解凍
    float2 fuv[12];
    float4 CubeMapValue1 = UNITY_SAMPLE_TEXCUBE_LOD(_CubeMap1, i.uv, 0);

    uint4 iuv1 = uint4((uint)(CubeMapValue1.x*0xFFFF),(uint)(CubeMapValue1.y*0xFFFF),
                        (uint)(CubeMapValue1.z*0xFFFF),(uint)(CubeMapValue1.w*0xFFFF));
    fuv[0] = ((iuv1.xy & 0xFF) + 0.5) / 255.0;
    fuv[1] = (((iuv1.xy >> 8) & 0xFF) + 0.5) / 255.0;
    fuv[2] = ((iuv1.zw & 0xFF) + 0.5) / 255.0;
    fuv[3] = (((iuv1.zw >> 8) & 0xFF) + 0.5) / 255.0;

    float4 CubeMapValue2 = UNITY_SAMPLE_TEXCUBE_LOD(_CubeMap2, i.uv, 0);
    uint4 iuv2 = uint4((uint)(CubeMapValue2.x*0xFFFF),(uint)(CubeMapValue2.y*0xFFFF),
                        (uint)(CubeMapValue2.z*0xFFFF),(uint)(CubeMapValue2.w*0xFFFF));
    fuv[4] = ((iuv2.xy & 0xFF) + 0.5) / 255.0;
    fuv[5] = (((iuv2.xy >> 8) & 0xFF) + 0.5) / 255.0;
    fuv[6] = ((iuv2.zw & 0xFF) + 0.5) / 255.0;
    fuv[7] = (((iuv2.zw >> 8) & 0xFF) + 0.5) / 255.0;
    
    float4 CubeMapValue3 = UNITY_SAMPLE_TEXCUBE_LOD(_CubeMap3, i.uv, 0);
    uint4 iuv3 = uint4((uint)(CubeMapValue3.x*0xFFFF),(uint)(CubeMapValue3.y*0xFFFF),
                        (uint)(CubeMapValue3.z*0xFFFF),(uint)(CubeMapValue3.w*0xFFFF));
    fuv[8] = ((iuv3.xy & 0xFF) + 0.5) / 255.0;
    fuv[9] = (((iuv3.xy >> 8) & 0xFF) + 0.5) / 255.0;
    fuv[10] = ((iuv3.zw & 0xFF) + 0.5) / 255.0;
    fuv[11] = (((iuv3.zw >> 8) & 0xFF) + 0.5) / 255.0;

    float scale = 3000;
    float power = _Power;
    float numerator = _Numerator;
    float3 final_col = 0.0;

    float attenuation = 1.0;
    // #ifdef _LDATN
    //     attenuation = saturate((-_WorldSpaceLightPos0.y + 0.0)*5.0);
    // #endif

    for(int j = 0; j < _LoopNum; j++)
    {
        //球面上の座標、明るさ、色を取得、解凍
        float4 data = tex2D(_PositionTex, fuv[j]);
        float3 pos = data.xyz * 2.0 - 1.0;
        uint idata = (uint)(data.w * 0xFFFF);
        float brightness = (idata & 0xFF) / 255.0;
        float bv_color = ((idata >> 8) & 0xFF) / 255.0;
        bv_color = pow(bv_color, 1.0 / _bv_correct);
        float magicTime = _Time.w + sin(_Time.w);
        float blink = 1.0 - (sin(magicTime * _BlinkSpeed + hash31(pos) * 6.28) * 0.5 + 0.5) * _BlinkRate;
        float3 col = lerp(_BlueColor, _RedColor, bv_color) * blink;

        //視線方向と各星の距離を測定
        //解像度に依らず各ピクセルで正しい色を取得できる
        float dist = distance(pos, i.uv)*scale;
        
        //距離の2乗に反比例して減衰する
        //CubeMap作成時に同様の方法で各ピクセルへの影響度を測定して上位8個を記録している
        final_col += saturate(numerator/(dist*dist))*pow(brightness * attenuation, power)*col;
    }

    // #ifdef _ISTEST
    //     final_col = CubeMapValue1.xyz;
    // #endif

    final_col *= _ColorBoost;
    final_col += _BGColor;

    return float4(final_col, 1.0);
}