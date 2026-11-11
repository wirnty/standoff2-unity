using System;
using System.Collections.Generic;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory.Drop;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Ragdoll
{
	public class RagdollManager : Singleton<RagdollManager>
	{
		private static readonly Log Log = Log.Create(typeof(RagdollManager));

		public RagdollParameters RagdollParameters;

		private BipedMap _sourceCharacter;

		private RagdollParameters _ragdollParameters;

		private const int RagdollCountPerSkin = 2;

		private Dictionary<string, BipedMap> _ctCharacters;

		private Dictionary<string, BipedMap> _trCharacters;

		private Dictionary<string, Queue<RagdollController>> _ctRagdollPool = new Dictionary<string, Queue<RagdollController>>();

		private Dictionary<string, Queue<RagdollController>> _trRagdollPool = new Dictionary<string, Queue<RagdollController>>();

		private Dictionary<string, Queue<RagdollController>> _ctUsedRagdolls = new Dictionary<string, Queue<RagdollController>>();

		private Dictionary<string, Queue<RagdollController>> _trUsedRagdolls = new Dictionary<string, Queue<RagdollController>>();

		private Dictionary<int, RagdollController> _playerIdMappedRagdolls = new Dictionary<int, RagdollController>();

		private Dictionary<BipedMap.Bip, float> _bodyPartsMassMap;

		private bool _isPreInitialized;

		private float _fixedUpdateCount;

		private Vector3 _chestSpineInitialVelocity;

		public bool IsInitialized { get; set; }

		public void Initialize(PlayerCharacters ctCharacter, PlayerCharacters trCharacter)
		{
			_sourceCharacter = ResourcesUtility.Load<BipedMap>("Ragdoll/Ragdoll");
			_ragdollParameters = ResourcesUtility.Load<RagdollParameters>("Ragdoll/RagdollParameters");
			_ctCharacters = PlayerUtility.LoadCharacters(ctCharacter);
			_trCharacters = PlayerUtility.LoadCharacters(trCharacter);
			InitializePools();
			_fixedUpdateCount = 0f;
			_isPreInitialized = true;
		}

		private void FinishInitialization()
		{
			foreach (KeyValuePair<string, Queue<RagdollController>> item in _ctRagdollPool)
			{
				foreach (RagdollController item2 in item.Value)
				{
					item2.FinishInitialization();
				}
			}
			foreach (KeyValuePair<string, Queue<RagdollController>> item3 in _trRagdollPool)
			{
				foreach (RagdollController item4 in item3.Value)
				{
					item4.FinishInitialization();
				}
			}
			IsInitialized = true;
		}

		private void InitializePools()
		{
			foreach (KeyValuePair<string, BipedMap> ctCharacter in _ctCharacters)
			{
				if (!_ctRagdollPool.ContainsKey(ctCharacter.Key))
				{
					_ctRagdollPool[ctCharacter.Key] = new Queue<RagdollController>();
				}
				for (int i = 0; i < 2; i++)
				{
					_ctRagdollPool[ctCharacter.Key].Enqueue(Create(Team.Ct, ctCharacter.Key));
				}
			}
			foreach (KeyValuePair<string, BipedMap> trCharacter in _trCharacters)
			{
				if (!_trRagdollPool.ContainsKey(trCharacter.Key))
				{
					_trRagdollPool[trCharacter.Key] = new Queue<RagdollController>();
				}
				for (int j = 0; j < 2; j++)
				{
					_trRagdollPool[trCharacter.Key].Enqueue(Create(Team.Tr, trCharacter.Key));
				}
			}
		}

		private void CheckIfInitialized()
		{
			if (!IsInitialized)
			{
				Log.Error("Not Initialized Yet");
			}
		}

		public void ParseName(string id, out Team team, out string character)
		{
			string[] array = id.Split('_');
			if (array.Length != 3)
			{
				Log.Error("Invalid Name (" + id + ")");
			}
			team = (Team)Enum.Parse(typeof(Team), array[1]);
			character = array[2];
		}

		private RagdollController Create(Team team, string skin)
		{
			BipedMap bipedMap = ((team != Team.Ct) ? _trCharacters[skin] : _ctCharacters[skin]);
			GameObject gameObject = UnityEngine.Object.Instantiate(bipedMap.gameObject);
			RagdollController ragdollController = gameObject.AddComponent<RagdollController>();
			ragdollController.RagdollParameters = _ragdollParameters;
			ragdollController.PreInitialize(_sourceCharacter);
			ragdollController.name = string.Concat("Ragdoll_", team, "_", skin);
			if (_bodyPartsMassMap == null)
			{
				_bodyPartsMassMap = ragdollController.BodyPartsMassMap;
			}
			return ragdollController;
		}

		private void RemoveFromPlayerIdMap(RagdollController ragdollController)
		{
			foreach (KeyValuePair<int, RagdollController> playerIdMappedRagdoll in _playerIdMappedRagdolls)
			{
				if (playerIdMappedRagdoll.Value == ragdollController)
				{
					_playerIdMappedRagdolls.Remove(playerIdMappedRagdoll.Key);
					break;
				}
			}
		}

		private RagdollController GetRagdoll(Team team, string skin, int playerId)
		{
			CheckIfInitialized();
			Dictionary<string, Queue<RagdollController>> dictionary = ((team != Team.Ct) ? _trRagdollPool : _ctRagdollPool);
			RagdollController ragdollController = ((dictionary[skin].Count <= 0) ? GetFromUsedRagdollPool(team, skin) : dictionary[skin].Dequeue());
			AddUsedRagdoll(team, skin, ragdollController);
			RemoveFromPlayerIdMap(ragdollController);
			_playerIdMappedRagdolls[playerId] = ragdollController;
			return ragdollController;
		}

		public void ReturnPool(RagdollController ragdoll)
		{
			CheckIfInitialized();
			Team team;
			string character;
			ParseName(ragdoll.name, out team, out character);
			Dictionary<string, Queue<RagdollController>> dictionary = ((team != Team.Ct) ? _trRagdollPool : _ctRagdollPool);
			dictionary[character].Enqueue(ragdoll);
			ragdoll.gameObject.SetActive(false);
		}

		private void AddUsedRagdoll(Team team, string skin, RagdollController ragdoll)
		{
			Dictionary<string, Queue<RagdollController>> dictionary = ((team != Team.Ct) ? _trUsedRagdolls : _ctUsedRagdolls);
			if (!dictionary.ContainsKey(skin))
			{
				dictionary[skin] = new Queue<RagdollController>();
			}
			dictionary[skin].Enqueue(ragdoll);
		}

		private RagdollController GetFromUsedRagdollPool(Team team, string skin)
		{
			Dictionary<string, Queue<RagdollController>> dictionary = ((team != Team.Ct) ? _trUsedRagdolls : _ctUsedRagdolls);
			RagdollController ragdollController = dictionary[skin].Dequeue();
			if (ragdollController == null)
			{
				Log.Error(string.Format("Used Ragdoll With Skin {0} Not Found.", skin));
			}
			return ragdollController;
		}

		public void Simulate(IFallingCharacter fallingCharacter, HitData hitData)
		{
			CheckIfInitialized();
			Team team;
			string character;
			PlayerUtility.ParseId(fallingCharacter.GetName(), out team, out character);
			RagdollController ragdoll = GetRagdoll(team, character, fallingCharacter.GetPlayerId());
			ragdoll.Simulate(fallingCharacter, hitData);
		}

		public RagdollController GetActiveRagdollByPlayerId(int playerId)
		{
			if (_playerIdMappedRagdolls.ContainsKey(playerId))
			{
				return _playerIdMappedRagdolls[playerId];
			}
			Debug.LogError("No Active Ragdoll With PlayerId :" + playerId + " Found");
			return null;
		}

		public void UnloadResources()
		{
			foreach (KeyValuePair<string, Queue<RagdollController>> item in _ctRagdollPool)
			{
				foreach (RagdollController item2 in item.Value)
				{
					UnityEngine.Object.Destroy(item2.gameObject);
				}
			}
			foreach (KeyValuePair<string, Queue<RagdollController>> item3 in _trRagdollPool)
			{
				foreach (RagdollController item4 in item3.Value)
				{
					UnityEngine.Object.Destroy(item4.gameObject);
				}
			}
			foreach (KeyValuePair<string, BipedMap> ctCharacter in _ctCharacters)
			{
				if (ctCharacter.Value != null)
				{
					UnityEngine.Object.Destroy(ctCharacter.Value.gameObject);
				}
			}
			foreach (KeyValuePair<string, BipedMap> trCharacter in _trCharacters)
			{
				if (trCharacter.Value != null)
				{
					UnityEngine.Object.Destroy(trCharacter.Value.gameObject);
				}
			}
			UnityEngine.Object.Destroy(_sourceCharacter);
			_trRagdollPool.Clear();
			_ctRagdollPool.Clear();
			_ctCharacters.Clear();
			_trCharacters.Clear();
			_playerIdMappedRagdolls.Clear();
			IsInitialized = (_isPreInitialized = false);
		}

		private InventoryDropVector GetInventoryDropVector(InventoryAttachmentType attachmentType)
		{
			foreach (InventoryDropVector inventoryDropVector in _ragdollParameters.inventoryDropVectors)
			{
				if (inventoryDropVector.attachmentType == attachmentType)
				{
					return inventoryDropVector;
				}
			}
			return null;
		}

		public List<DropableObjectData> GetObjectsDropData(HitData hitData, Vector3 characterVelocity, Transform characterTransform, List<IDropableObject> dropableObjects)
		{
			CheckIfInitialized();
			BipedMap.Bip bone = hitData.Bone;
			RagdollParameters.BoneAffectGroup boneAffectGroup = _ragdollParameters.boneAffectGroup.Find((RagdollParameters.BoneAffectGroup x) => x.targetBone == bone);
			_chestSpineInitialVelocity = Vector3.zero;
			int num = 0;
			float num2 = 0f;
			if (boneAffectGroup != null)
			{
				foreach (RagdollParameters.BoneImpulseEffectParameter affectedBone in boneAffectGroup.affectedBones)
				{
					num2 += _bodyPartsMassMap[affectedBone.bone] * affectedBone.impulseMult;
				}
				num2 += _bodyPartsMassMap[bone] * boneAffectGroup.targetBoneImpulseMult;
				foreach (RagdollParameters.BoneImpulseEffectParameter affectedBone2 in boneAffectGroup.affectedBones)
				{
					if (affectedBone2.bone == BipedMap.Bip.Spine1 || affectedBone2.bone == BipedMap.Bip.Spine2)
					{
						float num3 = _bodyPartsMassMap[affectedBone2.bone];
						Vector3 vector = hitData.Direction * hitData.Impulse * (num3 / num2) * affectedBone2.impulseMult;
						_chestSpineInitialVelocity += vector.normalized * (vector.magnitude / num3);
						num++;
					}
				}
				if (bone == BipedMap.Bip.Spine1 || bone == BipedMap.Bip.Spine2)
				{
					float num4 = _bodyPartsMassMap[bone];
					Vector3 vector2 = hitData.Direction * hitData.Impulse * (num4 / num2) * boneAffectGroup.targetBoneImpulseMult;
					_chestSpineInitialVelocity += vector2.normalized * (vector2.magnitude / num4);
					num++;
				}
			}
			num = ((num == 0) ? 1 : num);
			_chestSpineInitialVelocity *= 1f / (float)num;
			float num5 = Vector3.Angle(hitData.Direction, characterVelocity);
			Vector3 vector3 = characterVelocity * _ragdollParameters.weaponInertialVelocityDistribution.Evaluate(num5 / 180f) * 0.3f;
			float num6 = _ragdollParameters.weaponAdditiveAngSpeedDistributionIntertial.Evaluate(_chestSpineInitialVelocity.magnitude / _ragdollParameters.referenceInertialVelocity) * _ragdollParameters.weaponAdditiveAngSpeedDistributionAngular.Evaluate(num5 / 180f);
			List<DropableObjectData> list = new List<DropableObjectData>();
			foreach (IDropableObject dropableObject in dropableObjects)
			{
				DropableObjectParameters dropableObjectParameters = dropableObject.GetDropableObjectParameters();
				InventoryAttachmentType attachmentType = dropableObject.GetAttachmentType();
				InventoryDropVector inventoryDropVector = GetInventoryDropVector(attachmentType);
				GameObject gameObject = dropableObject.GetGameObject();
				Vector3 vector4 = characterTransform.TransformDirection(inventoryDropVector.throwVector).normalized * inventoryDropVector.throwSpeed;
				Vector3 vector5 = characterTransform.TransformDirection(inventoryDropVector.angularVelocityAxis).normalized * inventoryDropVector.angularSpeed;
				float num7 = ((!(Vector3.Cross(hitData.Direction, vector4).y < 0f)) ? 1f : (-1f));
				DropableObjectData dropableObjectData = new DropableObjectData();
				dropableObjectData.angularVelocity = vector5 * dropableObjectParameters.initialVelocityMult * num6 * num7;
				dropableObjectData.velocity = vector4 * dropableObjectParameters.initialVelocityMult + vector3 * dropableObjectParameters.inertialVelocityMult + _chestSpineInitialVelocity * dropableObjectParameters.impulseVelocityMult;
				dropableObjectData.transformData.pos = gameObject.transform.position;
				dropableObjectData.transformData.rot = gameObject.transform.eulerAngles;
				dropableObjectData.transformData.scale = gameObject.transform.localScale;
				dropableObjectData.identityData = dropableObject.GetDropableObejctIdentityData();
				DropableObjectData item = dropableObjectData;
				list.Add(item);
			}
			return list;
		}

		private void FixedUpdate()
		{
			if (_isPreInitialized && !IsInitialized && _fixedUpdateCount > 2f)
			{
				FinishInitialization();
			}
			_fixedUpdateCount += 1f;
		}
	}
}
