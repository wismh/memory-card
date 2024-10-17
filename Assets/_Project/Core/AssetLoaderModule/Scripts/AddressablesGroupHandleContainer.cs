using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project.Core.AssetLoaderModule 
{
    [Serializable]
    public class AddressablesGroupHandleContainer 
    {
        public readonly Dictionary<string, AsyncOperationHandle> CompletedHandles = new();
        public readonly Dictionary<string, List<AsyncOperationHandle>> AllHandles = new();
    }
}