namespace AbySalto.Mid.Domain.Entites
{
    public abstract class BelongToUser: BaseEntity
    {
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
