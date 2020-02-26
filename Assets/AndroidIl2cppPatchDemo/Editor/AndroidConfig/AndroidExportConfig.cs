using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class AndroidExportConfig
{
    public int exportversion;
    public string exportname;
    public string android_export_path;
    public string unityLibraryPath;
    public string unityLibraryMainPath;
    public string unityLibrary_java_src_path;
    public string unityLibrary_jar_lib_path;
    public string SO_DIR_NAME = "jniLibs";
    public string unityLibrary_SO_LIB_PATH;
    public string unityLibrary_EXPORTED_ASSETS_PATH;
    public string unityLibrary_R_JAVA_PATH;
    public string unityLibrary_RES_PATH;
    public string unityLibrary_MANIFEST_XML_PATH;
    public string unityLibrary_JAVA_OBJ_PATH;
    public string UNITY_PROJECT_DIR;
    public string ScriptingDefineSymbols;
    public AndroidExportConfig(int version){
        exportversion = version;
        ScriptingDefineSymbols = "TEST"+version;
        exportname = "AndroidGradleProject_v"+version;
        UNITY_PROJECT_DIR = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
        android_export_path = Path.GetFullPath(Path.Combine(UNITY_PROJECT_DIR,exportname));
        unityLibraryPath = Path.GetFullPath(Path.Combine(android_export_path,"unityLibrary"));
        unityLibraryMainPath = Path.GetFullPath(Path.Combine(unityLibraryPath,"src/main"));
        unityLibrary_jar_lib_path = Path.GetFullPath(Path.Combine(unityLibraryPath,"libs"));
        unityLibrary_java_src_path = Path.GetFullPath(Path.Combine(unityLibraryMainPath,"java"));
        unityLibrary_SO_LIB_PATH = Path.GetFullPath(Path.Combine(unityLibraryMainPath,"jniLibs"));
        unityLibrary_EXPORTED_ASSETS_PATH = Path.GetFullPath(Path.Combine(unityLibraryMainPath,"assets"));
        unityLibrary_R_JAVA_PATH = Path.GetFullPath(Path.Combine(unityLibraryMainPath,"gen"));
        unityLibrary_RES_PATH = Path.GetFullPath(Path.Combine(unityLibraryMainPath,"res"));
        unityLibrary_MANIFEST_XML_PATH = Path.GetFullPath(Path.Combine(unityLibraryMainPath,"AndroidManifest.xml"));
        unityLibrary_JAVA_OBJ_PATH = Path.GetFullPath(Path.Combine(unityLibraryMainPath,"objs"));
    }

}
