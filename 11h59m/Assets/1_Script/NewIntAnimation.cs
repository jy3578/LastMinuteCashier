using UnityEngine;
using System.Collections;

public class NewIntAnimation : MonoBehaviour
{
	public		float	duration = 0;

	public enum StringType
	{
		Default,
		Resource,
		Score,
	}

	public		StringType 		mType = StringType.Default;

	public		UILabel	label;
	private		int		mScore = 0;
	private		int		mTargetScore = 0;
	private		int		mFromScore = 0;
	private		bool	mPlay = false;
	private		float	mTime = 0f;


	void Start(){
		label = gameObject.GetComponent<UILabel>();
	}

	private IEnumerator ScoreDisplay()
	{


		while(mPlay)
		{

			mTime += Time.deltaTime * Time.timeScale / duration;
			mScore = (int)Mathf.Lerp(mFromScore, mTargetScore, mTime);
			UpdateDisplay();

			if(mFromScore >= mTargetScore && mScore <= mTargetScore)
			{
				mPlay = false;
				mScore = mTargetScore;
				UpdateDisplay();
			}
			if(mFromScore <= mTargetScore && mScore >= mTargetScore)
			{
				mPlay = false;
				mScore = mTargetScore;
				UpdateDisplay();
			}

			yield return null;
		}

	}

	public void Play(int targetScore)
	{	
		Play(mScore, targetScore);
	}

	public void Play(int fromScore, int targetScore)
	{
		mTime = 0f;
		mTargetScore = targetScore;
		mFromScore = fromScore;
		mPlay = true;
		StartCoroutine("ScoreDisplay");
	}
	
		
	public void UpdateDisplay()
	{
		if(SaveManager.GetLanguage() == "Korean"){
			SetLabel(label, mScore);
		}else if(SaveManager.GetLanguage() =="English"){
			label.text = (((float)mScore)/1000f).ToString("F2");
		}
	}
	

	void SetLabel(UILabel label, int value)
	{
		if(label != null)
		{
			value = (int)Mathf.Max(0, value);
			string text = GetFormatedValue(value);
			
			label.text = text;
		}
	}
	
	string GetFormatedValue(int value)
	{

		if(mType == StringType.Resource) return value >= 10000? string.Format("{0}k",((float)value/1000).ToString("N1")) : value.ToString();
		else if(mType == StringType.Score) return value.ToString("#,##0");
		else return value.ToString ();
	}
}
