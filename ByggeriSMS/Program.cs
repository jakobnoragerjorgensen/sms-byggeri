using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace ByggeriSMS
{
	class Program
	{

		static HttpClientHandler handler = new HttpClientHandler();
		static HttpClient client = new HttpClient(handler);

		static BeskederEntities beskedContext = new BeskederEntities();

		static void Main()
		{
			InitHttpClient();


			//Task.Run(() => SendSMSAsync());
			//Task.Run(() => GetSMSStatus());

			/* [{
			 * "message":"Test besked",
			 * "recipientNumber":"40616454"
			 * "senderNumber":"4140524000"
			 * "system":"testsystem"
			 * "systemCaseId":""
			 * "user":"ADM\\aztmbjj"
			 * "recipient":"40616454"
			 * "recipientType":"MOB"
			 * "messageId":null
			 * "oneWay":true
			 * "conversationPid":10281
			 * "messagePid":30135
			 * "senderAlpha":"SMSTEST"
			 * "messageStatus":{
			 *		"messageId":null
			 *		"createdTimestamp":1549980221000
			 *		"deliveryDetails":{
			 *			"deliveryTime":null
			 *			"deliveryStatus":"PENDING"
			 *		}
			 *		"messagePid":30135
			 *	}
			 *	"created":1549980221000
			 *	}]

	*/


			var data = beskedContext.Set<Besked>();
			data.Add(new Besked
			{
				messagePid = 30135, // messagePid
				ts_oprettet = ConvertUnixTimestamp(1549980221000 / 1000), // messageStatus->createdTimestamp (divider med 1000)
				afsender = "4140524000", // senderNumber telefonnummer på afsender
				afsenderUserID = "ADM\\aztmbjj", // user f.eks. ADM\\aztmbjj
				afsenderNavn = "SMSTEST", // senderAlpha - afsendernavnet som det fremgår på SMS'en
				modtager = "40616454", // recipientNumber - modtagerens telefonnummer
				modtagerType = "MOB", // recipientType
				besked = "Dette er en test besked" // message
			});

			var dataBeskedStatus = beskedContext.Set<BeskedStatus>();
			dataBeskedStatus.Add(new BeskedStatus
			{
				ts_status = ConvertUnixTimestamp(1549980221000 / 1000), // messageStatus->createdTimestamp (divider med 1000)
				status = "PENDING" // messageStatus->deliveryStatus
			});


			beskedContext.SaveChanges();

			Console.ReadLine();
			client = null;
		}

		private static DateTime ConvertUnixTimestamp(long unix_ts)
		{
			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dt = dt.AddSeconds(unix_ts).ToLocalTime();
			return dt;
		}

		private static void InitHttpClient()
		{
			handler.UseDefaultCredentials = true;
			client.BaseAddress = new Uri("https://nemsmsproxy.adm.aarhuskommune.dk");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}


		static async void SendSMSAsync()
		{
			var builder = new UriBuilder("https://nemsmsproxy.adm.aarhuskommune.dk/api/message/send");
			builder.Port = -1;
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["system"] = "testsystem";
			query["message"] = "Test besked";
			query["recipientPhoneNumber"] = "40616454";
			query["oneWay"] = "true";
			query["oneWayAlphaSender"] = "SMSTEST";
			builder.Query = query.ToString();

			HttpResponseMessage response = await client.GetAsync(builder.ToString());
			string responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseContent);



		}


		static async void GetSMSStatus()
		{
			HttpResponseMessage result = await client.GetAsync("/api/message/status?messagePid=30130");
			string resultContent = await result.Content.ReadAsStringAsync();
			Console.WriteLine(resultContent);
		}

	}
}
