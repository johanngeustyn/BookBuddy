using BookBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBuddy.Services
{
    public class QuizService
    {
        private readonly List<DeweyTreeNode> _treeNodes;
        private Random rnd = new Random();

        public QuizService(List<DeweyTreeNode> treeNodes)
        {
            _treeNodes = treeNodes;
        }

        public DeweyTreeNode GetRandomThirdLevelEntry()
        {
            var topLevel = _treeNodes[rnd.Next(_treeNodes.Count)];
            var secondLevel = topLevel.Children[rnd.Next(topLevel.Children.Count)];
            var thirdLevel = secondLevel.Children[rnd.Next(secondLevel.Children.Count)];
            return thirdLevel;
        }

        public List<DeweyTreeNode> GetOptionsForLevel(DeweyTreeNode correctNode)
        {
            List<DeweyTreeNode> sameLevelNodes;

            DeweyTreeNode parent = FindParentOf(correctNode);

            if (parent == null) // If it's a top-level node
            {
                sameLevelNodes = _treeNodes;
            }
            else if (_treeNodes.Contains(parent)) // If the node is second-level
            {
                sameLevelNodes = parent.Children;
            }
            else // If the node is third-level
            {
                DeweyTreeNode grandParent = FindParentOf(parent); // Get parent of the parent

                if (grandParent == null)
                {
                    throw new Exception("Grandparent not found for node: " + parent.CallNumber);
                }

                sameLevelNodes = grandParent.Children;
            }

            List<DeweyTreeNode> options = new List<DeweyTreeNode> { correctNode };

            while (options.Count < 4)
            {
                if (options.Count >= sameLevelNodes.Count) // Exit the loop if there are not enough unique nodes
                {
                    break;
                }

                DeweyTreeNode randomOption = sameLevelNodes[rnd.Next(sameLevelNodes.Count)];

                if (!options.Contains(randomOption))
                {
                    options.Add(randomOption);
                }
            }

            options.Sort((a, b) => a.CallNumber.CompareTo(b.CallNumber));

            return options;
        }

        public DeweyTreeNode FindParentOf(DeweyTreeNode targetChild)
        {
            return FindParentRecursive(_treeNodes, targetChild);
        }

        private DeweyTreeNode FindParentRecursive(List<DeweyTreeNode> currentNodes, DeweyTreeNode targetChild)
        {
            foreach (var node in currentNodes)
            {
                if (node.Children != null && node.Children.Contains(targetChild))
                {
                    return node;
                }

                if (node.Children != null)
                {
                    var result = FindParentRecursive(node.Children, targetChild);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }
    }
}
