﻿using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Windows.Forms;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        private static readonly List<string> services = new List<string> { "administratif", "prets  ", "culture" };
        /// <summary>
        /// Services ayant droit de modification
        /// </summary>
        private static readonly List<string> servicesModif = new List<string> { "administratif", "cuture" };
        /// <summary>
        /// Services ayant droits au commandes
        /// </summary>
        private static readonly List<string> servicesCommande = new List<string> { "administratif" };


        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }
        /*/// <summary>
        /// Supprime une commande livre/Dvd dans la bdd
        /// </summary>
        /// <param name="commandeLivreDvd"></param>
        /// <returns></returns>
        public bool SupprimerLivreDvdCom(CommandeDocument commandeLivreDvd)
        {
            return access.SupprimerEntite("commandedocument", commandeLivreDvd);
        }*/

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }
        /// <summary>
        /// Supprime une commande livre/Dvd dans la bdd
        /// </summary>
        /// <param name="commandeLivreDvd"></param>
        /// <returns></returns>
        public bool SupprimerLivreDvdCom(CommandeDocument commandeLivreDvd)
        {
           // MessageBox.Show(JsonConvert.SerializeObject(commandeLivreDvd));
            return access.SupprimerEntite("commandedocument", JsonConvert.SerializeObject(commandeLivreDvd, new CustomDateTimeConverter()));
            
        }

        /// <summary>
        /// Supprime un abonnement
        /// </summary>
        /// <param name="abonnement"></param>
        /// <returns></returns>

        public bool SupprimerAbonnement(Abonnement abonnement)
        {
            return access.SupprimerEntite("abonnement", JsonConvert.SerializeObject(abonnement, new CustomDateTimeConverter()));
        }
        /// <summary>
        /// Vérifie les droits pour accéder aux modifications
        /// </summary>
        /// <param name="utilisateur"></param>
        /// <returns></returns>
        public bool VerifDroitModif(Utilisateur utilisateur)
        {
            if (servicesModif.Contains(utilisateur.Service.Libelle))
                return true;
            return false;
        }
        /// <summary>
        /// Vérifie les droits pour accéder à l'accueil
        /// </summary>
        /// <param name="utilisateur"></param>
        /// <returns></returns>
        public bool VerifDroitAccueil(Utilisateur utilisateur)
        {
            if (services.Contains(utilisateur.Service.Libelle))
                return true;
            return false;
        }
        /// <summary>
        /// Vérifie une commande
        /// </summary>
        /// <param name="utilisateur"></param>
        /// <returns></returns>
        public bool VerifCommande(Utilisateur utilisateur)
        {
            if (servicesCommande.Contains(utilisateur.Service.Libelle))
                return true;
            return false;
        }



        /// <summary>
        /// Récupère les commandes d'une livre
        /// </summary>
        /// <param name="idLivre">id du livre concernée</param>
        /// <returns></returns>
        public List<CommandeDocument> GetCommandesLivres(string idLivre)
        {
            return access.GetCommandesLivres(idLivre);

        }

        /// <summary>
        /// Retourne tous les abonnements d'une revue
        /// </summary>
        /// <param name="idRevue"></param>
        /// <returns></returns>
        public List<Abonnement> GetAbonnements(string idRevue)
        {
            return access.GetAbonnements(idRevue);
        }

        /// <summary>
        /// Retourne l'id max des commandes
        /// </summary>
        /// <returns></returns>
        public string GetNbCommandeMax()
        {
            return access.GetMaxIndex("maxcommande");
        }
        /// <summary>
        /// getter sur les etats
        /// </summary>
        /// <returns></returns>
        public List<Suivi> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }

        /// <summary>
        /// Creer une commande livre/Dvd dans la bdd
        /// </summary>
        /// <param name="commandeLivreDvd"></param>
        /// <returns></returns>
        public bool CreerLivreDvdCom(CommandeDocument commandeLivreDvd)
        {
            return access.CreerEntite("commandedocument", commandeLivreDvd);
        }
        /// <summary>
        /// Modifie une commande livre/Dvd dans la bdd
        /// </summary>
        /// <param name="commandeLivreDvd"></param>
        /// <returns></returns>
        public bool UpdateLivreDvdCom(CommandeDocument commandeLivreDvd)
        {
            return access.UpdateEntite("commandedocument", commandeLivreDvd.Id, commandeLivreDvd);
        }

        /// <summary>
        /// Modifie un abonnement dans la bdd
        /// </summary>
        /// <param name="commandeLivreDvd"></param>
        /// <returns></returns>
        public bool UpdateAbonnement(Abonnement abonnement)
        {
            return access.UpdateEntite2("abonnement", abonnement.Id, abonnement);
        }

        /// <summary>
        /// Creer un abonnement dans la bdd
        /// </summary>
        /// <param name="commandeLivreDvd"></param>
        /// <returns></returns>
        public bool CreerAbonnement(Abonnement abonnement)
        {
            return access.CreerEntite2("abonnement", abonnement);
        }

    }
}