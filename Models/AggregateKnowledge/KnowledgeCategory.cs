using ProjectFinalEngineer.Models.AggregateCategory;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectFinalEngineer.Models.AggregateKnowledge
{
    [Table("KnowledgeCategory")]
    public class KnowledgeCategory
    {
        public int KnowledgeId { set; get; }

        public int CategoryId { set; get; }

        [ForeignKey("KnowledgeID")]
        public Knowledge Knowledge { set; get; }

        [ForeignKey("CategoryID")]
        public Category Category { set; get; }
    }
}
