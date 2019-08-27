﻿//
// Overmodded Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Unity.Editor.Objects;
using UnityEditor;
using UnityEngine;

namespace Overmodded.Unity.Editor.SharedSystem
{
    public class SharedConfigWindow : EditorWindow
    {
        public const string LocalSharedDataName = nameof(SharedConfigWindow) + "." + nameof(LocalSharedData);
        private SavedObject<SharedEditorData> LocalSharedData;

        private void OnEnable()
        {
            LocalSharedData = new SavedObject<SharedEditorData>(LocalSharedDataName, null);
        }

        private void OnGUI()
        {
            var localSharedData = LocalSharedData;
            EditorGUI.BeginChangeCheck();

            GUILayout.Label("Local", EditorStyles.boldLabel);
            localSharedData.value = (SharedEditorData) EditorGUILayout.ObjectField("Local Shared Data", localSharedData.value, typeof(SharedEditorData), false);

            EditorGUILayout.Space();
            GUILayout.Label("External", EditorStyles.boldLabel);
            var external = SharedEditorDataManager.GetListOfExternalEditorData();
            EditorGUILayout.LabelField("Total of", external.Count + " external SharedEditorData assets.");
            foreach (var e in external)
            {
                // TODO: Draw object fields.
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(this, "SharedConfigWindow.Change");

                LocalSharedData = localSharedData;
            }
        }

        [MenuItem("Tools/Overmodded/Shared Configuration")]
        public static void ShowWindow() => GetWindow<SharedConfigWindow>(true, "Overmodded Shared Configuration", true);       
    }
}
