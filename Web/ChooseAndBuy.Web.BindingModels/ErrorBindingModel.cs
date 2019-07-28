namespace ChooseAndBuy.Web.BindingModels
{
    public class ErrorBindingModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
    }
}
