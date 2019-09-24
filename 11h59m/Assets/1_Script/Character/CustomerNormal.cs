using UnityEngine;
using System.Collections;

public class CustomerNormal : Customer {
	

	public int normalType;

	public void InitializeCustomer (int type)
	{
		prefixName = "play_char_" + type.ToString();
		normalType = type;
		base.InitializeCustomer ();
		MakeCustomer ();

	}

	public void MakeCustomer(){

		customerHead.SetSprite(prefixName+"_face");
		customerBody[0].SetSprite(prefixName+"_top");
		customerBody[1].SetSprite(prefixName+"_pants");
		
	}
	
	override public int GetNormalType(){
		return normalType;
	}
}
