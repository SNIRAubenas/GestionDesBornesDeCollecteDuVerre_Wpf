# ♻️ Projet Fin d’Année – Gestion Intelligente des Bornes de Collecte de Verre (WPF)

## Contexte

La collecte du verre dans les villes est souvent :

- effectuée trop tôt (borne encore vide)
- effectuée trop tard (débordement)
- non optimisée (perte de carburant)
- polluante (tournées inutiles)

Le système actuel repose sur une collecte “à l’aveugle”.

## Objectif du Projet

Améliorer la collecte du verre grâce à :

- Des capteurs de niveau connectés
- Une transmission des données via Internet
- Une base de données centralisée
-  Une application WPF pour visualiser et gérer les bornes
- Objectif final : Optimiser les tournées et réduire la pollution.
- Vision Globale du Système

Capteur → Serveur → Base de Données → Application WPF

#### Capteurs
Mesure du niveau de remplissage
Transmission des données

#### Serveur & Base de Données
Stockage des mesures
Gestion des bornes
Centralisation des informations

#### Application WPF 
Interface utilisateur
Consultation des données
Gestion CRUD des conteneurs
Affichage des niveaux en temps réel


## Architecture Technique
Architecture en 3 couches

1. Couche Acquisition
Capteurs connectés
Mesure du remplissage

2. Couche Traitement & Stockage
Serveur d’application
Base de données

3. Couche Présentation
Application Desktop WPF (Windows)

## Acteurs du Système

🏠 Habitant
🚛 Éboueur
🧑‍💼 Gestionnaire
🖥 Serveur
📡 Capteur


## Répartition de l’Équipe
- Étudiant 1 – Acquisition & Transmission (NOE)
Gestion des capteurs
Mesures
Transmission des données

- Étudiant 2 – Application WPF (RYM)
Consultation des mesures
Gestion des conteneurs (CRUD)
Affichage des niveaux de remplissage
Organisation du dépôt GitHub
Architecture MVVM

- Étudiant 3 – Infrastructure Serveur (DIANA)
Installation du serveur d’application
Mise en place de la base de données
Administration BDD


## Diagrammes UML
- Diagramme de cas d’utilisation
- Diagramme de classes
- Diagramme de séquence 
- Diagramme de séquence 

## Technologies Utilisées
C#
.NET
WPF
XAML
MVVM
Base de données SQL
Git / GitHub
Capteur

## 🎓 Projet Réalisé dans le Cadre du BTS CIEL

Spécialité : Cybersécurité, Informatique et Réseaux
Année : 2026
