using jp.lilxyzw.ndmfmeshsimplifier.runtime;
using UnityEditor;

namespace jp.lilxyzw.ndmfmeshsimplifier
{
    [CustomEditor(typeof(NDMFMeshSimplifier))]
    [CanEditMultipleObjects]
    internal class NDMFMeshSimplifierEditor : Editor
    {
        SerializedProperty quality;
        SerializedProperty PreserveBorderEdges;
        SerializedProperty PreserveUVSeamEdges;
        SerializedProperty PreserveUVFoldoverEdges;
        SerializedProperty PreserveSurfaceCurvature;
        SerializedProperty EnableSmartLink;
        SerializedProperty VertexLinkDistance;
        SerializedProperty MaxIterationCount;
        SerializedProperty Agressiveness;
        SerializedProperty ManualUVComponentCount;
        SerializedProperty UVComponentCount;

        void OnEnable()
        {
            LoadProperties();
        }

        public sealed override void OnInspectorGUI()
        {
            Localization.SelectLanguageGUI();
            serializedObject.Update();
            DrawProperties();
            serializedObject.ApplyModifiedProperties();
        }

        private void LoadProperties()
        {
            quality = serializedObject.FindProperty("quality");
            PreserveBorderEdges = serializedObject.FindProperty("options.PreserveBorderEdges");
            PreserveUVSeamEdges = serializedObject.FindProperty("options.PreserveUVSeamEdges");
            PreserveUVFoldoverEdges = serializedObject.FindProperty("options.PreserveUVFoldoverEdges");
            PreserveSurfaceCurvature = serializedObject.FindProperty("options.PreserveSurfaceCurvature");
            EnableSmartLink = serializedObject.FindProperty("options.EnableSmartLink");
            VertexLinkDistance = serializedObject.FindProperty("options.VertexLinkDistance");
            MaxIterationCount = serializedObject.FindProperty("options.MaxIterationCount");
            Agressiveness = serializedObject.FindProperty("options.Agressiveness");
            ManualUVComponentCount = serializedObject.FindProperty("options.ManualUVComponentCount");
            UVComponentCount = serializedObject.FindProperty("options.UVComponentCount");
        }

        private void DrawProperties()
        {
            EditorGUILayout.PropertyField(quality, Localization.G("inspector.quality"));
            EditorGUILayout.PropertyField(PreserveBorderEdges, Localization.G("inspector.PreserveBorderEdges"));
            EditorGUILayout.PropertyField(PreserveUVSeamEdges, Localization.G("inspector.PreserveUVSeamEdges"));
            EditorGUILayout.PropertyField(PreserveUVFoldoverEdges, Localization.G("inspector.PreserveUVFoldoverEdges"));
            EditorGUILayout.PropertyField(PreserveSurfaceCurvature, Localization.G("inspector.PreserveSurfaceCurvature"));
            EditorGUILayout.PropertyField(EnableSmartLink, Localization.G("inspector.EnableSmartLink"));
            EditorGUILayout.PropertyField(VertexLinkDistance, Localization.G("inspector.VertexLinkDistance"));
            EditorGUILayout.PropertyField(MaxIterationCount, Localization.G("inspector.MaxIterationCount"));
            EditorGUILayout.PropertyField(Agressiveness, Localization.G("inspector.Agressiveness"));
            EditorGUILayout.PropertyField(ManualUVComponentCount, Localization.G("inspector.ManualUVComponentCount"));
            EditorGUILayout.PropertyField(UVComponentCount, Localization.G("inspector.UVComponentCount"));
        }
    }
}
