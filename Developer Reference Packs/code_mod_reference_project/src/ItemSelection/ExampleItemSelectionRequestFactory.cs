using NurtaleNesche.Modding.Items;
using PlayerGroup;
using UnityEngine;

namespace ExampleMods.ItemSelection
{
    public sealed class ExampleItemSelectionRequestFactory : IItemSelectionRequestFactory
    {
        public bool TryCreate(
            string requestTypeId,
            Player player,
            GameObject itemObject,
            out global::IItemSelectionRequest request)
        {
            request = new global::TokenizedItemSelectionRequest(requestTypeId, itemObject);
            return true;
        }
    }
}
