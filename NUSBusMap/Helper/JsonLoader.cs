﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NUSBusMap
{
	public static class JsonLoader
	{
		// class to load json file and deserialize to object for use in code

		const string stopsFilename = "BusStops.json";
		const string svcsFilename = "BusSvcs.json";
		const string publicBusFilename = "PublicBusSvcBusStops.json";

		public static IStreamLoader Loader { get; set;}

		public static  Dictionary<string,BusStop> LoadStops()
		{
			using (var reader = new StreamReader(OpenData(stopsFilename))) {
				return JsonConvert.DeserializeObject<Dictionary<string,BusStop>>( reader.ReadToEnd());
			}
		}

		public static Dictionary<string,BusSvc> LoadSvcs()
		{
			using (var reader = new StreamReader(OpenData(svcsFilename))) {
				return JsonConvert.DeserializeObject<Dictionary<string,BusSvc>>(reader.ReadToEnd());
			}
		}

		public static Dictionary<string,List<string>> LoadPublicBuses() 
		{
			using (var reader = new StreamReader(OpenData(publicBusFilename))) {
				return JsonConvert.DeserializeObject<Dictionary<string,List<string>>>(reader.ReadToEnd());
			}
		}

		public static async Task<PublicBusStop> LoadPublicBusInfo (string busStopCode, string busSvcNo = "")
		{
			// Create a HTTP request using the URL
			// add bus service no if provided
			var uri = (busSvcNo.Equals (String.Empty)) ? 
						"http://datamall2.mytransport.sg/ltaodataservice/BusArrival?BusStopID=" + busStopCode + "&SST=True" : 
						"http://datamall2.mytransport.sg/ltaodataservice/BusArrival?BusStopID=" + busStopCode + "&ServiceNo=" + busSvcNo + "&SST=True";
			using (HttpClient client = new HttpClient ()) {
				client.DefaultRequestHeaders.Add ("AccountKey", "r8s1r+KgQfKAVDqzWavckQ==");
				client.DefaultRequestHeaders.Add ("UniqueUserID", "0b795a48-f4b5-4a43-b1f3-7f01289b8ffc");
				client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));

				// get json response and convert to PublicBusStop object
				using (var response = await client.GetAsync (uri)) {
					string data = await response.Content.ReadAsStringAsync ();
					return JsonConvert.DeserializeObject<PublicBusStop> (data);
				}
			}
		}

		private static Stream OpenData(string filename)
		{
			if (Loader == null)
				throw new Exception ("Must set platform before calling Load.");
			
			return Loader.GetStreamFromFilename(filename);
		}
	}
}

