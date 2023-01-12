using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace AulTexter.Models
{
	public class XmlLoader
	{
		public string FilePath { get; set; } = "Setting.xml";

		public XmlLoader(string filepath) => FilePath = filepath;

		public void Serialize<T>(T data)
		{
			using (StreamWriter stw = new StreamWriter(FilePath, append: false, new UTF8Encoding(false)))
			{
				XmlSerializer xml = new XmlSerializer(typeof(T));

				xml.Serialize(stw, data);
			}
		}

		public T Deserialize<T>()
		{
			using (StreamReader str = new StreamReader(FilePath))
			{
				XmlSerializer xml = new XmlSerializer(typeof(T));

				return (T)xml.Deserialize(str);
			}
		}
	}
}
