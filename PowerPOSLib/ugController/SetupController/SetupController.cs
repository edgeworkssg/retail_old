using System.Collections.Generic;
using System.Data;

namespace PowerPOS.Setup
{
    public interface SetupController
    {
        DataTable FetchAll();
        DataTable FetchAll(object SearchValue);
        DataTable FetchSpecial(int SpecialCode, object SearchValue);

        void Update(Dictionary<string, object> Value);

        void SaveChanges();
    }
}
