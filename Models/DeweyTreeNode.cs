using System.Collections.Generic;

namespace BookBuddy.Models
{
    public class DeweyTreeNode {
        public string CallNumber { get; set; }
        public string Description { get; set; }
        public List<DeweyTreeNode> Children { get; set; } = new List<DeweyTreeNode>();

        public override string ToString()
        {
            return $"CallNumber: {CallNumber}, Description: {Description}";
        }
    }
}
