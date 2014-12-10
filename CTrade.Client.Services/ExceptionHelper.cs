using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrade.Client.Services
{
    static class ExceptionHelper
    {
        internal static void ThrowWrappedException()
        {
            throw new ServiceException(ErrorMessage.ServiceError);
        }
    }
}
