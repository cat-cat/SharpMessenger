#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using ChatClient.Core.Common.Models;
using ChatClient.Core.SAL.Methods;
using ChatClient.Core.UI.PopupPages;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
#endregion

namespace ChatClient.Core.UI.ViewModels {
	public class BaseViewModel : INotifyPropertyChanged
	{
		protected ChatMessage _chatMessage = new ChatMessage();
		ChatMessage _editMessage;

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
		Command _cancelEditCommand;
		Command _makeEditCommand;

		bool canLoadMore;
		string icon = null;
		bool isBusy;
		bool isValid;
		string subTitle = string.Empty;

		#endregion

		#region Properties

		public bool IsInitialized { get; set; }

		public async void StartEditMessage(ChatMessage m)
		{
			EditMessage = m;

			// will not show popup
			//await App.Navigation.PushPopupAsync(new pgEditMessage(this));

			await App.Navigation.PushAsync(new pgEditMessage(this));
		}

		async void CancelEditMessage()
		{
			EditMessage = null;
			await App.Navigation.PopAsync();
		}

		async void MakeEditMessage()
		{
			await App.Navigation.PopAsync();

			EditMessage.messageEdited = true;
			// do request to server for editing
			User lUser = await BL.Session.Authorization.GetUser();
			new MessageStatusGet(lUser.Token, EditMessage).Object();
		}

		public Command MakeEditCommand
		{
			get
			{
				return _makeEditCommand ?? (_makeEditCommand = new Command(() => { MakeEditMessage(); }));
			}
		}

		public Command CancelEditCommand
		{
			get
			{
				return _cancelEditCommand ?? (_cancelEditCommand = new Command(() => { CancelEditMessage(); }));
			}
		}

		public ChatMessage ChatMessage
		{
			get
			{
				return _chatMessage;
			}
			set
			{
				_chatMessage = value;
				OnPropertyChanged("ChatMessage");
			}
		}

		public ChatMessage EditMessage
		{
			get
			{
				return _editMessage;
			}
			set
			{
				_editMessage = value;
				OnPropertyChanged("EditMessage");
			}
		}

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
		//virtual public void SocketOff() { }
		virtual public void TypingBroadcast(DateTime d) { }
		#endregion
	}
}