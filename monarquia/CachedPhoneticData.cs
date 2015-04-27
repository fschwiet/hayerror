using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using CsvHelper;
using CsQuery;


namespace monarquia
{
	public class CachedPhoneticData
	{
		Dictionary<string,string> dictionary;
		string filepath;

		public CachedPhoneticData (string directory)
		{
			this.filepath = Path.Combine (directory, "english-phonetics.txt");

			if (File.Exists (filepath)) {
				dictionary = Load (filepath);
			} else {
				dictionary = new Dictionary<string,string> ();
			}
		}

		public string GetEnglishPhonetics(string value) {
		
			if (dictionary.ContainsKey (value)) {
				return dictionary [value];
			}
		
			var result = DownloadEnglishPhoneticTranscription (value);

			dictionary [value] = result;

			Save (filepath, dictionary);

			return result;
		}

		static public Dictionary<string,string> Load(string filepath) 
		{
			Dictionary<string,string> results = new Dictionary<string, string> ();

			using (var inputFile = File.OpenRead (filepath)) 
			{
				using (var inputStream = new StreamReader (inputFile, System.Text.Encoding.UTF8)) 
				{
					var configuration = new CsvHelper.Configuration.CsvConfiguration ();
					configuration.HasHeaderRecord = false;

					var reader = new CsvReader (inputStream, configuration);

					while (reader.Read()) {
						string original = reader.GetField<string> (0);
						string phonetic = reader.GetField<string> (1);

						results [original] = phonetic;
					}
				}
			}

			return results;
		}

		static public void Save(string filepath, Dictionary<string,string> values)
		{
			using (var outputFile = File.OpenWrite (filepath)) {

				var streamWriter = new StreamWriter (outputFile, Encoding.UTF8);

				var csvWriter = new CsvWriter (streamWriter);

				foreach(var key in values.Keys)
				{
					csvWriter.WriteField(key);
					csvWriter.WriteField(values[key]);
					csvWriter.NextRecord ();
				}

				streamWriter.Flush();
				outputFile.Flush ();
			}
		}

		public static string DownloadEnglishPhoneticTranscription (string value)
		{
			string resultText;
			using (var client = new WebClient ()) {
				var resultBytes = client.UploadValues ("http://upodn.com/phon.asp", new NameValueCollection () {
					{
						"intext",
						value
					},
					{
						"ipa",
						"0"
					}
				});
				var memoryStream = new MemoryStream (resultBytes);
				var reader = new StreamReader (memoryStream, System.Text.Encoding.UTF8);
				var document = (CQ)reader.ReadToEnd ();
				var resultElement = document ["tbody td[align=left]"];
				resultText = resultElement.Text ().Trim();
			}
			return resultText;
		}
	}
}

