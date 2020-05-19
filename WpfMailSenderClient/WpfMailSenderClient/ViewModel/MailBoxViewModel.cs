using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WpfMailSenderClient.Service;
using WpfMailSenderClient.Views;
using Common;

namespace WpfMailSenderClient.ViewModel
{
	public class MailBoxViewModel : ViewModelBase
	{
		private readonly IDataAccessService _dataService;
		private ObservableCollection<Emails> _emails = new ObservableCollection<Emails>();

		public ObservableCollection<Emails> Emails
		{
			get => _emails;
			set
			{
				if (!Set(ref _emails, value)) 
					return;
				_emailsView = new CollectionViewSource { Source = value };
				_emailsView.Filter += OnEmailsCollectionViewSourceFilter;
				RaisePropertyChanged(nameof(EmailsView));
			}
		}

		private void OnEmailsCollectionViewSourceFilter(object sender, FilterEventArgs e)
		{
			if (!(e.Item is Emails email) || string.IsNullOrWhiteSpace(_filterName)) 
				return;
			if (!email.Name.Contains(_filterName))
				e.Accepted = false;
		}

		private Emails _currentEmail = new Emails();
		public Emails CurrentEmail
		{
			get => _currentEmail;
			set => Set(ref _currentEmail, value);
		}

		private KeyValuePair<string, string> _curSenders;
		public KeyValuePair<string, string> CurSenders
		{
			get => _curSenders;
			set => Set(ref _curSenders, value);
		}

		private string _filterName;
		public string FilterName
		{
			get => _filterName;
			set
			{
				if (!Set(ref _filterName, value)) 
					return;
				EmailsView.Refresh();
			}
		}

		private CollectionViewSource _emailsView;
		public ICollectionView EmailsView => _emailsView?.View;

		public RelayCommand<Emails> UpdateEmailCommand { get; }
		public RelayCommand<Emails> AddEmailCommand { get; }
		public RelayCommand<Emails> DeleteEmailCommand { get; }
		public RelayCommand ReadAllMailsCommand { get; }
		
		public RelayCommand<ObservableCollection<ListViewItemScheduler>> BtnAddMail { get; set; }
		public ObservableCollection<ListViewItemScheduler> SendMails { get; set; }

		public MailBoxViewModel(IDataAccessService dataService)
		{
			_dataService = dataService;
			ReadAllMailsCommand = new RelayCommand(GetEmails);
			UpdateEmailCommand = new RelayCommand<Emails>(UpdateEmail);
			AddEmailCommand = new RelayCommand<Emails>(AddEmail);
			DeleteEmailCommand = new RelayCommand<Emails>(DeleteEmail);

			SendMails = new ObservableCollection<ListViewItemScheduler>();
			BtnAddMail = new RelayCommand<ObservableCollection<ListViewItemScheduler>>(AddMail);
		}

		private void UpdateEmail(Emails email) => _dataService.UpdateEmail(email);
		private void AddEmail(Emails email)
		{
			_dataService.AddEmail(email);
			GetEmails();
		}
		private void DeleteEmail(Emails email)
		{
			_dataService.DeleteEmail(email);
			GetEmails();
		}

		private void GetEmails() => Emails = _dataService.GetEmails();

		private void AddMail(ObservableCollection<ListViewItemScheduler> obj = null)
		{
			ListViewItemScheduler scheduler = new ListViewItemScheduler(SendMails);
			scheduler.btnEditMailTextClick += Scheduler_btnEditMailTextClick;
			SendMails.Add(scheduler);
		}

		private void Scheduler_btnEditMailTextClick(object sender, RoutedEventArgs e)
		{
			MailBox.me.tcMain.SelectedItem = MailBox.me.tabLetterEditor;
		}
	}
}
