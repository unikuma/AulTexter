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
					ExoTemplate = "（Exoテンプレートが入る）",
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

		private int _CurrentListIndex = 0;
		public int CurrentListIndex
		{
			get => _CurrentListIndex;
			set
			{
				if (value < 0)
					value = 0;
				RaisePropertyChangedIfSet(ref _CurrentListIndex, value);
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

		#endregion
	}
}
