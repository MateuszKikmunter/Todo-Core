using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoItem.Core.Entities;

namespace ToDoItem.Infrastructure.DataAccess.Config
{
    public class ToDoItemConfig : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Property(i => i.Deadline).IsRequired();
            builder.Property(i => i.Name).IsRequired().HasMaxLength(155);
            builder.Property(i => i.AdditionalInformation).HasMaxLength(255);
        }
    }
}
