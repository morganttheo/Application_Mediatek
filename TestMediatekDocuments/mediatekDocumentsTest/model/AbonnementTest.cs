﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MediaTekDocuments.model
{
        [TestClass()]
        public class AbonnementTests
        {
            private const string id = "20010";
            private static readonly DateTime dateCommande = DateTime.Now;
            private const double montant = 4;
            private static readonly DateTime dateFinAbonnement = DateTime.Now.AddMonths(2);
            private const string idRevue = "10002";
            private static readonly Abonnement abonnement = new Abonnement(id, dateCommande, montant, dateFinAbonnement, idRevue);

        [TestMethod()]
            public void AbonnementTest()
            {
                Assert.AreEqual(id, abonnement.Id, "devrait réussir : id valorisé");
                Assert.AreEqual(dateCommande, abonnement.DateCommande, "devrait réussir : date de commande valorisée");
                Assert.AreEqual(montant, abonnement.Montant, "devrait réussir : montant valorisé");
                Assert.AreEqual(dateFinAbonnement, abonnement.DateFinAbonnement, "devrait réussir : date de fin d'abonnement valorisée");
                Assert.AreEqual(idRevue, abonnement.IdRevue, "devrait réussir : idRevue valorisé");
            }
        }
}
