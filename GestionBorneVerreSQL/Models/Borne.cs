namespace GestionBornesCollecte.Models
{
    // modele qui represente une borne, chaque propriete correspond a une colonne en BDD
    // les tables conteneur et site sont fusionées ici pour simplifier l affichage
    public class Borne
    {
        public int IdBorne { get; set; }
        public string Nom { get; set; } = "";
        public string Adresse { get; set; } = "";

        // stocké en pourcentage pour l affichage, mais en mm dans la BDD
        public int NiveauRemplissage { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}