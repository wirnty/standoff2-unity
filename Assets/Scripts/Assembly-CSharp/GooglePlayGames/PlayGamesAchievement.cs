using System;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	internal class PlayGamesAchievement : IAchievement, IAchievementDescription
	{
		private readonly ReportProgress mProgressCallback;

		private string mId = string.Empty;

		private bool mIsIncremental = false;

		private int mCurrentSteps = 0;

		private int mTotalSteps = 0;

		private double mPercentComplete = 0.0;

		private bool mCompleted = false;

		private bool mHidden = false;

		private DateTime mLastModifiedTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		private string mTitle = string.Empty;

		private string mRevealedImageUrl = string.Empty;

		private string mUnlockedImageUrl = string.Empty;

		private WWW mImageFetcher = null;

		private Texture2D mImage = null;

		private string mDescription = string.Empty;

		private ulong mPoints = 0uL;

		public string id
		{
			get
			{
				return mId;
			}
			set
			{
				mId = value;
			}
		}

		public bool isIncremental
		{
			get
			{
				return mIsIncremental;
			}
		}

		public int currentSteps
		{
			get
			{
				return mCurrentSteps;
			}
		}

		public int totalSteps
		{
			get
			{
				return mTotalSteps;
			}
		}

		public double percentCompleted
		{
			get
			{
				return mPercentComplete;
			}
			set
			{
				mPercentComplete = value;
			}
		}

		public bool completed
		{
			get
			{
				return mCompleted;
			}
		}

		public bool hidden
		{
			get
			{
				return mHidden;
			}
		}

		public DateTime lastReportedDate
		{
			get
			{
				return mLastModifiedTime;
			}
		}

		public string title
		{
			get
			{
				return mTitle;
			}
		}

		public Texture2D image
		{
			get
			{
				return LoadImage();
			}
		}

		public string achievedDescription
		{
			get
			{
				return mDescription;
			}
		}

		public string unachievedDescription
		{
			get
			{
				return mDescription;
			}
		}

		public int points
		{
			get
			{
				return (int)mPoints;
			}
		}

		internal PlayGamesAchievement()
			: this(PlayGamesPlatform.Instance.ReportProgress)
		{
		}

		internal PlayGamesAchievement(ReportProgress progressCallback)
		{
			mProgressCallback = progressCallback;
		}

		internal PlayGamesAchievement(Achievement ach)
			: this()
		{
			mId = ach.Id;
			mIsIncremental = ach.IsIncremental;
			mCurrentSteps = ach.CurrentSteps;
			mTotalSteps = ach.TotalSteps;
			if (ach.IsIncremental)
			{
				if (ach.TotalSteps > 0)
				{
					mPercentComplete = (double)ach.CurrentSteps / (double)ach.TotalSteps * 100.0;
				}
				else
				{
					mPercentComplete = 0.0;
				}
			}
			else
			{
				mPercentComplete = ((!ach.IsUnlocked) ? 0.0 : 100.0);
			}
			mCompleted = ach.IsUnlocked;
			mHidden = !ach.IsRevealed;
			mLastModifiedTime = ach.LastModifiedTime;
			mTitle = ach.Name;
			mDescription = ach.Description;
			mPoints = ach.Points;
			mRevealedImageUrl = ach.RevealedImageUrl;
			mUnlockedImageUrl = ach.UnlockedImageUrl;
		}

		public void ReportProgress(Action<bool> callback)
		{
			mProgressCallback(mId, mPercentComplete, callback);
		}

		private Texture2D LoadImage()
		{
			if (hidden)
			{
				return null;
			}
			string text = ((!completed) ? mRevealedImageUrl : mUnlockedImageUrl);
			if (!string.IsNullOrEmpty(text))
			{
				if (mImageFetcher == null || mImageFetcher.url != text)
				{
					mImageFetcher = new WWW(text);
					mImage = null;
				}
				if (mImage != null)
				{
					return mImage;
				}
				if (mImageFetcher.isDone)
				{
					mImage = mImageFetcher.texture;
					return mImage;
				}
			}
			return null;
		}
	}
}
