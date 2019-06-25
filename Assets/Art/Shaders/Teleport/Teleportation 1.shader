// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Teleportation1"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Tint("Tint", Color) = (0.4264706,0.4264706,0.4264706,0)
		_Albedo("Albedo", 2D) = "white" {}
		_Smoothness("Smoothness", Float) = 0
		_Metalic("Metalic", Float) = 0
		_Normal("Normal", 2D) = "bump" {}
		[HDR]_GlowColor("GlowColor", Color) = (0.321,0.9196755,1,0)
		_Teleport("Teleport", Range( -30 , 30)) = 1.5
		[Toggle]_Reverse("Reverse", Float) = 1
		_Tiling("Tiling", Vector) = (5,5,0,0)
		_speed("speed", Float) = 1
		_VertOffset("VertOffset", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float4 _Tint;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float2 _Tiling;
		uniform float _speed;
		uniform float _Teleport;
		uniform float _Reverse;
		uniform float4 _GlowColor;
		uniform float _Metalic;
		uniform float _Smoothness;
		uniform float _VertOffset;
		uniform float _Cutoff = 0.5;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float4 temp_cast_0 = (ase_vertex3Pos.z).xxxx;
			float4 transform20 = mul(unity_ObjectToWorld,temp_cast_0);
			float GradientY18 = saturate( ( ( transform20.x + _Teleport ) / lerp(-10.0,10.0,_Reverse) ) );
			float2 panner5 = ( float2( 0,0 ) + ( _Time.y * _speed ) * float2( 0,-1 ));
			float2 uv_TexCoord1 = v.texcoord.xy * _Tiling + panner5;
			float simplePerlin2D2 = snoise( uv_TexCoord1 );
			float Noise10 = ( simplePerlin2D2 + 1.0 );
			float3 VertOffset60 = ( ( ( ase_vertex3Pos * GradientY18 ) * _VertOffset ) * Noise10 );
			v.vertex.xyz += VertOffset60;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 NormalMap49 = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			o.Normal = NormalMap49;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 Albedo46 = ( _Tint * tex2D( _Albedo, uv_Albedo ) );
			o.Albedo = Albedo46.rgb;
			float2 panner5 = ( float2( 0,0 ) + ( _Time.y * _speed ) * float2( 0,-1 ));
			float2 uv_TexCoord1 = i.uv_texcoord * _Tiling + panner5;
			float simplePerlin2D2 = snoise( uv_TexCoord1 );
			float Noise10 = ( simplePerlin2D2 + 1.0 );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 temp_cast_1 = (ase_vertex3Pos.z).xxxx;
			float4 transform20 = mul(unity_ObjectToWorld,temp_cast_1);
			float GradientY18 = saturate( ( ( transform20.x + _Teleport ) / lerp(-10.0,10.0,_Reverse) ) );
			float4 Emission40 = ( ( Noise10 * GradientY18 ) * _GlowColor );
			o.Emission = Emission40.rgb;
			o.Metallic = _Metalic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
			float temp_output_31_0 = ( GradientY18 * 1.0 );
			float OpacityMask28 = ( ( ( ( 1.0 - GradientY18 ) * Noise10 ) - temp_output_31_0 ) + ( 1.0 - temp_output_31_0 ) );
			clip( OpacityMask28 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14401
7;1;1906;1010;3674.35;1346.268;2.303279;True;True
Node;AmplifyShaderEditor.CommentaryNode;14;-2528.099,-1047.806;Float;False;1674.984;362.0156;Noise Generator;10;10;12;2;11;1;5;4;6;7;69;Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;24;-2493.614,-624.326;Float;False;1181.428;504.6405;Comment;10;18;23;22;17;68;16;20;67;21;15;Y Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;15;-2484.564,-572.1758;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;6;-2357.648,-887.9302;Float;False;1;0;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-2349.339,-796.9954;Float;False;Property;_speed;speed;10;0;Create;True;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-2213.653,-278.1674;Float;False;Constant;_Negative;Negative;14;0;Create;True;-10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;20;-2285.321,-572.326;Float;True;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-2156.857,-847.7913;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-2211.919,-200.7946;Float;False;Constant;_Positive;Positive;6;0;Create;True;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-2324.703,-358.7699;Float;False;Property;_Teleport;Teleport;7;0;Create;True;1.5;0;-30;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;4;-1948.347,-1001.382;Float;False;Property;_Tiling;Tiling;9;0;Create;True;5,5;150,150;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;5;-1965.69,-850.5083;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-2035.012,-471.0794;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;68;-2051.633,-253.2406;Float;False;Property;_Reverse;Reverse;8;0;Create;True;1;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;22;-1874.91,-422.909;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1754.183,-962.3924;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;2;-1506.706,-968.0557;Float;False;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1452.081,-899.922;Float;False;Constant;_Boost;Boost;13;0;Create;True;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;23;-1714.814,-520.9867;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;36;-2538.86,-77.97495;Float;False;1123.283;493.354;Comment;11;34;35;19;25;27;26;28;31;30;32;33;Opacity Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;-1513.819,-476.4846;Float;False;GradientY;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-1285.08,-945.9221;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;19;-2475.974,-27.97489;Float;False;18;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;66;-2632.186,890.8113;Float;False;1071.213;413.4266;Comment;8;58;57;63;59;64;62;65;60;Vert Offset;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;27;-2267.305,55.91425;Float;False;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;-2579.572,1089.975;Float;False;18;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-2488.86,148.4073;Float;False;18;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;25;-2250.098,-23.67239;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2464.233,270.9635;Float;False;Constant;_Float0;Float 0;7;0;Create;True;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;56;-2542.353,443.553;Float;False;874.7373;388.7005;Comment;6;37;38;39;41;42;40;Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;57;-2582.186,940.8113;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;10;-1063.041,-946.5726;Float;False;Noise;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;52;-2526.022,-1569.816;Float;False;858.4996;444.177;Comment;4;44;43;45;46;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-2437.663,1189.238;Float;False;Property;_VertOffset;VertOffset;11;0;Create;True;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-2311.191,1037.093;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-2232.892,217.2394;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-2069.416,8.592537;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;37;-2492.353,571.885;Float;False;18;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;38;-2488.529,493.5528;Float;False;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;34;-2010.372,305.3794;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-2285.044,523.3044;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;33;-1999.617,163.4135;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;44;-2444.75,-1519.816;Float;False;Property;_Tint;Tint;1;0;Create;True;0.4264706,0.4264706,0.4264706,0;0,0.7863998,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;41;-2294.292,625.2532;Float;False;Property;_GlowColor;GlowColor;6;1;[HDR];Create;True;0.321,0.9196755,1,0;0.03137256,0.2080136,0.6431373,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;43;-2476.022,-1355.639;Float;True;Property;_Albedo;Albedo;2;0;Create;True;None;2062039613765444d8929d2633a5a037;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;51;-1590.918,-1471.677;Float;False;616.1415;280;Comment;2;49;48;NormalMap;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-2139.663,1033.238;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-2139.663,1151.238;Float;False;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-2103.367,-1431.211;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-2068.156,558.5219;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;COLOR;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;48;-1540.918,-1421.677;Float;True;Property;_Normal;Normal;5;0;Create;True;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-1964.663,1042.238;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-1831.84,232.2456;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-326.6224,325.928;Float;False;Property;_Metalic;Metalic;4;0;Create;True;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;46;-1910.523,-1433.814;Float;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;28;-1680.085,227.9947;Float;False;OpacityMask;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;13;-331.3045,109.2364;Float;False;40;0;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-1910.616,554.8163;Float;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;-350,482.7386;Float;False;28;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-327.6224,397.928;Float;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;60;-1803.973,1019.39;Float;False;VertOffset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;61;-335.2944,570.5241;Float;False;60;0;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;-1217.778,-1406.041;Float;False;NormalMap;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;50;-333.2587,11.94141;Float;False;49;0;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-331.1912,-55.14647;Float;False;46;0;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Teleportation1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;15;3
WireConnection;69;0;6;0
WireConnection;69;1;7;0
WireConnection;5;1;69;0
WireConnection;17;0;20;1
WireConnection;17;1;16;0
WireConnection;68;0;67;0
WireConnection;68;1;21;0
WireConnection;22;0;17;0
WireConnection;22;1;68;0
WireConnection;1;0;4;0
WireConnection;1;1;5;0
WireConnection;2;0;1;0
WireConnection;23;0;22;0
WireConnection;18;0;23;0
WireConnection;12;0;2;0
WireConnection;12;1;11;0
WireConnection;25;0;19;0
WireConnection;10;0;12;0
WireConnection;59;0;57;0
WireConnection;59;1;58;0
WireConnection;31;0;30;0
WireConnection;31;1;32;0
WireConnection;26;0;25;0
WireConnection;26;1;27;0
WireConnection;34;0;31;0
WireConnection;39;0;38;0
WireConnection;39;1;37;0
WireConnection;33;0;26;0
WireConnection;33;1;31;0
WireConnection;62;0;59;0
WireConnection;62;1;63;0
WireConnection;45;0;44;0
WireConnection;45;1;43;0
WireConnection;42;0;39;0
WireConnection;42;1;41;0
WireConnection;65;0;62;0
WireConnection;65;1;64;0
WireConnection;35;0;33;0
WireConnection;35;1;34;0
WireConnection;46;0;45;0
WireConnection;28;0;35;0
WireConnection;40;0;42;0
WireConnection;60;0;65;0
WireConnection;49;0;48;0
WireConnection;0;0;47;0
WireConnection;0;1;50;0
WireConnection;0;2;13;0
WireConnection;0;3;54;0
WireConnection;0;4;55;0
WireConnection;0;10;29;0
WireConnection;0;11;61;0
ASEEND*/
//CHKSM=6BDD0B80AB22EAFC671A8F06CDAB5C5CA6CD5200