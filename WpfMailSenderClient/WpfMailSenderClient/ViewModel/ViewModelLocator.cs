using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using WpfMailSenderClient.Service;

namespace WpfMailSenderClient.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
			
            SimpleIoc.Default.Register<MailBoxViewModel>();
            SimpleIoc.Default.Register<IDataAccessService, DataBaseAccessService>();
		}

        public MailBoxViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MailBoxViewModel>();
            }
        }
    }
}