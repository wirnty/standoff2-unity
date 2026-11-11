using System;
using System.Collections.Generic;

namespace Axlebolt.Standoff.Common
{
	public abstract class SimplePool<TKey, TInstance, TPrefabAttr, TInstanceAttr>
	{
		public class Pair
		{
			public TInstance Instance;

			public TInstanceAttr Attr;
		}

		protected readonly Dictionary<TKey, Queue<TInstance>> MainPool = new Dictionary<TKey, Queue<TInstance>>();

		protected readonly Dictionary<TInstance, TKey> Instances = new Dictionary<TInstance, TKey>();

		protected readonly Dictionary<TInstance, TInstanceAttr> InstanceAttrs = new Dictionary<TInstance, TInstanceAttr>();

		protected Dictionary<TKey, TPrefabAttr> Prefabs;

		private bool _initialized;

		protected void Init(int poolSize)
		{
			if (_initialized)
			{
				throw new InvalidOperationException("Pool already initialized");
			}
			Prefabs = InitPrefabs();
			LoadMaterials();
			InitPool(poolSize);
			_initialized = true;
		}

		protected abstract Dictionary<TKey, TPrefabAttr> InitPrefabs();

		private void InitPool(int poolSize)
		{
			foreach (TKey key in Prefabs.Keys)
			{
				MainPool[key] = new Queue<TInstance>();
				for (int i = 0; i < poolSize; i++)
				{
					CreateAndCache(key);
				}
			}
		}

		private void CreateAndCache(TKey key)
		{
			Queue<TInstance> queue = MainPool[key];
			TPrefabAttr val = Prefabs[key];
			Pair pair = Create(key, val);
			queue.Enqueue(pair.Instance);
			InstanceAttrs[pair.Instance] = pair.Attr;
			UpdateMaterial(pair.Instance, pair.Attr, val);
		}

		protected abstract Pair Create(TKey key, TPrefabAttr character);

		protected abstract void LoadMaterial(TPrefabAttr prefabAttr);

		protected abstract void UpdateMaterial(TInstance instance, TInstanceAttr attr, TPrefabAttr prefabAttr);

		private void UpdateMaterial(TKey key, TInstance instance)
		{
			TInstanceAttr attr = InstanceAttrs[instance];
			TPrefabAttr prefabAttr = Prefabs[key];
			UpdateMaterial(instance, attr, prefabAttr);
		}

		protected virtual void LoadMaterials()
		{
			foreach (KeyValuePair<TKey, TPrefabAttr> prefab in Prefabs)
			{
				LoadMaterial(prefab.Value);
			}
		}

		public void UpdateMaterials()
		{
			LoadMaterials();
			foreach (KeyValuePair<TInstance, TKey> instance in Instances)
			{
				UpdateMaterial(instance.Value, instance.Key);
			}
			foreach (KeyValuePair<TKey, Queue<TInstance>> item in MainPool)
			{
				foreach (TInstance item2 in item.Value)
				{
					UpdateMaterial(item.Key, item2);
				}
			}
		}

		public virtual TInstance GetFromPool(TKey key)
		{
			Queue<TInstance> queue = MainPool[key];
			if (queue.Count == 0)
			{
				CreateAndCache(key);
			}
			TInstance val = queue.Dequeue();
			Instances[val] = key;
			return val;
		}

		public virtual void ReturnToPool(TInstance value)
		{
			TKey key = Instances[value];
			Instances.Remove(value);
			MainPool[key].Enqueue(value);
		}
	}
}
