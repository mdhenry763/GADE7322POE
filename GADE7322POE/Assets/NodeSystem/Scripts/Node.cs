using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeSystem
{
    public enum ResourceType
    {
        MetalOre,
        RockOre
    }
    
    public class Node : MonoBehaviour
    {
        public GameObject resourcePrefabs;
        public Transform[] nodes;
        public ResourceType currentResource;
    
    
        void Start()
        {
            SpawnNodes();
        }

        private void SpawnNodes()
        {
            NodeManager nodeManager = new NodeManager(nodes);

            currentResource = nodeManager.returnRandomResourceNode;

            foreach (var node in nodes)
            {
                Vector3 nodePos = nodes[nodeManager.randomNodeSelection].transform.position;

                if (currentResource == ResourceType.MetalOre || currentResource == ResourceType.RockOre)
                {
                    if (!nodeManager.doesResourceExist(nodePos))
                    {
                        GameObject nodeSpawned = Instantiate(
                            resourcePrefabs,
                            nodePos,
                            Quaternion.identity);
                        
                        nodeManager.nodeDuplicateCheck.Add(nodeSpawned.transform.position, nodePos);
                        nodeSpawned.transform.SetParent(this.transform);
                    }
                }
            }
        }

    
    }

    public class NodeManager
    {
        private Transform[] _nodes;

        public Hashtable nodeDuplicateCheck = new Hashtable();

        public NodeManager(Transform[] nodes)
        {
            _nodes = nodes;
        }

        public int randomNodeSelection => Random.Range(0, _nodes.Length);
        public int randomResourceSelection => Random.Range(0, 100) % 50;
        public bool doesResourceExist(Vector3 pos) => nodeDuplicateCheck.ContainsKey(pos);

        public ResourceType returnRandomResourceNode
        {
            get
            {
                if (randomResourceSelection >= 20) return ResourceType.MetalOre;
                else return ResourceType.RockOre;
            }
        } 
    }

    
}

