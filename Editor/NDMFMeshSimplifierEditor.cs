using jp.lilxyzw.ndmfmeshsimplifier.runtime;
using UnityEditor;

namespace jp.lilxyzw.ndmfmeshsimplifier
{
    [CustomEditor(typeof(NDMFMeshSimplifier))]
    [CanEditMultipleObjects]
    internal class NDMFMeshSimplifierEditor : Editor
    {
        public sealed override void OnInspectorGUI()
        {
            L10n.SelectLanguageGUI();
            serializedObject.UpdateIfRequiredOrScript();
            SerializedProperty iterator = serializedObject.GetIterator();
            iterator.NextVisible(true); // m_Script
            while(iterator.NextVisible(false))
            {
                if(iterator.name == "options") iterator.NextVisible(true);
                EditorGUILayout.PropertyField(iterator, L10n.G(iterator));
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
