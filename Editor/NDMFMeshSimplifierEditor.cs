using jp.lilxyzw.ndmfmeshsimplifier.runtime;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.ndmfmeshsimplifier
{
    [CustomEditor(typeof(NDMFMeshSimplifier))]
    [CanEditMultipleObjects]
    internal class NDMFMeshSimplifierEditor : Editor
    {
        public sealed override void OnInspectorGUI()
        {
            L10n.SelectLanguageGUI();
            EditorGUILayout.HelpBox(L10n.G("PlaceUseMeshia", "").text, MessageType.Warning, true);
#if MESHIA_CONTAINS
            if (GUILayout.Button(L10n.G("MigrationToMeshia", "")))
            {
                foreach (var t in targets)
                {
                    if (t == null) { continue; }
                    if (t is not NDMFMeshSimplifier targetSimplifier) { continue; }
                    MigrationToMeshiaMeshSimplification(targetSimplifier);
                }
                return;
            }
#else
            if (GUILayout.Button(L10n.G("OpenMeshiaBOOTH", "")))
            {
                Application.OpenURL("https://ramtype0.booth.pm/items/6944218");
            }
#endif
            serializedObject.UpdateIfRequiredOrScript();
            SerializedProperty iterator = serializedObject.GetIterator();
            iterator.NextVisible(true); // m_Script
            while (iterator.NextVisible(false))
            {
                if (iterator.name == "options") iterator.NextVisible(true);
                EditorGUILayout.PropertyField(iterator, L10n.G(iterator));
            }
            serializedObject.ApplyModifiedProperties();
        }

#if MESHIA_CONTAINS
        public static void MigrationToMeshiaMeshSimplification(NDMFMeshSimplifier lilNDMFMeshSimplifier)
        {
            var gameObject = lilNDMFMeshSimplifier.gameObject;
            var meshiaMeshSimplifier = Undo.AddComponent<Meshia.MeshSimplification.Ndmf.MeshiaMeshSimplifier>(gameObject);

            Undo.RecordObject(meshiaMeshSimplifier, "migrate from lilNDMFMeshSimplifier");
            meshiaMeshSimplifier.target = new()
            {
                Kind = Meshia.MeshSimplification.MeshSimplificationTargetKind.RelativeTriangleCount,
                Value = lilNDMFMeshSimplifier.quality,
            };
            var meshiaOptions = Meshia.MeshSimplification.MeshSimplifierOptions.Default;
            meshiaOptions.PreserveBorderEdges = lilNDMFMeshSimplifier.options.PreserveBorderEdges;
            meshiaOptions.PreserveSurfaceCurvature = lilNDMFMeshSimplifier.options.PreserveSurfaceCurvature;
            meshiaOptions.EnableSmartLink = lilNDMFMeshSimplifier.options.EnableSmartLink;
            meshiaOptions.VertexLinkDistance = (float)lilNDMFMeshSimplifier.options.VertexLinkDistance;
            meshiaMeshSimplifier.options = meshiaOptions;

            UnityEngine.Object.DestroyImmediate(lilNDMFMeshSimplifier);
        }
#endif
    }
}
