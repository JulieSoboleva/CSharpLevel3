using System.Collections.ObjectModel;
using System.Linq;

namespace WpfMailSenderClient.Service
{
	public class DataBaseAccessService : IDataAccessService
	{
		private readonly dbRecipientDataContext _dataContext = new dbRecipientDataContext();
		public ObservableCollection<Recipient> GetEmails() => new ObservableCollection<Recipient>(_dataContext.Recipients);
		public int CreateEmail(Recipient email)
		{
			if (_dataContext.Recipients.Contains(email)) 
				return email.ID;

			_dataContext.Recipients.InsertOnSubmit(email);
			_dataContext.SubmitChanges();
			return email.ID;
		}
	}
}