using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Utilisateur
    {
        public Utilisateur(string id, string nom, string prenom, string mail, string idService, string service)
        {
            this.Id = id;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Mail = mail;
            this.Service = new Service(idService, service);
        }
        public string Id { get; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
        public Service Service { get; }
    }
}
