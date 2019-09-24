using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using HutongGames.PlayMaker;

public class CustomerQueue : Singleton<CustomerQueue> {

	public int totalNumOfQ;
	public int totalNumOfNormal;     //미리 만들어 놓는 random normal 넘버 최대 개수.


	private List<int> normalManType;
	public List<Customer> customers;

	private SpawnPool pool;
	private bool onFever;

	//총 토탈 손님 수.
	private int numOfCustomer;
	private int numOfSuccess;
	private int numOfReceipt; //fever모드일 때 손님을 제외한 numOfCustomer.
	
	//총 토탈 손님 수 중, 진상 손님 수;
	private int numOfBads;
	private int numOfSuccessBads;


	//미션을 위해. - 0:girl / 1:bald / 2:Ignorant / 3:Homeless / 4:Thief / 5:Skeptic
	private int[,] numOfSpecific; 


	private int playerLv;

	void Start(){

		normalManType = new List<int>();
		customers = new List<Customer>();  //보이는 손님수 3~4명, 보이지 않는 대기자 4명.미리 생성

		onFever=false;

		pool = PoolManager.Pools["Customer"];

		numOfCustomer = 0;
		numOfReceipt = 0;
		numOfSuccess = 0;
		numOfBads = 0;
		numOfSuccessBads = 0;

		numOfSpecific = new int[6,4];

		playerLv = SaveManager.GetPlayerLevel();

	}

	public void InitializeQueue(){

		numOfCustomer = 0;
		numOfReceipt = 0;
		numOfSuccess = 0;
		numOfBads = 0;
		numOfSuccessBads = 0;

		numOfSpecific = new int[6,4];

		playerLv = SaveManager.GetPlayerLevel();

		onFever = false;
		if(normalManType !=null)	normalManType.Clear ();
		if(customers !=null)	customers.Clear ();

		MakeQueue ();

	}

	public void MakeQueue(){
		for(int i=0;i<totalNumOfQ;i++){
			AddCustomer ();
		}
	}

	private void AddCustomer(){
		//customer를 queue에 추가.initialize 된 customer.

		if(!isBad()){ //normals

			CustomerNormal oneMan = pool.Spawn ("CustomerNormal").gameObject.GetComponent<CustomerNormal>();
			oneMan.InitializeCustomer(GetNormalMan());
			customers.Add(oneMan);

		}else{
			switch(GetBadMan ())
			{
			case 1: //BadIgnorant
				Customer oneCustom2 = pool.Spawn ("CustomerIgnorant").gameObject.GetComponent<Customer>();
				oneCustom2.InitializeCustomer();
				customers.Add (oneCustom2);
				break;
			case 2: // BadHomeless
				Customer oneCustom1 = pool.Spawn ("CustomerHomeless").gameObject.GetComponent<Customer>();
				oneCustom1.InitializeCustomer();
				customers.Add (oneCustom1);

				break;
			case 3: //BadThief
				Customer oneCustom3 = pool.Spawn ("CustomerThief").gameObject.GetComponent<Customer>();
				oneCustom3.InitializeCustomer();
				customers.Add (oneCustom3);
				break;
			case 4: //BadSkeptic
				Customer oneCustom4 = pool.Spawn ("CustomerSkeptic").gameObject.GetComponent<Customer>();
				oneCustom4.InitializeCustomer();
				customers.Add (oneCustom4);
				break;
			}
		}
	}
	
	public void FeverStateOnOff(bool isFever){
		onFever = isFever;

		for(int i=0;i<3;i++){
			customers[i].FeverOnOff(onFever);
		}

		if(isFever){
			if(customers[0].onBadEffect){
				customers[0].EndPayment();
				customers[0].DespawnBadEffect();
			}
		}else{

			customers[0].InFrontOfDesk();
		
		}
	}

	public void FeverAttack(){
	
		Transform attackEffect = pool.Spawn("FeverAttack");
		attackEffect.localPosition= new Vector3(Random.Range (-200f,200f),Random.Range (50f,350f),0f);
		attackEffect.localScale = new Vector3(960f,960f,1f);

		customers[0].FeverAttack();

	}

	public float GetHP(){
		return customers[0].GetHP ();
	}

	private bool isBad(){
		//bad인지 아닌지 결정.
	

		if(playerLv==1) return false;

		//lv2 - (1/10) 확률로 등장. lv3 - (1/9). lv4 - (1/8). lv5 -(1/7).

		int rateOfBad;
		if(playerLv<6){
			rateOfBad = Random.Range(1,13-playerLv);
		}else{
			rateOfBad =	Random.Range(1,7);
		}

		if(rateOfBad == 1) return true;

		return false;

	}

	private int GetNormalMan(){
		//중복없이 normal Man Queue를 구성하기 위해.
	
		if(normalManType.Count ==0){ // 처음 시작했을때, 6개 만들어 놓기.

			int i=0;
			while(i<totalNumOfNormal){

				int manNumb = Random.Range (1,7);
				if(!normalManType.Contains (manNumb)){
					normalManType.Add (manNumb);
					i++;
				}
			}
		}else if(normalManType.Count == totalNumOfNormal/2){ // 3개가 빌때마다, 새로이 3개씩 생성.

			int i=0;
			while(i< totalNumOfNormal/2 ){
				
				int manNumb = Random.Range (1,7);
				if(!normalManType.Contains (manNumb)){
					normalManType.Add (manNumb);
					i++;
				}
			}
		}
		int normalMan = normalManType[0];
		normalManType.RemoveAt (0);
		return normalMan;
	}
	private int GetBadMan(){
		//1. BadIgnorant, 2. BadHomeless, 3. BadThief, 4. BadSkeptic.
		//사용자 레벨이 2 이상 - 초딩등장.
		//사용자 레벨이 3 이상 - 거지등장.
		//사용자 레벨이 4 이상 - 도둑등장.
		//5이상 - 커플등장.

		switch(playerLv){
		case 2: return 1;
		case 3: return Random.Range (1,3);
		case 4: return Random.Range (1,4);
		}
	
		return Random.Range (1,5);

	}

	public void EndPayment(bool isSuccess,bool isPerfect ,bool isFever){
		//손님이 계산을 끝냄.(confirm 누를때).계산 끝낸 손님은 바깥으로 나가고, queue에 있는 사람들은 앞으로 땡겨야 함.
		//계산 성공시, 각 number를 ++.

		if(onFever && !isFever){ //fever 모드 해제.
			FeverStateOnOff (false);
			return;
		}

		//move forward 애니메이션. - 제일 앞의 있는 손님은 move out.주의사항 - 애니메이션 끝난 후, despawn 해줘야 함.(exitCustomer에 저장을 한 다음, despawn).
		Customer exitCustomer = customers[0];
		customers.RemoveAt (0);
		AddCustomer ();
		
		//Sort image in Q. exit-5/ 1st-4 / 2nd-3 / 3rd-2.
		exitCustomer.SortImageInQ(5);
		exitCustomer.ChangeFaceBeforeExit(isSuccess);
		exitCustomer.MoveForward(0);


	
		CheckMission (isSuccess, isPerfect, exitCustomer);
	

		if(onFever) customers[2].FeverOnOff(true);
		
		if(!onFever && isFever){
			FeverStateOnOff(true);
		}

		//MoveForwardAll
		for(int i=0;i<3;i++){
			if(customers[i]!=null){
				
				customers[i].MoveForward(i+1);
				customers[i].SortImageInQ(4-i);
			}
		}

	}



	public void MoveForwardAll(){
		for(int i=0;i<3;i++){
			if(customers[i]!=null){
				
				customers[i].MoveForward(i+1);
				customers[i].SortImageInQ(4-i);
			}
		}
	}

	private void CheckMission(bool isSuccess,bool isPerfect,Customer exitCustomer){
		numOfCustomer++;
		if(!onFever) numOfReceipt++;
		
		if(exitCustomer.GetCustomerType() == CustomerType.Normal){
			if(isSuccess) numOfSuccess++;
			int normalType = exitCustomer.GetNormalType();
			
			if(normalType ==4 || normalType==5)	CheckMissionForEachType(normalType-4,isSuccess,isPerfect);
			//girl - normalType 4, bald - normalType 5.
			
		}else{ // Bad
			
			numOfBads++;
			if(isSuccess){
				numOfSuccess++;
				numOfSuccessBads++;
			}
			CheckMissionForEachType(exitCustomer.GetBadType()+1,isSuccess,isPerfect);
			//ignorant - badType 1, homeless - 2, thief - 3, skeptic - 4.
			
		}

	}

	private void CheckMissionForEachType(int whichCustomer, bool isSuccess, bool isPerfect){
		//미션을 위해. - 0:girl / 1:bald / 2:Ignorant / 3:Homeless / 4:Thief / 5:Skeptic
		if(isSuccess){
			if(isPerfect){
				numOfSpecific[whichCustomer,0]++;
				if(numOfSpecific[whichCustomer,1] != -1) numOfSpecific[whichCustomer,1]++;
				numOfSpecific[whichCustomer,2]++;
				if(numOfSpecific[whichCustomer,3] != -1) numOfSpecific[whichCustomer,3]++;
			}else{
				numOfSpecific[whichCustomer,0]++;
				if(numOfSpecific[whichCustomer,1] != -1) numOfSpecific[whichCustomer,1]++;

				numOfSpecific[whichCustomer,3] = -1;
			}
		}else{
			numOfSpecific[whichCustomer,1] = -1;
			numOfSpecific[whichCustomer,3] = -1;
		}
	}

	public void OnGameOver(){
		//despawn all the customers.
		customers[0].EndPayment();
		customers[0].DespawnBadEffect();

		foreach(Customer customer in customers){

			customer.DespawnChar();

		}
		customers.Clear();
	}

	public int GetTotalSuccessCustomer(){
		return numOfSuccess;
	}
	public int GetTotalSuccessBad(){
		return numOfSuccessBads;
	}

	public int GetNumberOfCustomer(){
		return numOfCustomer;
	}

	public int GetNumberOfReceipt(){
		return numOfReceipt;
	}

	public int GetNumberOfFailed(){
		return (numOfCustomer - numOfSuccess);
	}

	public int[,] GetNumberOfSpecific(){
		//girl, bald, ignorant, homeless, thief, skeptic.
		return numOfSpecific;
	}
}
