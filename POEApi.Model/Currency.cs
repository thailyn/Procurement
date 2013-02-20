﻿namespace POEApi.Model
{
    public class Currency : Item
    {
        public OrbType Type { get; private set; }
        public float GCPValue { get; private set; }
        public StackInfo StackInfo { get; private set; }

        public Currency(JSONProxy.Item item) : base(item)
        {
            this.Type = ProxyMapper.GetOrbType(item);
            this.GCPValue = CurrencyHandler.GetGCPValue(this.Type);
            this.StackInfo = ProxyMapper.GetStackInfo(item.Properties);
        }
    }
}
