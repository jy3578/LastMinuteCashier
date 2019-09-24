using Google.JarResolver;
using UnityEditor;

/// AdMob dependencies file.
[InitializeOnLoad]
public static class AnalyticsDependencies
{
    /// The name of the plugin.
    private static readonly string PluginName = "Analytics";

    /// Initializes static members of the class.
	static AnalyticsDependencies()
    {
        PlayServicesSupport svcSupport =
            PlayServicesSupport.CreateInstance(PluginName, EditorPrefs.GetString("AndroidSdkRoot"),
                    "ProjectSettings");

		svcSupport.DependOn("com.google.android.gms", "play-services-analytics", "LATEST");
    }
}
