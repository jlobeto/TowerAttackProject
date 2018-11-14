// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AnimatedTexture"
{
	Properties
	{
		_EmissionQty("EmissionQty", Range( -20 , 20)) = 1.5
		_AlbedoTint("Albedo Tint", Color) = (0.4264706,0.4264706,0.4264706,0)
		[Toggle]_ToggleSwitch0("Toggle Switch0", Float) = 1
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HDR]_EmissionColor("Emission Color", Color) = (0.321,0.9196755,1,0)
		_Emission("Emission", 2D) = "white" {}
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
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float4 _AlbedoTint;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _EmissionQty;
		uniform float _ToggleSwitch0;
		uniform float4 _EmissionColor;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 Albedo33 = ( _AlbedoTint * tex2D( _TextureSample0, uv_TextureSample0 ) );
			o.Albedo = Albedo33.rgb;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 transform5 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float GradientY10 = saturate( ( ( transform5.x + _EmissionQty ) / lerp(-10.0,10.0,_ToggleSwitch0) ) );
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			float4 Emission27 = ( GradientY10 * ( _EmissionColor * tex2D( _Emission, uv_Emission ) ) );
			o.Emission = Emission27.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14401
7;29;1906;1004;2596.18;212.4023;1;True;True
Node;AmplifyShaderEditor.PosVertexDataNode;1;-2199.626,215.4443;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;5;-2000.385,215.2941;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-2039.767,428.8503;Float;False;Property;_EmissionQty;EmissionQty;0;0;Create;True;1.5;-20;-20;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1998.717,517.4528;Float;False;Constant;_Float2;Float 2;14;0;Create;True;-10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1994.983,592.8257;Float;False;Constant;_Float1;Float 1;6;0;Create;True;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-1750.076,316.5408;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;6;-1825.697,532.3797;Float;False;Property;_ToggleSwitch0;Toggle Switch0;2;0;Create;True;1;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;14;-2095.892,-281.7588;Float;False;874.7373;388.7005;Comment;6;27;26;23;19;35;34;Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;8;-1589.974,364.7112;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;23;-2086.699,-245.8404;Float;False;Property;_EmissionColor;Emission Color;4;1;[HDR];Create;True;0.321,0.9196755,1,0;1,0.3350483,0.321,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;34;-2089.036,-83.53262;Float;True;Property;_Emission;Emission;5;0;Create;True;None;f49218d7697fc3a4f807fc5eae529cd5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;29;-2067.094,-1223.283;Float;False;858.4996;444.177;Comment;4;33;32;31;30;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.SaturateNode;9;-1429.878,266.6334;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;31;-2017.094,-1009.107;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;None;60735b01e3402f3488445ac0e61b8d4d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;-1985.822,-1173.283;Float;False;Property;_AlbedoTint;Albedo Tint;1;0;Create;True;0.4264706,0.4264706,0.4264706,0;1,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;19;-1806.69,-244.7196;Float;False;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;10;-1228.883,311.1357;Float;False;GradientY;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-1769.373,-120.4172;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-1621.695,-166.7899;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;COLOR;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1644.439,-1084.678;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-1385.379,-493.0662;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;37;-237.8929,68.32657;Float;False;27;0;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;-1464.155,-170.4955;Float;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;36;-233.8928,-23.84668;Float;False;33;0;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;-1451.595,-1087.281;Float;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;28;-1233.624,-497.3171;Float;False;OpacityMask;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2017.772,-454.3483;Float;False;Constant;_Float3;Float 3;7;0;Create;True;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;11;-2051.815,-673.6328;Float;False;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;16;-2042.399,-576.9045;Float;False;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-1786.431,-508.0724;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;22;-1553.156,-561.8983;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;21;-1563.911,-419.9324;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;15;-1786.113,-634.2825;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;AnimatedTexture;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;0;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;1;0
WireConnection;7;0;5;1
WireConnection;7;1;2;0
WireConnection;6;0;4;0
WireConnection;6;1;3;0
WireConnection;8;0;7;0
WireConnection;8;1;6;0
WireConnection;9;0;8;0
WireConnection;10;0;9;0
WireConnection;35;0;23;0
WireConnection;35;1;34;0
WireConnection;26;0;19;0
WireConnection;26;1;35;0
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;25;0;22;0
WireConnection;25;1;21;0
WireConnection;27;0;26;0
WireConnection;33;0;32;0
WireConnection;28;0;25;0
WireConnection;18;0;16;0
WireConnection;18;1;12;0
WireConnection;22;0;15;0
WireConnection;22;1;18;0
WireConnection;21;0;18;0
WireConnection;15;0;11;0
WireConnection;0;0;36;0
WireConnection;0;2;37;0
ASEEND*/
//CHKSM=A81EDD898BAC7EBAE18A83E8964148ED9F68A0E4