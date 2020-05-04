using System;
using System.Collections.ObjectModel;

namespace WpfMailSenderClient.Service
{
	class XmlFileAccessService : IDataAccessService
	{
		public ObservableCollection<Recipient> GetEmails() => throw new	NotImplementedException();
		public int CreateEmail(Recipient email) => throw new NotImplementedException();
	}
}