using System.Collections.ObjectModel;
using System.Net.Http.Json;
using CalendAnim.Modeles;
using System.Text.Json;

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
    
    public async Task<List<Episode>> ObtenirEpisodesAsync(int animeId)
    {
        try
        {
            // On appelle l'URL spécifique pour les épisodes d'un animé
            var reponse = await _httpClient.GetAsync($"https://api.jikan.moe/v4/anime/{animeId}/episodes");
            
            if (reponse.IsSuccessStatusCode)
            {
                var json = await reponse.Content.ReadAsStringAsync();
                var resultat = JsonSerializer.Deserialize<EpisodeResponse>(json);
                return resultat?.Data ?? new List<Episode>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur API : {ex.Message}");
        }
        
        return new List<Episode>(); // Si ça plante, on renvoie une liste vide
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