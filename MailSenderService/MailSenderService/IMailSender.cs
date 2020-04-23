using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MailSenderService
{
    [ServiceContract]
    public interface IMailSender
    {
        [OperationContract]
        bool SendMail(string from, string password, string mailto, string caption, string message, string[] attachFiles);
    }
}
