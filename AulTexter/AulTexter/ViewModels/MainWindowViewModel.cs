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

		public MainWindowViewModel()
		{
			ExoConfigs = new ObservableCollection<ExoConfig>()
			{
				new ExoConfig()
				{
					Name = "通常",
					ExoTemplate = "（Exoテンプレートが入る）",
				},
				new ExoConfig()
				{
					Name = "通常2",
					ExoTemplate = "（Exoテンプレートが入る２）",
				}
			};
		}

		private XmlLoader setting = new XmlLoader("Setting.xml");

		private string _ExoText;
		public string ExoText
		{
			get => _ExoText;
			set => RaisePropertyChangedIfSet(ref _ExoText, value);
		}

		private int _CurrentExoConfigsIndex = 0;
		public int CurrentExoConfigsIndex
		{
			get => _CurrentExoConfigsIndex;
			set
			{
				if (value < 0)
					value = 0;
				RaisePropertyChangedIfSet(ref _CurrentExoConfigsIndex, value);
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

		public void MakeExoFile()
		{
			
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

			if (CurrentExoConfigsIndex == index)
			{
				MessageBox.Show("コンボボックスで選択中の設定は削除できません", "エラー", 
								MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (CurrentExoConfigsIndex > index)
				CurrentExoConfigsIndex--;

			ExoConfigs.RemoveAt(index);
		}

		#endregion
		
		public new void Dispose()
		{
			setting.Serialize(ExoConfigs);
		}
	}
}
