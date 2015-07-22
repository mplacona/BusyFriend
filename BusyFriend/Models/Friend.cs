using System.ComponentModel.DataAnnotations;

namespace BusyFriend.Models
{
    public class Friend
    {
        [Key]
        public int FriendId { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
