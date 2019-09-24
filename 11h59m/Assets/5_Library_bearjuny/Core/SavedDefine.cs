using UnityEngine;
using System.Collections;

public enum SaveKey
{	
	RemoveAds,
	StartTutorial,
	GameTutorial,
	MissionTutorial,
	FeverTutorial,
	Sound,
	PlayerExp,
	CollectionUnlocked,
	CollectionStates,
	MaxScore,
	MaxCombo,
	MaxChange,
	TotalScore,
	TotalPlay,
	TotalReceipt,
	MaxFeverSuccess,
	DisPlayCount,
	TotalDissatisfied,
	IgnorantMission,
	HomelessMission,
	ThiefMission,
	SkepticMission,
	BaldMission,
	GirlMission,
	BaldLastPlayedTime,
	BaldPlayingTime
}

public partial class Saved
{
	protected static object Default(SaveKey key)
	{
		switch(key)
		{
		case SaveKey.RemoveAds: return false;
		case SaveKey.StartTutorial: return false;
		case SaveKey.GameTutorial: return false;
		case SaveKey.MissionTutorial: return false;
		case SaveKey.FeverTutorial: return false;
		case SaveKey.Sound: return true;
		case SaveKey.PlayerExp: return 0;
		case SaveKey.CollectionUnlocked: return "00000000000000";
		case SaveKey.CollectionStates: return "11110000000111";
		case SaveKey.MaxScore: return 0;
		case SaveKey.MaxCombo: return 0;
		case SaveKey.MaxChange: return 0;
		case SaveKey.TotalScore: return 0;
		case SaveKey.TotalPlay: return 0;
		case SaveKey.TotalReceipt: return 0;
		case SaveKey.MaxFeverSuccess: return 0;
		
		case SaveKey.DisPlayCount: return 0;
		case SaveKey.TotalDissatisfied: return 0;

		case SaveKey.IgnorantMission: return 0;
		case SaveKey.HomelessMission: return 0;
		case SaveKey.ThiefMission: return 0;
		case SaveKey.SkepticMission: return 0;
		
		case SaveKey.BaldMission: return 0;
		case SaveKey.GirlMission: return 0;
		
		case SaveKey.BaldLastPlayedTime: return "";
		case SaveKey.BaldPlayingTime: return 0f;
		}
		return null;
	}
}