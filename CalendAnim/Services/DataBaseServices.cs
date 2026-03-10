using SQLite;
using CalendAnim.Modeles;
using System.Collections.ObjectModel;
using CalendAnim.Services;


namespace CalendAnim.Services;

public class DataBaseService
{
    private SQLiteAsyncConnection _db;
    
    private async Task Init()
    {
        if (_db != null) return;

        // On cherche le dossier sécurisé l'application sur le téléphone
        var cheminFichier = Path.Combine(FileSystem.AppDataDirectory, "CalendAnim.db3");
        
        _db = new SQLiteAsyncConnection(cheminFichier);
        
        // On crée la table pour ranger nos AnimeFavori
        await _db.CreateTableAsync<AnimeFavori>();
    }

    // Fonction pour ajouter un animé à ta liste
    public async Task AjouterFavoriAsync(AnimeFavori anime)
    {
        await Init(); // On s'assure que la base est prête
        
        //On verifie que l'animé a des episodes sinon on cherche s'il en a
        if (anime.Episodes == null)
        {
            var animeServices = new AnimeServices();
            anime.Episodes = await animeServices.ObtenirEpisodesAsync(anime.Id);
        }
        
        if (anime.Episodes != null)
        {
            anime.EpisodesJson = System.Text.Json.JsonSerializer.Serialize(anime.Episodes);
        }
        
        // InsertOrReplace : S'il n'existe pas, il l'ajoute. Sinon met à jour 
        await _db.InsertOrReplaceAsync(anime); 
    }

    // Fonction pour lire tous tes favoris (pour ton futur écran d'accueil !)
    public async Task<List<AnimeFavori>> ObtenirFavorisAsync()
    {
        await Init();
        return await _db.Table<AnimeFavori>().ToListAsync();
    }
    
    //Fonction pour verifier l'existance d'un anime dans la base de données 
    public async Task<AnimeFavori> ObtenirUnFavori(Anime anime)
    {
        await Init();
        AnimeFavori a = await _db.Table<AnimeFavori>().FirstOrDefaultAsync(e => e.Id == anime.Id);
        if (!String.IsNullOrEmpty(a?.EpisodesJson))
            a.Episodes = await Task.Run(() => System.Text.Json.JsonSerializer.Deserialize<List<Episode>>(a.EpisodesJson));
        return a;
    }

    public async Task SupprimerFavori(AnimeFavori anime)
    {
        await Init();
        await _db.DeleteAsync(anime);
    }

}