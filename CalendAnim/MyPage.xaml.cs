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
}