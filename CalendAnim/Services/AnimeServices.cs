using System.Collections.ObjectModel;
using System.Net.Http.Json;
using CalendAnim.Modeles;

namespace CalendAnim.Services;

public class AnimeServices
{
    private readonly HttpClient _httpClient;

    public AnimeServices()
    {
        // On prépare notre "navigateur web" interne
        _httpClient = new HttpClient();
        
        // On lui donne l'adresse de base de l'API pour ne pas la retaper à chaque fois
        _httpClient.BaseAddress = new Uri("https://api.jikan.moe/v4/");
    }
    // Le mot "Task" et "async" signifient que cette action se fait en arrière-plan
    // pour ne pas figer l'écran du téléphone pendant le chargement.

    /*public async Task<Anime> GetAnime(string AnimeTitle)
    {
        return await _httpClient.GetFromJsonAsync<Anime>($"anime/{AnimeTitle}");   
    }*/
    
    public async Task<int> GetNombreEpisodesSortisAsync(int animeId)
    {
        try
        {
            // GetFromJsonAsync va télécharger le texte de l'API et le transformer automatiquement en nos objets C# !
            var reponse = await _httpClient.GetFromJsonAsync<EpisodeResponse>($"anime/{animeId}/episodes");

            if (reponse != null && reponse.Data != null && reponse.Data.Count > 0)
            {
                if (reponse.Pagination.LastVisiblePage>1)
                {
                    var episodes = await _httpClient.GetFromJsonAsync<EpisodeResponse>($"anime/{animeId}/episodes?page={reponse.Pagination.LastVisiblePage}"); 
                    return episodes.Data.Max(e => e.NumeroEpisode);
                    
                }
                return reponse.Data.Max(e => e.NumeroEpisode);
            }

            return 0; // Aucun épisode trouvé
        }
        catch (Exception ex)
        {
            //  Prévoir le cas où l'utilisateur n'a pas internet
            Console.WriteLine($"Erreur de connexion : {ex.Message}");
            return 0;
        }
    }
    
    public async Task<ObservableCollection<Anime>> RechercherAnimeAsync(string rechercheUtilisateur)
    {
        try
        {
            // On appelle l'API avec la recherche tapée par l'utilisateur
            var reponse = await _httpClient.GetFromJsonAsync<AnimeSearchResponse>($"anime?q={rechercheUtilisateur}");

            // On vérifie que le colis n'est pas vide (comme on a appris !)
            if (reponse != null && reponse.Data != null)
            {
                return reponse.Data; // On renvoie la liste complète des animés trouvés
            }

            return new ObservableCollection<Anime>(); // Si rien trouvé, on renvoie une liste vide
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur de recherche : {ex.Message}");
            return new ObservableCollection<Anime>();
        }
    }
    
}