﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloEpidBot.Model.Domain
{
    public class TokenOption
    {
        public class TokenOptions
        {
            public string Audience { get; set; }
            public string Issuer { get; set; }
            public long accessExpiration { get; set; }
            public long RefreshTokenExpiration { get; set; }
            public string Secret { get; set; }

        }

    }
}
