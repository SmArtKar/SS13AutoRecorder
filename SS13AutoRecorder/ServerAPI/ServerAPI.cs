using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SS13AutoRecorder.ServerAPI
{
	abstract internal class ServerAPI
	{
		/// <summary>User-facing API name. Return empty string to make it an abstract type.</summary>
		public static string APIName() => string.Empty; // Needs to be a method and not a getter to avoid reflection nightmares

		/// <summary>
		/// Poll a specific server API at an address:port for current round status info
		/// </summary>
		/// <param name="address">Server IPv4 address</param>
		/// <param name="port">Current server port</param>
		/// <returns>ServerStatus instance populated with feedback data, or null</returns>
		public static ServerStatus? GetServerStatus(string address, int port) { return null; }

		/// <summary>
		/// Returns a UTF8 response string from a server. Will need QSL parsing/decoding!
		/// Encoding/decoding partially sourced from https://github.com/qwertyquerty/ss13rp/blob/master/util.py
		/// </summary>
		/// <exception cref="BadServerResponseException">The server did not respond with a valid package</exception>
		protected static string Topic(string address, int port, string querystr)
		{
			IPAddress ip = null;
			if (address.All(c => (c >= '0' && c <= '9') || c == '.')) {
				if (address.IndexOf(":") != -1)
				{
					port = int.Parse(address.Split(':')[1]);
					address = address.Remove(address.IndexOf(':'));
				}
				ip = IPAddress.Parse(address);
			} else
			{
				ip = Dns.GetHostAddresses(address).First();
			}


			// Nightmarishly jank but this is required to communicate with BYOND's Topics
			short packetLen = (short)(querystr.Length + 6);
			byte[] queryBytes = BitConverter.GetBytes(packetLen);
			if (BitConverter.IsLittleEndian) // Why is this god-forsaken engine big endian
				Array.Reverse(queryBytes);

			byte[] byondQuery = (new byte[] { 0x00, 0x83 }).Concat( 
				queryBytes.ToList()).Concat( 
				new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 }).Concat( 
				Encoding.UTF8.GetBytes(querystr)).Concat(
				new byte[] { 0x00 }).ToArray();

			IPEndPoint endpoint = new IPEndPoint(ip, port);
			Socket sender = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.IP);
			// BYOND is slow
			sender.SendTimeout = 10000;
			sender.ReceiveTimeout = 10000;
			try
			{
				sender.Connect(endpoint);
				int byteSent = sender.Send(byondQuery);
				byte[] topicResponse = new byte[4096];
				int byteReceived = sender.Receive(topicResponse);
				sender.Shutdown(SocketShutdown.Both);
				sender.Close();
				if (byteReceived < 6)
				{
					throw new BadServerResponseException();
				}
				// Why does it pad the package with 6 bytes?
				return Encoding.UTF8.GetString(topicResponse.Take(byteReceived - 1).Skip(5).ToArray());
			}
			catch (ArgumentNullException ane)
			{
				AutoRecorder.ErrorHandle(ane, String.Format("ArgumentNullException has occured while trying to connect to {0}:{1}: ", ip, port));
			}
			catch (SocketException se)
			{
				AutoRecorder.ErrorHandle(se, String.Format("SocketException has occured while trying to connect to {0}:{1}: ", ip, port));
				Console.WriteLine("SocketException : {0}", se.ToString());
			}
			catch (Exception e)
			{
				AutoRecorder.ErrorHandle(e);
			}
			return null;
		} 
	}

	public class BadServerResponseException : Exception { };
}
