Shader "First Fantasy/Water/Water Hightmap" {

	Properties {
		_MainTex ("Water", 2D) = "" {} 		
		_HeightMap1 ("Height Map1", 2D) = "" {}
		_HeightMap2 ("Height Map2", 2D) = "" {}
		 
		_MainTexTile ("Water Tile", float) = 1.0 
		_HeightMapTile ("Heightmap Tile", float) = 0.5
		_MainTexRefraction ("Refraction", Range(0.1,5)) = 1.0 
		_HeightMapStrength ("Strength", Range(0,5)) = 2.0
		_HeightMapMultiply ("Multiply", Range(0.05,0.5)) = 0.05
	}

	//Define a shader
	SubShader {
		Tags { "RenderType"="Opaque" }

		//Define a pass
		Pass {
			
			CGPROGRAM
			#pragma fragment frag
			#pragma vertex vert
			
			sampler2D _MainTex;
			fixed _MainTexTile;  
			fixed _MainTexRefraction;
			fixed _HeightMapStrength; 
			
			fixed _MainTexScrollU1;
			fixed _MainTexScrollV1;
			
			sampler2D _HeightMap1;
			sampler2D _HeightMap2;
			fixed _HeightMap1ScrollU;
			fixed _HeightMap1ScrollV; 
			fixed _HeightMapMultiply;
			fixed _HeightMapTile; 
			
			struct AppData {
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
			};
			
			struct VertexToFragment {
				float4 pos : POSITION;
				half2 uv : TEXCOORD0;
			};

			// Vertex shader
			VertexToFragment vert(AppData v) {
				VertexToFragment o;
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}

			// Fragment shader
			fixed4 frag(VertexToFragment i) : COLOR {
			
				_HeightMap1ScrollU = _HeightMapStrength * _Time;
				_HeightMap1ScrollV = _HeightMapStrength * _Time;
			
				// Distortion
				fixed2 distortion1 = tex2D(_HeightMap1, float2(_HeightMapTile,_HeightMapTile)*i.uv+float2(_HeightMap1ScrollU,_HeightMap1ScrollV).xy) * _HeightMapMultiply - (_HeightMapMultiply*0.5);
 				fixed2 distortion2 = tex2D(_HeightMap2, float2(_HeightMapTile,_HeightMapTile)*i.uv+float2(-_HeightMap1ScrollU,-_HeightMap1ScrollV).xy) * _HeightMapMultiply - (_HeightMapMultiply*0.5);

				// Background
  				fixed4 background1 = tex2D(_MainTex, float2(_MainTexTile,_MainTexTile)*i.uv+(_MainTexRefraction*(distortion1+distortion2))+float2(_MainTexScrollU1,_MainTexScrollV1));
   				fixed alpha = background1.a;
   				fixed inverse = 1.0-alpha;
   				return fixed4((background1.r*alpha),(background1.g*alpha),(background1.b*alpha),1.0);
			}

			ENDCG

		}
	}
}