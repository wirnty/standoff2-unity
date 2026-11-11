using System;
using System.Collections.Generic;
using Axlebolt.Standoff.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace Axlebolt.Standoff.Effects
{
	public class SimpleDecalsDrawer : DecalsDrawer
	{
		private static readonly Log Log = Log.Create(typeof(SimpleDecalsDrawer));

		private MeshFilter _meshFilter;

		private MeshRenderer _meshRenderer;

		private Sprite[] _decals;

		private readonly List<Vector3> _vertices = new List<Vector3>();

		private readonly List<int> _triangles = new List<int>();

		private readonly List<Vector2> _uv = new List<Vector2>();

		private Transform _helper;

		public static SimpleDecalsDrawer Create([NotNull] Material material, [NotNull] Sprite[] decals)
		{
			if (material == null)
			{
				throw new ArgumentNullException("material");
			}
			if (decals == null)
			{
				throw new ArgumentNullException("decals");
			}
			if (decals.Length == 0)
			{
				throw new ArgumentException("Decals are empty");
			}
			ValidateDecals(decals);
			GameObject gameObject = new GameObject(material.name + "SimpleDecalsDrawer");
			SimpleDecalsDrawer simpleDecalsDrawer = gameObject.AddComponent<SimpleDecalsDrawer>();
			simpleDecalsDrawer.Init(material, decals);
			return simpleDecalsDrawer;
		}

		private static void ValidateDecals(Sprite[] declas)
		{
			Texture2D texture = declas[0].texture;
			bool flag = true;
			for (int i = 1; i < declas.Length; i++)
			{
				if (declas[i].texture != texture)
				{
					Log.Error(string.Format("Multiple textures not supported, {0}!={1}. Different PackingTag?", texture, declas[i].texture));
					flag = false;
				}
			}
			if (!flag)
			{
				throw new InvalidOperationException("Invalid decals");
			}
		}

		private void Init(Material material, Sprite[] decals)
		{
			_decals = decals;
			_meshFilter = base.gameObject.AddComponent<MeshFilter>();
			_meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			_meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			_meshRenderer.receiveShadows = false;
			_meshRenderer.lightProbeUsage = LightProbeUsage.Off;
			_meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
			_meshRenderer.material = material;
			_meshRenderer.material.mainTexture = _decals[0].texture;
			_meshFilter.sharedMesh = new Mesh
			{
				bounds = new Bounds(Vector3.zero, new Vector3(500f, 500f, 500f))
			};
			GameObject gameObject = new GameObject("DecalHelper");
			gameObject.transform.SetParent(base.transform);
			gameObject.SetActive(false);
			_helper = gameObject.transform;
		}

		public override void DrawDecal(RaycastHit raycastHit, Vector2 size, int decalIndex)
		{
			_helper.transform.position = raycastHit.point + 0.001f * raycastHit.normal;
			_helper.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
			int count = _vertices.Count;
			float num = size.x / 2f;
			float num2 = size.y / 2f;
			Vector3[] array = new Vector3[4]
			{
				new Vector3(0f - num, num2, 0f),
				new Vector3(num, num2, 0f),
				new Vector3(0f - num, 0f - num2, 0f),
				new Vector3(num, 0f - num2, 0f)
			};
			int[] collection = new int[6]
			{
				count,
				count + 1,
				count + 2,
				count + 2,
				count + 1,
				count + 3
			};
			_triangles.AddRange(collection);
			Vector3[] array2 = array;
			foreach (Vector3 position in array2)
			{
				_vertices.Add(_helper.transform.TransformPoint(position));
			}
			_uv.AddRange(_decals[decalIndex].uv);
			_meshFilter.sharedMesh.SetVertices(_vertices);
			_meshFilter.sharedMesh.SetTriangles(_triangles, 0);
			_meshFilter.sharedMesh.SetUVs(0, _uv);
		}
	}
}
