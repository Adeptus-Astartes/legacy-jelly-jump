Shader "Particles/Bumped Specular"
{
    Properties {
        _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
        _MainTex ("Particle Texture", 2D) = "white" {}
        _NormalTex ("Normal Map", 2D) = "bump" {}
        _SpecColor ("Specular color", color) = (1, 1, 1, 1)
        _SpecPower ("Specular power", float) = 1
    }
    SubShader
    {
            Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
            Blend SrcAlpha OneMinusSrcAlpha
            Lighting Off ZWrite Off Ztest LEqual Fog { Color (0,0,0,0) }
            LOD 200
         
            CGPROGRAM
            #pragma surface surf BlinnPhong alpha nolightmap noforwardadd  novertexlights
            #pragma debug
         
            sampler2D _MainTex;
            sampler2D _NormalTex;
            fixed4 _TintColor;
            float _SpecPower;
 
            struct Input {
                float2 uv_MainTex;
                float4 color : COLOR;
            };
         
         
            void surf (Input i, inout SurfaceOutput o)
            {
                fixed4 tex_diffuse = tex2D(_MainTex, i.uv_MainTex);
                fixed3 tex_normal = UnpackNormal( tex2D(_NormalTex, i.uv_MainTex) );
                o.Albedo = i.color.rgb;
                o.Alpha = tex_diffuse.a * i.color.a;
                o.Normal = tex_normal;
                o.Gloss = 1;
                o.Specular = _SpecPower;
            }
         
            ENDCG
    }
}
 