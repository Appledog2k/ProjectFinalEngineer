using System.ComponentModel.DataAnnotations;

namespace ProjectFinalEngineer.Models.AggregateKnowledge
{
    public class CreateKnowledgeModel : Knowledge
    {
        [Display(Name = "Chuyên mục")]
        public int[] CategoryIDs { get; set; }
    }
}
