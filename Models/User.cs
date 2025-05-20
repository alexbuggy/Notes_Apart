using Microsoft.AspNetCore.Identity;

namespace NotesApart.Models
{
    public class User:IdentityUser
    {
        public ICollection<Review> Reviews { get; set; }
        public ICollection<FavouriteSong> FavoriteSongs { get; set; }
        public ICollection<FavouriteAlbum> FavoriteAlbums { get; set; }
        public ICollection<LikedReview> LikedReviews { get; set; }
        public string ProfileImg { get; set; } = "https://static.vecteezy.com/system/resources/previews/009/292/244/non_2x/default-avatar-icon-of-social-media-user-vector.jpg";
    }
}
