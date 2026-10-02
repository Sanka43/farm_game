using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>Editor/CI build helpers. The game builds itself at runtime (GameBootstrapper), so the scene is empty.</summary>
public static class BuildScript
{
    const string ScenePath = "Assets/_Project/Scenes/Main.unity";

    [MenuItem("Meadowbrook/Create Main Scene")]
    public static void CreateMainScene()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        AssetDatabase.SaveAssets();
    }

    /// <summary>Called by CI (game-ci buildMethod). Produces an Xcode project.</summary>
    public static void BuildIOS()
    {
        CreateMainScene();

        PlayerSettings.companyName = "Sanka43";
        PlayerSettings.productName = "Meadowbrook";
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.iOS, "com.sanka43.meadowbrook");
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.iOS.targetOSVersionString = "13.0";
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;

        string outPath = "build/iOS";
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
            if (args[i] == "-customBuildPath") outPath = args[i + 1];
        Directory.CreateDirectory(outPath);

        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = new[] { ScenePath },
            locationPathName = outPath,
            target = BuildTarget.iOS,
            options = BuildOptions.None
        });

        Debug.Log("iOS build result: " + report.summary.result);
        if (report.summary.result != BuildResult.Succeeded)
            EditorApplication.Exit(1);
    }
}
