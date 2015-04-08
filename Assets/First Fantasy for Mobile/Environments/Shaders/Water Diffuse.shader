Shader "First Fantasy/Water/Water Diffuse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MainTexColor ("Diffuse", Color) = (1,1,1,0.55)  
		_MainTexMultiply ("Multiply", Range(0,5)) = 1
		_MainTexMoveSpeedU ("U Move Speed", Range(-6,6)) = 0.5
		_MainTexMoveSpeedV ("V Move Speed", Range(-6,6)) = 0.5
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" } 
		ZWrite Off
		Alphatest Greater 0
		Blend SrcAlpha OneMinusSrcAlpha 
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		fixed4 _MainTexColor; 
		fixed _MainTexMultiply;
		fixed _MainTexMoveSpeedU;
		fixed _MainTexMoveSpeedV;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) { 
		
			fixed2 MainTexMoveScrolledUV = IN.uv_MainTex;
			
			fixed MainTexMoveU = _MainTexMoveSpeedU * _Time;
			fixed MainTexMoveV = _MainTexMoveSpeedV * _Time;
			
			MainTexMoveScrolledUV += fixed2(MainTexMoveU, MainTexMoveV);
		
			half4 c = tex2D (_MainTex, MainTexMoveScrolledUV);
			o.Albedo = c.rgb * _MainTexColor * _MainTexMultiply;
			o.Alpha = _MainTexColor.a;
		}
		ENDCG
	} 
	FallBack "Diffuse" 
}