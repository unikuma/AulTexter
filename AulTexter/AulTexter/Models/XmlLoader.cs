using System.IO;
using System.Text;
using System.Windows;
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
			try
			{
				using (StreamReader str = new StreamReader(FilePath))
				{
					XmlSerializer xml = new XmlSerializer(typeof(T));

					return (T)xml.Deserialize(str);
				}
			}
			catch
			{
				MessageBox.Show($"{FilePath}が開けませんでした", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
				return default;
			}
		}
	}
}
