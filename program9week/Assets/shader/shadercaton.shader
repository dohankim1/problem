Shader "Unlit/shadercaton"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}  // 메인 텍스처
        _OutlineColor("Outline Color", Color) = (0,0,0,1)  // 윤곽선 색상
        _OutlineWidth("Outline Width", Range(0.002, 0.03)) = 0.005  // 윤곽선 두께
        _Brightness("Brightness", Range(0.0, 2.0)) = 1.0  // 밝기 조절 변수
        _Color("Main Color", Color) = (1,1,1,1) // 추가한 색상 프로퍼티
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            struct Input
            {
                float2 uv_MainTex;
            };

            sampler2D _MainTex;
            fixed4 _OutlineColor;
            float _OutlineWidth;
            float _Brightness;
            fixed4 _Color; // 색상 프로퍼티

            void surf(Input IN, inout SurfaceOutput o)
            {
                // 텍스처 가져오기
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

                // 밝기 조절
                c.rgb *= _Brightness;

                // 카툰 쉐이딩 적용
                fixed4 outline = tex2D(_MainTex, IN.uv_MainTex + fixed2(_OutlineWidth, 0)) +
                                 tex2D(_MainTex, IN.uv_MainTex - fixed2(_OutlineWidth, 0)) +
                                 tex2D(_MainTex, IN.uv_MainTex + fixed2(0, _OutlineWidth)) +
                                 tex2D(_MainTex, IN.uv_MainTex - fixed2(0, _OutlineWidth));
                outline /= 4;

                fixed4 outlineMask = step(0.01, abs(c - outline));
                o.Albedo = lerp(c, _Color, outlineMask.r); // 색상을 추가하여 사용
            }
            ENDCG
        }
            FallBack "Diffuse"
}
