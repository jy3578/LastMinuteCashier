using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyLibrary;

public class RateLocalization
{
	public class Localization {
		public string title = null;
		public string message = null;
		public string yes = null;
		public string no = null;
		public string later = null;

		public override string ToString()
		{
			return "Title: " + title + 
				"\nMessage: " + message +
				"\nYes: " + yes +
				"\nLater: " + later +
				"\nNo: " + no;
		}
	}

	private const string FILE = @"RateLocalization";
	private const SystemLanguage DEFAULT_LANGUAGE = SystemLanguage.English;

	private static	Dictionary<string, Localization>		m_Table = new Dictionary<string, Localization>();

	private static	SystemLanguage		m_CurrentLanguage = DEFAULT_LANGUAGE;

	private static	bool		m_Loaded = false;

/*	public static Dictionary<string, Localization> Table {
		get {
			if( !m_Loaded ) LoadLocalization();
			return m_Table;
		}
	}*/
		
	public static SystemLanguage CurrentLanguage {
		get {
			if( !m_Loaded ) LoadLocalization();
			return m_CurrentLanguage;
		}
	}

	private static void LoadLocalization()
	{
		try {
			TextAsset textAsset = Resources.Load<TextAsset>( FILE );
			Dictionary<string, object> table = Json.Deserialize( textAsset.text ) as Dictionary<string, object>;

			Build( table );
			LoadLanguage();

			m_Loaded = true;
		} catch(Exception e) {
			Debug.LogError("Can not load locationzation file.");
		}
	}

	private static void Build( Dictionary<string, object> table )
	{
		if( table.Count > 0 )
		{
			foreach( KeyValuePair<string, object> keyVal in table )
			{
				string key = keyVal.Key;
				Dictionary<string, object> dic = keyVal.Value as Dictionary<string, object>;

				Localization language = BuildLanguage( dic );
				m_Table[ key ] = language;
			}
		}
	}

	private static Localization BuildLanguage(Dictionary<string, object> dic)
	{
		if( dic == null ) return null;

		Localization language = new Localization();
		if( dic.ContainsKey("Title") ) language.title = dic["Title"] as string;
		if( dic.ContainsKey("Message") ) language.message = dic["Message"] as string;
		if( dic.ContainsKey("Yes") ) language.yes = dic["Yes"] as string;
		if( dic.ContainsKey("Later") ) language.later = dic["Later"] as string;
		if( dic.ContainsKey("No") ) language.no = dic["No"] as string;

		return language;
	}

	private static void LoadLanguage()
	{
		if( m_Table.ContainsKey(Application.systemLanguage.ToString()) ) m_CurrentLanguage = Application.systemLanguage;
		else m_CurrentLanguage = DEFAULT_LANGUAGE;
	}

	public static Localization Get()
	{
		if( !m_Loaded ) LoadLocalization();

		if( m_Loaded )
		{
			string key = m_CurrentLanguage.ToString();
			if( m_Table.ContainsKey(key) )
			{
				return m_Table[key];
			}
		}
		return null;
	}
}
