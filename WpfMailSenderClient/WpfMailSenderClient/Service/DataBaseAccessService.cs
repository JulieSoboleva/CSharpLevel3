using Common;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace WpfMailSenderClient.Service
{
	public class DataAccessService : IDataAccessService
	{
        EmailsModelContainer context;
        public DataAccessService()
        {
            context = new EmailsModelContainer();
        }
        public ObservableCollection<Emails> GetEmails()
        {
            ObservableCollection<Emails> Emails = new ObservableCollection<Emails>();
            foreach (Emails item in context.Emails)
                Emails.Add(item);
            return Emails;
        }

        public int AddEmail(Emails email)
        {
            context.Emails.Add(email);
            context.SaveChanges();
            return email.Id;
        }

        public int UpdateEmail(Emails email)
        {
            context.Emails.Attach(email);
            context.Entry(email).State = EntityState.Modified;
            context.SaveChanges();
            return email.Id;
        }
        public int DeleteEmail(Emails email)
        {
            if (context.Entry(email).State == EntityState.Detached)
                context.Emails.Attach(email);
            context.Emails.Remove(email);
            context.SaveChanges();
            return email.Id;
        }
    }

}