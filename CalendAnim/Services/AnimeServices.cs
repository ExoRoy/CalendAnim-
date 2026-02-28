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
            // Magie du C# : GetFromJsonAsync va télécharger le texte de l'API 
            // et le transformer automatiquement en nos objets C# !
            var reponse = await _httpClient.GetFromJsonAsync<EpisodeResponse>($"anime/{animeId}/episodes");

            if (reponse != null && reponse.Data != null && reponse.Data.Count > 0)
            {
                return reponse.Data.Max(e => e.NumeroEpisode);
            }

            return 0; // Aucun épisode trouvé
        }
        catch (Exception ex)
        {
            // Règle d'or sur mobile : toujours prévoir le cas où l'utilisateur n'a pas internet
            Console.WriteLine($"Erreur de connexion : {ex.Message}");
            return 0;
        }
    }
    
}