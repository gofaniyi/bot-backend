using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloEpidBot.Model.Domain
{
    public class PartnerToken
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string PartnerId { get; set; }
        public string PartnerName { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateJoined { get; set; }
       

    }


    public class PartnerTokenModel 
    {
       
        public string Token { get; set; }
        public string PartnerName { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateJoined { get; set; }


    }
}
