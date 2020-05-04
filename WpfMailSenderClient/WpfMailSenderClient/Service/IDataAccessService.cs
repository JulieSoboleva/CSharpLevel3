using System.Collections.ObjectModel;

namespace WpfMailSenderClient.Service
{
	public interface IDataAccessService
	{
		ObservableCollection<Recipient> GetEmails();
		int CreateEmail(Recipient email);
	}
}