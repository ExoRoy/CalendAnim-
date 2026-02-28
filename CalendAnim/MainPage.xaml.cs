
using System.Diagnostics; // Indispensable pour écrire des messages dans la console de Rider
using CalendAnim.Services; // Pour que ton écran connaisse l'existence de ton service
using CalendAnim.Modeles;
using System.Collections.ObjectModel;

namespace CalendAnim;

public partial class MainPage : ContentPage
{
    int count = 0;
    private readonly AnimeServices _animeServices;
    ObservableCollection<Anime> AnimesTrouves;
    
    
    public MainPage()
    {
        InitializeComponent();
        _animeServices = new AnimeServices();
        AnimesTrouves = new ObservableCollection<Anime>();
        AnimeCollectionView.ItemsSource = AnimesTrouves;
    }

    private async void OnCounterClicked(object? sender, EventArgs e)
    {
        var animes = await _animeServices.RechercherAnimeAsync("Hunter X Hunter (2011)");
        Debug.WriteLine("J'ai trouvé: " + animes.Count + " animes");
        if (animes.Count > 0 && animes[0] != null)
        {
            Anime premier_anime = animes[0];
            Debug.WriteLine("Le premiere anime est: " + premier_anime.Title);
            Debug.WriteLine("Il possède: " + await _animeServices.GetNombreEpisodesSortisAsync(premier_anime.Id)+"episode");
            
        }
    }

    private async void OnSearchButtonPressed(object? sender, EventArgs e)
    {
        if (LoadingSpinner.IsVisible) return; //Empeche les double clicks
        
        string SearchText = AnimeSearchBar.Text;
        if (SearchText.Length == 0)
        {
            return;
        }
        LoadingSpinner.IsVisible = true;
        LoadingSpinner.IsRunning = true;
        AnimesTrouves.Clear();
        
        var animes = await _animeServices.RechercherAnimeAsync(SearchText);
        Debug.WriteLine("J'ai trouvé: " + animes.Count + " animes");

        if (animes != null && animes.Count > 0)
        {
            foreach (var anime in animes)
            {
                AnimesTrouves.Add(anime);
            }
        }
        LoadingSpinner.IsVisible = false;
        LoadingSpinner.IsRunning = false;

       
    }

    private async void ImageButton_OnClicked(object? sender, EventArgs e)
    {
        ImageButton AnimeButton = (ImageButton)sender;
        Anime AnimeClique = (Anime) AnimeButton.BindingContext;
        await Navigation.PushAsync(new Page_Anime(AnimeClique));
    }
}

