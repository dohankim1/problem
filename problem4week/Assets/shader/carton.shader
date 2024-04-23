Shader "Unlit/carton"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (0,0,0,1) // °ËÀº»ö
        _RimColor("Rim Color", Color) = (1,1,1,1) // Èò»ö
        _RimPower("Rim Power", Range(0.5, 8)) = 3
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Toon

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _RimColor;
            float _RimPower;

            struct Input
            {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;
                o.Alpha = c.a;
                o.Normal = float3(0, 0, 1);

                float rim = 1.0 - saturate(dot(o.Normal, _WorldSpaceCameraPos - o.PosWorld));
                rim = pow(rim, _RimPower);
                o.Emission = _RimColor.rgb * rim;
            }
            ENDCG
        }

            FallBack "Diffuse"
}