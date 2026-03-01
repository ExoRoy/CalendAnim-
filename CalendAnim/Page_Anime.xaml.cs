using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalendAnim.Modeles;
using CalendAnim.Services;
using System.Collections.ObjectModel;

namespace CalendAnim;

public partial class Page_Anime : ContentPage
{
    private Anime _animeCourant;
    private AnimeServices _animeServices;
    private DataBaseService _dbService;
    public ObservableCollection<Episode> ListeEpisodes { get; set; } = new ObservableCollection<Episode>();
    
    public Page_Anime(Anime animeSelectionne)
    {
        InitializeComponent();
        BindingContext = animeSelectionne;
        _animeCourant = animeSelectionne;
        _animeServices = new AnimeServices();
        _dbService = new DataBaseService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        EpisodeSpinner.IsRunning = true;
        EpisodeSpinner.IsVisible = true;
        // On vide la liste au cas où
        ListeEpisodes.Clear();
        
        ListeEpisodesContainer.BindingContext = this;

        // On lance le téléchargement
        EpisodeSpinner.IsRunning = true;
        EpisodeSpinner.IsVisible = true;

        var episodesTelecharges = await _animeServices.ObtenirEpisodesAsync(_animeCourant.Id);

        EpisodeSpinner.IsRunning = false;
        EpisodeSpinner.IsVisible = false;

        // On remplit notre liste pour l'affichage
        if (episodesTelecharges.Count > 0)
        {
            TitreSectionEpisodes.Text = $"{episodesTelecharges.Count} Épisodes";
            foreach (var ep in episodesTelecharges)
            {
                ListeEpisodes.Add(ep);
            }
        }
        else
        {
            TitreSectionEpisodes.Text = "Aucun épisode trouvé";
        }
        
        // Affiche le statut de l'anime
        if (_animeCourant.Status =="Currently Airing")
            EnCours.Text = "Anime en cours, les episodes sortent tout les " +_animeCourant.Broadcast.Day + " à " +_animeCourant.Broadcast.Time;
        else if (_animeCourant.Status == "Not yet aired")
            EnCours.Text = "Anime pas encore sortie, les episodes sortiront tout les " +_animeCourant.Broadcast.Day + "à" +_animeCourant.Broadcast.Time;
        else
            EnCours.Text = "Anime fini";
        
        //Affiche les genres
        if (_animeCourant.Genres != null)
            GenresLabel.Text = string.Join(" • ", _animeCourant.Genres.Select(g => g.Name));
        
        var a = await _dbService.ObtenirUnFavori(_animeCourant);
        if (a!=null)
        {
            IconeFavori.Text = "💙"; // Cœur bleu
            TexteFavori.Text = "Dans la liste";
            TexteFavori.TextColor = Color.FromArgb("#8A2BE2");
        }
        else
        {
            IconeFavori.Text = "🤍"; 
            TexteFavori.Text = "Ajouter";
            TexteFavori.TextColor = Colors.White;
        }
        
        
        ;

    }

    private async void OnAjouterFavoriClicked(object? sender, EventArgs e)
    {
        AnimeFavori animeFavori = await _dbService.ObtenirUnFavori(_animeCourant);
        if (animeFavori == null)
        {
             await _dbService.AjouterFavoriAsync(_animeCourant.ToAnimeFavori());
             IconeFavori.Text = "💙"; // Cœur bleu
             TexteFavori.Text = "Dans la liste";
             TexteFavori.TextColor = Color.FromArgb("#8A2BE2");
        }
        else
        {
           await _dbService.SupprimerFavori(animeFavori);
           IconeFavori.Text = "🤍"; // Cœur blanc vide
           TexteFavori.Text = "Ajouter";
           TexteFavori.TextColor = Colors.White;
        }
        
        
    }

    
    
}