using CreditCardValidation;

#if DEBUG
var service = new BenchmarkService();
var result1 = service.SourceGeneratorValidateCreditCards();
var result2 = service.ValidateCreditCards();
#else
BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchmarkService>();
#endif