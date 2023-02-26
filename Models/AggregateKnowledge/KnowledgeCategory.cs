using ProjectFinalEngineer.Models.AggregateCategory;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectFinalEngineer.Models.AggregateKnowledge
{
    [Table("KnowledgeCategory")]
    public class KnowledgeCategory
    {
        public int KnowledgeID { set; get; }

        public int CategoryID { set; get; }

        [ForeignKey("KnowledgeID")]
        public Knowledge Knowledge { set; get; }

        [ForeignKey("CategoryID")]
        public Category Category { set; get; }
    }
}
