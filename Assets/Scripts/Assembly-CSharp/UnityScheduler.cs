using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class UnityScheduler : MonoBehaviour
{
	public static int MainThreadId { get; private set; }

	[Obsolete]
	public static UnityTaskScheduler MainThreadScheduler
	{
		[CompilerGenerated]
		get
		{
			return UpdateScheduler;
		}
	}

	public static UnityTaskScheduler UpdateScheduler { get; private set; }

	public static UnityTaskScheduler LateUpdateScheduler { get; private set; }

	public static UnityTaskScheduler FixedUpdateScheduler { get; private set; }

	public static UnityTaskScheduler EditorUpdateScheduler { get; private set; }

	public static TaskScheduler ThreadPoolScheduler
	{
		[CompilerGenerated]
		get
		{
			return TaskScheduler.Default;
		}
	}

	[RuntimeInitializeOnLoadMethod]
	private static void Initialize()
	{
		MainThreadId = Thread.CurrentThread.ManagedThreadId;
		UpdateScheduler = new UnityTaskScheduler("Update");
		LateUpdateScheduler = new UnityTaskScheduler("LateUpdate");
		FixedUpdateScheduler = new UnityTaskScheduler("FixedUpdate");
		SynchronizationContext.SetSynchronizationContext(UpdateScheduler.Context);
		GameObject gameObject = new GameObject("UnityScheduler");
		gameObject.hideFlags = HideFlags.HideAndDontSave;
		gameObject.AddComponent<UnityScheduler>();
	}

	public static void InitializeInEditor()
	{
		MainThreadId = Thread.CurrentThread.ManagedThreadId;
		EditorUpdateScheduler = new UnityTaskScheduler("EditorUpdate");
		SynchronizationContext.SetSynchronizationContext(EditorUpdateScheduler.Context);
	}

	public static void ProcessEditorUpdate()
	{
		EditorUpdateScheduler.Activate();
	}

	private void Update()
	{
		UpdateScheduler.Activate();
	}

	private void LateUpdate()
	{
		LateUpdateScheduler.Activate();
	}

	private void FixedUpdate()
	{
		FixedUpdateScheduler.Activate();
	}
}
