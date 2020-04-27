using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMailSenderClient
{
    internal static class Database
    {
        private static readonly dbRecipientDataContext recipientsDataContext = new dbRecipientDataContext();

        public static IQueryable<Recipient> Recipients =>
            from recipient in recipientsDataContext.Recipients select recipient;
    }
}
