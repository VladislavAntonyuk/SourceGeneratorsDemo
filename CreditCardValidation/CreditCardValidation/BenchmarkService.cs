using System.Globalization;
using BenchmarkDotNet.Attributes;
using CsvHelper;
using CsvHelper.Configuration;

namespace CreditCardValidation;

[MemoryDiagnoser]
public class BenchmarkService
{
	private readonly List<BaseModel> _data = new();

	public BenchmarkService()
	{
		using var reader = new StreamReader("data.csv");
		using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = true
		});
		csv.Context.RegisterClassMap<Model1Mapper>();
		csv.Context.RegisterClassMap<Model2Mapper>();

		csv.Read();
		csv.ReadHeader();

		while (csv.Read())
		{
			var property16 = csv.GetField<string>(nameof(Model2.Property16));
			if (string.IsNullOrEmpty(property16))
			{
				_data.Add(csv.GetRecord<Model1>());
			}
			else
			{
				_data.Add(csv.GetRecord<Model2>());
			}
		}
	}

	[Benchmark]
	public List<ValidationResult> ValidateCreditCards() => CreditCardValidationService.ValidateCreditCards(_data);

	[Benchmark]
	public List<ValidationResult> SourceGeneratorValidateCreditCards() => CreditCardValidationService.SourceGeneratorValidateCreditCards(_data);
}

public class Model1Mapper : ClassMap<Model1>
{
	public Model1Mapper()
	{
		Map(x => x.Property1).Name("Property1");
		Map(x => x.Property2).Name("Property2");
		Map(x => x.Property3).Name("Property3");
		Map(x => x.Property4).Name("Property4");
		Map(x => x.Property5).Name("Property5");
		Map(x => x.Property6).Name("Property6");
		Map(x => x.Property7).Name("Property7");
		Map(x => x.Property8).Name("Property8");
		Map(x => x.Property9).Name("Property9");
		Map(x => x.Property10).Name("Property10");

		Map(x => x.Property11).Name("Property11");
		Map(x => x.Property12).Name("Property12");
		Map(x => x.Property13).Name("Property13");
		Map(x => x.Property14).Name("Property14");
		Map(x => x.Property15).Name("Property15");
	}
}
public class Model2Mapper : ClassMap<Model2>
{
	public Model2Mapper()
	{
		Map(x => x.Property1).Name("Property1");
		Map(x => x.Property2).Name("Property2");
		Map(x => x.Property3).Name("Property3");
		Map(x => x.Property4).Name("Property4");
		Map(x => x.Property5).Name("Property5");
		Map(x => x.Property6).Name("Property6");
		Map(x => x.Property7).Name("Property7");
		Map(x => x.Property8).Name("Property8");
		Map(x => x.Property9).Name("Property9");
		Map(x => x.Property10).Name("Property10");

		Map(x => x.Property16).Name("Property16");
		Map(x => x.Property17).Name("Property17");
		Map(x => x.Property18).Name("Property18");
		Map(x => x.Property19).Name("Property19");
		Map(x => x.Property20).Name("Property20");
	}
}