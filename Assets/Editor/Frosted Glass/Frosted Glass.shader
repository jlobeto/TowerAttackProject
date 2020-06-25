// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FrostedGlass"
{
	Properties
	{
		_BrushedMetalNormal("BrushedMetalNormal", 2D) = "bump" {}
		_Distortion("Distortion", Range( 0 , 1)) = 0.292
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Tint("Tint", Color) = (0,0,0,0)
		_Opacity("Opacity", Range( 0.1 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
		};

		uniform float4 _Tint;
		uniform sampler2D _GrabTexture;
		uniform sampler2D _BrushedMetalNormal;
		uniform float4 _BrushedMetalNormal_ST;
		uniform float _Distortion;
		uniform float _Smoothness;
		uniform float _Opacity;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Tint.rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float2 uv_BrushedMetalNormal = i.uv_texcoord * _BrushedMetalNormal_ST.xy + _BrushedMetalNormal_ST.zw;
			float4 screenColor8 = tex2D( _GrabTexture, ( (ase_grabScreenPosNorm).xy + (( UnpackNormal( tex2D( _BrushedMetalNormal, uv_BrushedMetalNormal ) ) * _Distortion )).xy ) );
			o.Emission = screenColor8.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14401
7;7;1906;1004;1156.124;369.685;1.338723;True;True
Node;AmplifyShaderEditor.SamplerNode;29;-855.48,221.599;Float;True;Property;_BrushedMetalNormal;BrushedMetalNormal;0;0;Create;True;None;7a632f967e8ad42f5bd275898151ab6a;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;-830.9375,433.5321;Float;False;Property;_Distortion;Distortion;1;0;Create;True;0.292;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-509.0476,272.3269;Float;True;2;2;0;FLOAT3;0.0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GrabScreenPosition;40;-605.248,-34.68068;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;39;-340.8339,-5.049578;Float;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;36;-248.5805,285.0987;Float;True;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-23.26285,64.93491;Float;True;2;2;0;FLOAT2;0.0,0;False;1;FLOAT2;0.0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenColorNode;8;224.0004,85.8997;Float;False;Global;_ScreenGrab0;Screen Grab 0;-1;0;Create;True;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;45;140.904,371.7158;Float;False;Property;_Opacity;Opacity;4;0;Create;True;0;1;0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;43;106.215,-114.5369;Float;False;Property;_Tint;Tint;3;0;Create;True;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;141.9542,260.0898;Float;False;Property;_Smoothness;Smoothness;2;0;Create;True;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;726.6995,-25.33316;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;FrostedGlass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;3;False;0;0;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;4;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0.0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;32;0;29;0
WireConnection;32;1;31;0
WireConnection;39;0;40;0
WireConnection;36;0;32;0
WireConnection;30;0;39;0
WireConnection;30;1;36;0
WireConnection;8;0;30;0
WireConnection;0;0;43;0
WireConnection;0;2;8;0
WireConnection;0;4;41;0
WireConnection;0;9;45;0
ASEEND*/
//CHKSM=B5535447D0170D83736256EE988CAC4D857B628D