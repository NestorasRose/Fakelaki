using Fakelaki.Api.Lib.Models;
using System.Collections.Generic;

namespace Fakelaki.Api.Lib.Services.Interfaces
{
    public interface IFakelakiService
    {
        ICollection<Models.Fakelaki> GetByEvent(int fakelakiId);

        Models.Fakelaki Create(Models.Fakelaki fakelaki, int emailTemplateId, int eventId);
    }
}
