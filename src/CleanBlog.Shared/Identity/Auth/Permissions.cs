using System;
using System.Collections.Generic;
using System.Text;

namespace CleanBlog.Shared.Identity.Auth
{
    public static class Permissions
    {
        public static class Admin
        {
            public const string View = "Permissions.Admin.View";
            public const string Create = "Permissions.Admin.Create";
            public const string Edit = "Permissions.Admin.Edit";
            public const string Delete = "Permissions.Admin.Delete";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }
    }
}
