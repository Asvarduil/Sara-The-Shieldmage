Shader "Particles/Rim Additive" {
    Properties {
		_TintColor ("Tint Color", Color) = (1.0,1.0,1.0,0.5)

		_MainTex ("Texture", 2D) = "white" {}

		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
    }
    
    SubShader {
		Tags { 
			"RenderType" = "Transparent"
		}
      
		CGPROGRAM
			#pragma surface surf Lambert alpha finalcolor:mycolor
			#pragma target 2.0

			struct Input {
				float2 uv_MainTex;
				float3 viewDir;
			};

			float4 _TintColor;
			sampler2D _MainTex;
			float4 _RimColor;
			float _RimPower;
			
			void mycolor (Input IN, SurfaceOutput o, inout fixed4 color)
			{
				color *= _TintColor;
			}

			void surf (Input IN, inout SurfaceOutput o) {
				o.Albedo = dot( tex2D (_MainTex, IN.uv_MainTex).rgba, _TintColor);
				o.Alpha = dot( tex2D(_MainTex, IN.uv_MainTex).rgba, _TintColor);
				
				half rim = 1.0 - saturate(o.Normal);
				o.Emission = dot(tex2D(_MainTex, IN.uv_MainTex).rgba, (_RimColor.rgb * pow(rim, _RimPower)));
			}
		ENDCG
    }
    
    Fallback "Diffuse"
}