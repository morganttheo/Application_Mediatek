﻿using MediaTekDocuments.manager;
using MediaTekDocuments.model;
using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using Serilog;
using System.Configuration;
using System.Collections.Generic;
using System.Windows.Forms;
using Log = Serilog.Log;



namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        /// 
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";

        private static readonly string uriApiName = "MediaTekDocuments.Properties.Settings.mediatekConnectionString";

        private static readonly string authenticationName = "MediaTekDocuments.Properties.Settings.mediatekAuthenticationString";

        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update
        private const string PUT = "PUT";
        ///</summary 
        /// <summary>
        /// méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";
        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        /// 
        private Access()
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.File(@"C:\logs\log.txt")
                    .CreateLogger();
                String authenticationString = GetAuthentificationString(authenticationName);
                //String uriApi = GetAuthentificationString(uriApiName);
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Log.Fatal("Access catch erreur={0}", e.Message);
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }

            /*String authenticationString;
            try
            {
                authenticationString = "admin:adminpwd";

                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }*/
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if (instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre");
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon");
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public");
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre");
            return lesLivres;
        }
        /// <summary>
        /// Retourne les commandes d'un livre
        /// </summary>
        /// <param name="idLivre"></param>
        /// <returns></returns>
        /// 


        /// <summary>
        /// Retourne tous les etats à partir de la BDD
        /// </summary>
        /// <returns></returns>
        public List<Etat> GetAllEtats()
        {
            IEnumerable<Etat> lesEtats = TraitementRecup<Etat>(GET, "etat");
            return new List<Etat>(lesEtats);
        }

        /// <summary>
        /// Retourne les commandes d'un livre
        /// </summary>
        /// <param name="idLivre"></param>
        /// <returns></returns>
        public List<CommandeDocument> GetCommandesLivres(string idLivre)
        {
            String jsonIdDocument = convertToJson("idLivreDvd", idLivre);
            Console.WriteLine("commandedocument/" + jsonIdDocument);
            List<CommandeDocument> lesCommandesLivres = TraitementRecup<CommandeDocument>(GET, "commandedocument/" + jsonIdDocument);
            return lesCommandesLivres;
        }


        /// <summary>
        /// Retourne tous les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd");
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue");
            return lesRevues;
        }


        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument);
            return lesExemplaires;
        }

        /// <summary>
        /// Récupération de la chaîne de connexion
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string GetAuthentificationString(string name)
        {
            string returnValue = null;
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            if (settings != null)
                returnValue = settings.ConnectionString;
            return returnValue;
        }

        /// <summary>
        /// Retourne l'index max en string
        /// de certaines tables
        /// </summary>
        /// <param name="maxIndex"></param>
        /// <returns></returns>
        public string GetMaxIndex(string maxIndex)
        {
            List<Categorie> maxindex = TraitementRecup<Categorie>(GET, maxIndex);
            return maxindex[0].Id;
        }
        /// <summary>
        /// Retourne les abonnements d'une revue
        /// </summary>
        /// <param name="idRevue"></param>
        /// <returns></returns>
        public List<Abonnement> GetAbonnements(string idRevue)
        {
            String jsonAbonnementIdRevue = convertToJson("idRevue", idRevue);
            List<Abonnement> abonnements = TraitementRecup<Abonnement>(GET, "abonnements/" + jsonAbonnementIdRevue);
            return abonnements;
        }

        /// <summary>
        /// Créer une entite dans la BDD, return true si l'opération s'est correctement déroulée
        /// </summary>
        /// <param name="type"></param>
        /// <param name="commandeLivreDvd"></param>
        /// <returns></returns>
        public bool CreerEntite(string type, CommandeDocument commandeLivreDvd)
        {
            String jsonEntite = JsonConvert.SerializeObject(commandeLivreDvd, new CustomDateTimeConverter());
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Object> liste = TraitementRecup<Object>(POST, type + "/" + jsonEntite);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Log.Error("Access.CreerEntite catch type erreur={0}, table={1}, champs={2}", ex, type, jsonEntite);
                Console.WriteLine(ex.Message);
            }
            return false;

        }
        /// <summary>
        ///Modifie une entite dans la BDD, return true si l'opération, c'est correctement déroulé
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="commandeLivreDvd"></param>
        /// <returns></returns>
        public bool UpdateEntite(string type, string id, CommandeDocument commandeLivreDvd)
        {
            String jsonEntite = JsonConvert.SerializeObject(commandeLivreDvd, new CustomDateTimeConverter());

            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Object> liste = TraitementRecup<Object>(PUT, type + "/" + id + "/" + jsonEntite);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Log.Error("Access.UpdateEntite catch type erreur={0}, table={1}, champs={2}", ex, type, jsonEntite);

                Console.WriteLine(ex.Message);
            }
            return false;
        }


        public bool CreerEntite2(string type, Abonnement abonnement)
        {
            String jsonEntite = JsonConvert.SerializeObject(abonnement, new CustomDateTimeConverter());
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Object> liste = TraitementRecup<Object>(POST, type + "/" + jsonEntite);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }

        public bool UpdateEntite2(string type, string id, Abonnement abonnement)
        {
            String jsonEntite = JsonConvert.SerializeObject(abonnement, new CustomDateTimeConverter());

            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Object> liste = TraitementRecup<Object>(PUT, type + "/" + id + "/" + jsonEntite);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        /// <summary>
        /// Supprime une entité dans la BDD, return true si l'opération s'est correctement déroulée
        /// </summary>
        /// <param name="jsonEntite"></param>
        /// <returns></returns>
        public bool SupprimerEntite(string type, String jsonEntite)
        {
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Object> liste = TraitementRecup<Object>(DELETE, type + "/" + jsonEntite);
                

                return (liste != null);
            }
            catch (Exception ex)
            {
                Log.Error("Access.UpdateEntite catch type erreur={0}, table={1}, champs={2}", ex, type, jsonEntite);
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        public Utilisateur GetLogin(string mail, string hash)
        {
            Dictionary<string, string> login = new Dictionary<string, string>
            {
                { "mail", mail },
                { "password", hash }
            };
            String mailHash = JsonConvert.SerializeObject(login);
            List<Utilisateur> utilisateurs = TraitementRecup<Utilisateur>(GET, "utilisateur/" + mailHash);
            Console.Write(mailHash);
            if (utilisateurs.Count > 0)
                return utilisateurs[0];
            Log.Error("Access.GetLogin catch user fail connection :" + mail);
            return null;
        }




        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                // récupération soit d'une liste vide (requête ok) soit de null (erreur)
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire/" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Log.Error("Access.CreerExemplaire catch type erreur={0} champs={1}", ex, jsonExemplaire);
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T>(String methode, String message)
        {
            List<T> liste = new List<T>();
            try
            {
                Console.WriteLine("TraitementRecup " + methode + " et " + message);
                JObject retour = api.RecupDistant(methode, message);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                    Log.Error("Access.TraitementRecup code erreur = " + code + " message = " + (String)retour["message"]);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : " + e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

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
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

        public List<Suivi> GetAllSuivis()
        {
            IEnumerable<Suivi> LesSuivis = TraitementRecup<Suivi>(GET, "suivi");
            return new List<Suivi>(LesSuivis);
        }

    }
}
