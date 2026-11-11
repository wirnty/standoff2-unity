using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32;

namespace NUnit.Framework.Internal
{
	[SecuritySafeCritical]
	public class OSPlatform
	{
		public enum ProductType
		{
			Unknown = 0,
			WorkStation = 1,
			DomainController = 2,
			Server = 3
		}

		private struct OSVERSIONINFOEX
		{
			public uint dwOSVersionInfoSize;

			public readonly uint dwMajorVersion;

			public readonly uint dwMinorVersion;

			public readonly uint dwBuildNumber;

			public readonly uint dwPlatformId;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public readonly string szCSDVersion;

			public readonly short wServicePackMajor;

			public readonly short wServicePackMinor;

			public readonly short wSuiteMask;

			public readonly byte ProductType;

			public readonly byte Reserved;
		}

		private readonly PlatformID _platform;

		private readonly Version _version;

		private readonly ProductType _product;

		private static readonly Lazy<OSPlatform> currentPlatform = new Lazy<OSPlatform>(delegate
		{
			OperatingSystem operatingSystem = Environment.OSVersion;
			if (operatingSystem.Platform == PlatformID.Win32NT && operatingSystem.Version.Major >= 5)
			{
				OSVERSIONINFOEX osvi = default(OSVERSIONINFOEX);
				osvi.dwOSVersionInfoSize = (uint)Marshal.SizeOf(osvi);
				GetVersionEx(ref osvi);
				if (operatingSystem.Version.Major == 6 && operatingSystem.Version.Minor >= 2)
				{
					operatingSystem = new OperatingSystem(operatingSystem.Platform, GetWindows81PlusVersion(operatingSystem.Version));
				}
				return new OSPlatform(operatingSystem.Platform, operatingSystem.Version, (ProductType)osvi.ProductType);
			}
			return CheckIfIsMacOSX(operatingSystem.Platform) ? new OSPlatform(PlatformID.MacOSX, operatingSystem.Version) : new OSPlatform(operatingSystem.Platform, operatingSystem.Version);
		});

		public static readonly PlatformID UnixPlatformID_Microsoft = PlatformID.Unix;

		public static readonly PlatformID UnixPlatformID_Mono = (PlatformID)128;

		public static readonly PlatformID XBoxPlatformID = PlatformID.Xbox;

		public static readonly PlatformID MacOSXPlatformID = PlatformID.MacOSX;

		public static OSPlatform CurrentPlatform
		{
			get
			{
				return currentPlatform.Value;
			}
		}

		public PlatformID Platform
		{
			get
			{
				return _platform;
			}
		}

		public Version Version
		{
			get
			{
				return _version;
			}
		}

		public ProductType Product
		{
			get
			{
				return _product;
			}
		}

		public bool IsWindows
		{
			get
			{
				if (_platform != PlatformID.Win32NT && _platform != PlatformID.Win32Windows && _platform != 0)
				{
					return _platform == PlatformID.WinCE;
				}
				return true;
			}
		}

		public bool IsUnix
		{
			get
			{
				if (_platform != UnixPlatformID_Microsoft)
				{
					return _platform == UnixPlatformID_Mono;
				}
				return true;
			}
		}

		public bool IsWin32S
		{
			get
			{
				return _platform == PlatformID.Win32S;
			}
		}

		public bool IsWin32Windows
		{
			get
			{
				return _platform == PlatformID.Win32Windows;
			}
		}

		public bool IsWin32NT
		{
			get
			{
				return _platform == PlatformID.Win32NT;
			}
		}

		public bool IsWinCE
		{
			get
			{
				return _platform == PlatformID.WinCE;
			}
		}

		public bool IsXbox
		{
			get
			{
				return _platform == XBoxPlatformID;
			}
		}

		public bool IsMacOSX
		{
			get
			{
				return _platform == MacOSXPlatformID;
			}
		}

		public bool IsWin95
		{
			get
			{
				if (_platform == PlatformID.Win32Windows && _version.Major == 4)
				{
					return _version.Minor == 0;
				}
				return false;
			}
		}

		public bool IsWin98
		{
			get
			{
				if (_platform == PlatformID.Win32Windows && _version.Major == 4)
				{
					return _version.Minor == 10;
				}
				return false;
			}
		}

		public bool IsWinME
		{
			get
			{
				if (_platform == PlatformID.Win32Windows && _version.Major == 4)
				{
					return _version.Minor == 90;
				}
				return false;
			}
		}

		public bool IsNT3
		{
			get
			{
				if (_platform == PlatformID.Win32NT)
				{
					return _version.Major == 3;
				}
				return false;
			}
		}

		public bool IsNT4
		{
			get
			{
				if (_platform == PlatformID.Win32NT)
				{
					return _version.Major == 4;
				}
				return false;
			}
		}

		public bool IsNT5
		{
			get
			{
				if (_platform == PlatformID.Win32NT)
				{
					return _version.Major == 5;
				}
				return false;
			}
		}

		public bool IsWin2K
		{
			get
			{
				if (IsNT5)
				{
					return _version.Minor == 0;
				}
				return false;
			}
		}

		public bool IsWinXP
		{
			get
			{
				if (IsNT5)
				{
					if (_version.Minor != 1)
					{
						if (_version.Minor == 2)
						{
							return Product == ProductType.WorkStation;
						}
						return false;
					}
					return true;
				}
				return false;
			}
		}

		public bool IsWin2003Server
		{
			get
			{
				if (IsNT5 && _version.Minor == 2)
				{
					return Product == ProductType.Server;
				}
				return false;
			}
		}

		public bool IsNT6
		{
			get
			{
				if (_platform == PlatformID.Win32NT)
				{
					return _version.Major == 6;
				}
				return false;
			}
		}

		public bool IsNT60
		{
			get
			{
				if (IsNT6)
				{
					return _version.Minor == 0;
				}
				return false;
			}
		}

		public bool IsNT61
		{
			get
			{
				if (IsNT6)
				{
					return _version.Minor == 1;
				}
				return false;
			}
		}

		public bool IsNT62
		{
			get
			{
				if (IsNT6)
				{
					return _version.Minor == 2;
				}
				return false;
			}
		}

		public bool IsNT63
		{
			get
			{
				if (IsNT6)
				{
					return _version.Minor == 3;
				}
				return false;
			}
		}

		public bool IsVista
		{
			get
			{
				if (IsNT60)
				{
					return Product == ProductType.WorkStation;
				}
				return false;
			}
		}

		public bool IsWin2008Server
		{
			get
			{
				if (!IsWin2008ServerR1)
				{
					return IsWin2008ServerR2;
				}
				return true;
			}
		}

		public bool IsWin2008ServerR1
		{
			get
			{
				if (IsNT60)
				{
					return Product == ProductType.Server;
				}
				return false;
			}
		}

		public bool IsWin2008ServerR2
		{
			get
			{
				if (IsNT61)
				{
					return Product == ProductType.Server;
				}
				return false;
			}
		}

		public bool IsWin2012Server
		{
			get
			{
				if (!IsWin2012ServerR1)
				{
					return IsWin2012ServerR2;
				}
				return true;
			}
		}

		public bool IsWin2012ServerR1
		{
			get
			{
				if (IsNT62)
				{
					return Product == ProductType.Server;
				}
				return false;
			}
		}

		public bool IsWin2012ServerR2
		{
			get
			{
				if (IsNT63)
				{
					return Product == ProductType.Server;
				}
				return false;
			}
		}

		public bool IsWindows7
		{
			get
			{
				if (IsNT61)
				{
					return Product == ProductType.WorkStation;
				}
				return false;
			}
		}

		public bool IsWindows8
		{
			get
			{
				if (IsNT62)
				{
					return Product == ProductType.WorkStation;
				}
				return false;
			}
		}

		public bool IsWindows81
		{
			get
			{
				if (IsNT63)
				{
					return Product == ProductType.WorkStation;
				}
				return false;
			}
		}

		public bool IsWindows10
		{
			get
			{
				if (_platform == PlatformID.Win32NT && _version.Major == 10)
				{
					return Product == ProductType.WorkStation;
				}
				return false;
			}
		}

		public bool IsWindowsServer10
		{
			get
			{
				if (_platform == PlatformID.Win32NT && _version.Major == 10)
				{
					return Product == ProductType.Server;
				}
				return false;
			}
		}

		private static Version GetWindows81PlusVersion(Version version)
		{
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion"))
				{
					if (registryKey != null)
					{
						string s = registryKey.GetValue("CurrentBuildNumber") as string;
						int result = 0;
						int.TryParse(s, out result);
						int? num = registryKey.GetValue("CurrentMajorVersionNumber") as int?;
						int? num2 = registryKey.GetValue("CurrentMinorVersionNumber") as int?;
						if (num.HasValue && num2.HasValue)
						{
							return new Version(num.Value, num2.Value, result);
						}
						if (registryKey.GetValue("CurrentVersion") as string == "6.3")
						{
							return new Version(6, 3, result);
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return version;
		}

		[DllImport("Kernel32.dll")]
		private static extern bool GetVersionEx(ref OSVERSIONINFOEX osvi);

		public OSPlatform(PlatformID platform, Version version)
		{
			_platform = platform;
			_version = version;
		}

		public OSPlatform(PlatformID platform, Version version, ProductType product)
			: this(platform, version)
		{
			_product = product;
		}

		[DllImport("libc")]
		private static extern int uname(IntPtr buf);

		private static bool CheckIfIsMacOSX(PlatformID platform)
		{
			switch (platform)
			{
			case PlatformID.MacOSX:
				return true;
			default:
				return false;
			case PlatformID.Unix:
			{
				IntPtr intPtr = Marshal.AllocHGlobal(8192);
				bool result = false;
				if (uname(intPtr) == 0)
				{
					result = Marshal.PtrToStringAnsi(intPtr).Equals("Darwin");
				}
				Marshal.FreeHGlobal(intPtr);
				return result;
			}
			}
		}
	}
}
