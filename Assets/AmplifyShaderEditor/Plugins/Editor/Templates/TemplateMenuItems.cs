// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
using UnityEditor;

namespace AmplifyShaderEditor
{
	public class TemplateMenuItems
	{
		[MenuItem( "Assets/Create/Amplify Shader/Legacy/Samples/DoublePassUnlit", false, 85 )]
		public static void ApplyTemplateLegacySamplesDoublePassUnlit()
		{
			AmplifyShaderEditorWindow.CreateConfirmationTemplateShader( "5a6b6b7a528f75d408b7d9fbd2497126" );
		}
	}
}
