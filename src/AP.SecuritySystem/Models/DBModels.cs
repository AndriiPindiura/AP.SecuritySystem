using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AP.SecuritySystem.Models
{
    public class Server
    {
        public Server ()
        {
            this.Roles = new HashSet<ServersRoles>();
            this.Entries = new HashSet<Entry>();
            this.Protocols = new HashSet<Protocol>();
            this.ProtocolsCRM = new HashSet<ProtocolCRM>();
        }

        public int Id { get; set; }
        public string Ip { get; set; }
        public string Description { get; set; }
        public int Mode { get; set; }
        public string ConnectionString { get; set; }
        public string ItvName { get; set; }
        public virtual ICollection<ServersRoles> Roles { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
        public virtual ICollection<Protocol> Protocols { get; set; }
        public virtual ICollection<ProtocolCRM> ProtocolsCRM { get; set; }
    }

    public class ServersRoles
    {
        public int Id { get; set; }

        public int ServerId { get; set; }
        public virtual Server Server { get; set; }

        //public string RoleId { get; set; }
        public virtual IdentityRole Role { get; set; }
    }

    public class Entry
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public virtual Server Server { get; set; }
        public int NoE { get; set; }
        public bool RaysType { get; set; }
        public string Description { get; set; }
        public int EnterRay { get; set; }
        public int ExitRay { get; set; }
        public int EnterCamera { get; set; }
        public int ExitCamera { get; set; }
        public int UpCamera { get; set; }
        public int EnterDelay { get; set; }
        public int ExitDelay { get; set; }
    }

    public class Protocol
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public virtual Server Server { get; set; }
        public int ObjId { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public Guid Guid { get; set; }
    }

    public class ProtocolCRM
    {
        public int Id { get; set; }
        public int ServerId { get; set; }
        public virtual Server Server { get; set; }
        public string CMRId { get; set; }
        public string LicensePlate { get; set; }
        public DateTime Date { get; set; }
        public int NoE { get; set; }
        public string Culture { get; set; }
        public string CRMState { get; set; }
        public Guid Guid { get; set; }

    }

}
