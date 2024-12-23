using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Core.Common.BaseClasses;

public class BaseModel<TKey>
{
    [Key]
    public TKey Id { get; set; } = default!;
    public DateTime? EntryDate { get; set; }
    public DateTime? UpdateDate { get; set; }
}
