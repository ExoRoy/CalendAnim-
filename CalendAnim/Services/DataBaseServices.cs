using SQLite;
using CalendAnim.Modeles;
using System.Collections.ObjectModel;

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
        return await _db.Table<AnimeFavori>().FirstOrDefaultAsync(e => e.Id == anime.Id);
    }

    public async Task SupprimerFavori(AnimeFavori anime)
    {
        await Init();
        await _db.DeleteAsync(anime);
    }

}