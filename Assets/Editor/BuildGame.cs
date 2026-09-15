using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildGame
{
    private const string ScenePath = "Assets/Scenes/MainLevel.unity";

    [MenuItem("Last Line of Defense/Build Windows Game")]
    public static void BuildWindowsGame()
    {
        string buildFolder = Path.GetFullPath("Builds/Windows");
        Directory.CreateDirectory(buildFolder);

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { ScenePath },
            locationPathName = Path.Combine(buildFolder, "LastLineOfDefense.exe"),
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Windows build completed: {summary.outputPath} ({summary.totalSize} bytes)");
            EditorUtility.RevealInFinder(summary.outputPath);
        }
        else
        {
            Debug.LogError($"Windows build failed with {summary.totalErrors} error(s). Check the Console.");
        }
    }
}
