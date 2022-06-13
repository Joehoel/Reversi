using System.Collections.Generic;

namespace ReversiMvcApp.Data
{
    public static class ApplicationClaimTypes
    {
        public static List<string> AppClaimTypes = new List<string>()
        {
            "Admin", "Moderator", "Speler"
        };
    }
}