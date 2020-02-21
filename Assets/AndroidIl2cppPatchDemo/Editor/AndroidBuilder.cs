using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
// using System.Diagnostics;
public class AndroidBuilder : MonoBehaviour {

    //-----------------------------------------  config ---------------------------------
    //set SDK/NDK/JDK via Unity Menu Path: Edit -> Preferences... -> External Tools -> Android
    public static readonly string ANDROID_BUILD_TOOLS_VERSION = "29.0.2";
    public static readonly string ANDROID_PLATFORM = "android-29";

    //-----------------------------------------------------------------------------------
    public static readonly string PROJECT_DIR = Path.GetFullPath(Application.dataPath.Substring(0, Application.dataPath.Length - 6));
    
    public static string ZIP_PATH = PROJECT_DIR + "/Assets/AndroidIl2cppPatchDemo/Editor/Exe/zip.exe";

    static bool ExecBat(string filename, string args)
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        process.StartInfo.FileName = filename;
        process.StartInfo.Arguments = args;

        int exit_code = -1;

        try
        {
            process.Start();
            if (process.StartInfo.RedirectStandardOutput && process.StartInfo.RedirectStandardError)
            {
                process.BeginOutputReadLine();
                Debug.LogError(process.StandardError.ReadToEnd());
            }
            else if (process.StartInfo.RedirectStandardOutput)
            {
                string data = process.StandardOutput.ReadToEnd();
                Debug.Log(data);
            }
            else if (process.StartInfo.RedirectStandardError)
            {
                string data = process.StandardError.ReadToEnd();
                Debug.LogError(data);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
        process.WaitForExit();
        exit_code = process.ExitCode;
        process.Close();
        return exit_code == 0;
    }
    public static bool ExecChmod(string shellpath,string WorkingDirectory){
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.FileName = "chmod";
        p.StartInfo.WorkingDirectory =  WorkingDirectory;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = false;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        p.StartInfo.Arguments = " 777 "+shellpath;
        p.Start();
        string presult = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        int exit_code = -1;
        exit_code = p.ExitCode;
        p.Close();
        return exit_code == 0;
    }
    public static bool ExecShell(string shellpath,string WorkingDirectory){
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.FileName = "sh";
        p.StartInfo.WorkingDirectory =  WorkingDirectory;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = false;
        p.StartInfo.CreateNoWindow = true;
        p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        p.StartInfo.Arguments = " "+shellpath;
        p.Start();
        string presult = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        int exit_code = -1;
        exit_code = p.ExitCode;
        p.Close();
        return exit_code == 0;
    }
    
    public static bool ValidateConfig()
    {
        if(Application.platform == RuntimePlatform.OSXEditor){
            string sdkPath = Path.GetFullPath("/Users/carltonyu/Library/Android/sdk");//EditorPrefs.GetString("AndroidSdkRoot", "");
            if (string.IsNullOrEmpty(sdkPath))
            {
                Debug.LogError("sdk path is empty! please config via menu path:Edit/Preference->External tools.");
                return false;
            }

            string jdkPath = Path.GetFullPath("/Applications/Unity/Hub/Editor/2019.3.2f1/PlaybackEngines/AndroidPlayer/OpenJDK");//EditorPrefs.GetString("JdkPath", "");
            if (string.IsNullOrEmpty(jdkPath))
            {
                Debug.LogError("jdk path is empty! please config via menu path:Edit/Preference->External tools.");
                return false;
            }

            string ndkPath = Path.GetFullPath("/Users/carltonyu/Library/Android/ndk/android-ndk-r19");//EditorPrefs.GetString("AndroidNdkRootR16b", "");
            if (string.IsNullOrEmpty(ndkPath))
            {
                ndkPath = EditorPrefs.GetString("AndroidNdkRoot", "");
                if (string.IsNullOrEmpty(ndkPath))
                {
                    Debug.LogError("ndk path is empty! please config via menu path:Edit/Preference->External tools.");
                    return false;
                }
            }

            string buildToolPath = Path.GetFullPath(sdkPath + "/build-tools/" + ANDROID_BUILD_TOOLS_VERSION);
            if (!Directory.Exists(buildToolPath))
            {
                Debug.LogError("Android Build Tools not found. Try to reconfig version on the top of AndroidBuilder.cs. In Unity2018, it can't be work if less than 26.0.2. current:" + buildToolPath);
                return false;
            }

            string platformJar = Path.GetFullPath(sdkPath + "/platforms/" + ANDROID_PLATFORM + "/android.jar");
            if (!File.Exists(platformJar))
            {
                Debug.LogError("Android Platform not found. Try to reconfig version on the top of AndroidBuilder.cs. current:" + platformJar);
                return false;
            }

            Debug.Log("Build Env is ready!");
            Debug.Log("Build Options:");
            Debug.Log("SDK PATH=" + sdkPath);
            Debug.Log("JDK PATH=" + jdkPath);
            Debug.Log("BUILD TOOLS PATH=" + buildToolPath);
            Debug.Log("ANDROID PLATFORM=" + platformJar);
        }else if(Application.platform == RuntimePlatform.WindowsEditor){
            string sdkPath = Path.GetFullPath("D:\\AndroidEnv\\SDK");//EditorPrefs.GetString("AndroidSdkRoot", "");
            if (string.IsNullOrEmpty(sdkPath))
            {
                Debug.LogError("sdk path is empty! please config via menu path:Edit/Preference->External tools.");
                return false;
            }

            string jdkPath = Path.GetFullPath("D:\\AndroidEnv\\OpenJDK");//EditorPrefs.GetString("JdkPath", "");
            if (string.IsNullOrEmpty(jdkPath))
            {
                Debug.LogError("jdk path is empty! please config via menu path:Edit/Preference->External tools.");
                return false;
            }

            string ndkPath = Path.GetFullPath("D:\\AndroidEnv\\NDK\\android-ndk-r19");//EditorPrefs.GetString("AndroidNdkRootR16b", "");
            if (string.IsNullOrEmpty(ndkPath))
            {
                ndkPath = EditorPrefs.GetString("AndroidNdkRoot", "");
                if (string.IsNullOrEmpty(ndkPath))
                {
                    Debug.LogError("ndk path is empty! please config via menu path:Edit/Preference->External tools.");
                    return false;
                }
            }

            string buildToolPath = sdkPath + "/build-tools/" + ANDROID_BUILD_TOOLS_VERSION + "/";
            if (!Directory.Exists(buildToolPath))
            {
                Debug.LogError("Android Build Tools not found. Try to reconfig version on the top of AndroidBuilder.cs. In Unity2018, it can't be work if less than 26.0.2. current:" + buildToolPath);
                return false;
            }

            string platformJar = sdkPath + "/platforms/" + ANDROID_PLATFORM + "/android.jar";
            if (!File.Exists(platformJar))
            {
                Debug.LogError("Android Platform not found. Try to reconfig version on the top of AndroidBuilder.cs. current:" + platformJar);
                return false;
            }

            Debug.Log("Build Env is ready!");
            Debug.Log("Build Options:");
            Debug.Log("SDK PATH=" + sdkPath);
            Debug.Log("JDK PATH=" + jdkPath);
            Debug.Log("BUILD TOOLS PATH=" + buildToolPath);
            Debug.Log("ANDROID PLATFORM=" + platformJar);
        }
        
        
        return true;
    }

    [MenuItem("AndroidBuilder/Export 0", false, 100)]
    public static bool Export0(){
        AndroidExportConfig config = new AndroidExportConfig(0);
        if(ExportGradleProject(config)){
            if(PatchAndroidProject(config)){
                return GenerateBinPatches(config);
            }else{
                Debug.LogError(config.exportname+" PatchAndroidProject error");
                return false;
            }
        }else{
            Debug.LogError(config.exportname+" ExportGradleProject error");
            return false;
        }
    }
    [MenuItem("AndroidBuilder/Export 1", false, 101)]
    public static bool Export1(){
        AndroidExportConfig config = new AndroidExportConfig(1);
        if(ExportGradleProject(config)){
            if(PatchAndroidProject(config)){
                return GenerateBinPatches(config);
            }else{
                Debug.LogError(config.exportname+" PatchAndroidProject error");
                return false;
            }
        }else{
            Debug.LogError(config.exportname+" ExportGradleProject error");
            return false;
        }
    }
    [MenuItem("AndroidBuilder/Export 2", false, 102)]
    public static bool Export2(){
        AndroidExportConfig config = new AndroidExportConfig(2);
        if(ExportGradleProject(config)){
            if(PatchAndroidProject(config)){
                return GenerateBinPatches(config);
            }else{
                Debug.LogError(config.exportname+" PatchAndroidProject error");
                return false;
            }
        }else{
            Debug.LogError(config.exportname+" ExportGradleProject error");
            return false;
        }
    }
    [MenuItem("AndroidBuilder/Export 3", false, 103)]
    public static bool Export3(){
        AndroidExportConfig config = new AndroidExportConfig(3);
        if(ExportGradleProject(config)){
            if(PatchAndroidProject(config)){
                return GenerateBinPatches(config);
            }else{
                Debug.LogError(config.exportname+" PatchAndroidProject error");
                return false;
            }
        }else{
            Debug.LogError(config.exportname+" ExportGradleProject error");
            return false;
        }
    }
    public static bool ExportGradleProject(AndroidExportConfig config)
    {
        //build settings
        if (!ValidateConfig()) { return false; }

        PlayerSettings.applicationIdentifier = "cn.noodle1983.unitypatchdemo";
        PlayerSettings.companyName = "noodle1983";
        PlayerSettings.productName = "UnityAndroidIl2cppPatchDemo";
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.stripEngineCode = false;
#if UNITY_2018 || UNITY_2019
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
#endif

        //export project
        string error_msg = string.Empty;
        string[] levels = new string[] { string.Format("Assets/AndroidIl2cppPatchDemo/Scene/java{0}.unity",config.exportversion) };
        Debug.Log("export level:"+levels);
        BuildOptions options = BuildOptions.AcceptExternalModificationsToPlayer;       
        if (Directory.Exists(config.android_export_path)) { FileUtil.DeleteFileOrDirectory(config.android_export_path);}
        Directory.CreateDirectory(config.android_export_path);
        PlayerSettings.Android.bundleVersionCode = config.exportversion;
        PlayerSettings.bundleVersion = "1.0";
        try
        {
            error_msg = BuildPipeline.BuildPlayer(levels, config.android_export_path, EditorUserBuildSettings.activeBuildTarget, options).summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded ? string.Empty : "Failed to export project!";
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
        }

        if (!string.IsNullOrEmpty(error_msg))
        {
            Debug.LogError(error_msg);
            return false;
        }

        return true;
    }
    public static bool PatchAndroidProject(AndroidExportConfig config)
    {
        //1. patch java file
        string[] javaEntranceFiles = Directory.GetFiles(config.unityLibrary_java_src_path, "UnityPlayerActivity.java", SearchOption.AllDirectories);
        if (javaEntranceFiles.Length != 1)
        {
            Debug.LogError("UnityPlayerActivity.java not found or more than one.");
            return false;
        }
        string javaEntranceFilepath = Path.GetFullPath(javaEntranceFiles[0]);
        string pluginEntranceFilepath = Path.GetFullPath(Path.Combine(Application.dataPath,"AndroidIl2cppPatchDemo/Plugins/Android/java/com/unity3d/player/UnityPlayerActivity.java"));
        
        if(File.Exists(pluginEntranceFilepath)){
            if(File.Exists(javaEntranceFilepath)){
                File.Delete(javaEntranceFilepath);
            }
            FileUtil.CopyFileOrDirectory(pluginEntranceFilepath, javaEntranceFilepath);
            Debug.Log("javaEntranceFilepath:"+javaEntranceFilepath);
        }else{
            Debug.LogError("Plugin UnityPlayerActivity.java not found. path:"+pluginEntranceFilepath);
            return false;
        }
        string manifestPath = Path.GetFullPath(config.unityLibrary_MANIFEST_XML_PATH);
        string pluginManifestPath = Path.GetFullPath(Path.Combine(Application.dataPath,"AndroidIl2cppPatchDemo/Plugins/Android/AndroidManifest.xml"));
        if(File.Exists(pluginManifestPath)){
            if(File.Exists(manifestPath)){
                File.Delete(manifestPath);
            }
            FileUtil.CopyFileOrDirectory(pluginManifestPath, manifestPath);
            Debug.Log("manifestPath:"+manifestPath);
        }else{
            Debug.LogError("Plugin Manifest.xml not found. path:"+pluginManifestPath);
            return false;
        }

        return true;
    }

    public static bool GenerateBinPatches(AndroidExportConfig config)
    {
        string assetBinDataPath = Path.GetFullPath(config.unityLibrary_EXPORTED_ASSETS_PATH + "/bin/Data/");
        string patchTopPath = Path.GetFullPath(PROJECT_DIR + "/AllAndroidPatchFiles/");
        string assertBinDataPatchPath = Path.GetFullPath(patchTopPath + "/assets_bin_Data/");
     
        if (Directory.Exists(patchTopPath)) { FileUtil.DeleteFileOrDirectory(patchTopPath); }
        Directory.CreateDirectory(assertBinDataPatchPath);

        string[][] soPatchFile =
        {
                // path_in_android_project, filename inside zip, zip file anme
                new string[3]{ "/"+ config.SO_DIR_NAME + "/armeabi-v7a/libil2cpp.so", "libil2cpp.so.new", "lib_armeabi-v7a_libil2cpp.so.zip" },
                // new string[3]{ "/"+ SO_DIR_NAME + "/x86/libil2cpp.so", "libil2cpp.so.new", "lib_x86_libil2cpp.so.zip" },
#if UNITY_2018 || UNITY_2019              
                new string[3]{ "/"+ config.SO_DIR_NAME + "/arm64-v8a/libil2cpp.so", "libil2cpp.so.new", "lib_arm64-v8a_libil2cpp.so.zip" },
#endif
        };

        for (int i = 0; i < soPatchFile.Length; i++)
        {
            string[] specialPaths = soPatchFile[i];
            string projectRelativePath = specialPaths[0];
            string pathInZipFile = specialPaths[1];
            string zipFileName = specialPaths[2];

            string projectFullPath = config.unityLibraryMainPath + projectRelativePath;
            ZipHelper.ZipFile(projectFullPath, pathInZipFile, patchTopPath + zipFileName, 9);
        }

        string[] allAssetsBinDataFiles = Directory.GetFiles(assetBinDataPath, "*", SearchOption.AllDirectories);
        StringBuilder allZipCmds = new StringBuilder();

        if(Application.platform == RuntimePlatform.WindowsEditor){
            allZipCmds.AppendFormat("if not exist \"{0}\" (MD \"{0}\") \n", PROJECT_DIR + "/AllAndroidPatchFiles/");
            allZipCmds.AppendFormat("if not exist \"{0}\" (MD \"{0}\") \n", PROJECT_DIR + "/AllAndroidPatchFiles/assets_bin_Data/");
            foreach (string apk_file in allAssetsBinDataFiles)
            {
                string relativePathHeader = "assets/bin/Data/";
                int relativePathStart = apk_file.IndexOf(relativePathHeader);
                string filenameInZip = apk_file.Substring(relativePathStart);                                                //file: assets/bin/Data/xxx/xxx
                string relativePath = apk_file.Substring(relativePathStart + relativePathHeader.Length).Replace('\\', '/'); //file: xxx/xxx
                string zipFileName = relativePath.Replace("/", "__").Replace("\\", "__") + ".bin";                                     //file: xxx__xxx.bin

                allZipCmds.AppendFormat("cd {0} && {1} -8 \"{2}\" \"{3}\"\n", config.unityLibraryMainPath, ZIP_PATH, PROJECT_DIR + "/AllAndroidPatchFiles/assets_bin_Data/" + zipFileName, filenameInZip);
            }
            string zippedPatchFile = Path.GetFullPath(Path.Combine( PROJECT_DIR , string.Format("/Assets/AndroidIl2cppPatchDemo/PrebuiltPatches/AllAndroidPatchFiles_Version{0}.zip",config.exportversion)));
            if (File.Exists(zippedPatchFile)) { FileUtil.DeleteFileOrDirectory(zippedPatchFile);  }
            allZipCmds.AppendFormat("sleep 1 && cd {0} && {1} -9 -r \"{2}\" \"{3}\"\n", patchTopPath, ZIP_PATH, zippedPatchFile, "*");
            allZipCmds.AppendFormat("explorer.exe {0} \n\n", (PROJECT_DIR + "/Assets/AndroidIl2cppPatchDemo/PrebuiltPatches/").Replace("//", "/").Replace("/", "\\"));
            allZipCmds.AppendFormat("@echo on\n\n"); //explorer as the last line wont return success, so...

            if (allZipCmds.Length > 0)
            {
                string zipPatchesFile = Path.GetFullPath(Path.Combine(config.unityLibraryMainPath,"zip_patches.bat"));
                File.WriteAllText(zipPatchesFile, allZipCmds.ToString());
                if (!ExecBat(zipPatchesFile, zipPatchesFile))
                {
                    Debug.LogError("exec failed:" + zipPatchesFile);
                    return false;
                }
            }
        }else{
            ZIP_PATH = "zip";
            foreach (string apk_file in allAssetsBinDataFiles)
            {
                string relativePathHeader = "assets/bin/Data/";
                int relativePathStart = apk_file.IndexOf(relativePathHeader);
                string filenameInZip = apk_file.Substring(relativePathStart);                                                //file: assets/bin/Data/xxx/xxx
                string relativePath = apk_file.Substring(relativePathStart + relativePathHeader.Length).Replace('\\', '/'); //file: xxx/xxx
                string zipFileName = relativePath.Replace("/", "__").Replace("\\", "__") + ".bin";                                     //file: xxx__xxx.bin

                allZipCmds.AppendFormat("cd {0}\n{1} -8 \"{2}\" \"{3}\"\n", Path.GetFullPath(config.unityLibraryMainPath), ZIP_PATH, Path.GetFullPath(Path.Combine(assertBinDataPatchPath,zipFileName)), filenameInZip);
            }
            string zippedPatchFile = Path.GetFullPath(Path.Combine( Application.dataPath , string.Format("AndroidIl2cppPatchDemo/PrebuiltPatches/AllAndroidPatchFiles_Version{0}.zip",config.exportversion)));
            Debug.Log("zippedPatchFile:"+zippedPatchFile);
            if (File.Exists(zippedPatchFile)) { FileUtil.DeleteFileOrDirectory(zippedPatchFile);  }
            allZipCmds.AppendFormat("sleep 1\ncd {0}\n{1} -9 -r \"{2}\" .\n", patchTopPath, ZIP_PATH, zippedPatchFile);

            if (allZipCmds.Length > 0)
            {
                string zipPatchesFile = Path.GetFullPath(Path.Combine(config.unityLibraryMainPath,"zip_patches.sh"));
                File.WriteAllText(zipPatchesFile, allZipCmds.ToString());
                if (!ExecChmod(zipPatchesFile, config.unityLibraryMainPath))
                {
                    Debug.LogError("ExecChmod failed:" + zipPatchesFile);
                    return false;
                }
                if (!ExecShell(zipPatchesFile, config.unityLibraryMainPath))
                {
                    Debug.LogError("ExecShell failed:" + zipPatchesFile);
                    return false;
                }
            }
        }
        
        return true;
    }
}
