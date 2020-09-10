using System;
using System.Collections.Generic;
using System.Text;

namespace IMark.Core.Helpers
{
  public  class AppConstant
    {
        #region regex
        public static string EmailRegex = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
        public static string PhoneRegex = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$";
        public static string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!% *? &])[A-Za-z\d@$!% *? &]{8,15}$";
        #endregion

    }
}
