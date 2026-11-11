using System.Collections.Generic;
using Axlebolt.Standoff.Inventory.Drop;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Ragdoll;
using UnityEngine;

public class FallingCharacterController : MonoBehaviour, IFallingCharacter
{
	public List<TestGunController> gunList;

	private Vector3 prevPosition;

	private Vector3 velocity;

	public BipedMap GetBipedMap()
	{
		return GetComponent<BipedMap>();
	}

	public Vector3 GetCharacterVelocity()
	{
		return velocity;
	}

	public List<IDropableObject> GetThrowableObjects()
	{
		List<IDropableObject> list = new List<IDropableObject>();
		foreach (TestGunController gun in gunList)
		{
			list.Add(gun);
		}
		return list;
	}

	public FallingCharacterConfig GetFallingCharacterConfig()
	{
		return new FallingCharacterConfig();
	}

	public void OnSimulateFalling()
	{
		base.gameObject.SetActive(false);
	}

	public int GetPlayerId()
	{
		return 0;
	}

	public string GetName()
	{
		return base.gameObject.name;
	}

	private void Start()
	{
		prevPosition = base.transform.position;
	}

	private void Update()
	{
		velocity = base.transform.position - prevPosition;
		velocity /= Time.deltaTime;
		prevPosition = base.transform.position;
	}
}
