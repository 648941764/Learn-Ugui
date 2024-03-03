 using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleTest : MonoBehaviour
{
    [MenuItem("AssetBundle/Build Windows")]
    public static void BuildAssetBundle()
    {
        string outPath = Application.streamingAssetsPath;
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath); 
        }
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = "ui";
        build.assetBundleVariant = "unity2d";
        build.assetNames = new string[] {
            "Assets/UIPanel/CountPanel.prefab",
            //还可以添加更多的路径
        };
        builds.Add(build);


        BuildPipeline.BuildAssetBundles(outPath, builds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
