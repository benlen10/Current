using System.Collections.Generic;

namespace UniCade
{
    public interface IDatabase
    {
        List<IConsole> ConsoleList { get; set; }
        string HashKey { get; set; }
        int TotalGameCount { get; set; }
        List<IUser> UserList { get; set; }
    }
}