using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class PostBuild {
		[PostProcessBuild]
		public static void OnPostprocessBuild (BuildTarget buildTarget, string path)
		{
			ProcessPostBuild (buildTarget, path);
		}

		private static void ProcessPostBuild (BuildTarget buildTarget, string path)
		{
			#if UNITY_IOS

			string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

			PBXProject proj = new PBXProject ();
			proj.ReadFromFile (projPath);

			string target = proj.TargetGuidByName("Unity-iPhone");

			proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
			
			List<string> frameworks = new List<string>() {
				// Google Analytics
				"AdSupport.framework",
				"CoreData.framework",
				"SystemConfiguration.framework",
				"libz.dylib",
				"libsqlite3.dylib"
			};

			frameworks.ForEach((framework) => {
				proj.AddFrameworkToProject(target, framework, false);
			});

			List<string> noARCFilePaths = new List<string>() {
				"Libraries/Plugins/iOS/IOSNative.mm",
				"Libraries/Plugins/iOS/ChartBoostManager.mm",
				"Libraries/Plugins/iOS/ChartBoostBinding.m"
			};

			List<string> optionList = new List<string>();
			optionList.Add("-fno-objc-arc");

			foreach(string tempPath in noARCFilePaths)
			{
				string fileGuid = proj.FindFileGuidByProjectPath(tempPath);
						Debug.Log(fileGuid);
				proj.SetCompileFlagsForFile(target, fileGuid, optionList);
			}
			
			proj.WriteToFile (projPath);

			#endif
		}
}