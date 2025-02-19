using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MySql.Data.Entity;

namespace VisorObraCFI.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext() : base("name=MySqlConnection")
        {
            this.Database.Connection.Open();
            using (var command = this.Database.Connection.CreateCommand())
            {
                command.CommandText = "SET NAMES utf8mb4;";
                command.ExecuteNonQuery();
            }
        }
        public DbSet<vw_looker_obras> vw_looker_obras { get; set; }
        public DbSet<PryArchivosObra> PryArchivosObra { get; set; }
    }
}