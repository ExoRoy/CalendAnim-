using System.Collections.ObjectModel;
using CalendAnim.Modeles;
using CalendAnim.Services;

namespace CalendAnim;

public partial class MyPage : ContentPage
{
    private DataBaseService _dbService;
    public ObservableCollection<AnimeFavori> MesFavoris { get; set; }
    
    public MyPage()
    {
        InitializeComponent();
        _dbService = new DataBaseService();
        MesFavoris = new ObservableCollection<AnimeFavori>();
        BindingContext = this;
        
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        MesFavoris.Clear();
        var AnimeSauvegarder = await _dbService.ObtenirFavorisAsync();
        foreach (var anime in AnimeSauvegarder)
        {
            MesFavoris.Add(anime);
        }
    }
    
    
    private async void OnFavoriSelectionne(object sender, SelectionChangedEventArgs e)
    {
        // 1. On récupère l'animé cliqué
        var favoriClique = e.CurrentSelection.FirstOrDefault() as AnimeFavori;
        
        if (favoriClique != null)
        {
            // 2. On enlève la sélection visuelle (pour éviter que la carte reste grisée)
            ((CollectionView)sender).SelectedItem = null;
            
            var animePourLaPage = new Anime 
            { 
                Id = favoriClique.Id, 
                Title = favoriClique.Title,
                Synopsis = favoriClique.Synopsis,
                Images = new AnimeImages { Jpg = new CalendAnim.Modeles.ImageFormat { ImageUrl = favoriClique.ImageUrl } }
            };

            await Navigation.PushAsync(new Page_Anime(animePourLaPage));
        }
    }
    
}