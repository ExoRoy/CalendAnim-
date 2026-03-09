using SQLite;

namespace CalendAnim.Modeles;

public class AnimeFavori
{
    [PrimaryKey] public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string ImageUrl { get; set; } // On sauvegarde directement le lien en texte, c'est plus simple
    
    public string Synopsis { get; set; }
    
    public string GenresString { get; set; }
    
    public string BroadcastDay { get; set; }
    public string BroadcastTime { get; set; }

    public string EpisodesJson;
    
    [Ignore] public List<Episode> Episodes { get; set; }
    
}