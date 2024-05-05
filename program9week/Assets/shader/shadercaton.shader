Shader "Unlit/shadercaton"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}  // ���� �ؽ�ó
        _OutlineColor("Outline Color", Color) = (0,0,0,1)  // ������ ����
        _OutlineWidth("Outline Width", Range(0.002, 0.03)) = 0.005  // ������ �β�
        _Brightness("Brightness", Range(0.0, 2.0)) = 1.0  // ��� ���� ����
        _Color("Main Color", Color) = (1,1,1,1) // �߰��� ���� ������Ƽ
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
            fixed4 _Color; // ���� ������Ƽ

            void surf(Input IN, inout SurfaceOutput o)
            {
                // �ؽ�ó ��������
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

                // ��� ����
                c.rgb *= _Brightness;

                // ī�� ���̵� ����
                fixed4 outline = tex2D(_MainTex, IN.uv_MainTex + fixed2(_OutlineWidth, 0)) +
                                 tex2D(_MainTex, IN.uv_MainTex - fixed2(_OutlineWidth, 0)) +
                                 tex2D(_MainTex, IN.uv_MainTex + fixed2(0, _OutlineWidth)) +
                                 tex2D(_MainTex, IN.uv_MainTex - fixed2(0, _OutlineWidth));
                outline /= 4;

                fixed4 outlineMask = step(0.01, abs(c - outline));
                o.Albedo = lerp(c, _Color, outlineMask.r); // ������ �߰��Ͽ� ���
            }
            ENDCG
        }
            FallBack "Diffuse"
}
