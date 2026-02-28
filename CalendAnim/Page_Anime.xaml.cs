using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalendAnim.Modeles;
using CalendAnim.Services;

namespace CalendAnim;

public partial class Page_Anime : ContentPage
{
    private Anime _animeCourant;
    private AnimeServices _animeServices;
    
    public Page_Anime(Anime animeSelectionne)
    {
        InitializeComponent();
        BindingContext = animeSelectionne;
        _animeCourant = animeSelectionne;
        _animeServices = new AnimeServices();
        
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

    }
    
}