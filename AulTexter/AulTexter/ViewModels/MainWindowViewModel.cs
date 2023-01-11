using AulTexter.Models;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace AulTexter.ViewModels
{
	public class MainWindowViewModel : ViewModel
	{
		// Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
		public void Initialize()
		{
			
		}

		public MainWindowViewModel()
		{
			ExoConfigs = new ObservableCollection<ExoConfing>()
			{
				new ExoConfing()
				{
					Name = "通常",
					ExoTemplate = "（Exoテンプレートが入る）",
				},
				new ExoConfing()
				{
					Name = "通常2",
					ExoTemplate = "（Exoテンプレートが入る２）",
				}
			};
		}

		private string _ExoText;
		public string ExoText
		{
			get => _ExoText;
			set => RaisePropertyChangedIfSet(ref _ExoText, value);
		}

		private int _CurrentComboIndex = 0;
		public int CurrentComboIndex
		{
			get => _CurrentComboIndex;
			set
			{
				if (value < 0)
					value = 0;
				RaisePropertyChangedIfSet(ref _CurrentComboIndex, value);
			}	
		}

		#region ExoConfigs

		private ObservableCollection<ExoConfing> _ExoConfigs = new ObservableCollection<ExoConfing>();
		public ObservableCollection<ExoConfing> ExoConfigs
		{
			get => _ExoConfigs;
			set => RaisePropertyChangedIfSet(ref _ExoConfigs, value);
		}

		#endregion

		#region CallMethodAction

		public void MakeExoFile()
		{
			CurrentComboIndex = -1;
		}

		public void AddExoConfig()
		{
			ExoConfigs.Add(new ExoConfing()
			{
				Name = "デフォルト",
				ExoTemplate = "",
			});
		}

		public void RemoveExoConfig(ExoConfing ec)
		{
			var index = ExoConfigs.IndexOf(ec);

			if (CurrentComboIndex == index)
			{
				MessageBox.Show("コンボボックスで選択中の設定は削除できません", "エラー", 
								MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (CurrentComboIndex > index)
				CurrentComboIndex--;

			ExoConfigs.RemoveAt(index);
		}

		#endregion
	}
}
