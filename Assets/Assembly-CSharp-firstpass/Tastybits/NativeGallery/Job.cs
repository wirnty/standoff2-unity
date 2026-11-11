using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tastybits.NativeGallery
{
	public class Job : MonoBehaviour
	{
		[Serializable]
		public class Action
		{
			public string[] stack;

			public System.Action cb;

			public Action<float, float> cb2;

			public float timeCount = 0f;

			public int type = 99;

			public bool kill = false;

			public float delay = 0f;

			public int frames = 0;

			public int state = 0;

			public void Invoke()
			{
				if (!kill && cb != null)
				{
					cb();
					cb = null;
					kill = true;
				}
			}

			public void InvokeNoKill()
			{
				if (!kill && cb != null)
				{
					cb();
				}
			}

			public void InvokeNoKillForWaitWithStep(float absTime, float dt)
			{
				if (!kill && cb2 != null)
				{
					cb2(absTime, dt);
				}
			}

			public void Die()
			{
				kill = true;
				cb = null;
				cb2 = null;
			}
		}

		private static Job instance = null;

		public bool manualUpdate = false;

		public bool releaseManualUpdate = false;

		private bool initialized = false;

		private List<Action> actions = new List<Action>();

		private List<Action> toBeAdded = new List<Action>();

		private List<Action> toBeRemoved = new List<Action>();

		public bool processing = false;

		private bool killCurrentStep = false;

		public bool verbose = false;

		private static System.Action tmpdeleg;

		private static System.Action deleg1;

		private static System.Action deleg2;

		private static System.Action deleg3;

		private static System.Action deleg4;

		private static System.Action deleg5;

		private static System.Action deleg6;

		private static System.Action deleg7;

		private static System.Action deleg8;

		private static System.Action deleg9;

		private static System.Action deleg10;

		private Job()
		{
			instance = this;
			initialized = false;
		}

		public void EditCmdFindDuplicates()
		{
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(Job));
			Debug.Log("comps : " + array.Length);
		}

		public void CmdReleaseManualUpdate()
		{
			releaseManualUpdate = true;
		}

		private void Awake()
		{
			base.hideFlags |= HideFlags.HideInHierarchy;
			if (instance != null && instance != this)
			{
				Debug.LogError("Error there is only supposed to be one instance of this...");
			}
			instance = this;
			initialized = true;
			toBeAdded.Clear();
			toBeRemoved.Clear();
			processing = false;
			killCurrentStep = false;
		}

		private void Update()
		{
			if (manualUpdate)
			{
				if (releaseManualUpdate)
				{
					releaseManualUpdate = false;
					UpdateImpl();
				}
			}
			else
			{
				UpdateImpl();
			}
		}

		private void UpdateImpl()
		{
			if (toBeAdded.Count > 0)
			{
				foreach (Action item in toBeAdded)
				{
					if (verbose)
					{
						Debug.Log("Adding action : " + item.type);
					}
					Action action = new Action();
					action = item;
					action.kill = false;
					actions.Add(action);
				}
				toBeAdded.Clear();
			}
			processing = true;
			float num = Time.deltaTime;
			if (num <= 0f && !Application.isPlaying)
			{
				num = 0.5f;
			}
			foreach (Action action2 in actions)
			{
				killCurrentStep = false;
				if (!action2.kill)
				{
					if (action2.type == 0)
					{
						action2.delay -= num;
						if (action2.delay <= 0f)
						{
							action2.Invoke();
							action2.kill = true;
						}
					}
					else if (action2.type == 1)
					{
						if (action2.frames - Time.frameCount <= 0)
						{
							action2.Invoke();
							action2.kill = true;
						}
					}
					else if (action2.type == 2)
					{
						killCurrentStep = false;
						action2.kill = false;
						action2.InvokeNoKill();
						if (killCurrentStep)
						{
							action2.kill = true;
						}
					}
					else if (action2.type == 22)
					{
						action2.delay -= num;
						killCurrentStep = false;
						action2.kill = false;
						if (action2.delay <= 0f)
						{
							num -= Mathf.Abs(action2.delay);
							action2.delay = 0f;
						}
						action2.timeCount += num;
						action2.InvokeNoKillForWaitWithStep(action2.timeCount, num);
						if (killCurrentStep || action2.delay <= 0f)
						{
							if (action2.cb != null)
							{
								action2.cb();
							}
							action2.cb = null;
							action2.kill = true;
						}
					}
					else if (action2.type == 3)
					{
						action2.delay -= num;
						if (action2.delay <= 0f)
						{
							action2.Invoke();
							action2.state = -1;
							action2.kill = true;
						}
					}
				}
				else if (verbose)
				{
					Debug.Log("we are interating on a objet with was flagged for killed");
				}
				if (action2.kill)
				{
					toBeRemoved.Add(action2);
				}
			}
			processing = false;
			foreach (Action item2 in toBeRemoved)
			{
				if (verbose)
				{
					Debug.Log("Removing action : " + item2.type);
				}
				actions.Remove(item2);
			}
			toBeRemoved.Clear();
		}

		private static void EnsureCreated()
		{
			if (!(instance == null))
			{
				return;
			}
			Job job = UnityEngine.Object.FindObjectOfType<Job>();
			if (job != null)
			{
				instance = job;
				if (instance.name != "Job")
				{
					Debug.LogError("Error the name of gameobject was : " + job.name + " but was expected to be 'Worker'");
				}
				return;
			}
			GameObject gameObject = null;
			if (!Application.isPlaying)
			{
				gameObject = GameObject.Find("Job");
			}
			if (gameObject == null)
			{
				gameObject = new GameObject("Job");
			}
			if (gameObject.GetComponent<Job>() != null)
			{
				instance = gameObject.GetComponent<Job>();
			}
			else
			{
				instance = gameObject.AddComponent<Job>();
			}
		}

		public static Action WaitForSeconds(float t, System.Action a)
		{
			EnsureCreated();
			return AddAction(0, t, 0, a);
		}

		public static Action WaitForSecondsOrClick(float t, System.Action a)
		{
			EnsureCreated();
			return AddAction(0, t, 0, a);
		}

		public static void WaitForFrames(int frames, System.Action a)
		{
			EnsureCreated();
			AddAction(1, 0f, frames, a);
		}

		private static IEnumerator WaitForEndOfFrameImpl()
		{
			yield return new WaitForEndOfFrame();
		}

		public static void WaitForEndOfFrame(System.Action callback)
		{
			EnsureCreated();
			StartAndWaitforCoroutine(WaitForEndOfFrameImpl(), callback);
		}

		public static void Step(System.Action a)
		{
			EnsureCreated();
			AddAction(2, 0f, 0, a);
		}

		public static Action StepWaitForSeconds(float time, Action<float, float> c2, System.Action c)
		{
			EnsureCreated();
			Action action = AddAction(22, time, 0, c);
			action.cb2 = c2;
			return action;
		}

		public static Action AnimateValue(float duration, float startVal, float endVal, Action<float> setter, System.Action done)
		{
			return StepWaitForSeconds(duration, delegate(float absTime, float dt)
			{
				float obj = Mathf.SmoothStep(startVal, endVal, absTime / duration);
				setter(obj);
			}, done);
		}

		public static void OnUpdate(System.Action a)
		{
			EnsureCreated();
			AddAction(2, 0f, 0, a);
		}

		public static System.Action TimedOutCallback(System.Action cb)
		{
			return cb;
		}

		public static Action<bool> TimedOutCallback(Action<bool> cb)
		{
			return cb;
		}

		public static void KillCurrentStep()
		{
			if (instance.verbose)
			{
				Debug.Log("KillCurrentStep called");
			}
			instance.killCurrentStep = true;
		}

		public static void StopListen()
		{
			KillCurrentStep();
		}

		public static Coroutine StartCo(IEnumerator ienum)
		{
			EnsureCreated();
			return instance.StartCoroutine(ienum);
		}

		public static bool StartAndWaitforCoroutine(IEnumerator ienum, System.Action deleg)
		{
			if (deleg1 == null)
			{
				deleg1 = deleg;
				instance.StartCoroutine(instance.StartCoroutine1(ienum));
				return true;
			}
			if (deleg2 == null)
			{
				deleg2 = deleg;
				instance.StartCoroutine(instance.StartCoroutine2(ienum));
				return true;
			}
			if (deleg3 == null)
			{
				deleg3 = deleg;
				instance.StartCoroutine(instance.StartCoroutine3(ienum));
				return true;
			}
			if (deleg4 == null)
			{
				deleg4 = deleg;
				instance.StartCoroutine(instance.StartCoroutine4(ienum));
				return true;
			}
			if (deleg5 == null)
			{
				deleg5 = deleg;
				instance.StartCoroutine(instance.StartCoroutine5(ienum));
				return true;
			}
			if (deleg6 == null)
			{
				deleg6 = deleg;
				instance.StartCoroutine(instance.StartCoroutine6(ienum));
				return true;
			}
			if (deleg7 == null)
			{
				deleg7 = deleg;
				instance.StartCoroutine(instance.StartCoroutine7(ienum));
				return true;
			}
			if (deleg8 == null)
			{
				deleg8 = deleg;
				instance.StartCoroutine(instance.StartCoroutine8(ienum));
				return true;
			}
			if (deleg9 == null)
			{
				deleg9 = deleg;
				instance.StartCoroutine(instance.StartCoroutine9(ienum));
				return true;
			}
			if (deleg10 == null)
			{
				deleg10 = deleg;
				instance.StartCoroutine(instance.StartCoroutine10(ienum));
				return true;
			}
			Debug.LogError("Error: there is no slot available for the deledate to call");
			return false;
		}

		private IEnumerator StartCoroutine1(IEnumerator ienum)
		{
			yield return StartCoroutine(ienum);
			tmpdeleg = deleg1;
			deleg1 = null;
			if (tmpdeleg != null)
			{
				tmpdeleg();
			}
			else
			{
				Debug.LogError("Error callback is null LOOK INTO THIS!!!");
			}
		}

		private IEnumerator StartCoroutine2(IEnumerator ienum)
		{
			yield return StartCoroutine(ienum);
			tmpdeleg = deleg2;
			deleg2 = null;
			if (tmpdeleg != null)
			{
				tmpdeleg();
			}
			else
			{
				Debug.LogError("Error callback is null LOOK INTO THIS!!!");
			}
		}

		private IEnumerator StartCoroutine3(IEnumerator ienum)
		{
			yield return StartCoroutine(ienum);
			tmpdeleg = deleg3;
			deleg3 = null;
			if (tmpdeleg != null)
			{
				tmpdeleg();
			}
			else
			{
				Debug.LogError("Error callback is null LOOK INTO THIS!!!");
			}
		}

		private IEnumerator StartCoroutine4(IEnumerator ienum)
		{
			yield return StartCoroutine(ienum);
			tmpdeleg = deleg4;
			deleg4 = null;
			if (tmpdeleg != null)
			{
				tmpdeleg();
			}
			else
			{
				Debug.LogError("Error callback is null LOOK INTO THIS!!!");
			}
		}

		private IEnumerator StartCoroutine5(IEnumerator ienum)
		{
			yield return StartCoroutine(ienum);
			tmpdeleg = deleg5;
			deleg5 = null;
			if (tmpdeleg != null)
			{
				tmpdeleg();
			}
			else
			{
				Debug.LogError("Error callback is null LOOK INTO THIS!!!");
			}
		}

		private IEnumerator StartCoroutine6(IEnumerator ienum)
		{
			yield return StartCoroutine(ienum);
			tmpdeleg = deleg6;
			deleg6 = null;
			if (tmpdeleg != null)
			{
				tmpdeleg();
			}
			else
			{
				Debug.LogError("Error callback is null LOOK INTO THIS!!!");
			}
		}

		private IEnumerator StartCoroutine7(IEnumerator ienum)
		{
			yield return StartCoroutine(ienum);
			tmpdeleg = deleg7;
			deleg7 = null;
			if (tmpdeleg != null)
			{
				tmpdeleg();
			}
			else
			{
				Debug.LogError("Error callback is null LOOK INTO THIS!!!");
			}
		}

		private IEnumerator StartCoroutine8(IEnumerator ienum)
		{
			yield return StartCoroutine(ienum);
			tmpdeleg = deleg8;
			deleg8 = null;
			if (tmpdeleg != null)
			{
				tmpdeleg();
			}
			else
			{
				Debug.LogError("Error callback is null LOOK INTO THIS!!!");
			}
		}

		private IEnumerator StartCoroutine9(IEnumerator ienum)
		{
			yield return StartCoroutine(ienum);
			tmpdeleg = deleg9;
			deleg9 = null;
			if (tmpdeleg != null)
			{
				tmpdeleg();
			}
			else
			{
				Debug.LogError("Error callback is null LOOK INTO THIS!!!");
			}
		}

		private IEnumerator StartCoroutine10(IEnumerator ienum)
		{
			yield return StartCoroutine(ienum);
			tmpdeleg = deleg10;
			deleg10 = null;
			if (tmpdeleg != null)
			{
				tmpdeleg();
			}
			else
			{
				Debug.LogError("Error callback is null LOOK INTO THIS!!!");
			}
		}

		private static Action AddAction(int type, float t, int frames, System.Action a)
		{
			Action action = new Action();
			action.cb = a;
			action.delay = t;
			action.type = type;
			action.frames = Time.frameCount + frames;
			action.kill = false;
			if (frames > 0)
			{
			}
			if (!instance.processing)
			{
				if (instance.verbose)
				{
					Debug.Log("adding action : " + action.type + "  ( now )");
				}
				instance.actions.Add(action);
			}
			else
			{
				if (instance.verbose)
				{
					Debug.Log("adding action : " + action.type + "  ( add later )");
				}
				instance.toBeAdded.Add(action);
			}
			return action;
		}
	}
}
