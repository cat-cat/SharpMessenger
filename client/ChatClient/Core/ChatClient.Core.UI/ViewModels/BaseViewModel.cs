#region

using System;
using System.Collections.Generic;
using System.ComponentModel;

using Xamarin.Forms;

#endregion

namespace ChatClient.Core.UI.ViewModels {
	public class BaseViewModel : INotifyPropertyChanged
	{
		#region Static & Const

		/// <summary>
		///     Gets or sets the "IsBusy" property
		/// </summary>
		/// <value>The isbusy property.</value>
		public const string CanLoadMorePropertyName = "CanLoadMore";

		/// <summary>
		///     Gets or sets the "IsBusy" property
		/// </summary>
		/// <value>The isbusy property.</value>
		public const string IsBusyPropertyName = "IsBusy";

		/// <summary>
		///     Gets or sets the "IsValid" property
		/// </summary>
		/// <value>The isbusy property.</value>
		public const string IsValidPropertyName = "IsValid";

		/// <summary>
		///     Gets or sets the "Subtitle" property
		/// </summary>
		public const string SubtitlePropertyName = "Subtitle";

		/// <summary>
		///     Gets or sets the "Icon" of the viewmodel
		/// </summary>
		public const string IconPropertyName = "Icon";

		#endregion

		#region Fields

		private bool canLoadMore;
		private string icon = null;
		private bool isBusy;
		private bool isValid;
		private string subTitle = string.Empty;

		#endregion

		#region Properties

		public bool IsInitialized { get; set; }

		public bool CanLoadMore
		{
			get
			{
				return canLoadMore;
			}
			set
			{
				SetProperty(ref canLoadMore, value, CanLoadMorePropertyName);
			}
		}

		public bool IsBusy
		{
			get
			{
				return isBusy;
			}
			set
			{
				SetProperty(ref isBusy, value, IsBusyPropertyName);
			}
		}

		public bool IsValid
		{
			get
			{
				return isValid;
			}
			set
			{
				SetProperty(ref isValid, value, IsValidPropertyName);
			}
		}

		public string Subtitle
		{
			get
			{
				return subTitle;
			}
			set
			{
				SetProperty(ref subTitle, value, SubtitlePropertyName);
			}
		}

		public string Icon
		{
			get
			{
				return icon;
			}
			set
			{
				SetProperty(ref icon, value, IconPropertyName);
			}
		}

		#endregion

		#region INotifyPropertyChanged Members

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#endregion

		#region Public Methods and Operators

		#region INotifyPropertyChanging implementation

		public event PropertyChangingEventHandler PropertyChanging;

		#endregion

		public void OnPropertyChanging(string propertyName)
		{
			if (PropertyChanging == null)
				return;

			PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
		}

		public void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region Protected Methods and Operators

		protected void SetProperty<U>(
			ref U backingStore,
			U value,
			string propertyName,
			Action onChanged = null,
			Action<U> onChanging = null)
		{
			if (EqualityComparer<U>.Default.Equals(backingStore, value))
				return;

			if (onChanging != null)
				onChanging(value);

			OnPropertyChanging(propertyName);

			backingStore = value;

			if (onChanged != null)
				onChanged();

			OnPropertyChanged(propertyName);
		}

		#endregion

		#region Custom functions
		virtual public void SocketOff() { }
		virtual public void TypingBroadcast(DateTime d) { }
		#endregion
	}
}