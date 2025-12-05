using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.class1
{
    public static class TokenStore
    {
        public static string? Token { get; set; }
        public static DateTime? ExpiresAt { get; set; }

        public static bool IsExpired =>
            !ExpiresAt.HasValue || DateTime.UtcNow >= ExpiresAt.Value.AddSeconds(-30);
    }
}
