using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloEpidBot.Model.Domain
{
    public class User
    {
    }
    public class InviteUserModel
    {
        public string[] UserMailList { get; set; }
        public UserType UserType { get; set; }

    }

    public enum UserType
    {
        Admin,
        Client
    }
}
