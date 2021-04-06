using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ApplicationServer.DB {
    public interface IAvatarsDB {

        IEnumerable<Avatar> GetAllAvatars();

    }
}