namespace PowerPOS.Container
{
    public enum CostingMethods
    { FIFO, FixedAvg, WeightedAvg }

    public class InventorySettings
    {
        public static CostingMethods CostingMethod = CostingMethods.FIFO; /* Default: FIFO */
    }
}
