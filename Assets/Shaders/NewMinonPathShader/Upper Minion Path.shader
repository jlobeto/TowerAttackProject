// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MinionPathShader"
{
	Properties
	{
		[Header(Burn Effect)]
		_FireTexture("Fire Texture", 2D) = "white" {}
		_BurnMask("Burn Mask", 2D) = "white" {}
		_FireIntensity("Fire Intensity", Range( 0 , 2)) = 2
		_Albedo("Albedo", 2D) = "white" {}
		_Tinte("Tinte", Color) = (0,0,0,0)
		_Normals("Normals", 2D) = "bump" {}
		_Specular("Specular", 2D) = "white" {}
		_Smoothness("Smoothness", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normals;
		uniform sampler2D _Albedo;
		uniform float4 _Tinte;
		uniform sampler2D _BurnMask;
		uniform float4 _BurnMask_ST;
		uniform sampler2D _FireTexture;
		uniform float _FireIntensity;
		uniform sampler2D _Specular;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_TexCoord4 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			o.Normal = UnpackNormal( tex2D( _Normals, uv_TexCoord4 ) );
			o.Albedo = ( tex2D( _Albedo, uv_TexCoord4 ) * _Tinte ).rgb;
			float2 uv_BurnMask = i.uv_texcoord * _BurnMask_ST.xy + _BurnMask_ST.zw;
			float2 panner2_g2 = ( uv_TexCoord4 + _Time.x * float2( -1,0 ));
			o.Emission = ( ( tex2D( _BurnMask, uv_BurnMask ) * tex2D( _FireTexture, panner2_g2 ) ) * ( _FireIntensity * ( _SinTime.w + 1.5 ) ) ).rgb;
			o.Specular = tex2D( _Specular, uv_TexCoord4 ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14401
7;29;1906;1004;1929.419;1074.144;2.086815;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1413.839,183.3731;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-487.8256,-486.8473;Float;False;Property;_Tinte;Tinte;5;0;Create;True;0,0,0,0;0.2119377,0.4048741,0.7205882,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-535.7401,-292.4268;Float;True;Property;_Albedo;Albedo;4;0;Create;True;None;05fe0ccba2994bf49ab5ba1303dcf6ba;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-855.2775,119.1449;Float;False;Property;_FireIntensity;Fire Intensity;3;0;Create;True;2;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-520.8406,351.0728;Float;True;Property;_Specular;Specular;7;0;Create;True;None;b968e9b37dd4eaf4ea828fd613e9d859;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-157.3947,-372.1463;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-9.840205,290.5776;Float;False;Property;_Smoothness;Smoothness;8;0;Create;True;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;21;-541.476,199.8449;Float;False;Burn Effect;0;;2;e412e392e3db9574fbf6397db39b4c51;0;2;12;FLOAT;0;False;14;FLOAT2;0.0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;13;-535.8399,-92.72685;Float;True;Property;_Normals;Normals;6;0;Create;True;None;72305bfab308875489e12996a48eadf8;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;291.1997,-174.2;Float;False;True;2;Float;ASEMaterialInspector;0;0;StandardSpecular;MinionPathShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0.0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;1;4;0
WireConnection;16;1;4;0
WireConnection;23;0;17;0
WireConnection;23;1;22;0
WireConnection;21;12;19;0
WireConnection;21;14;4;0
WireConnection;13;1;4;0
WireConnection;0;0;23;0
WireConnection;0;1;13;0
WireConnection;0;2;21;0
WireConnection;0;3;16;0
WireConnection;0;4;15;0
ASEEND*/
//CHKSM=5CEB6E52233C5CCD3979C0BE753D29665C9A9DB9