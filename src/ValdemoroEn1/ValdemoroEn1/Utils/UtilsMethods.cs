using Akavache;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ValdemoroEn1.Models;

namespace ValdemoroEn1.Utils
{
    public class UtilsMethods
    {
        public static async Task SaveUserAsync(ValdeUser user)
        {
            await BlobCache.Secure.InsertObject("user", user);
        }

        public static async Task<ValdeUser> GetUserAsync()
        {
            try
            {
                return await BlobCache.Secure.GetObject<ValdeUser>("user");
            }
            catch (KeyNotFoundException ex)
            {
                _ = ex.Message;
                return null;
            }
        }

    }
}
