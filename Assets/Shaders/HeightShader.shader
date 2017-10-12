Shader "Custom/HeightShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _HeightMin ("Lowest height", Float) = 1
        _HeightMax ("Largest height", Float) = 1
        _Height0 ("Height 0", Range(0, 1)) = 0
        _Height1 ("Height 1", Range(0, 1)) = 0
        _Height2 ("Height 2", Range(0, 1)) = 0
        _Height3 ("Height 3", Range(0, 1)) = 0
        _Height4 ("Height 4", Range(0, 1)) = 0
        _Height5 ("Height 5", Range(0, 1)) = 0
        
        _Color0 ("Color at 0", Color) = (0,0,0,1)
        _Color1 ("Color at 1", Color) = (0,0,0,1)
        _Color2 ("Color at 2", Color) = (0,0,0,1)
        _Color3 ("Color at 3", Color) = (0,0,0,1)
        _Color4 ("Color at 4", Color) = (0,0,0,1)
        _Color5 ("Color at 5", Color) = (0,0,0,1)
    }
    
    SubShader {
        Tags { "RenderType" = "Opaque" }
        
        CGPROGRAM
        #pragma surface surf Lambert
        
        sampler2D _MainTex;
        float _HeightMin;
        float _HeightMax;
        
        float _Height0;
        float _Height1;
        float _Height2;
        float _Height3;
        float _Height4;
        float _Height5;
        
        fixed4 _Color0;
        fixed4 _Color1;
        fixed4 _Color2;
        fixed4 _Color3;
        fixed4 _Color4;
        fixed4 _Color5;
        
        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
            float3 worldNormal;
        };
        
        float map(float low, float high, float value) {
            return clamp(0, 1, (value - low) / (high - low));
        }
        
        float rgb2grey(half3 rgb) {
            return dot(rgb, half3(0.299, 0.587, 0.114));
        }
        
        void surf (Input IN, inout SurfaceOutput o) {
            //float h = clamp((IN.worldPos.y - _HeightMin) / (_HeightMax - _HeightMin), 0, 1);
            float h = map(_HeightMin, _HeightMax, IN.worldPos.y);
        
            fixed4 tintColor;
            if (h < _Height1) {
                tintColor = lerp(_Color0.rgba, _Color1.rgba, map(_Height0, _Height1, h)); 
            } else if (h < _Height2) {
                tintColor = lerp(_Color1.rgba, _Color2.rgba, map(_Height1, _Height2, h)); 
            } else if (h < _Height3) {
                tintColor = lerp(_Color2.rgba, _Color3.rgba, map(_Height2, _Height3, h));
            } else if (h < _Height4) {
                tintColor = lerp(_Color3.rgba, _Color4.rgba, map(_Height3, _Height4, h));
            } else {
                tintColor = lerp(_Color4.rgba, _Color5.rgba, map(_Height4, _Height5, h));
            }
            
            float texStrength = IN.worldNormal.y;
            
            float tex = rgb2grey(tex2D(_MainTex, IN.uv_MainTex).rgb);
            tex = texStrength * tex + (1 - texStrength) * (1, 1, 1, 1);
            
            o.Albedo = tex * tintColor.rgb;
            o.Alpha = tintColor.a;
        }
        ENDCG
    }
    
    Fallback "Diffuse"
}
