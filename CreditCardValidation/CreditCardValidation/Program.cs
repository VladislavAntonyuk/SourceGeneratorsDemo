using BenchmarkDotNet.Running;
using CreditCardValidation;

#if DEBUG
var service = new BenchmarkService();
var result1 = service.SourceGeneratorValidateCreditCards();
var result2 = service.ValidateCreditCards();
var result3 = service.ValidateCreditCards();
#else
BenchmarkRunner.Run<BenchmarkService>();
#endif