using System;
using System.Threading;
using UnityEngine;

namespace Axlebolt.Standoff.Utils
{
	public class TextureUtility
	{
		public static void Point(Texture2D tex, int newWidth, int newHeight)
		{
			TextureUtility.ThreadedScale(tex, newWidth, newHeight, false);
		}

		public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
		{
			TextureUtility.ThreadedScale(tex, newWidth, newHeight, true);
		}

		private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
		{
			TextureUtility._texColors = tex.GetPixels();
			TextureUtility._newColors = new Color[newWidth * newHeight];
			if (useBilinear)
			{
				TextureUtility._ratioX = 1f / ((float)newWidth / (float)(tex.width - 1));
				TextureUtility._ratioY = 1f / ((float)newHeight / (float)(tex.height - 1));
			}
			else
			{
				TextureUtility._ratioX = (float)tex.width / (float)newWidth;
				TextureUtility._ratioY = (float)tex.height / (float)newHeight;
			}
			TextureUtility._w = tex.width;
			TextureUtility._w2 = newWidth;
			int num = Mathf.Min(SystemInfo.processorCount, newHeight);
			int num2 = newHeight / num;
			TextureUtility._finishCount = 0;
			bool flag = TextureUtility._mutex == null;
			if (flag)
			{
				TextureUtility._mutex = new Mutex(false);
			}
			bool flag2 = num > 1;
			if (flag2)
			{
				int i;
				TextureUtility.ThreadData threadData;
				int num3;
				for (i = 0; i < num - 1; i = num3 + 1)
				{
					threadData = new TextureUtility.ThreadData(num2 * i, num2 * (i + 1));
					ParameterizedThreadStart start = useBilinear ? new ParameterizedThreadStart(TextureUtility.BilinearScale) : new ParameterizedThreadStart(TextureUtility.PointScale);
					Thread thread = new Thread(start);
					thread.Start(threadData);
					num3 = i;
				}
				threadData = new TextureUtility.ThreadData(num2 * i, newHeight);
				if (useBilinear)
				{
					TextureUtility.BilinearScale(threadData);
				}
				else
				{
					TextureUtility.PointScale(threadData);
				}
				while (TextureUtility._finishCount < num)
				{
					Thread.Sleep(1);
				}
			}
			else
			{
				TextureUtility.ThreadData obj = new TextureUtility.ThreadData(0, newHeight);
				if (useBilinear)
				{
					TextureUtility.BilinearScale(obj);
				}
				else
				{
					TextureUtility.PointScale(obj);
				}
			}
			tex.Resize(newWidth, newHeight);
			tex.SetPixels(TextureUtility._newColors);
			tex.Apply();
			TextureUtility._texColors = null;
			TextureUtility._newColors = null;
		}

		private static void BilinearScale(object obj)
		{
			TextureUtility.ThreadData threadData = (TextureUtility.ThreadData)obj;
			int num6;
			for (int i = threadData.start; i < threadData.end; i = num6 + 1)
			{
				int num = (int)Mathf.Floor((float)i * TextureUtility._ratioY);
				int num2 = num * TextureUtility._w;
				int num3 = (num + 1) * TextureUtility._w;
				int num4 = i * TextureUtility._w2;
				for (int j = 0; j < TextureUtility._w2; j = num6 + 1)
				{
					int num5 = (int)Mathf.Floor((float)j * TextureUtility._ratioX);
					float value = (float)j * TextureUtility._ratioX - (float)num5;
					TextureUtility._newColors[num4 + j] = TextureUtility.ColorLerpUnclamped(TextureUtility.ColorLerpUnclamped(TextureUtility._texColors[num2 + num5], TextureUtility._texColors[num2 + num5 + 1], value), TextureUtility.ColorLerpUnclamped(TextureUtility._texColors[num3 + num5], TextureUtility._texColors[num3 + num5 + 1], value), (float)i * TextureUtility._ratioY - (float)num);
					num6 = j;
				}
				num6 = i;
			}
			TextureUtility._mutex.WaitOne();
			num6 = TextureUtility._finishCount;
			TextureUtility._finishCount = num6 + 1;
			TextureUtility._mutex.ReleaseMutex();
		}

		private static void PointScale(object obj)
		{
			TextureUtility.ThreadData threadData = (TextureUtility.ThreadData)obj;
			int num3;
			for (int i = threadData.start; i < threadData.end; i = num3 + 1)
			{
				int num = (int)(TextureUtility._ratioY * (float)i) * TextureUtility._w;
				int num2 = i * TextureUtility._w2;
				for (int j = 0; j < TextureUtility._w2; j = num3 + 1)
				{
					TextureUtility._newColors[num2 + j] = TextureUtility._texColors[(int)((float)num + TextureUtility._ratioX * (float)j)];
					num3 = j;
				}
				num3 = i;
			}
			TextureUtility._mutex.WaitOne();
			num3 = TextureUtility._finishCount;
			TextureUtility._finishCount = num3 + 1;
			TextureUtility._mutex.ReleaseMutex();
		}

		private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
		{
			return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
		}

		public static Texture2D FromBytes(byte[] bytes)
		{
			Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGB24, false);
			texture2D.LoadImage(bytes);
			return texture2D;
		}

		private static Color[] _texColors;

		private static Color[] _newColors;

		private static int _w;

		private static float _ratioX;

		private static float _ratioY;

		private static int _w2;

		private static int _finishCount;

		private static Mutex _mutex;

		public class ThreadData
		{
			public ThreadData(int s, int e)
			{
				this.start = s;
				this.end = e;
			}

			public int start;

			public int end;
		}
	}
}
