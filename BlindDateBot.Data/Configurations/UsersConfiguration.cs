
using BlindDateBot.Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlindDateBot.Data.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(e => e.Id).HasName("user_pkey");
            builder.Property(e => e.Id).HasColumnName("user_id");
            builder.Property(e => e.Username).HasColumnName("username");
            builder.Property(e => e.Gender).HasColumnName("user_gender");
            builder.Property(e => e.InterlocutorGender).HasColumnName("interlocutor_gender");
            builder.Property(e => e.IsFree).HasColumnName("is_free");
        }
    }
}
