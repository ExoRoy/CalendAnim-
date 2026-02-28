using System.Text.Json.Serialization; // Indispensable pour lier le JSON au C#
using System.Collections.ObjectModel;


namespace CalendAnim.Modeles;

public class Anime
{
    [JsonPropertyName("mal_id")] public int Id { get; set; }

    [JsonPropertyName("title")] public string Title { get; set; } 

    [JsonPropertyName("synopsis")] public string Synopsis { get; set; }

    // Utilisation d'un int? (nullable) car certains animés en cours n'ont pas de nombre d'épisodes défini
    [JsonPropertyName("episodes")] public int? EpisodesCount { get; set; } 

    [JsonPropertyName("score")] public float? Score { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; }

    [JsonPropertyName("rating")] public string Rating { get; set; } // Remplace ton AgeClass

    // Jikan renvoie la durée sous forme de texte ("24 min per ep"), donc c'est un string !
    [JsonPropertyName("duration")] public string Duration { get; set; } 

    // ---- Sous-objets complexes de Jikan ----

    [JsonPropertyName("images")]
    public AnimeImages Images { get; set; }

    [JsonPropertyName("broadcast")]
    public BroadcastInfo Broadcast { get; set; }

    public AnimeFavori ToAnimeFavori()
    {
        AnimeFavori af = new AnimeFavori();
        af.Id = Id;
        af.Title = Title;
        af.Synopsis = Synopsis;
        af.ImageUrl = Images.Jpg.ImageUrl;
        return af;
    }
}

// Sous-classe pour lire la structure des images de Jikan
public class AnimeImages
{
    [JsonPropertyName("jpg")] public ImageFormat Jpg { get; set; }
}

public class ImageFormat
{
    [JsonPropertyName("image_url")] public string ImageUrl { get; set; } // C'est ici qu'on récupère enfin le lien de l'image !
}

// Sous-classe pour la date de sortie des épisodes
public class BroadcastInfo
{
    [JsonPropertyName("day")] public string Day { get; set; } // Ex: "Tuesdays"

    [JsonPropertyName("time")] public string Time { get; set; } // Ex: "00:00"
}

public class AnimeSearchResponse
{
    [JsonPropertyName("data")] public ObservableCollection<Anime> Data { get; set; }
}