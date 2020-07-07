// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FlipBook"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Colums("Colums", Float) = 3
		_Rows("Rows", Float) = 2
		_ScaleTime("Scale Time", Range( 1 , 5)) = 1
		_Speed("Speed", Range( 0 , 10)) = 0
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
		LOD 100
		Cull Off


		Pass
		{
			CGPROGRAM
			#pragma target 3.0 
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MainTex;
			uniform fixed4 _Color;
			uniform sampler2D _TextureSample0;
			uniform float _Colums;
			uniform float _Rows;
			uniform float _Speed;
			uniform float _ScaleTime;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.texcoord.xy = v.texcoord.xy;
				o.texcoord.zw = v.texcoord1.xy;
				
				// ase common template code
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				fixed4 myColorVar;
				// ase common template code
				float2 uv1 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float mulTime7 = _Time.y * _ScaleTime;
				// *** BEGIN Flipbook UV Animation vars ***
				// Total tiles of Flipbook Texture
				float fbtotaltiles3 = _Colums * _Rows;
				// Offsets for cols and rows of Flipbook Texture
				float fbcolsoffset3 = 1.0f / _Colums;
				float fbrowsoffset3 = 1.0f / _Rows;
				// Speed of animation
				float fbspeed3 = mulTime7 * _Speed;
				// UV Tiling (col and row offset)
				float2 fbtiling3 = float2(fbcolsoffset3, fbrowsoffset3);
				// UV Offset - calculate current tile linear index, and convert it to (X * coloffset, Y * rowoffset)
				// Calculate current tile linear index
				float fbcurrenttileindex3 = round( fmod( fbspeed3 + 0.0, fbtotaltiles3) );
				fbcurrenttileindex3 += ( fbcurrenttileindex3 < 0) ? fbtotaltiles3 : 0;
				// Obtain Offset X coordinate from current tile linear index
				float fblinearindextox3 = round ( fmod ( fbcurrenttileindex3, _Colums ) );
				// Multiply Offset X by coloffset
				float fboffsetx3 = fblinearindextox3 * fbcolsoffset3;
				// Obtain Offset Y coordinate from current tile linear index
				float fblinearindextoy3 = round( fmod( ( fbcurrenttileindex3 - fblinearindextox3 ) / _Colums, _Rows ) );
				// Reverse Y to get tiles from Top to Bottom
				fblinearindextoy3 = (int)(_Rows-1) - fblinearindextoy3;
				// Multiply Offset Y by rowoffset
				float fboffsety3 = fblinearindextoy3 * fbrowsoffset3;
				// UV Offset
				float2 fboffset3 = float2(fboffsetx3, fboffsety3);
				// Flipbook UV
				half2 fbuv3 = uv1 * fbtiling3 + fboffset3;
				// *** END Flipbook UV Animation vars ***
				
				
				myColorVar = tex2D( _TextureSample0, fbuv3 );
				return myColorVar;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14401
2037;171;1506;713;1658.219;251.1063;1.419396;True;True
Node;AmplifyShaderEditor.RangedFloatNode;8;-1142.455,278.1254;Float;False;Property;_ScaleTime;Scale Time;3;0;Create;True;1;1;1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-933,-78.5;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;7;-855.1564,272.9253;Float;False;1;0;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1081,-27.5;Float;False;Property;_Colums;Colums;1;0;Create;True;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1078,47.5;Float;False;Property;_Rows;Rows;2;0;Create;True;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1082.311,133.5134;Float;False;Property;_Speed;Speed;4;0;Create;True;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCFlipBookUVAnimation;3;-682,21.5;Float;False;0;0;6;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;10.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;2;-444,17.5;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;91ddb3be25f02b648b8a209871d14830;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMasterNode;9;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;2;FlipBook;6e114a916ca3e4b4bb51972669d463bf;ASETemplateShaders/DefaultUnlit;Off;2;RenderType=Opaque;LightMode=ForwardBase;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;7;0;8;0
WireConnection;3;0;1;0
WireConnection;3;1;4;0
WireConnection;3;2;5;0
WireConnection;3;3;10;0
WireConnection;3;5;7;0
WireConnection;2;1;3;0
WireConnection;9;0;2;0
ASEEND*/
//CHKSM=8FF0124733F0BF9FC1DFCC5BF289D1EEE92AA643