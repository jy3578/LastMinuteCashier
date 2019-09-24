using UnityEngine;
using System.Collections;
using PathologicalGames;

public class DespawnFeverEffects : MonoBehaviour {

	private Transform originPool;

	public string spawnpoolName;
	private SpawnPool pools;
	

	public void TurnOff(){

		pools = PoolManager.Pools[spawnpoolName];
		pools.Despawn(gameObject.transform);

	}

}
