namespace CreditCardValidation;

public abstract class BaseModel
{
	[ValidateCreditCard]
	public string? Property1 { get; set; }
	[ValidateCreditCard]
	public string? Property2 { get; set; }
	[ValidateCreditCard]
	public string? Property3 { get; set; }
	[ValidateCreditCard]
	public string? Property4 { get; set; }
	[ValidateCreditCard]
	public string? Property5 { get; set; }
	[ValidateCreditCard]
	public string? Property6 { get; set; }
	[ValidateCreditCard]
	public string? Property7 { get; set; }
	[ValidateCreditCard]
	public string? Property8 { get; set; }
	[ValidateCreditCard]
	public string? Property9 { get; set; }
	public string? Property10 { get; set; }

	public abstract IEnumerable<PropertyValue> GetPropertiesForCreditCardValidation();
}

public partial class Model1 : BaseModel
{
	[ValidateCreditCard]
	public string? Property11 { get; set; }
	[ValidateCreditCard]
	public string? Property12 { get; set; }
	[ValidateCreditCard]
	public string? Property13 { get; set; }
	[ValidateCreditCard]
	public string? Property14 { get; set; }
	[ValidateCreditCard]
	public string? Property15 { get; set; }
}

public partial class Model2 : BaseModel
{
	[ValidateCreditCard]
	public string? Property16 { get; set; }
	[ValidateCreditCard]
	public string? Property17 { get; set; }
	[ValidateCreditCard]
	public string? Property18 { get; set; }
	[ValidateCreditCard]
	public string? Property19 { get; set; }
	public string? Property20 { get; set; }
}