using System.Data.Entity.ModelConfiguration;

namespace Bidding.Data.Mappings
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("Users");
            this.HasKey(t => t.UserId);
        }
    }
}
