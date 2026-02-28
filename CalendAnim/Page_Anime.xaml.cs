using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalendAnim.Modeles;
using CalendAnim.Services;
using SQLite;

namespace CalendAnim;

public partial class Page_Anime : ContentPage
{
    private Anime _animeCourant;
    private AnimeServices _animeServices;
    private DataBaseService _dbService;
    
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
        int nb_episode = await _animeServices.GetNombreEpisodesSortisAsync(_animeCourant.Id);
        EpisodeSpinner.IsVisible = false;
        EpisodeSpinner.IsRunning = false;
        EpisodeLabel.Text = "Il y'a actuellement " + nb_episode + "episodes";
        if (_animeCourant.Status =="Currently Airing")
            EnCours.Text = "Anime en cours, les episodes sortent tout les " +_animeCourant.Broadcast.Day + " à " +_animeCourant.Broadcast.Time;
        else if (_animeCourant.Status == "Not yet aired")
            EnCours.Text = "Anime pas encore sortie, les episodes sortiront tout les " +_animeCourant.Broadcast.Day + "à" +_animeCourant.Broadcast.Time;
        else
            EnCours.Text = "Anime fini";
        var a = await _dbService.ObtenirUnFavori(_animeCourant);
        if (a!=null)
        {
            BtnAjouterFavori.Text = "Dans ta liste ✓";
            BtnAjouterFavori.BackgroundColor = Colors.Green;
            BtnAjouterFavori.IsEnabled = false;    
        }
        else
        {
            BtnAjouterFavori.Text = "⭐ Ajouter à ma liste";
            BtnAjouterFavori.BackgroundColor = Color.FromArgb("#2196F3");
            BtnAjouterFavori.IsEnabled = true; 
        }

        ;

    }

    private async void OnAjouterFavoriClicked(object? sender, EventArgs e)
    {
        
        _dbService.AjouterFavoriAsync(_animeCourant.ToAnimeFavori());
        BtnAjouterFavori.Text = "Dans ta liste ✓";
        BtnAjouterFavori.BackgroundColor = Colors.Green;
        BtnAjouterFavori.IsEnabled = false; // On empêche de recliquer dessus
        
    }
    
}