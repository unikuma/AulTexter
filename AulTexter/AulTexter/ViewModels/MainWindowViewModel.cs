using AulTexter.Models;
using Livet;
using Livet.Behaviors;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace AulTexter.ViewModels
{
	public class MainWindowViewModel : ViewModel, IDisposable
	{
		// Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
		public void Initialize()
		{
			ExoConfigs = setting.Deserialize<ObservableCollection<ExoConfig>>();
		}

		private XmlLoader setting = new XmlLoader("Setting.xml");

		private string _ExoText;
		public string ExoText
		{
			get => _ExoText;
			set => RaisePropertyChangedIfSet(ref _ExoText, value);
		}

		private int _CurrentExoConfIndex = 0;
		public int CurrentExoConfIndex
		{
			get => _CurrentExoConfIndex;
			set
			{
				if (value < 0)
					value = 0;
				RaisePropertyChangedIfSet(ref _CurrentExoConfIndex, value);
			}	
		}

		#region ExoConfigs

		private ObservableCollection<ExoConfig> _ExoConfigs = new ObservableCollection<ExoConfig>();
		public ObservableCollection<ExoConfig> ExoConfigs
		{
			get => _ExoConfigs;
			set => RaisePropertyChangedIfSet(ref _ExoConfigs, value);
		}

		#endregion

		#region CallMethodAction

		public void MakeExoFile(object sender)
		{
			if (Directory.Exists("ExoFilesOut"))
			{
				string ExoFile = $"{Environment.CurrentDirectory}\\ExoFilesOut\\" + DateTime.Now.ToString($"yyyyMMddHHmmss") + $"-{ExoConfigs[CurrentExoConfIndex].Name}-{ExoText}.exo";

				using (StreamWriter stw = new StreamWriter(ExoFile, append: false, Encoding.GetEncoding("shift-jis")))
					stw.Write(string.Format(ExoConfigs[CurrentExoConfIndex].ExoTemplate, BitConverter.ToString(Encoding.Unicode.GetBytes(ExoText)).Replace("-", "").ToLower().PadRight(4096, '0')).Replace("\n", "\r\n"));

				DragDrop.DoDragDrop(sender as DependencyObject, new DataObject(DataFormats.FileDrop, new string[] { ExoFile }), DragDropEffects.Copy);
			}
			else
			{
				var result = MessageBox.Show("'ExoFilesOut'フォルダが存在しない為Exoファイルが出力できません。\r\n" +
					"'ExoFilesOut'フォルダを作成しますか？", "エラー", MessageBoxButton.YesNo, MessageBoxImage.Error);
			
				if (result == MessageBoxResult.Yes)
				{
					var dirInfo = Directory.CreateDirectory("ExoFilesOut");
					if (dirInfo.Exists)
						MessageBox.Show("フォルダを作成しました。再度D&Dを試してください。", "AulTexter", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
		}

		public void AddExoConfig()
		{
			ExoConfigs.Add(new ExoConfig()
			{
				Name = "デフォルト",
				ExoTemplate = "",
			});
		}

		public void RemoveExoConfig(ExoConfig ec)
		{
			var index = ExoConfigs.IndexOf(ec);

			if (CurrentExoConfIndex == index)
			{
				MessageBox.Show("コンボボックスで選択中の設定は削除できません", "エラー", 
								MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (CurrentExoConfIndex > index)
				CurrentExoConfIndex--;

			ExoConfigs.RemoveAt(index);
		}

		#endregion
		
		public new void Dispose()
		{
			setting.Serialize(ExoConfigs);
		}
	}
}
