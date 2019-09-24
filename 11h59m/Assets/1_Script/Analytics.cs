using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Analytics : MonoBehaviour {
	public GoogleAnalyticsV4 ga = null;
	
		private const string metricPrefKey = "Metric Send Date";

	// dictionary key   : dimension & metric name
	// dictionary value : dimension & metric GA index 

	private static Dictionary<string, int> dimensionDict = new Dictionary<string, int>()
	{
		{"Total Attend", 1},
		{"Total Chartboost", 2},
		{"Total Unityads", 3},
		{"Total Admob", 4},
		{"Total Play", 5},
		{"No Ads", 6}
	};

	private static Dictionary<string, int> metricDict = new Dictionary<string, int>()
	{
		{"Total Play", 1},
		{"Best Score", 2},
		{"Avg Score", 3},
		{"Total Exp", 4},
		{"Mission Completed", 5},
		{"Total Chartboost", 6},
		{"Total Unityads", 7},
		{"Total Admob", 8}
	};

	void Start()
	{
		ga.dispatchPeriod = 30;
		ga.DispatchHits();
			
		ga.StartSession();
	}

	public void StartSession(bool isStart)
	{
		if( isStart )
			ga.StartSession();
		else
			ga.StopSession();
	}

	public void SendView(string view)
	{
		if(ga != null)
		{
			AppViewHitBuilder builder = GetViewHitBuilder();
			builder.SetScreenName(view);
			ga.LogScreen(builder);
		}
	}

	public void SendException(string message, bool fatal)
	{
		if(ga != null)
		{
			ga.LogException(message, fatal);
		}
	}

	public void SendEvent(string category, string action=null, string label=null, long value=0)
	{	
		if(ga != null)
		{
			EventHitBuilder builder = GetEventHitBuilder();
			builder.SetEventCategory(category);
			builder.SetEventAction(action);
			builder.SetEventLabel(label);
			builder.SetEventValue(value);
			ga.LogEvent (builder);
		}
	}

	public void SendTiming(string category, long milliseconds, string name, string label)
	{
		ga.LogTiming (category, milliseconds, name, label);
	}

	private EventHitBuilder	GetEventHitBuilder()
	{
		EventHitBuilder builder = new EventHitBuilder();
		
		builder.SetCustomDimension(dimensionDict["Total Attend"], PlayerPrefs.GetInt("total attend", 1).ToString());
		builder.SetCustomDimension(dimensionDict["Total Chartboost"], PlayerPrefs.GetInt("Play Chartboost Ads", 0).ToString());
		builder.SetCustomDimension(dimensionDict["Total Unityads"], PlayerPrefs.GetInt("Play Unity Ads", 0).ToString());
		builder.SetCustomDimension(dimensionDict["Total Admob"], PlayerPrefs.GetInt("Play Admob Ads", 0).ToString());
		builder.SetCustomDimension(dimensionDict["Total Play"], SaveManager.GetTotalPlay().ToString());
		builder.SetCustomDimension(dimensionDict["No Ads"], SaveManager.GetRemoveAdsPurchased()? "1": "0");

		return builder;
	}

	private AppViewHitBuilder GetViewHitBuilder()
	{
		AppViewHitBuilder builder = new AppViewHitBuilder();

		builder.SetCustomDimension(dimensionDict["Total Attend"], PlayerPrefs.GetInt("total attend", 1).ToString());
		builder.SetCustomDimension(dimensionDict["Total Chartboost"], PlayerPrefs.GetInt("Play Chartboost Ads", 0).ToString());
		builder.SetCustomDimension(dimensionDict["Total Unityads"], PlayerPrefs.GetInt("Play Unity Ads", 0).ToString());
		builder.SetCustomDimension(dimensionDict["Total Admob"], PlayerPrefs.GetInt("Play Admob Ads", 0).ToString());
		builder.SetCustomDimension(dimensionDict["Total Play"], SaveManager.GetTotalPlay().ToString());
		builder.SetCustomDimension(dimensionDict["No Ads"], SaveManager.GetRemoveAdsPurchased()? "1": "0");

		return builder;
	}

	public void SendCustomMetric()
	{
		EventHitBuilder builder = GetEventHitBuilder();

		float averageScore = SaveManager.GetTotalScore() / SaveManager.GetTotalPlay();

		builder.SetCustomMetric(metricDict["Total Play"], SaveManager.GetTotalPlay().ToString());
		builder.SetCustomMetric(metricDict["Best Score"], SaveManager.GetMaxScore().ToString());
		builder.SetCustomMetric(metricDict["Avg Score"], ((int)averageScore).ToString());
		builder.SetCustomMetric(metricDict["Total Exp"], Saved.GetInt(SaveKey.PlayerExp).ToString());
		builder.SetCustomDimension(dimensionDict["Total Chartboost"], PlayerPrefs.GetInt("Play Chartboost Ads", 0).ToString());
		builder.SetCustomDimension(dimensionDict["Total Unityads"], PlayerPrefs.GetInt("Play Unity Ads", 0).ToString());
		builder.SetCustomDimension(dimensionDict["Total Admob"], PlayerPrefs.GetInt("Play Admob Ads", 0).ToString());
		builder.SetCustomMetric(metricDict["Mission Completed"], "0");
		builder.SetEventCategory("Event");
		builder.SetEventAction("Attend");
		builder.SetEventLabel("attend");
		builder.SetEventValue(1);
	}
}
