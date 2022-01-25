using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalDecisions.Shared;

namespace Notifier.Module
{
    interface IMailBase
    {
        void SentNotificationMail(bool isBodyHTML);
        void SentNotificationMail(bool isBodyHTML, string attachmentFilePath);
        void SentNotificationMail(string subject, bool isBodyHTML, string body, string attachmentFileName);
        string GenerateReport(string reportPath, ExportFormatType type);
        SortedList<string, string> GetCRParam();
    }
}
