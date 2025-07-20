using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Photon.Voice.Unity.Editor
{
    [InitializeOnLoad]
    public static class PhotonVoiceEditorUtils
    {
        public const string PHOTON_VIDEO_DEFINE_SYMBOL = "PHOTON_VOICE_VIDEO_ENABLE";
        public const string PHOTON_VIDEO_AVAILABLE_DEFINE_SYMBOL = "PHOTON_VOICE_VIDEO_AVAILABLE";

        static PhotonVoiceEditorUtils()
        {
            if (HasVideo)
            {
#if !PHOTON_VOICE_VIDEO_AVAILABLE
                Debug.Log("Photon Video is available");
                AddScriptingDefineSymbolToAllBuildTargetGroups(PHOTON_VIDEO_AVAILABLE_DEFINE_SYMBOL);
                TriggerRecompile();
#endif
            }
            else
            {
#if PHOTON_VOICE_VIDEO_AVAILABLE
                RemoveScriptingDefineSymbolToAllBuildTargetGroups(PHOTON_VIDEO_AVAILABLE_DEFINE_SYMBOL);
                TriggerRecompile();
#endif
            }
        }

        private static void TriggerRecompile()
        {
            AssetDatabase.ImportAsset("Assets/Photon/PhotonVoice/Code/Editor/PhotonVoiceEditorUtils.cs");
        }

        public static bool HasChat
        {
            get
            {
                return Type.GetType("Photon.Chat.ChatClient, Assembly-CSharp") != null ||
                       Type.GetType("Photon.Chat.ChatClient, Assembly-CSharp-firstpass") != null ||
                       Type.GetType("Photon.Chat.ChatClient, PhotonChat") != null;
            }
        }

        public static bool HasPun
        {
            get
            {
                return Type.GetType("Photon.Pun.PhotonNetwork, Assembly-CSharp") != null ||
                       Type.GetType("Photon.Pun.PhotonNetwork, Assembly-CSharp-firstpass") != null ||
                       Type.GetType("Photon.Pun.PhotonNetwork, PhotonUnityNetworking") != null;
            }
        }

        public static bool HasVideo
        {
            get
            {
                return Directory.Exists("Assets/Photon/PhotonVoice/PhotonVoiceApi/Core/Video");
            }
        }

        [MenuItem("Window/Photon Voice/Remove PUN", true, 1)]
        private static bool RemovePunValidate()
        {
#if PHOTON_UNITY_NETWORKING
            return true;
#else
            return HasPun;
#endif
        }

        [MenuItem("Window/Photon Voice/Remove PUN", false, 1)]
        private static void RemovePun()
        {
            DeleteDirectory("Assets/Photon/PhotonVoice/Demos/DemoProximityVoiceChat");
            DeleteDirectory("Assets/Photon/PhotonVoice/Demos/DemoVoicePun");
            DeleteDirectory("Assets/Photon/PhotonVoice/Code/PUN");
            DeleteDirectory("Assets/Photon/PhotonUnityNetworking");
            CleanUpPunDefineSymbols();
        }

        [MenuItem("Window/Photon Voice/Remove Photon Chat", true, 2)]
        private static bool RemovePhotonChatValidate()
        {
            return HasChat;
        }

        [MenuItem("Window/Photon Voice/Remove Photon Chat", false, 2)]
        private static void RemovePhotonChat()
        {
            DeleteDirectory("Assets/Photon/PhotonChat");
        }

        [MenuItem("Window/Photon Voice/Leave a review", false, 3)]
        private static void OpenAssetStorePage()
        {
            Application.OpenURL("https://assetstore.unity.com/packages/tools/audio/photon-voice-2-130518");
        }

#if PHOTON_VOICE_VIDEO_AVAILABLE

#if PHOTON_VOICE_VIDEO_ENABLE
        [MenuItem("Window/Photon Voice/Disable Video", false, 4)]
        private static void DisableVideo()
        {
            RemoveScriptingDefineSymbolToAllBuildTargetGroups(PHOTON_VIDEO_DEFINE_SYMBOL);
            TriggerRecompile();
        }
#else
        [MenuItem("Window/Photon Voice/Enable Video", false, 4)]
        private static void EnableVideo()
        {
            Debug.Log("Enabling Photon Video (setting define '" + PHOTON_VIDEO_DEFINE_SYMBOL + "').");
            AddScriptingDefineSymbolToAllBuildTargetGroups(PHOTON_VIDEO_DEFINE_SYMBOL);
            TriggerRecompile();
        }
#endif

#endif

        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                if (!FileUtil.DeleteFileOrDirectory(path))
                {
                    Debug.LogWarningFormat("Directory \"{0}\" not deleted.", path);
                }
                DeleteFile(path + ".meta");
            }
            else
            {
                Debug.LogWarningFormat("Directory \"{0}\" does not exist.", path);
            }
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                if (!FileUtil.DeleteFileOrDirectory(path))
                {
                    Debug.LogWarningFormat("File \"{0}\" not deleted.", path);
                }
            }
            else
            {
                Debug.LogWarningFormat("File \"{0}\" does not exist.", path);
            }
        }

        public static bool IsInTheSceneInPlayMode(GameObject go)
        {
            return Application.isPlaying && !IsPrefab(go);
        }

        public static bool IsPrefab(GameObject go)
        {
#if UNITY_2021_2_OR_NEWER
            return UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(go) != null || EditorUtility.IsPersistent(go);
#elif UNITY_2018_3_OR_NEWER
            return UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetPrefabStage(go) != null || EditorUtility.IsPersistent(go);
#else
            return EditorUtility.IsPersistent(go);
#endif
        }

        public static void RemoveScriptingDefineSymbolToAllBuildTargetGroups(string defineSymbol)
        {
            foreach (BuildTarget target in Enum.GetValues(typeof(BuildTarget)))
            {
                BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(target);
                if (group == BuildTargetGroup.Unknown) continue;

                var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group)
                    .Split(';').Select(d => d.Trim()).ToList();

                if (defineSymbols.Contains(defineSymbol) && defineSymbols.Remove(defineSymbol))
                {
                    try
                    {
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defineSymbols));
                    }
                    catch (Exception e)
                    {
                        Debug.LogErrorFormat("Could not remove \"{0}\" for target: {1}, group: {2}, {3}", defineSymbol, target, group, e);
                    }
                }
            }
        }

        public static void AddScriptingDefineSymbolToAllBuildTargetGroups(string defineSymbol)
        {
            foreach (BuildTarget target in Enum.GetValues(typeof(BuildTarget)))
            {
                BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(target);
                if (group == BuildTargetGroup.Unknown) continue;

                var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group)
                    .Split(';').Select(d => d.Trim()).ToList();

                if (!defineSymbols.Contains(defineSymbol))
                {
                    defineSymbols.Add(defineSymbol);
                    try
                    {
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", defineSymbols));
                    }
                    catch (Exception e)
                    {
                        Debug.LogErrorFormat("Could not add \"{0}\" for target: {1}, group: {2}, {3}", defineSymbol, target, group, e);
                    }
                }
            }
        }

        public static void CleanUpPunDefineSymbols()
        {
            foreach (BuildTarget target in Enum.GetValues(typeof(BuildTarget)))
            {
                BuildTargetGroup group = BuildPipeline.GetBuildTargetGroup(target);
                if (group == BuildTargetGroup.Unknown) continue;

                var defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(group)
                    .Split(';').Select(d => d.Trim()).ToList();

                List<string> newDefineSymbols = new List<string>();
                foreach (var symbol in defineSymbols)
                {
                    if ("PHOTON_UNITY_NETWORKING".Equals(symbol) || symbol.StartsWith("PUN_2_"))
                        continue;

                    newDefineSymbols.Add(symbol);
                }

                try
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", newDefineSymbols));
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("Could not clean PUN2 define symbols for target: {0}, group: {1}, {2}", target, group, e);
                }
            }
        }
    }
}
