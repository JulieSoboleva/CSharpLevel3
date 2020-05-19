using System.Collections.ObjectModel;
using System.Data.Entity;
using Common;

namespace WpfMailSenderClient.Service
{
	public interface IDataAccessService
	{
		ObservableCollection<Emails> GetEmails();
		int AddEmail(Emails email);
		int UpdateEmail(Emails email);
		int DeleteEmail(Emails email);
	}
}