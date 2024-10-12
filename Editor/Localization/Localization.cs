using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.ndmfmeshsimplifier
{
    [FilePath("jp.lilxyzw/ndmfmeshsimplifier.asset", FilePathAttribute.Location.PreferencesFolder)]
    internal partial class L10n : ScriptableSingleton<L10n>
    {
        public string language;
        public LocalizationAsset localizationAsset;
        private static string[] languages;
        private static string[] languageNames;
        private static readonly Dictionary<string, GUIContent> guicontents = new();
        private static string localizationFolder => AssetDatabase.GUIDToAssetPath("1cb6b060ffc4d7d4d8672a457cb7d3c7");

        internal static void Load()
        {
            guicontents.Clear();
            var path = localizationFolder + "/" + instance.language + ".po";
            if(File.Exists(path)) instance.localizationAsset = AssetDatabase.LoadAssetAtPath<LocalizationAsset>(path);

            if(!instance.localizationAsset) instance.localizationAsset = new LocalizationAsset();
        }

        internal static string[] GetLanguages()
        {
            return languages ??= Directory.GetFiles(localizationFolder).Where(f => f.EndsWith(".po")).Select(f => Path.GetFileNameWithoutExtension(f)).ToArray();
        }

        internal static string[] GetLanguageNames()
        {
            return languageNames ??= languages.Select(l => {
                if(l == "zh-Hans") return "简体中文";
                if(l == "zh-Hant") return "繁體中文";
                return new CultureInfo(l).NativeName;
            }).ToArray();
        }

        internal static string L(string key)
        {
            if(!instance.localizationAsset) Load();
            return instance.localizationAsset.GetLocalizedString(key);
        }

        private static GUIContent G(string key) => G(key, null, "");
        private static GUIContent G(string[] key) => key.Length == 2 ? G(key[0], null, key[1]) : G(key[0], null, null);
        internal static GUIContent G(string key, string tooltip) => G(key, null, tooltip); // From EditorToolboxSettings
        private static GUIContent G(string key, Texture image) => G(key, image, "");
        internal static GUIContent G(SerializedProperty property) => G(property.name, $"{property.name}.tooltip");

        private static GUIContent G(string key, Texture image, string tooltip)
        {
            if(!instance.localizationAsset) Load();
            if(guicontents.TryGetValue(key, out var content)) return content;
            return guicontents[key] = new GUIContent(L(key), image, L(tooltip));
        }

        internal static void SelectLanguageGUI()
        {
            var langs = GetLanguages();
            var names = GetLanguageNames();
            EditorGUI.BeginChangeCheck();
            var ind = EditorGUILayout.Popup("Language", Array.IndexOf(langs, instance.language), names);
            if(EditorGUI.EndChangeCheck())
            {
                instance.language = langs[ind];
                Load();
            }
        }
    }

    internal class LocalizationData
    {
        public string languageName;
        public Dictionary<string, string> localizeDatas;
        internal string GetValue(string key)
        {
            if(localizeDatas.ContainsKey(key)) return localizeDatas[key];
            return key;
        }
    }

    internal class SafeIO
    {
        internal static void SaveFile(string path, string content)
        {
            if(!File.Exists(path))
            {
                File.WriteAllText(path, content);
                return;
            }
            using(var fs = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            using(var sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(content);
            }
        }

        internal static string LoadFile(string path)
        {
            using(var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using(var sr = new StreamReader(fs, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
