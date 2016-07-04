﻿using System;

namespace NUSBusMap
{
	public static class SettingsVars
	{
		// all variables which user can set in Settings Page
		public static double MEAN_MAP_RADIUS = 0.5; // in km
		public static int REFRESH_BUS_INTERVAL = 3; // in s
		public static int REFRESH_STOP_INTERVAL = 30; // in s
		public static double MARGIN_OF_ERROR = 10.0; // in m, max distance allowed between bus and stop to be considered reached stop
	}
}
