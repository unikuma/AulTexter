using AulTexter.Models;
using Livet;
using Livet.Commands;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;

namespace AulTexter.ViewModels
{
	public class MainWindowViewModel : ViewModel, IDisposable
	{
		// Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
		public void Initialize()
		{
			ExoConfigs = setting.Deserialize<ObservableCollection<ExoConfig>>() ?? new ObservableCollection<ExoConfig>();
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

				if (value >= ExoConfigs.Count)
					value = ExoConfigs.Count - 1;
				
				RaisePropertyChangedIfSet(ref _CurrentExoConfIndex, value);
			}	
		}

		private ObservableCollection<ExoConfig> _ExoConfigs = new ObservableCollection<ExoConfig>();
		public ObservableCollection<ExoConfig> ExoConfigs
		{
			get => _ExoConfigs;
			set => RaisePropertyChangedIfSet(ref _ExoConfigs, value);
		}

		#region CallMethodAction

		// Exoファイル作成
		public void MakeExoFile(object sender)
		{
			if (!Directory.Exists("ExoFilesOut"))
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
			else if (string.IsNullOrWhiteSpace(ExoText))
			{
				MessageBox.Show("字幕用テキストがnullや空白文字、空の可能性があります。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else if (ExoConfigs.Count == 0)
			{
				MessageBox.Show("設定が存在しません。設定タブの「追加」ボタンから設定を追加してください", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				string ExoFile = $"{Environment.CurrentDirectory}\\ExoFilesOut\\" + DateTime.Now.ToString($"yyyyMMddHHmmss") + $"-{ExoConfigs[CurrentExoConfIndex].Name}-{ExoText}.exo";

				using (StreamWriter stw = new StreamWriter(ExoFile, append: false, Encoding.GetEncoding("shift-jis")))
					stw.Write(string.Format(ExoConfigs[CurrentExoConfIndex].ExoTemplate, BitConverter.ToString(Encoding.Unicode.GetBytes(ExoText)).Replace("-", "").ToLower().PadRight(4096, '0')).Replace("\n", "\r\n"));

				DragDrop.DoDragDrop(sender as DependencyObject, new DataObject(DataFormats.FileDrop, new string[] { ExoFile }), DragDropEffects.Copy);
			}
		}

		// 設定の追加
		public void AddExoConfig()
		{
			ExoConfigs.Add(new ExoConfig()
			{
				Name = "デフォルト",
				ExoTemplate = "",
			});
		}

		// 設定の削除
		public void RemoveExoConfig(ExoConfig ec)
		{
			if (MessageBoxResult.Yes == MessageBox.Show($"「{ec.Name}」を削除しますか？", "AulTexter", MessageBoxButton.OK, MessageBoxImage.Warning))
			{
				var index = ExoConfigs.IndexOf(ec);

				if (CurrentExoConfIndex == index)
				{
					MessageBox.Show("コンボボックスで選択中の設定は削除できません", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				if (CurrentExoConfIndex > index)
					CurrentExoConfIndex--;

				ExoConfigs.RemoveAt(index);
			}
		}

		#endregion

		#region InvokeCommandAction

		private ViewModelCommand _ComboDown = null;
		public ViewModelCommand ComboDown => _ComboDown ?? new ViewModelCommand(() => CurrentExoConfIndex++);
		
		private ViewModelCommand _ComboUp = null;
		public ViewModelCommand ComboUp => _ComboUp ?? new ViewModelCommand(() => CurrentExoConfIndex--);

		#endregion

		public new void Dispose()
		{
			setting.Serialize(ExoConfigs);
		}
	}
}
