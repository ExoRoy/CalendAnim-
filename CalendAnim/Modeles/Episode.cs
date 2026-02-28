using System.Text.Json.Serialization;

namespace CalendAnim.Modeles;

// La boîte globale que Jikan nous renvoie
public class EpisodeResponse
{
    [JsonPropertyName("data")] public List<Episode> Data { get; set; }
    
    [JsonPropertyName("pagination")] public PaginationInfo Pagination { get; set; }
}

// Le détail d'un épisode
public class Episode
{
    // Dans l'API Jikan, le "mal_id" de l'épisode correspond à son numéro exact !
    [JsonPropertyName("mal_id")] public int NumeroEpisode { get; set; } 

    [JsonPropertyName("title")] public string Titre { get; set; }
}

// Les infos de pagination (utile pour les très longs animés)
public class PaginationInfo
{
    [JsonPropertyName("last_visible_page")] public int LastVisiblePage { get; set; }
}