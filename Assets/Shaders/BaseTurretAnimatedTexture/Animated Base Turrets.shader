// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Animated Base Turrets"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		_TinteAlbedo("Tinte Albedo", Color) = (0.4264706,0.4264706,0.4264706,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Emissionpoints("Emission points", 2D) = "white" {}
		_EmissionFeed("EmissionFeed", Range( 0 , 1)) = 0
		_ColorPoints("ColorPoints", Color) = (0.9448276,1,0,0)
		[HDR]_ColorLines("Color Lines", Color) = (0.321,0.9196755,1,0)
		_EmissionLines("EmissionLines", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _TinteAlbedo;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _EmissionLines;
		uniform float4 _EmissionLines_ST;
		uniform float4 _ColorLines;
		uniform sampler2D _Emissionpoints;
		uniform float4 _Emissionpoints_ST;
		uniform float4 _ColorPoints;
		uniform float _EmissionFeed;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 Albedo5 = ( _TinteAlbedo * tex2D( _TextureSample0, uv_TextureSample0 ) );
			o.Albedo = Albedo5.rgb;
			float2 uv_EmissionLines = i.uv_texcoord * _EmissionLines_ST.xy + _EmissionLines_ST.zw;
			float2 uv_Emissionpoints = i.uv_texcoord * _Emissionpoints_ST.xy + _Emissionpoints_ST.zw;
			float4 Emission12 = ( ( ( (float4( 0,0,0,0 ) + (tex2D( _EmissionLines, uv_EmissionLines ) - float4( 0,0,0,0 )) * (float4( 0.4779412,0.4779412,0.4779412,0 ) - float4( 0,0,0,0 )) / (float4( 1,1,1,0 ) - float4( 0,0,0,0 ))) * _ColorLines ) + ( tex2D( _Emissionpoints, uv_Emissionpoints ) * _ColorPoints ) ) * _EmissionFeed );
			o.Emission = Emission12.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14401
204;92;1252;655;1636.556;1260.809;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;34;-1674.227,-509.6314;Float;False;1426.426;825.1584;Comment;11;12;16;17;28;33;13;32;31;30;9;27;Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;27;-1624.227,-434.4735;Float;True;Property;_EmissionLines;EmissionLines;13;0;Create;True;b2e332ed854f5ea4da325698e3fe8b37;b2e332ed854f5ea4da325698e3fe8b37;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-1540.367,-92.49084;Float;True;Property;_Emissionpoints;Emission points;7;0;Create;True;0884f82e29bb42b43b3a71d2bd2a227d;0884f82e29bb42b43b3a71d2bd2a227d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;30;-1309.429,-459.6314;Float;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0.4779412,0.4779412,0.4779412,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;32;-1461.805,108.527;Float;False;Property;_ColorPoints;ColorPoints;9;0;Create;True;0.9448276,1,0,0;0.9448276,1,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;-1314.95,-281.6539;Float;False;Property;_ColorLines;Color Lines;10;1;[HDR];Create;True;0.321,0.9196755,1,0;0.9905679,1,0.3161765,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1184.308,-62.3545;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;1;-1211.776,-1129.313;Float;False;858.4996;444.177;Comment;4;5;4;3;2;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1065.719,-285.6687;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-922.7012,-113.7501;Float;False;Property;_EmissionFeed;EmissionFeed;8;0;Create;True;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-1130.503,-1079.313;Float;False;Property;_TinteAlbedo;Tinte Albedo;5;0;Create;True;0.4264706,0.4264706,0.4264706,0;0.1557093,0.6617647,0.4733027,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-876.5766,-239.1617;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-1161.775,-915.1347;Float;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;None;35e832c3716bd6744895261f30cf39b7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;23;-1255.636,351.8443;Float;False;971.5001;476;Comment;6;22;20;21;18;19;24;Tesselation;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-652.1926,-212.2248;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-789.1183,-990.7067;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-655.8034,538.6765;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;5;-596.2744,-993.3096;Float;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;12;-490.8003,-215.9689;Float;True;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-955.6361,712.8442;Float;False;Property;_Float0;Float 0;12;0;Create;True;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;-485.5466,553.7339;Float;False;Tesselation;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;19;-1205.636,401.8443;Float;True;Property;_TextureSample1;Texture Sample 1;11;0;Create;True;b2e332ed854f5ea4da325698e3fe8b37;9789d23040cb1fb45ad60392430c3c15;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;15;-191.0535,72.02689;Float;False;12;0;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;18;-1188.636,632.8442;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-851.4611,521.012;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;FLOAT3;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;14;-188.6919,-7.06743;Float;False;5;0;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Animated Base Turrets;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;True;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;30;0;27;0
WireConnection;33;0;13;0
WireConnection;33;1;32;0
WireConnection;31;0;30;0
WireConnection;31;1;9;0
WireConnection;28;0;31;0
WireConnection;28;1;33;0
WireConnection;16;0;28;0
WireConnection;16;1;17;0
WireConnection;4;0;2;0
WireConnection;4;1;3;0
WireConnection;22;0;21;0
WireConnection;22;1;20;0
WireConnection;5;0;4;0
WireConnection;12;0;16;0
WireConnection;24;0;22;0
WireConnection;21;0;19;0
WireConnection;21;1;18;0
WireConnection;0;0;14;0
WireConnection;0;2;15;0
ASEEND*/
//CHKSM=C228BEA1438686B024AA0338D82980D3E1385923