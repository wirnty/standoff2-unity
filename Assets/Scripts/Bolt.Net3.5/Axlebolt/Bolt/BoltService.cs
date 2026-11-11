namespace Axlebolt.Bolt
{
	public abstract class BoltService<T> : Service where T : BoltService<T>
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				BoltApi.Instance.CheckAuthenication();
				return _instance;
			}
			internal set
			{
				_instance = value;
			}
		}

		protected BoltService()
		{
			_instance = (T)this;
		}

		internal override void Destroy()
		{
			_instance = (T)null;
		}
	}
}
